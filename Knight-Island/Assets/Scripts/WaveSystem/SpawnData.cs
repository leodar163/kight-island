using System;
using UnityEngine;

namespace WaveSystem
{
    [Serializable]
    public class SpawnData
    {
        [SerializeField] public int nbrToSpawn;
        [SerializeField] [Min(0)] public int spawnPoint;
        [SerializeField] [Min(0)] public float spawnTime; 
    }
}