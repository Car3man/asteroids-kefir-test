using UnityEngine;

namespace Asteroids.Frontend
{
    public static class CameraUtility
    {
        public static Camera GetCamera()
        {
            return Camera.main;
        }

        public static float GetCameraViewWidth()
        {
            var screenAspect = Screen.width / (float)Screen.height;
            return GetCameraViewHeight() * screenAspect;
        }

        public static float GetCameraViewHeight()
        {
            return GetCamera().orthographicSize * 2f;
        }
    }
}