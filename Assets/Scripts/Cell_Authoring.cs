using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

[RequiresEntityConversion]
public class Cell_Authoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        CellData data = new CellData
        {
            cellPosition = Vector3.zero,
            isAlive = false,
            neighbors = 0
        };

        // cellPrefab = conversionSystem.GetPrimaryEntity(cellPrefab);
        dstManager.AddComponentData(entity, data);
    }
}
