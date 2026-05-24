using System;
using System.Collections.Generic;
using Characters;
using UnityEngine;
using Utils;

namespace Managers
{
    public class BuildingManager : Singleton<BuildingManager>
    {
        [SerializeField] private List<GameObject> buildings;
        public List<GameObject> Buildings => new (buildings);

        private void Awake()
        {
            foreach (GameObject building in buildings)
            {
                if (building.TryGetComponent(out Health health))
                {
                    health.onHealthReachZero.AddListener(() =>
                    {
                        buildings.Remove(building);
                    });
                }
            }
        }
        
        public void RestoreBuildings()
        {
            foreach (GameObject building in buildings)
            {
                if (building.TryGetComponent(out Health health))
                {
                    health.Heal();
                }
            }
        }
    }
}