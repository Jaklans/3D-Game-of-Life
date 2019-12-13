using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject cellPrefab;
    public const int gridHeight =20;
    public const int gridWidth = 20;
    public const int gridDepth = 20;

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

                    CellTransition cTran = new CellTransition { value = 0.0f };

                    CellIndex cIndex;

                    if (x == 0 || x == gridWidth - 1 || y == 0 ||  y == gridHeight - 1 || z == 0 || z == gridDepth - 1)
                    {
                        cIndex = new CellIndex { deadCell = true, index = cellCounter };
                    }
                    else
                    {
                        cIndex = new CellIndex { deadCell = false, index = cellCounter };
                    }

                    cellCounter++;

                    entityManager.AddComponent<CellIndex>(cellInstance);
                    entityManager.AddComponent<CellTransition>(cellInstance);
                    entityManager.AddComponent<CellStatus>(cellInstance);
                    entityManager.AddComponent<Scale>(cellInstance);
                    //entityManager.AddComponent<CellNeighbors>(cellInstance);

                    entityManager.SetComponentData(cellInstance, cTran);
                    entityManager.SetComponentData(cellInstance, cIndex);
                    entityManager.SetComponentData(cellInstance, new Translation { Value = position });
                    entityManager.SetComponentData(cellInstance, new Scale { Value = cIndex.deadCell ? 0.0f:1.0f });
                }
            }
        }
    }
}
