using System;
using UnityEngine;

namespace Characters
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement : MonoBehaviour
    {
        [HideInInspector][SerializeField] private Rigidbody2D rb;
        [SerializeField] private float maxSpeed;
        [SerializeField] private float acceleration;
        [SerializeField] private float deceleration;
        private PlayerInputs _playerInputs;
        
        private void OnValidate()
        {
            if(rb == null) TryGetComponent(out rb);
        }

        private void Start()
        {
            _playerInputs = new PlayerInputs();
            _playerInputs.Enable();
        }

        private void FixedUpdate()
        {
            Vector2 direction = _playerInputs.Movements.Direction.ReadValue<Vector2>();
            
            Accelerate(direction);

            ClampSpeed();
            
            Decelerate(direction);
        }

        private void Accelerate(Vector2 direction)
        {
            rb.linearVelocity += direction * (acceleration * Time.fixedDeltaTime);
        }

        private void ClampSpeed()
        {
            if (rb.linearVelocity.magnitude > maxSpeed)
            {
                rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
            }
        }

        private void Decelerate(Vector2 direction)
        {
            Vector2 velocityDirection = rb.linearVelocity.normalized;

            float inputDeltaX = Mathf.Abs(rb.linearVelocityX + direction.x);
            float inputDeltaY = Mathf.Abs(rb.linearVelocityY + direction.y);

            
            if (velocityDirection.x != 0 && inputDeltaX <= Mathf.Abs(rb.linearVelocityX))
            {
                rb.linearVelocityX -= velocityDirection.x * deceleration * Time.fixedDeltaTime;
            }

            if (velocityDirection.y != 0 && inputDeltaY <= Mathf.Abs(rb.linearVelocityY))
            {
                rb.linearVelocityY -= velocityDirection.y * deceleration * Time.fixedDeltaTime;
            }
        }
    }
}
