using System;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine;

namespace Characters
{
    public class PlayerAttack : MonoBehaviour
    {
        [SerializeField] private float attackDuration = 0.2f;
        
        private PlayerAnimator _playerAnimator;
        private PlayerMovement _playerMovement;
        private bool _isAttacking = false;

        private void Awake()
        {
            _playerAnimator = GetComponentInChildren<PlayerAnimator>();
            _playerMovement = GetComponent<PlayerMovement>();
            
            if (_playerAnimator == null)
                print("PlayerAnimator manquant sur le Player !");
            if (_playerMovement == null)
                print("PlayerMovement manquant sur le Player !");
        }

        private void Update()
        {
            if (_playerMovement.isDead) return;

            bool isMousePressed = Mouse.current.leftButton.isPressed;

            if (isMousePressed && !_isAttacking)
            {
                PerformAttack();
            }
        }

        private void PerformAttack()
        {
            _isAttacking = true;
            _playerMovement.isAttacking = true;

            _playerAnimator.PlayAttackAnimation();

            StartCoroutine(AttackCooldownRoutine());
        }

        private IEnumerator AttackCooldownRoutine()
        {
            yield return new WaitForSeconds(attackDuration);
            
            _isAttacking = false;
            _playerMovement.isAttacking = false;
            print("[ATTACK] Fin de l'attaque, mouvement débloqué.");
        }
    }
}

