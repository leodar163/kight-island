using UnityEngine;

namespace Characters
{
    public class PlayerAttackHitbox : MonoBehaviour
    {
        [SerializeField] private int damageAmount = 25;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out CharacterHealth enemyHealth))
            {
                Debug.Log("L'épée a touché : " + other.name);
                enemyHealth.TakeDamage(damageAmount);
            }
        }
    }
}