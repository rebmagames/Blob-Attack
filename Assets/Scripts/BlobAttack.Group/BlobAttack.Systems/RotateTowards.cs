//Author: Amber Voskamp, StickyLock
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

/// <summary>
/// System that makes the player rotate in the right direction with the right speed
/// </summary>

public partial class RotateTowards : SystemBase
{
    protected override void OnUpdate()
    { 
        Entities.ForEach((ref  Rotation rotation, in MoveData movement, in RotateData rotateData) => 
        {
            //check if input is not zero
            if (!movement.targetDirection.Equals(float3.zero))
            {
                //lerp current rotation to new input direction
                var direction = math.normalize(movement.targetDirection);
                quaternion targetRotation = quaternion.LookRotationSafe(direction, math.up());
                rotation.Value = math.slerp(rotation.Value, targetRotation, rotateData.rotateSpeed);
            }
            
        }).Schedule();
    }
}
