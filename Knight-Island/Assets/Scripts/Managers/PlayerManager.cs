using System;
using Characters;
using UnityEngine;
using Utils;

namespace Managers
{
    public class PlayerManager :  Singleton<PlayerManager>
    {
        [SerializeField] private Health player;
        private Health playerHealth;
        public static Health Player => Instance.playerHealth;

        private void Awake()
        {
            if (player != null) player.TryGetComponent(out playerHealth);
        }

        public void RestorePlayer()
        {
            if (playerHealth != null)
            {
                playerHealth.Heal();
            }
        }
    }
}