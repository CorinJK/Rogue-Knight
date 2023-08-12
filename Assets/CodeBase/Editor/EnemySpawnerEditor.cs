using CodeBase.Logic;
using UnityEditor;
using UnityEngine;

namespace CodeBase.Editor
{
    [CustomEditor(typeof(EnemySpawner))]                        // Для какого компоненты эдитор
    public class EnemySpawnerEditor : UnityEditor.Editor        // Что это кастомный эдитор
    {
        // Спецаильная стрка с опциями: рисовать для активных, чтобы можно было выделять объект и игнорировать выделенность объектов
        [DrawGizmo(GizmoType.Active | GizmoType.Pickable | GizmoType.NonSelected)]
        public static void RenderCustomGizmo(EnemySpawner spawner, GizmoType gizmo)
        {
            // Каким цветом рисовать
            Gizmos.color = Color.red;

            // Что рисовать: позиция, радиус
            Gizmos.DrawSphere(spawner.transform.position, 0.5f);
        }
    }
}