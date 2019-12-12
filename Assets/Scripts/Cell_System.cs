using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using static Unity.Mathematics.math;

/*public class Cell_System : JobComponentSystem
{
    [BurstCompile]
    struct Cell_System : IJobForEach<CellData>
    {
        public void Execute(ref CellData c0)
        {
            throw new System.NotImplementedException();
        }
    };

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new Cell_System();

        return job.Schedule(this, inputDeps);
    }
}*/
