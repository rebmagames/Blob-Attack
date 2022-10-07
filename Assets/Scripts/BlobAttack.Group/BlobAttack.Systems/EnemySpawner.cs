//Author: Amber Voskamp, StickyLock
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

/// <summary>
/// Spawn 5 enemies when N is pressed
/// </summary>
public partial class EnemySpawner : SystemBase
{
    private Entity _enemyPrefab;
    private float3 _minPos = new float3(-13, 1, -7);
    private float3 _maxPos = new float3(13, 1, 7);
    private Unity.Mathematics.Random _random;

    private BeginSimulationEntityCommandBufferSystem _ecbSystem;

    protected override void OnStartRunning()
    {
        _enemyPrefab = GetSingleton<enemyPrefab>().Value;

        _random.InitState(454);
        _ecbSystem = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();

        SpawnEnemyWave();
    }

    protected override void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            SpawnEnemyWave();
        }
    }

    private void SpawnEnemyWave()
    {
        var ecb = _ecbSystem.CreateCommandBuffer();
        for (int i = 0; i < 5; i++)
        {

            var newEnemy = ecb.Instantiate(_enemyPrefab);

            //Let the enemies spawn on random positions
            var randPos = _random.NextFloat3(_minPos, _maxPos);
            var newPos = new Translation
            {
                Value = randPos
            };
            ecb.SetComponent(newEnemy, newPos);

            ecb.AddComponent(newEnemy, new MoveData
            {
                moveSpeed = 3
            });
        }
    }
}
