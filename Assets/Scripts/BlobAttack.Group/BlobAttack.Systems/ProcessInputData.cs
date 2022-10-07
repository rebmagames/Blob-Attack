//Author: Amber Voskamp, StickyLock
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

/// <summary>
/// Processing the horizontal and vertical data to movement
/// </summary>

public partial class ProcessInputData : SystemBase
{
    protected override void OnUpdate()
    {
        float inputH = Input.GetAxis("Horizontal");
        float inputV = Input.GetAxis("Vertical");

        Entities.ForEach((ref RawInputData input, ref MoveData movement) =>
        {
            //set input data 
            input.inputH = inputH;
            input.inputV = inputV;

            //set direction data
            movement.targetDirection = new Unity.Mathematics.float3(input.inputH, 0, input.inputV);
        }).Schedule();
    }
}
