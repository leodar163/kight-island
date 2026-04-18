using System;
using Characters.Movements;
using Managers;
using UnityEngine;

namespace Characters
{
    [RequireComponent(typeof(CharacterMovement))]
    public class EnemyTargetController : MonoBehaviour
    {
        [SerializeField][HideInInspector] private EnemyMovementController movementController;
        [SerializeField][Min(0)] private float playerTargetingDistance = 5f;

        private static GameObject _player;
        private static BuildingManager _buildingManager;


        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, playerTargetingDistance);
        }

        private void OnValidate()
        {
            if (movementController == null) TryGetComponent(out movementController);
        }

        private void Awake()
        {
            if (_player == null) _player = PlayerManager.Player;
            if (_buildingManager == null) _buildingManager = BuildingManager.Instance;
        }

        private void Update()
        {
            if (playerTargetingDistance > 0 && TryTargetOnPlayer()) return;
            if (TryTargetOnBuildings()) return;
            movementController.Target = null;
        }

        private bool TryTargetOnPlayer()
        {
            if (_player == null) return false;
            if (!(Vector2.Distance(_player.transform.position, transform.position) < playerTargetingDistance))
                return false;
            
            movementController.Target = _player.transform;
            return true;
        }

        private bool TryTargetOnBuildings()
        {
            Transform closest = null;
            float closestDistance = float.MaxValue;
            
            foreach (GameObject building in _buildingManager.Buildings)
            {
                float distance = Vector2.Distance(building.transform.position, transform.position);
                if (!(distance < closestDistance)) continue;
                
                closest = building.transform;
                closestDistance = distance;
            }

            if (closest == null) return false;
            
            movementController.Target = closest;
            return true;
        }
    }
}