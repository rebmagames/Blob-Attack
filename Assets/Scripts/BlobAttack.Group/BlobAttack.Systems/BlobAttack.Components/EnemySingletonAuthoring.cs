//Author: Amber Voskamp, StickyLock
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

/// <summary>
/// convert prefab to entity
/// declare referenced prefab to enemy
/// 
/// </summary>

[DisallowMultipleComponent]
public class EnemySingletonAuthoring : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
{
    public GameObject enemy;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponentData(entity, new enemyPrefab
        {
            Value = conversionSystem.GetPrimaryEntity(enemy)
        });   
    }

    public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
    {
        referencedPrefabs.Add(enemy);
    }
}

