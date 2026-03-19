using UnityEngine;

namespace Characters
{
    public class PlayerAttackHitbox : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Enemy"))
            {
                Debug.Log("L'épée a touché : " + other.name);
                
                // other.GetComponent<EnemyHealth>().TakeDamage(10);
            }
        }
    }
}
