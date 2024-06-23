using System;
using UnityEngine;
using UnityEditor;
using SakeShooterSystems;

// 必要なかったクラス
namespace SakeShooterEditor
{
    [CustomEditor(typeof(ICollider))]
    public class ColliderInspectorScript : Editor
    {
        private ICollider _collider = null;
        private Color _gizmoColor = Color.red; // default color
        private bool _showGizmo = false;

        private void OnEnable()
        {
            _collider = target as ICollider;
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
                _collider.OnDrawGizmos();
            }
            
            if (EditorGUI.EndChangeCheck() && _showGizmo)
            {
                _collider.OnDrawGizmos();
            }
        }
    }
}