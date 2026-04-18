using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Managers
{
    public class BuildingManager : Singleton<BuildingManager>
    {
        [SerializeField] private List<GameObject> buildings;
        public List<GameObject> Buildings => new (buildings);
    }
}