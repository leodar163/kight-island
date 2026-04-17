using UnityEngine;
using UnityEngine.InputSystem;

namespace Characters.Test
{
    public class HealthTester : MonoBehaviour
    {
        [SerializeField] private CharacterHealth characterHealth;

        private void Awake()
        {
            if (!characterHealth) 
            {
                Debug.LogWarning($"[TEST] PlayerHealth manquant sur {gameObject.name}. Script désactivé.");
                enabled = false;
            }
        }

        private void Update()
        {
            if (Keyboard.current.gKey.wasPressedThisFrame)
            {
                characterHealth.TakeDamage(50f);
                print($"[TEST] Dégâts appliqués. Vie : {characterHealth.CurrentHealth}");
            }

            if (Keyboard.current.hKey.wasPressedThisFrame)
            {
                characterHealth.Heal(10f);
                print($"[TEST] Soin appliqué. Vie : {characterHealth.CurrentHealth}");
            }
        }
    }
}