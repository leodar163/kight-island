using UnityEngine;

namespace Characters.Movements
{
    [RequireComponent(typeof(CharacterMovement))]
    public class PlayerMovementController : MonoBehaviour
    {
        private CharacterMovement _charaMovement;
        private PlayerInputs _playerInputs;
        
        public Vector2 CurrentDirection => _playerInputs.Movements.Direction.ReadValue<Vector2>();

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
            Vector2 direction = _playerInputs.Movements.Direction.ReadValue<Vector2>();
            _charaMovement.Direction = direction;
        }
    }
}