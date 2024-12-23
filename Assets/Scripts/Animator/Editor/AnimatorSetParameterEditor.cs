using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Breacher.Animation
{
    /// <summary>
    /// Custom inspector for AnimatorSetParameter
    /// </summary>
    [CustomEditor(typeof(AnimatorSetParameter))]
    public class AnimatorSetParameterEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            AnimatorSetParameter targetedScript = (AnimatorSetParameter)target;

            targetedScript._ParameterName = EditorGUILayout.TextField("Parameter Name", targetedScript._ParameterName);
            targetedScript._TriggerType = (AnimatorSetParameter.TriggerState)EditorGUILayout.EnumPopup("Trigger Type", targetedScript._TriggerType);
            targetedScript._ValueType = (AnimatorSetParameter.ValueState)EditorGUILayout.EnumPopup("Value Type", targetedScript._ValueType);

            switch (targetedScript._ValueType)
            {
                case AnimatorSetParameter.ValueState.Float:
                    targetedScript._SetFloat = EditorGUILayout.FloatField("Set Float", targetedScript._SetFloat);
                    break;
                case AnimatorSetParameter.ValueState.Int:
                    targetedScript._SetInt = EditorGUILayout.IntField("Set Int", targetedScript._SetInt);
                    break;
                case AnimatorSetParameter.ValueState.Bool:
                    targetedScript._SetBool = EditorGUILayout.Toggle("Set Bool", targetedScript._SetBool);
                    break;
                default:
                    break;
            }
        }
    }
}
