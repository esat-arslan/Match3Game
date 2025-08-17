using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
    public class InputReader : MonoBehaviour {
        PlayerInput playerInput;
        InputAction selectAction;
        InputAction attackAction;

        // NOTE: Make sure to set the Player Input component to fire C# events
        public event Action Attack;

        public Vector2 Selected => selectAction.ReadValue<Vector2>();
        
        void Start() {
            playerInput = GetComponent<PlayerInput>();
            selectAction = playerInput.actions["Select"];
            attackAction = playerInput.actions["Attack"];
            
            attackAction.performed += OnAttack;
        }
        
        void OnDestroy() {
            attackAction.performed -= OnAttack;
        }

        void OnAttack(InputAction.CallbackContext obj) => Attack?.Invoke();
    }
