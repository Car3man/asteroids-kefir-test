using System;
using System.Collections.Generic;

namespace Asteroids.ECS
{
    public class World
    {
        internal readonly List<ISystem> Systems;
        internal readonly List<IUpdateSystem> UpdateSystems;
        internal readonly List<IFixedUpdateSystem> FixedUpdateSystems;
        internal readonly Dictionary<Type, IComponentPool> ComponentPools;
        internal readonly List<bool> Entities;
        public readonly EntityManager EntityManager;
        internal Dictionary<Type, ISharedDataContainer> SharedDataContainers;
        public bool IsRunning{ get; private set; }

        public World()
        {
            Systems = new List<ISystem>();
            UpdateSystems = new List<IUpdateSystem>();
            FixedUpdateSystems = new List<IFixedUpdateSystem>();
            ComponentPools = new Dictionary<Type, IComponentPool>();
            Entities = new List<bool>();
            EntityManager = new EntityManager(this);
            SharedDataContainers = new Dictionary<Type, ISharedDataContainer>();
        }

        public void Start()
        {
            if (!IsRunning)
            {
                IsRunning = true;
                
                for (int i = 0, count = Systems.Count; i < count; i++)
                {
                    var system = Systems[i];
                    system.OnStart(this);
                }
            }
        }

        public void Stop(bool clearSharedData)
        {
            if (IsRunning)
            {
                for (int i = 0, count = Systems.Count; i < count; i++)
                {
                    var system = Systems[i];
                    system.OnStop(this);
                }
                
                for (var entity = 0; entity < Entities.Count; entity++)
                {
                    if (Entities[entity])
                    {
                        EntityManager.DestroyEntity(entity);
                    }
                }

                if (clearSharedData)
                {
                    foreach (var sharedDataContainer in SharedDataContainers.Values)
                    {
                        sharedDataContainer.Reset();
                    }
                }

                IsRunning = false;
            }
        }

        public void Restart(bool clearSharedData)
        {
            Stop(clearSharedData);
            Start();
        }

        public void Update(float deltaTime)
        {
            if (IsRunning)
            {
                for (int i = 0, count = UpdateSystems.Count; i < count; i++)
                {
                    var system = UpdateSystems[i];
                    system.OnUpdate(this, deltaTime);
                }
            }
        }
        
        public void FixedUpdate(float deltaTime)
        {
            if (IsRunning)
            {
                for (int i = 0, count = FixedUpdateSystems.Count; i < count; i++)
                {
                    var system = FixedUpdateSystems[i];
                    system.OnFixedUpdate(this, deltaTime);
                }
            }
        }
        
        public void RegisterComponent<T>() where T : struct
        {
            ComponentPools[typeof(T)] = new ComponentPool<T>();
        }
        
        public void RegisterSharedData<T>() where T : struct
        {
            SharedDataContainers[typeof(T)] = new SharedDataContainer<T>();
        }
        
        public void RegisterSystem<T>(T system) where T : ISystem
        {
            Systems.Add(system);

            if (system is IUpdateSystem updateSystem)
            {
                UpdateSystems.Add(updateSystem);
            }

            if (system is IFixedUpdateSystem fixedUpdateSystem)
            {
                FixedUpdateSystems.Add(fixedUpdateSystem);
            }
        }
        
        public ref T GetSharedData<T>() where T : struct
        {
            var container = (SharedDataContainer<T>) SharedDataContainers[typeof(T)];
            return ref container.Get();
        }
        
        public void SetSharedData<T>(ref T component) where T : struct
        {
            var container = (SharedDataContainer<T>) SharedDataContainers[typeof(T)];
            container.Set(ref component);
        }
    }
}