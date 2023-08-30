namespace Asteroids.Model.SharedData
{
    public struct AsteroidConfigData
    {
        public float MinScale;
        public float MaxScale;
        public float MinInitialVelocity;
        public float MaxInitialVelocity;
        public float Drag;
        public int CollisionLayer;
        public int CollisionMask;
        public int ScoreReward;
        public float SpawnEverySeconds;
    }
}