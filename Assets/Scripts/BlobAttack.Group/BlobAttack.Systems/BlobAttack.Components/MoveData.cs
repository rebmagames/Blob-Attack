//Author: Amber Voskamp, StickyLock
using Unity.Entities;
using Unity.Mathematics;

/// <summary>
/// The movement data
/// The speed of the player and direction 
/// 
/// </summary>

[GenerateAuthoringComponent]
public struct MoveData : IComponentData
{
                      public float moveSpeed;
    public float3 targetDirection;
    
    
}
