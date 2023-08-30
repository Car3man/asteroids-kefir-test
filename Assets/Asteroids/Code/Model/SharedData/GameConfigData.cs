namespace Asteroids.Model.SharedData
{
    public struct GameConfigData
    {
        public SpaceshipConfigData Spaceship;
        public BulletConfigData Bullet;
        public LaserConfigData Laser;
        public AsteroidConfigData Asteroid;
        public UfoConfigData Ufo;
        public float WorldWidth;
        public float WorldHeight;
        public float OcclusionDistance;
    }
}