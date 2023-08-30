using System;
using System.Numerics;

namespace Asteroids.Model.Utility
{
    public static class MathUtility
    {
        public const float Deg2Rad = (float)(Math.PI * 2 / 360f);

        public static Vector2 RotateVector(this Vector2 vector, float angle)
        {
            var angleRads = Deg2Rad * angle;
            var cosAngle = (float)Math.Cos(angleRads);
            var sinAngle = (float)Math.Sin(angleRads);
            return new Vector2(
                cosAngle * vector.X - sinAngle * vector.Y,
                sinAngle * vector.X + cosAngle * vector.Y
            );
        }

        public static bool IsCircleIntersectCircle(Vector2 circlePositionA, float circleRadiusA, Vector2 circlePositionB, float circleRadiusB)
        {
            var dx = circlePositionA.X - circlePositionB.X;
            var dy = circlePositionA.Y - circlePositionB.Y;
            var distance = Math.Sqrt(dx * dx + dy * dy);
            return distance < circleRadiusA + circleRadiusB;
        }

        public static bool IsLineIntersectCircle(Vector2 lineStart, Vector2 lineEnd, Vector2 circlePosition, float circleRadius)
        {
            var d = lineEnd - lineStart;
            var f = lineStart - circlePosition;
            var a = Vector2.Dot(d, d);
            var b = 2 * Vector2.Dot(f, d);
            var c = Vector2.Dot(f, f) - circleRadius * circleRadius;

            var discriminant = b * b - 4 * a * c;
            if (discriminant < 0)
            {
                return false;
            }
            
            discriminant = (float)Math.Sqrt(discriminant);
            
            var t1 = (-b - discriminant) / (2 * a);
            var t2 = (-b + discriminant) / (2 * a);
            
            if (t1 is >= 0 and <= 1)
            {
                return true;
            }
            
            if (t2 is >= 0 and <= 1)
            {
                return true;
            }
            
            return false;
        }
    }
}