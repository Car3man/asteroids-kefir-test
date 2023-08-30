using Asteroids.ECS;
using Asteroids.Model;
using UnityEngine;

namespace Asteroids.Frontend
{
    public class RenderSystem : IUpdateSystem
    {
        private readonly Camera _camera = CameraUtility.GetCamera();
        private Mesh _quadMesh;

        public void OnStart(World world)
        {
            _quadMesh = ResourcesUtility.Load<Mesh>("Models/Quad");
        }

        public void OnUpdate(World world, float deltaTime)
        {
            var entityManager = world.EntityManager;
            
            foreach (var entity in entityManager.Query<TransformComponent>())
            {
                ref var transformComponent = ref entityManager.GetComponent<TransformComponent>(entity);
                
                if (entityManager.HasComponent<SpaceshipComponent>(entity))
                {
                    RenderSimpleEntity(ref transformComponent, ResourcesUtility.Load<Material>("Materials/Spaceship"));
                }
                else if (entityManager.HasComponent<BulletComponent>(entity))
                {
                    RenderSimpleEntity(ref transformComponent, ResourcesUtility.Load<Material>("Materials/Bullet"));
                }
                else if (entityManager.HasComponent<AsteroidComponent>(entity))
                {
                    RenderSimpleEntity(ref transformComponent, ResourcesUtility.Load<Material>("Materials/Asteroid"));
                }
                else if (entityManager.HasComponent<UfoComponent>(entity))
                {
                    RenderSimpleEntity(ref transformComponent, ResourcesUtility.Load<Material>("Materials/Ufo"));
                } 
                else if (entityManager.HasComponent<LaserComponent>(entity))
                {
                    RenderLaser(ref transformComponent, ResourcesUtility.Load<Material>("Materials/Laser"));
                }
            }
        }

        private void RenderSimpleEntity(ref TransformComponent transformComponent, Material material)
        {
            var position = new Vector3(transformComponent.Position.X, transformComponent.Position.Y);
            var rotation = Quaternion.Euler(0f, 0f, transformComponent.Angle);
            var scale = Vector3.one * transformComponent.Scale;
            DrawMesh(position, rotation, scale, _quadMesh, material);
        }

        private void RenderLaser(ref TransformComponent transformComponent, Material material)
        {
            const float renderLength = 100f;
            var pivot = new Vector3(transformComponent.Position.X, transformComponent.Position.Y);
            var direction = Vector3.Normalize(RotateVector(new Vector3(0f, 1f), transformComponent.Angle));
            var position = pivot + direction * (renderLength / 2f);
            var rotation = Quaternion.Euler(0f, 0f, transformComponent.Angle);
            var scale = new Vector3(0.05f, renderLength, 1f);
            DrawMesh(position, rotation, scale, _quadMesh, material);
        }

        private void DrawMesh(Vector3 position, Quaternion rotation, Vector3 scale, Mesh mesh, Material material)
        {
            var matrix = Matrix4x4.TRS(position, rotation, scale);
            Graphics.DrawMesh(mesh, matrix, material, 0, _camera);
        }
        
        private Vector3 RotateVector(Vector3 vector, float angle)
        {
            var angleRads = Mathf.Deg2Rad * angle;
            var cosAngle = Mathf.Cos(angleRads);
            var sinAngle = Mathf.Sin(angleRads);
            return new Vector3(
                cosAngle * vector.x - sinAngle * vector.y,
                sinAngle * vector.x + cosAngle * vector.y,
                vector.z
            );
        }

        public void OnStop(World world)
        {
        }
    }
}