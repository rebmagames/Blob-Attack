//Author: Amber Voskamp, StickyLock
using Unity.Entities;
using UnityEngine;

/// <summary>
/// The data to move horizontal or vertical 
/// 
/// </summary>

[GenerateAuthoringComponent]
public struct RawInputData : IComponentData
{
    [HideInInspector] public float inputH;  //horizontal input
    [HideInInspector] public float inputV;  //vertical input
}
