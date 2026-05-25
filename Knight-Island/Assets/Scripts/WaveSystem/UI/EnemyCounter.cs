using System;
using Managers;
using TMPro;
using UnityEngine;

namespace WaveSystem.UI
{
    public class EnemyCounter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;

        private void Update()
        {
            text.SetText($"{EnemyFactory.EnemyCount} ennemis restants");
        }
    }
}