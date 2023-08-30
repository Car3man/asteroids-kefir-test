namespace Asteroids.ECS
{
    public interface IFixedUpdateSystem : ISystem
    {
        void OnFixedUpdate(World world, float deltaTime);
    }
}