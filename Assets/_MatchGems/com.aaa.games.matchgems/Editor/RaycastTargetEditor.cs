using AAA.Games.MatchGems.Runtime;
using System;
using UnityEngine;
using UnityEditor;
using UnityEditor.UI;

namespace AAA.Games.MatchGems.Editor
{
    [CanEditMultipleObjects, CustomEditor(typeof(RaycastTarget))]
    public class RaycastTargetEditor : GraphicEditor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            using (new EditorGUI.DisabledScope(true))
                EditorGUILayout.PropertyField(m_Script);
            RaycastControlsGUI();
            serializedObject.ApplyModifiedProperties();
        }
    }
}