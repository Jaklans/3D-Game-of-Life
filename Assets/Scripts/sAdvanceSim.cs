using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using static Unity.Mathematics.math;



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