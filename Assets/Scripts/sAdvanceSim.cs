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
    const int eU = 7; //Existance max
    const int fT = 6; //Fertility min
    const int fU = 6; //Fertility max

    int3 dimmensions = new int3(20);

    NativeArray<bool> stateA;
    NativeArray<bool> stateB;
    bool currentState = false;

    [BurstCompile]
    struct sAdvanceSimJob : IJobForEach<CellIndex, CellStatus>
    {
        public bool skip;

        [ReadOnly]
        public NativeArray<bool> activeState;

        [NativeDisableParallelForRestriction]
        public NativeArray<bool> nextState;

        [ReadOnly]
        public int3 dim;

        public void Execute([ReadOnly] ref CellIndex cell, [WriteOnly] ref CellStatus status)
        {
            if (cell.deadCell || skip) return;
            if (cell.deadCell)
            {
                nextState[cell.index] = false;
                return;
            }

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

            status.activeState = activeState[cell.index];

            //If currently alive
            if (status.activeState)
            {
                //Kill if survival conditions not met
                status.nextState = !(activeNeighbors < eT || activeNeighbors > eU);
                nextState[cell.index] = status.nextState;
            }
            //If currently dead
            else
            {
                //Spawn if fertility conditions met
                status.nextState = activeNeighbors >= fT && activeNeighbors <= fU;
                nextState[cell.index] = status.nextState;
            }
        }
    }

    public new bool ShouldRunSystem()
    {
        return base.ShouldRunSystem() && gameState.shouldUpdate();
    }

    protected override void OnCreate()
    {
        base.OnCreate();

        bool[] initialState = new bool[dimmensions.x * dimmensions.y * dimmensions.z];

        Random random = new Random();
        random.InitState();
        for(int i = 0; i < initialState.Length; i++)
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
        job.skip = !gameState.shouldUpdate();

        if(!job.skip) currentState = !currentState;
        gameState.setUpdate(false);

        return job.Schedule(this, inputDependencies);
    }
}