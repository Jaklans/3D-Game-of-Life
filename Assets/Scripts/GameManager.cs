using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject cellPrefab;
    public const int gridHeight = 10;
    public const int gridWidth = 10;
    public const int gridDepth = 10;

    private float positionalOffset = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        var prefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(cellPrefab, World.Active);
        var entityManager = World.Active.EntityManager;

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                for (int z = 0; z < gridDepth; z++)
                {
                    // Instantiate our cell prefab as a entity
                    var cellInstance = entityManager.Instantiate(cellPrefab);

                    var position = transform.TransformPoint(x + positionalOffset, y + positionalOffset, z + positionalOffset);

                    //entityManager.SetComponentData(cellInstance, new Translation { Value = position });

                    CellData cData = new CellData {cellPosition = position, currentState = false, nextState = false, transition = 1.0f};

                    entityManager.SetComponentData(cellInstance, cData);
                }
            }
        }
    }
}
