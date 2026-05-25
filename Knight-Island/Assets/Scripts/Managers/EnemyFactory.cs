using System.Collections.Generic;
using Characters;
using UnityEngine;
using Utils;

namespace Managers
{
    public class EnemyFactory : Singleton<EnemyFactory>
    {
        [SerializeField] private Health enemyBase;
        private readonly List<Health> enemies = new ();
        private readonly List<Health> deadEnemies = new ();
        
        public static int EnemyCount => Instance.enemies.Count;
        
        public static bool TryInstantiateEnemy(Vector2 position, out Health newEnemy)
        {
            EnemyFactory instance = Instance;
            if (!Instantiate(instance.enemyBase.gameObject, position, Quaternion.identity)
                    .TryGetComponent(out newEnemy)) return false;
            
            instance.enemies.Add(newEnemy);
            Health localHealth = newEnemy;
            
            newEnemy.onHealthReachZero.AddListener(() =>
            {
                instance.enemies.Remove(localHealth);
                instance.deadEnemies.Add(localHealth);
            });
            
            return true;
        }

        public void ClearDeadEnemies()
        {
            foreach (Health enemy in deadEnemies)
            {
                Destroy(enemy.gameObject);
            }
            deadEnemies.Clear();
        }
    }
}