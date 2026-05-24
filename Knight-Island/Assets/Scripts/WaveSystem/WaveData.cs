using System;
using System.Collections.Generic;
using UnityEngine;

namespace WaveSystem
{
    [Serializable]
    public class WaveData
    { 
        [SerializeField] public List<SpawnData> spawns = new ();
    }
}
