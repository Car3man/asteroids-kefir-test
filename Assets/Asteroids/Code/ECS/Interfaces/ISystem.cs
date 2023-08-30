namespace Asteroids.ECS
{
    public interface ISystem
    {
        void OnStart(World world);
        void OnStop(World world);
    }
}