using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using static Unity.Mathematics.math;

public class sIncrementTransition : JobComponentSystem
{
    const float speedModifier = 1.0f;
    float transitionValue = 0.0f;

    [BurstCompile]
    struct sIncrementTransitionJob : IJobForEach<CellTransition>
    {
        public float currentValue;

        public void Execute(ref CellTransition cellTransition)
        {
            cellTransition.value = currentValue;
        }
    }
    public new bool ShouldRunSystem()
    {
        return base.ShouldRunSystem() && !gameState.shouldUpdate();
    }

    protected override JobHandle OnUpdate(JobHandle inputDependencies)
    {
        var job = new sIncrementTransitionJob();

        transitionValue += UnityEngine.Time.deltaTime * speedModifier;

        job.currentValue = transitionValue;

        if (transitionValue >= 1.0f)
        {
            transitionValue = 0f;
            gameState.setUpdate(true);
        }

        return job.Schedule(this, inputDependencies);
    }
}