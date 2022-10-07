//Author: Amber Voskamp, StickyLock
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;

/// <summary>
/// All the trigger events. 
/// </summary>

//TODO stickylock (amber): Let player die

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateAfter(typeof(StepPhysicsWorld))]
[UpdateBefore(typeof(ExportPhysicsWorld))]
public partial class TriggerSystem : SystemBase
{
    private BeginSimulationEntityCommandBufferSystem _ecbSystem;
    private StepPhysicsWorld stepPhysicsWorld;

    protected override void OnCreate()
    {
        _ecbSystem = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
        stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
    }

    protected override void OnStartRunning()
    {
        this.RegisterPhysicsRuntimeSystemReadOnly();
    }

    protected override void OnUpdate()
    {
        var commandBuffer = _ecbSystem.CreateCommandBuffer();

        var triggerJob = new TriggerJob
        {
            allEnemies = GetComponentDataFromEntity<EnemyTag>(true),
            allWals = GetComponentDataFromEntity<WallTag>(),
            player = GetComponentDataFromEntity<Player>(),
            
            allBullets = GetComponentDataFromEntity<Bullet>(),
            allEnemyBullets = GetComponentDataFromEntity<EnemyBulletTag>(),
            allPlayerBullets = GetComponentDataFromEntity<PlayerBulletTag>(),
            ecb = commandBuffer
        };

        Dependency = triggerJob.Schedule(stepPhysicsWorld.Simulation, Dependency);
        _ecbSystem.AddJobHandleForProducer(Dependency);
    }

    [BurstCompile]
    struct TriggerJob : ITriggerEventsJob
    {
        [ReadOnly] public ComponentDataFromEntity<EnemyTag> allEnemies;
        [ReadOnly] public ComponentDataFromEntity<WallTag> allWals;
        [ReadOnly] public ComponentDataFromEntity<Player> player;

        [ReadOnly] public ComponentDataFromEntity<Bullet> allBullets;
        [ReadOnly] public ComponentDataFromEntity<EnemyBulletTag> allEnemyBullets;
        [ReadOnly] public ComponentDataFromEntity<PlayerBulletTag> allPlayerBullets;
        public EntityCommandBuffer ecb;

        public void Execute(TriggerEvent triggerEvent)
        {
            Entity entityA = triggerEvent.EntityA;
            Entity entityB = triggerEvent.EntityB;

            //if enemy hit enemy do nothing
            if (allEnemies.HasComponent(entityA) && allEnemies.HasComponent(entityB)) return;

            //if enemy hit PlayerBullet delete enemy and bullet
            if (allEnemies.HasComponent(entityA) && allPlayerBullets.HasComponent(entityB))
            {
                ecb.DestroyEntity(entityA);
                ecb.DestroyEntity(entityB);
            }
            //if PlayerBullet hit enemy delete bullet and enemy
            if (allEnemies.HasComponent(entityB) && allPlayerBullets.HasComponent(entityA))
            {
                ecb.DestroyEntity(entityB);
                ecb.DestroyEntity(entityA);
            }

            //if bullet hit wall delete bullet
            if (allBullets.HasComponent(entityA) && allWals.HasComponent(entityB))
            {
                ecb.DestroyEntity(entityA);
            }
            //if wall hit bullet delete bullet
            if (allBullets.HasComponent(entityB) && allWals.HasComponent(entityA))
            {
                ecb.DestroyEntity(entityB);
            }
        }
    }
}