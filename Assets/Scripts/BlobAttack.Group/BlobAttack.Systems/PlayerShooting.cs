//Author: Amber Voskamp, StickyLock
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;
using UnityEngine;

/// <summary>
/// Spawn bullet of player when spacebar is hit.
/// Give bullet translation of player, rotation to mouse cursor and movedata
/// </summary>

public partial class PlayerShooting : SystemBase
{
    private Entity _bulletPrefab;
    private Unity.Mathematics.Random _random;
    private BeginSimulationEntityCommandBufferSystem _ecbSystem;

    private Camera _mainCamera;
    private BuildPhysicsWorld _buildPhysicsWorld;
    private CollisionWorld _collisionWorld;

    protected override void OnCreate()
    {
        _mainCamera = Camera.main;
        _buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
    }

    protected override void OnStartRunning()
    {
        Application.targetFrameRate = 30;
        _bulletPrefab = GetSingleton<BulletPrefab>().Value;

        _random.InitState(454);
        _ecbSystem = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
    }

    protected override void OnUpdate()
    {         
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CreateNewBullet();
        }

        void CreateNewBullet()
        {
            var ecb = _ecbSystem.CreateCommandBuffer();

            var newBullet = ecb.Instantiate(_bulletPrefab);

            _collisionWorld = _buildPhysicsWorld.PhysicsWorld.CollisionWorld;
            var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            float3 rayEnd = ray.GetPoint(10f);
            float3 playerPos = GetComponent<Translation>(GetSingletonEntity<Player>()).Value;
            float3 rayTemp = new float3(rayEnd.x, playerPos.y, rayEnd.z);
            float3 direction = rayTemp - playerPos;

            ecb.AddComponent(newBullet, new MoveData
            {
                moveSpeed = 6,
                targetDirection = direction  
            });

            ecb.AddComponent(newBullet, new Translation
            {
                Value = playerPos
            });

            quaternion targetRotation = quaternion.LookRotationSafe(rayEnd, math.up());

            ecb.AddComponent(newBullet, new Rotation
            {
                Value = targetRotation
            }) ;

            ecb.AddComponent(newBullet, new PlayerBulletTag { });
        }
    }
}
