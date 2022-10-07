//Author: Amber Voskamp, StickyLock
using Unity.Entities;

/// <summary>
/// Data of the rotate speed of the player
/// 
/// </summary>

[GenerateAuthoringComponent]
public struct RotateData : IComponentData
{
    public float rotateSpeed;
}
