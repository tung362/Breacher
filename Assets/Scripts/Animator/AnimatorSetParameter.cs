using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Breacher.Animation
{
    /// <summary>
    /// Set animator parameters within animation states
    /// </summary>
    public class AnimatorSetParameter : StateMachineBehaviour
    {
        /*Enums*/
        public enum TriggerState { OnEnter, OnExit };
        public enum ValueState { Float, Int, Bool, Trigger }

        public string _ParameterName;
        public TriggerState _TriggerType;
        public ValueState _ValueType;
        public float _SetFloat;
        public int _SetInt;
        public bool _SetBool;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (_TriggerType == TriggerState.OnEnter) SetParameter(animator, _ValueType);
        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (_TriggerType == TriggerState.OnExit) SetParameter(animator, _ValueType);
        }

        void SetParameter(Animator animator, ValueState valueState)
        {
            switch (valueState)
            {
                case ValueState.Float:
                    animator.SetFloat(_ParameterName, _SetFloat);
                    break;
                case ValueState.Int:
                    animator.SetInteger(_ParameterName, _SetInt);
                    break;
                case ValueState.Bool:
                    animator.SetBool(_ParameterName, _SetBool);
                    break;
                case ValueState.Trigger:
                    animator.SetTrigger(_ParameterName);
                    break;
                default:
                    break;
            }
        }
    }
}
