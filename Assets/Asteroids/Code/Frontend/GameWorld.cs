using Asteroids.ECS;
using Asteroids.Model;
using UnityEngine;

namespace Asteroids.Frontend
{
    public class GameWorld
    {
        private readonly IAudioService _audioService;
        private readonly IUiService _uiService;

        public World EcsWorld { get; private set; }

        public GameWorld(
            IAudioService audioService,
            IUiService uiService)
        {
            _audioService = audioService;
            _uiService = uiService;

            EcsWorld = CreateWorld();
        }

        public void Start()
        {
            var newGameState = new GameStateData();
            EcsWorld.SetSharedData(ref newGameState);
            EcsWorld.Start();
        }

        public void Update()
        {
            EcsWorld.Update(Time.deltaTime);
        }

        public void FixedUpdate()
        {
            EcsWorld.FixedUpdate(Time.fixedDeltaTime);
        }

        public void Restart()
        {
            Stop();
            Start();
        }

        public void Stop()
        {
            EcsWorld.Stop(false);
        }
        
        private World CreateWorld()
        {
            var world = new World();
            RegisterComponents(world);
            RegisterSharedData(world);
            RegisterSystems(world);

            var gameConfig = GetGameConfig();
            world.SetSharedData(ref gameConfig);
            return world;
        }

        private void RegisterComponents(World world)
        {
            world.RegisterComponent<PhysicsBodyComponent>();
            world.RegisterComponent<PhysicsCollisionComponent>();
            world.RegisterComponent<PhysicsRaycastComponent>();
            world.RegisterComponent<TransformComponent>();
            world.RegisterComponent<SpaceshipComponent>();
            world.RegisterComponent<SpaceshipInputComponent>();
            world.RegisterComponent<UfoComponent>();
            world.RegisterComponent<AsteroidComponent>();
            world.RegisterComponent<BulletComponent>();
            world.RegisterComponent<LaserComponent>();
        }
        
        private void RegisterSharedData(World world)
        {
            world.RegisterSharedData<GameConfigData>();
            world.RegisterSharedData<GameStateData>();
        }

        private void RegisterSystems(World world)
        {
            world.RegisterSystem(new PhysicsSystem());
            
            world.RegisterSystem(new SpaceshipSpawnSystem());
            world.RegisterSystem(new SpaceshipMoveSystem());
            world.RegisterSystem(new SpaceshipBulletShootSystem(_audioService));
            world.RegisterSystem(new SpaceshipLaserShootSystem(_audioService));
            world.RegisterSystem(new SpaceshipCollisionSystem(_uiService));
            world.RegisterSystem(new SpaceshipTeleportSystem());
            world.RegisterSystem(new AsteroidSpawnSystem());
            world.RegisterSystem(new AsteroidDestroySystem());
            world.RegisterSystem(new UfoSpawnSystem());
            world.RegisterSystem(new UfoFollowSystem());
            world.RegisterSystem(new UfoDestroySystem());
            world.RegisterSystem(new BulletDestroySystem());
            world.RegisterSystem(new OcclusionSystem());

            world.RegisterSystem(new PlayerInputSystem());
            world.RegisterSystem(new RenderSystem());
        }

        private GameConfigData GetGameConfig()
        {
            return new GameConfigData
            {
                Spaceship = new SpaceshipConfigData
                {
                    MoveSpeed = 3f,
                    RotateSpeed = 160f,
                    LaserCapacity = 3,
                    LaserCooldown = 5f,
                    Scale = 0.8f,
                    Drag = 0.5f,
                    CollisionLayer = (int)CollisionLayer.Spaceship,
                    CollisionMask = (int)CollisionLayer.Asteroid | (int)CollisionLayer.Ufo
                },
                Bullet = new BulletConfigData
                {
                    Scale = 0.15f,
                    InitialVelocity = 20f,
                    Drag = 0.5f,
                    CollisionLayer = (int)CollisionLayer.Bullet,
                    CollisionMask = (int)CollisionLayer.Asteroid | (int)CollisionLayer.Ufo
                },
                Laser = new LaserConfigData
                {
                    Lifetime = 0.1f,
                    Length = 100f,
                    CollisionMask = (int)CollisionLayer.Asteroid | (int)CollisionLayer.Ufo
                },
                Asteroid = new AsteroidConfigData
                {
                    MinScale = 1f, MaxScale = 1.5f,
                    MinInitialVelocity = 0.05f, MaxInitialVelocity = 0.1f,
                    Drag = 0f,
                    CollisionLayer = (int)CollisionLayer.Asteroid,
                    CollisionMask = (int)CollisionLayer.Bullet | (int)CollisionLayer.Spaceship,
                    ScoreReward = 10,
                    SpawnEverySeconds = 3f
                },
                Ufo = new UfoConfigData
                {
                    MoveSpeed = 2f,
                    Scale = 1f,
                    Drag = 1f,
                    CollisionLayer = (int)CollisionLayer.Ufo,
                    CollisionMask = (int)CollisionLayer.Bullet | (int)CollisionLayer.Spaceship,
                    ScoreReward = 30,
                    SpawnEverySeconds = 3f
                },
                WorldWidth = CameraUtility.GetCameraViewWidth(),
                WorldHeight = CameraUtility.GetCameraViewHeight(),
                OcclusionDistance = 15f
            };
        }
    }
}