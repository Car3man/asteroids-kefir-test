namespace Asteroids.ECS
{
    public class SharedDataContainer<T> : ISharedDataContainer where T : struct
    {
        private readonly Component<T>[] _components = new Component<T>[1];
        
        public ref T Get()
        {
            ref var component = ref _components[0];
            return ref component.Value;
        }

        public void Set(ref T data)
        {
            ref var component = ref _components[0];
            component.Exists = true;
            component.Value = data;
        }

        public void Remove()
        {
            ref var component = ref _components[0];
            component.Exists = false;
            component.Value = default;
        }

        public void Reset()
        {
            Remove();
        }
    }
}