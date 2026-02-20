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
            if (Keyboard.current.fKey.wasPressedThisFrame)
            {
                playerHealth.TakeDamage(10f);
                Debug.Log($"Dégâts ! Vie restante : {playerHealth.CurrentHealth}");
            }

            if (Keyboard.current.hKey.wasPressedThisFrame)
            {
                playerHealth.Heal(10f);
                Debug.Log($"Soin ! Vie actuelle : {playerHealth.CurrentHealth}");            }
        }
    }
}