//Author: Amber Voskamp, StickyLock
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Mathematics;

/// <summary>
/// System that makes the player move in the right direction with the right speed and rotation
/// </summary>

public partial class MoveSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;
        Entities.ForEach((ref Translation translation, in MoveData movement) => {
            if(!movement.targetDirection.Equals (float3.zero))
            {
                var direction = math.normalize(movement.targetDirection);
                translation.Value += direction * movement.moveSpeed * deltaTime;
            }
        }).Schedule();
    }
}
