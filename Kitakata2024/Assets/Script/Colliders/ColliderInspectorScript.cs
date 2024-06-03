using System;
using UnityEngine;
using UnityEditor;
using SakeShooterSystems;

// 必要なかったクラス
namespace SakeShooterEditor
{
    [CustomEditor(typeof(ICollisionSystem))]
    public class ColliderInspectorScript : Editor
    {
        private ICollisionSystem _colliderSystem = null;
        private Color _gizmoColor = Color.red; // default color
        private bool _showGizmo = false;
        private void OnEnable()
        {
            _colliderSystem = target as ICollisionSystem;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            // Add a color field to the inspector
            EditorGUI.BeginChangeCheck();
            _gizmoColor = EditorGUILayout.ColorField("コライダーの色", _gizmoColor);

            if(GUILayout.Button("コライダーの表示"))
            {
                _showGizmo = true;
                _colliderSystem.OnDrawGizmos();
            }
            
            if (EditorGUI.EndChangeCheck() && _showGizmo)
            {
                _colliderSystem.GizmoColor = _gizmoColor;
                _colliderSystem.OnDrawGizmos();
            }
        }
    }

}