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
            
            rb.linearVelocity += direction * (acceleration * Time.fixedDeltaTime);

            if (rb.linearVelocity.magnitude > maxSpeed)
            {
                rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
            }
        }
    }
}
