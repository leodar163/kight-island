using System.Collections;
using Characters.Movements;
using UnityEngine;
using UnityEngine.Events;

namespace Characters.Attack
{
    public abstract class CharacterAttackController : MonoBehaviour
    {
        private static readonly int AttackTrigger = Animator.StringToHash("Attack");
        [SerializeField] private Animator animator;
        [SerializeField] private CharacterMovement charaMovement;
        [SerializeField] private float attackCooldown = 1;

        private IEnumerator _attackCooldownRoutine;

        [Header("Attack zones")] [SerializeField]
        private GameObject azFront;

        [SerializeField] private GameObject azBack;
        [SerializeField] private GameObject azLeft;
        [SerializeField] private GameObject azRight;

        private bool _isAttacking;
        public bool canAttack = true;

        public bool IsAttacking => _isAttacking;

        [Space]
        public UnityEvent onAttacking = new();
        public UnityEvent onAttackEnded = new();
        
        protected virtual void Awake()
        {
            UntriggerAttackZone();
        }
        
        protected void Attack()
        {
            if (!canAttack) return;

            canAttack = false;
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
            onAttackEnded.Invoke();
            charaMovement.CanMove = true;
            StartAttackCooldown();
        }

        private void StartAttackCooldown()
        {
            if (_attackCooldownRoutine != null) 
                StopCoroutine(_attackCooldownRoutine);

            _attackCooldownRoutine = AttackCooldownRoutine();
            StartCoroutine(_attackCooldownRoutine);
        }

        private IEnumerator AttackCooldownRoutine()
        {
            yield return new WaitForSeconds(attackCooldown);
            canAttack = true;
        }
    }
}