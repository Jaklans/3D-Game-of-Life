using UnityEngine;
using Unity.Entities;

public struct CellTransition : IComponentData
{
    public float transition;
}

public struct CellIndex : IComponentData
{
    public int index;
    public bool deadCell;
}