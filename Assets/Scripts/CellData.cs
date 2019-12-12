using UnityEngine;
using Unity.Entities;

public struct CellData : IComponentData
{
    public Vector3 cellPosition;
    public bool currentState;
    public bool nextState;
    public float transition;
    //public Entity[] neighbors;
}
