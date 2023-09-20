using CodeBase.Logic.EnemySpawners;
using UnityEditor;
using UnityEngine;

namespace CodeBase.Editor
{
    [CustomEditor(typeof(SpawnMarker))]                        // Для какого компоненты эдитор
    public class SpawnMarkerEditor : UnityEditor.Editor        // Что это кастомный эдитор
    {
        // Спецаильная стрка с опциями: рисовать для активных, чтобы можно было выделять объект и игнорировать выделенность объектов
        [DrawGizmo(GizmoType.Active | GizmoType.Pickable | GizmoType.NonSelected)]
        public static void RenderCustomGizmo(SpawnMarker spawner, GizmoType gizmo)
        {
            // Каким цветом рисовать
            Gizmos.color = Color.red;

            // Что рисовать: позиция, радиус
            Gizmos.DrawSphere(spawner.transform.position, 0.5f);
        }
    }
}