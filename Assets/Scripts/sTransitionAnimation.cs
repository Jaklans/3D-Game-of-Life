using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using static Unity.Mathematics.math;

public class sTransitionAnimation : JobComponentSystem
{
    [BurstCompile]
    struct sTransitionAnimationJob : IJobForEach<Scale, Rotation, CellTransition, CellStatus>
    {
        public void Execute(ref Scale scale, ref Rotation rotation, [ReadOnly] ref CellTransition u, [ReadOnly] ref CellStatus status)
        {
            if (status.activeState == status.nextState) return;
            if (status.activeState)
            {
                scale.Value = 1.0f - u.value;
                //rotation.Value =
            }
            else
            {
                scale.Value = u.value;
            }
        }
    }

    public new bool ShouldRunSystem()
    {
        return base.ShouldRunSystem() && !gameState.shouldUpdate();
    }

    protected override JobHandle OnUpdate(JobHandle inputDependencies)
    {
        var job = new sTransitionAnimationJob();
        
        // Assign values to the fields on your job here, so that it has
        // everything it needs to do its work when it runs later.
        // For example,
        //     job.deltaTime = UnityEngine.Time.deltaTime;
        
        
        
        // Now that the job is set up, schedule it to be run. 
        return job.Schedule(this, inputDependencies);
    }
}