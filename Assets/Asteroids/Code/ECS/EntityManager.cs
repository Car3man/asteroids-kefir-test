namespace Asteroids.ECS
{
    public class EntityManager
    {
        private readonly World _world;

        public EntityManager(World world)
        {
            _world = world;
        }

        public int CreateEntity()
        {
            var id = 0;
            var count = _world.Entities.Count;

            for (; id < count; id++)
            {
                if (!_world.Entities[id])
                {
                    _world.Entities[id] = true;
                    return id;
                }
            }

            id = count;
            _world.Entities.Add(true);

            foreach (var pool in _world.ComponentPools.Values)
            {
                pool.AllocateComponent();
            }

            return id;
        }

        public void DestroyEntity(int entity)
        {
            _world.Entities[entity] = false;
            
            foreach (var pool in _world.ComponentPools.Values)
            {
                pool.RemoveComponent(entity);
            }
        }
        
        public int GetSingleEntity<T1>() where T1 : struct
        {
            var poolT1 = (ComponentPool<T1>) _world.ComponentPools[typeof(T1)];
            foreach (var entity in Query())
            {
                if (poolT1.HasComponent(entity))
                {
                    return entity;
                }
            }
            return -1;
        }
        
        public int GetSingleEntity<T1, T2>() 
            where T1 : struct
            where T2 : struct
        {
            var poolT1 = (ComponentPool<T1>) _world.ComponentPools[typeof(T1)];
            var poolT2 = (ComponentPool<T2>) _world.ComponentPools[typeof(T2)];
            foreach (var entity in Query())
            {
                if (poolT1.HasComponent(entity) &&
                    poolT2.HasComponent(entity))
                {
                    return entity;
                }
            }
            return -1;
        }
        
        public bool HasComponent<T>() where T : struct
        {
            var pool = (ComponentPool<T>) _world.ComponentPools[typeof(T)];
            foreach (var entity in Query())
            {
                if (pool.HasComponent(entity))
                {
                    return true;
                }
            }
            return false;
        }
        
        public bool HasComponent<T>(int entity) where T : struct
        {
            var pool = (ComponentPool<T>) _world.ComponentPools[typeof(T)];
            return pool.HasComponent(entity);
        }

        public ref T GetComponent<T>(int entity) where T : struct
        {
            var pool = (ComponentPool<T>) _world.ComponentPools[typeof(T)];
            return ref pool.GetComponent(entity);
        }
        
        public void SetComponent<T>(int entity, ref T component) where T : struct
        {
            var pool = (ComponentPool<T>) _world.ComponentPools[typeof(T)];
            pool.SetComponent(entity, ref component);
        }

        public void RemoveComponent<T>(int entity) where T : struct
        {
            var pool = (ComponentPool<T>) _world.ComponentPools[typeof(T)];
            pool.RemoveComponent(entity);
        }
        
        public EntityEnumerable Query()
        {
            return new EntityEnumerable(_world);
        }
        
        public EntityEnumerable<T1> Query<T1>()
            where T1 : struct
        {
            return new EntityEnumerable<T1>(_world);
        }
        
        public EntityEnumerable<T1, T2> Query<T1, T2>()
            where T1 : struct
            where T2 : struct
        {
            return new EntityEnumerable<T1, T2>(_world);
        }
        
        public EntityEnumerable<T1, T2, T3> Query<T1, T2, T3>()
            where T1 : struct
            where T2 : struct
            where T3 : struct
        {
            return new EntityEnumerable<T1, T2, T3>(_world);
        }
        
        public EntityEnumerable<T1, T2, T3, T4> Query<T1, T2, T3, T4>()
            where T1 : struct
            where T2 : struct
            where T3 : struct
            where T4 : struct
        {
            return new EntityEnumerable<T1, T2, T3, T4>(_world);
        }
    }
}