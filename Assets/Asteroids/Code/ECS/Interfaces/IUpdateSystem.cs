namespace Asteroids.ECS
{
    public interface IUpdateSystem : ISystem
    {
        void OnUpdate(World world, float deltaTime);
    }
}