namespace Asteroids.Model
{
    public struct AsteroidComponent
    {
        public bool CanBreakIntoSmallParts;

        public AsteroidComponent(bool canBreakIntoSmallParts)
        {
            CanBreakIntoSmallParts = canBreakIntoSmallParts;
        }
    }
}