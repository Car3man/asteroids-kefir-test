namespace Asteroids.ECS
{
    public interface IComponentPool
    {
        void AllocateComponent();
        void RemoveComponent(int entity);
    }
}