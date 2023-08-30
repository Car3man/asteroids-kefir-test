namespace Asteroids.Model
{
    public struct SpaceshipComponent
    {
        public int LaserCharges;
        public float LaserCooldownTimeDown;

        public SpaceshipComponent(int laserCharges)
        {
            LaserCharges = laserCharges;
            LaserCooldownTimeDown = 0f;
        }
    }
}