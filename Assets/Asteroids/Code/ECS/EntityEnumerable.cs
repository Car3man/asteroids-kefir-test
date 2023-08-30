using System.Collections;
using System.Collections.Generic;

namespace Asteroids.ECS
{
    public class EntityEnumerable : IEnumerable<int>
    {
        private readonly EntityEnumerator _enumerator;

        public EntityEnumerable(World world)
        {
            _enumerator = new EntityEnumerator(world);
        }
        
        public IEnumerator<int> GetEnumerator()
        {
            return _enumerator;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
    
    public class EntityEnumerable<T1> : IEnumerable<int>
        where T1 : struct
    {
        private readonly EntityEnumerator _enumerator;

        public EntityEnumerable(World world)
        {
            _enumerator = new EntityEnumerator<T1>(world);
        }
        
        public IEnumerator<int> GetEnumerator()
        {
            return _enumerator;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
    
    public class EntityEnumerable<T1, T2> : IEnumerable<int>
        where T1 : struct
        where T2 : struct
    {
        private readonly EntityEnumerator _enumerator;

        public EntityEnumerable(World world)
        {
            _enumerator = new EntityEnumerator<T1, T2>(world);
        }
        
        public IEnumerator<int> GetEnumerator()
        {
            return _enumerator;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
    
    public class EntityEnumerable<T1, T2, T3> : IEnumerable<int>
        where T1 : struct
        where T2 : struct
        where T3 : struct
    {
        private readonly EntityEnumerator _enumerator;

        public EntityEnumerable(World world)
        {
            _enumerator = new EntityEnumerator<T1, T2, T3>(world);
        }
        
        public IEnumerator<int> GetEnumerator()
        {
            return _enumerator;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
    
    public class EntityEnumerable<T1, T2, T3, T4> : IEnumerable<int>
        where T1 : struct
        where T2 : struct
        where T3 : struct
        where T4 : struct
    {
        private readonly EntityEnumerator _enumerator;

        public EntityEnumerable(World world)
        {
            _enumerator = new EntityEnumerator<T1, T2, T3, T4>(world);
        }
        
        public IEnumerator<int> GetEnumerator()
        {
            return _enumerator;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    internal class EntityEnumerator : IEnumerator<int>
    {
        protected readonly World World;
        public int Current { get; protected set; }

        object IEnumerator.Current => Current;

        public EntityEnumerator(World world)
        {
            World = world;
            Reset();
        }

        public bool MoveNext()
        {
            var entitiesCount = World.Entities.Count;
            
            for (int entity = Current + 1; entity < entitiesCount; entity++)
            {
                if (HasComponents(entity))
                {
                    Current = entity;
                    return true;
                }
            }
            
            Reset();
            return false;
        }

        protected virtual bool HasComponents(int entity)
        {
            return true;
        }

        public void Reset()
        {
            Current = -1;
        }

        public void Dispose()
        {
        }
    }

    internal class EntityEnumerator<T1> : EntityEnumerator 
        where T1 : struct
    {
        private readonly ComponentPool<T1> _poolT1;

        public EntityEnumerator(World world) : base(world)
        {
            _poolT1 = (ComponentPool<T1>)World.ComponentPools[typeof(T1)];
        }

        protected override bool HasComponents(int entity)
        {
            return _poolT1.HasComponent(entity);
        }
    }
    
    internal class EntityEnumerator<T1, T2> : EntityEnumerator 
        where T1 : struct 
        where T2 : struct
    {
        private readonly ComponentPool<T1> _poolT1;
        private readonly ComponentPool<T2> _poolT2;
        
        public EntityEnumerator(World world) : base(world)
        {
            _poolT1 = (ComponentPool<T1>)World.ComponentPools[typeof(T1)];
            _poolT2 = (ComponentPool<T2>)World.ComponentPools[typeof(T2)];
        }

        protected override bool HasComponents(int entity)
        {
            return _poolT1.HasComponent(entity) &&
                   _poolT2.HasComponent(entity);
        }
    }
    
    internal class EntityEnumerator<T1, T2, T3> : EntityEnumerator 
        where T1 : struct 
        where T2 : struct
        where T3 : struct
    {
        private readonly ComponentPool<T1> _poolT1;
        private readonly ComponentPool<T2> _poolT2;
        private readonly ComponentPool<T3> _poolT3;
        
        public EntityEnumerator(World world) : base(world)
        {
            _poolT1 = (ComponentPool<T1>)World.ComponentPools[typeof(T1)];
            _poolT2 = (ComponentPool<T2>)World.ComponentPools[typeof(T2)];
            _poolT3 = (ComponentPool<T3>)World.ComponentPools[typeof(T3)];
        }

        protected override bool HasComponents(int entity)
        {
            return _poolT1.HasComponent(entity) &&
                   _poolT2.HasComponent(entity) &&
                   _poolT3.HasComponent(entity);
        }
    }
    
    internal class EntityEnumerator<T1, T2, T3, T4> : EntityEnumerator 
        where T1 : struct 
        where T2 : struct
        where T3 : struct
        where T4 : struct
    {
        private readonly ComponentPool<T1> _poolT1;
        private readonly ComponentPool<T2> _poolT2;
        private readonly ComponentPool<T3> _poolT3;
        private readonly ComponentPool<T4> _poolT4;
        
        public EntityEnumerator(World world) : base(world)
        {
            _poolT1 = (ComponentPool<T1>)World.ComponentPools[typeof(T1)];
            _poolT2 = (ComponentPool<T2>)World.ComponentPools[typeof(T2)];
            _poolT3 = (ComponentPool<T3>)World.ComponentPools[typeof(T3)];
            _poolT4 = (ComponentPool<T4>)World.ComponentPools[typeof(T4)];
        }

        protected override bool HasComponents(int entity)
        {
            return _poolT1.HasComponent(entity) &&
                   _poolT2.HasComponent(entity) &&
                   _poolT3.HasComponent(entity) &&
                   _poolT4.HasComponent(entity);
        }
    }
}