using System;
using UnityEngine;
using Characters;
using UnityEngine.InputSystem;

namespace Characters.Test
{
    public class HealthTester : MonoBehaviour
    {
        [SerializeField] private PlayerHealth playerHealth;

        void Update()
        {
            if (Keyboard.current.gKey.wasPressedThisFrame)
            {
                playerHealth.TakeDamage(10f);
                print($"[TEST] Dégâts infligés. Vie actuelle : {playerHealth.CurrentHealth}");
            }

            if (Keyboard.current.hKey.wasPressedThisFrame)
            {
                playerHealth.Heal(10f);
                print($"[TEST] Soin appliqué. Vie actuelle : {playerHealth.CurrentHealth}");
            }
        }
    }
}