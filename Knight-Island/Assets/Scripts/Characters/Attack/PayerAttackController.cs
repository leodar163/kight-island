using System;
using Characters.Movements;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Characters.Attack
{
    public class PayerAttackController : CharacterAttackController
    {
        private PlayerInputs _playerInputs;
        
        protected override void Awake()
        {
            base.Awake();
            _playerInputs = new PlayerInputs();
        }

        private void OnEnable()
        {
            if (_playerInputs != null)
            {
                _playerInputs.Actions.Attack.canceled += PlayerAttack;
                _playerInputs.Enable();
            }
        }

        private void OnDisable()
        {
            if (_playerInputs != null)
            {
                _playerInputs.Actions.Attack.canceled -= PlayerAttack;
                _playerInputs.Enable();
            }
        }

        private void PlayerAttack(InputAction.CallbackContext context)
        {
            if (!context.canceled) return;
            Attack();
        }
    }
}