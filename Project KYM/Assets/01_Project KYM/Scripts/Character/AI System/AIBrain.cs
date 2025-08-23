using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KYM
{
    public class AIBrain : MonoBehaviour
    {
        public AIController AIController => controller; // AIController를 외부에서 접근할 수 있도록 프로퍼티로 노출합니다.
        public AISensor AISensor => sensor; // AISensor를 외부에서 접근할 수 있도록 프로퍼티로 노출합니다.

        [Header("State Componenets")]
        [SerializeField] private AIStateBase[] states;  // (인스펙터에서 설정해야함) AIStateBase를 상속받은 상태들을 저장할 배열입니다. 
        [SerializeField] private AIStateBase defaultState;

        [Header("AI State")]
        [SerializeField] private AIStateBase currentState;

        [Header("Third Party")]
        [SerializeField] private AISensor sensor;
        [SerializeField] private AIController controller;
        [SerializeField] private CharacterBase aiCharacter;

        private Dictionary<AIStateType, AIStateBase> stateMap = new(); // AIStateType를 키로 하고 AIStateBase를 값으로 가지는 딕셔너리입니다.

        private void Awake()
        {
            sensor = GetComponentInChildren<AISensor>();
            controller = GetComponent<AIController>();
            aiCharacter = GetComponent<CharacterBase>();

            foreach (var state in states) // states 배열에 있는 각 상태를 딕셔너리에 추가합니다.
            {
                if (stateMap.ContainsKey(state.StateType) == false)
                {
                    stateMap.Add(state.StateType, state);
                }
            }
        }
        private void Start()
        {
            ChangeState(defaultState);
            sensor.OnDetectedCharacterEvent += OnCallbackDetectedCharacter; // sensor에서 OnDetectedCharacter 이벤트가 발생하면 CallbackDetectedCharacter 메서드를 호출합니다.
            sensor.OnLostCharacterEvent += OnCallbackLostCharacter; // sensor에서 OnLostCharacter 이벤트가 발생하면 CallbackLostCharacter 메서드를 호출합니다.

            aiCharacter.OnCharacterMoribund += OnCallbackCharacterMoribund; // aiCharacter에서 OnCharacterMoribund 이벤트가 발생하면 OnCallbackCharacterMoribund 메서드를 호출합니다.
        }

        private void Update()
        {
            currentState?.onUpdateState(this); // currentstate가 null이 아니라면 onUpdateState를 호출합니다.
        }

        private void OnDestroy()
        {
            if (sensor != null)
            {
                sensor.OnDetectedCharacterEvent -= OnCallbackDetectedCharacter; // sensor에서 OnDetectedCharacter 이벤트 구독을 해제합니다.
                sensor.OnLostCharacterEvent -= OnCallbackLostCharacter; // sensor에서 OnLostCharacter 이벤트 구독을 해제합니다.
            }
        }

        private void ChangeState(AIStateBase newState) 
        {
            if (currentState == newState) return; // 현재 상태와 새 상태가 같으면 아무 작업도 하지 않습니다.

            currentState?.onExitState(this);
            currentState = newState;
            currentState.onEnterState(this);
        }
        private void OnCallbackDetectedCharacter(CharacterBase character)
        {
            if (character.gameObject.CompareTag("Player"))
            {
                // TODO : ChangeState => To CombatState
                if (stateMap.TryGetValue(AIStateType.Combat, out AIStateBase combatState) && currentState.StateType != AIStateType.Abort) // AbortState가 아닐 때만 CombatState로 전환 
                {
                    ChangeState(combatState);
                }
            }
        }
        private void OnCallbackLostCharacter(CharacterBase character)
        {
            if (character.gameObject.CompareTag("Player")) 
            {
                // TODO : ChangeState => To PatrolState
                if (stateMap.TryGetValue(AIStateType.Patrol, out AIStateBase patrolState) && currentState.StateType == AIStateType.Combat)  // CombatState에서 타겟을 놓쳤을 때만 PatrolState로 전환 
                {
                    ChangeState(patrolState);
                }
            }
        }
        private void OnCallbackCharacterMoribund(CharacterBase aiCharacter)
        {
            // TODO : ChangeState => To AbortState
            if (stateMap.TryGetValue(AIStateType.Abort, out AIStateBase abortState))
            {
                ChangeState(abortState);
                Debug.Log("Switched to Abort State.");
            }
        }
    }
}
