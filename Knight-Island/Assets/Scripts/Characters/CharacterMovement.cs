using UnityEngine;

namespace Characters
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class CharacterMovement : MonoBehaviour
    {
        [HideInInspector] [SerializeField] private Rigidbody2D rb;
        [SerializeField] private float maxSpeed;
        [SerializeField] private float acceleration;
        [SerializeField] private float deceleration;

        private Vector2 _direction;

        public Vector2 Direction
        {
            get => _direction;
            set => _direction = value;
        }

        private void OnValidate()
        {
            if (rb == null) TryGetComponent(out rb);
        }

        private void FixedUpdate()
        {
            Accelerate(_direction);

            ClampSpeed();
            
            Decelerate(_direction);
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
                float decelerationX = velocityDirection.x * deceleration * Time.fixedDeltaTime;

                decelerationX = velocityDirection.x switch
                {
                    > 0 => decelerationX > rb.linearVelocityX ? rb.linearVelocityX : decelerationX,
                    < 0 => decelerationX < rb.linearVelocityX ? rb.linearVelocityX : decelerationX,
                    _ => decelerationX
                };

                rb.linearVelocityX -= decelerationX;
            }

            if (velocityDirection.y != 0 && inputDeltaY <= Mathf.Abs(rb.linearVelocityY))
            {
                float decelerationY = velocityDirection.y * deceleration * Time.fixedDeltaTime;

                decelerationY = velocityDirection.y switch
                {
                    > 0 => decelerationY > rb.linearVelocityY ? rb.linearVelocityY : decelerationY,
                    < 0 => decelerationY < rb.linearVelocityY ? rb.linearVelocityY : decelerationY,
                    _ => decelerationY
                };

                rb.linearVelocityY -= decelerationY;
            }
        }
    }
}