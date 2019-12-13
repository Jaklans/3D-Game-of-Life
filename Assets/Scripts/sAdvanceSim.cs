using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using static Unity.Mathematics.math;


public class gameState
{
    static gameState state;
    bool update = false;
    public static bool shouldUpdate()
    {
        if (state == null) state = new gameState();
        return state.update;
    }
    public static void setUpdate(bool update)
    {
        if (state == null) state = new gameState();
        state.update = update;
    }
}

public class sAdvanceSim : JobComponentSystem
{
    const int eT = 4; //Existance min
    const int eU = 5; //Existance max
    const int fT = 5; //Fertility min
    const int fU = 5; //Fertility max

    int3 dimmensions = new int3(5, 5, 5);

    NativeArray<bool> stateA;
    NativeArray<bool> stateB;
    bool currentState;

        [BurstCompile]
    struct sAdvanceSimJob : IJobForEach<CellIndex>
    {
        [ReadOnly]
        public NativeArray<bool> activeState;
        [WriteOnly]
        public NativeArray<bool> nextState;

        [ReadOnly]
        public int3 dim;

        public void Execute([ReadOnly] ref CellIndex cell)
        {

            
            if (cell.deadCell) return;

            //Run through each neighbor based on indicies
            int activeNeighbors = 0;


            for (int z = -1; z <= 1; z++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    for (int x = -1; x <= 1; x++)
                    {
                        if (activeState[cell.index + x + y * dim.x + z * dim.x * dim.y])
                        {
                            activeNeighbors++;
                        }
                    }
                }
            }

            //If currently alive
            if (activeState[cell.index])
            {
                //Kill if survival conditions not met
                nextState[cell.index] = !(activeNeighbors < eT || activeNeighbors > eU);
            }
            //If currently dead
            else
            {
                //Spawn if fertility conditions met
                nextState[cell.index] = activeNeighbors >= fT && activeNeighbors <= fU;
            }
        }
    }

    public new bool ShouldRunSystem()
    {
        return gameState.shouldUpdate();
    }

    protected override void OnCreate()
    {
        base.OnCreate();

        bool[] initialState = new bool[1000];

        Random random = new Random();
        random.InitState();
        for(int i = 0; i < 1000; i++)
        {
            initialState[i] = random.NextBool();
        }
        stateA = new NativeArray<bool>(initialState, Allocator.Persistent);
        stateB = new NativeArray<bool>(initialState, Allocator.Persistent);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        stateA.Dispose();
        stateB.Dispose();
    }

    protected override JobHandle OnUpdate(JobHandle inputDependencies)
    {
        var job = new sAdvanceSimJob();

        job.activeState = currentState ? stateA : stateB;
        job.nextState = currentState ? stateB : stateA;
        job.dim = dimmensions;

        // Now that the job is set up, schedule it to be run. 
        return job.Schedule(this, inputDependencies);
    }
}