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

    private int cellCounter = 0;

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
                    var cellInstance = entityManager.Instantiate(prefab);

                    var position = transform.TransformPoint(x * 1.2f - 4.5f, y * 1.2f, z * 1.2f);

                    CellTransition cTran = new CellTransition { transition = 1.0f };

                    CellIndex cInd;

                    if (x == 0 || x == gridWidth - 1 || y == 0 ||  y == gridHeight - 1 || z == 0 || z == gridDepth - 1)
                    {
                        cInd = new CellIndex { deadCell = true, index = cellCounter };
                    }
                    else
                    {
                        cInd = new CellIndex { deadCell = false, index = cellCounter };
                    }

                    cellCounter++;

                    entityManager.AddComponent<CellIndex>(cellInstance);
                    entityManager.AddComponent<CellTransition>(cellInstance);
                    entityManager.AddComponent<CellNeighbors>(cellInstance);

                    entityManager.SetComponentData(cellInstance, cTran);
                    entityManager.SetComponentData(cellInstance, cInd);
                    entityManager.SetComponentData(cellInstance, new Translation { Value = position });
                }
            }
        }
    }
}
