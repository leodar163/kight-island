using System.Collections.Generic;
using UnityEngine;

namespace WaveSystem
{
    [CreateAssetMenu(fileName = "WaveSequence", menuName = "WaveSystem/WaveSequence")]
    public class WaveSequence : ScriptableObject
    {
        [SerializeField] private List<WaveData> waves;
        public List<WaveData> Waves =>  waves;
    }
}