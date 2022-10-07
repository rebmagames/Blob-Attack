//Author: Amber Voskamp, StickyLock
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

/// <summary>
/// A system that let enemies walk forward with the movement speed 
/// </summary>

public partial class EnemyMovementSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float3 playerPos = GetComponent<Translation>(GetSingletonEntity<Player>()).Value;

        float deltaTime = Time.DeltaTime;
        Entities.WithAll<EnemyTag>().ForEach(( ref MoveData moveData, in Translation trans) =>
        {
            //Rotate to player
            float3 direction = playerPos - trans.Value;
            direction.y = 0f;
            direction = math.normalize(direction);
            moveData.targetDirection = direction;
        }).Run();
    }
}