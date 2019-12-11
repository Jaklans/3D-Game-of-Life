using UnityEngine;
using Unity.Entities;

public struct CellData : IComponentData
{
    public Vector3 cellPosition;
    public bool isAlive;
    public int neighbors;
}
