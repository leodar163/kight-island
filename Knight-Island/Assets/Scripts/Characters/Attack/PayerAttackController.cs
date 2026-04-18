using System;
using Characters.Movements;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Characters.Attack
{
    public class PayerAttackController : MonoBehaviour
    {
        private static readonly int AttackTrigger = Animator.StringToHash("Attack");
        [SerializeField] private Animator animator;
        [SerializeField] private CharacterMovement charaMovement;

        [Header("Attack zones")] [SerializeField]
        private GameObject azFront;

        [SerializeField] private GameObject azBack;
        [SerializeField] private GameObject azLeft;
        [SerializeField] private GameObject azRight;

        private bool _isAttacking;

        public bool IsAttacking => _isAttacking;

        public UnityEvent onAttacking = new();
        public UnityEvent onAttackEnded = new();

        private PlayerInputs _playerInputs;


        private void Awake()
        {
            _playerInputs = new PlayerInputs();
            UntriggerAttackZone();
        }

        private void OnEnable()
        {
            if (_playerInputs != null)
            {
                _playerInputs.Actions.Attack.canceled += Attack;
                _playerInputs.Enable();
            }
        }

        private void OnDisable()
        {
            if (_playerInputs != null)
            {
                _playerInputs.Actions.Attack.canceled -= Attack;
                _playerInputs.Enable();
            }
        }

        private void Attack(InputAction.CallbackContext context)
        {
            if (!context.canceled || _isAttacking) return;

            _isAttacking = true;
            animator.SetTrigger(AttackTrigger);
            charaMovement.CanMove = false;
            onAttacking.Invoke();
        }

        public void TriggerAttackZone()
        {
            if (Mathf.Abs(charaMovement.FacingDirection.x) > Mathf.Abs(charaMovement.FacingDirection.y))
            {
                if (charaMovement.FacingDirection.x > 0) azRight.SetActive(true);
                else azLeft.SetActive(true);
            }
            else
            {
                if (charaMovement.FacingDirection.y > 0) azBack.SetActive(true);
                else azFront.SetActive(true);
            }
        }

        public void UntriggerAttackZone()
        {
            azFront.SetActive(false);
            azBack.SetActive(false);
            azLeft.SetActive(false);
            azRight.SetActive(false);
        }

        public void CancelAttack()
        {
            _isAttacking = false;
            charaMovement.CanMove = true;
            onAttackEnded.Invoke();
        }
    }
}