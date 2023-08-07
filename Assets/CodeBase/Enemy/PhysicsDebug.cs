using UnityEngine;

namespace CodeBase.Enemy
{
    public static class PhysicsDebug
    {
        // Рисует дебажные лучи: из центра сферы по размеру радиуса, красные, весит нест сек
        public static void DrawDebug(UnityEngine.Vector3 worldPos, float radius, float seconds)
        {
            Debug.DrawRay(worldPos, radius * Vector3.up, Color.red, seconds);
            Debug.DrawRay(worldPos, radius * Vector3.down, Color.red, seconds);
            Debug.DrawRay(worldPos, radius * Vector3.left, Color.red, seconds);
            Debug.DrawRay(worldPos, radius * Vector3.right, Color.red, seconds);
            Debug.DrawRay(worldPos, radius * Vector3.forward, Color.red, seconds);
            Debug.DrawRay(worldPos, radius * Vector3.back, Color.red, seconds);
        }
    }
}