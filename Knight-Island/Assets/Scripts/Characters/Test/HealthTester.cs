using UnityEngine;
using UnityEngine.InputSystem;

namespace Characters.Test
{
    public class HealthTester : MonoBehaviour
    {
        [SerializeField] private Health health;

        private void Awake()
        {
            if (!health) 
            {
                Debug.LogWarning($"[TEST] PlayerHealth manquant sur {gameObject.name}. Script désactivé.");
                enabled = false;
            }
        }

        private void Update()
        {
            if (Keyboard.current.gKey.wasPressedThisFrame)
            {
                health.TakeDamage(50f);
                print($"[TEST] Dégâts appliqués. Vie : {health.CurrentHealth}");
            }

            if (Keyboard.current.hKey.wasPressedThisFrame)
            {
                health.Heal(10f);
                print($"[TEST] Soin appliqué. Vie : {health.CurrentHealth}");
            }
        }
    }
}