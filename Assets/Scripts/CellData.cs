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

public struct CellStatus : IComponentData
{
    public bool activeState;
    public bool nextState;
}