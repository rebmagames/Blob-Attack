//Author: Amber Voskamp, StickyLock
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

/// <summary>
/// Spawn bullet of enemy at random.
/// Give bullet translation of enemy, rotation to player, movedata, enemybullettag and URPMaterialPropertyBaseColor
/// </summary>
public partial class EnemyShooting : SystemBase
{

    //TODO Stickylock (amber): Change from player to enemy
    private Entity _bulletPrefab;
    private Unity.Mathematics.Random _random;
    private BeginSimulationEntityCommandBufferSystem _ecbSystem;

    private BuildPhysicsWorld _buildPhysicsWorld;
    private CollisionWorld _collisionWorld;

    protected override void OnCreate()
    {
        _buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
    }

    protected override void OnStartRunning()
    {
        _bulletPrefab = GetSingleton<BulletPrefab>().Value;
        _ecbSystem = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
    }

    protected override void OnUpdate()
    {         
            var ecb = _ecbSystem.CreateCommandBuffer();
            float3 playerPos = GetComponent<Translation>(GetSingletonEntity<Player>()).Value;
            var bulletPrefab = _bulletPrefab;

            _collisionWorld = _buildPhysicsWorld.PhysicsWorld.CollisionWorld;

            Entities.WithAll<EnemyTag>().ForEach((in Translation trans) =>
            {
                var random = UnityEngine.Random.Range(1, 10);
                int time = 5;

                if(time == random)
                {
                var newBullet = ecb.Instantiate(bulletPrefab);

                float3 direction = playerPos - trans.Value;
                direction.y = 0f;
                direction = math.normalize(direction);
                
                ecb.AddComponent(newBullet, new MoveData
                {
                    moveSpeed = 6,
                    targetDirection = direction
                });

                ecb.AddComponent(newBullet, new Translation
                {
                    Value = trans.Value
                });

                quaternion targetRotation = quaternion.LookRotationSafe(playerPos, math.up());

                ecb.AddComponent(newBullet, new Rotation
                {
                    Value = targetRotation
                });

                ecb.AddComponent(newBullet, new EnemyBulletTag { });

                    ecb.AddComponent(newBullet, new URPMaterialPropertyBaseColor { 
                    Value = new float4(0f, 255f, 0f, 0f)
                    });

                }


            }).Run();
    }
}
