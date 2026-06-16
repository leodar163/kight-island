using System;
using TMPro;
using UnityEngine;

namespace WaveSystem.UI
{
    public class WaveCounter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;
        
        private void Update()
        {
            text.SetText($"Vague {WaveManager.Instance.WaveIndex}");
        }
    }
}