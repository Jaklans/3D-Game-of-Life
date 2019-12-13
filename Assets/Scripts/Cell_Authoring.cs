using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

[RequiresEntityConversion]
public class Cell_Authoring : MonoBehaviour ,IConvertGameObjectToEntity
{
    [SerializeField]
    private float transition;

    [SerializeField]
    private int index;

    [SerializeField]
    private bool deadCell;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponentData(entity, new CellIndex { index = 0, deadCell = true });
        dstManager.AddComponentData(entity, new CellTransition { value = 0.0f });
    }
}
