using UnityEngine;
using Utils;

namespace Managers
{
    public class PlayerManager :  Singleton<PlayerManager>
    {
        [SerializeField] private GameObject player;
        public static GameObject Player => Instance.player;
    }
}