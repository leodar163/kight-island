using System;
using UnityEngine;

namespace Characters
{
    [RequireComponent(typeof(CharacterMovement))]
    public class PlayerMovementController : MonoBehaviour
    {
        private CharacterMovement _charaMovement;
        private PlayerInputs _playerInputs;

        private bool _isDead;
        public bool IsDead => _isDead;  
        
        public void SetDead(bool state) 
        {
            _isDead = state;
            if (_isDead) _charaMovement.Direction = Vector2.zero;
        }
        
        public void SetDeadTrue() => SetDead(true);
        
        public Vector2 CurrentDirection => _isDead ? Vector2.zero : _playerInputs.Movements.Direction.ReadValue<Vector2>();

        private void OnValidate()
        {
            if(_charaMovement == null) TryGetComponent(out _charaMovement);
        }

        private void Start()
        {
            _playerInputs = new PlayerInputs();
            _playerInputs.Enable();
        }

        private void FixedUpdate()
        {
            if (_isDead)
            {
                _charaMovement.Direction = Vector2.zero;
                return;
            }

            Vector2 direction = _playerInputs.Movements.Direction.ReadValue<Vector2>();
            _charaMovement.Direction = direction;
        }
    }
}