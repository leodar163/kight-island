using System;
using TMPro;
using UnityEngine;
using Utils;

namespace UI.Health
{
    public class ResurrectionChronoUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private Chrono resurrectionChrono;

        private void Update()
        {
            if (resurrectionChrono.OnGoing)
            {
                text.gameObject.SetActive(true);
                text.SetText($"Resurrection dans {(int)resurrectionChrono.RemainingTime} " +
                             $"seconde{(resurrectionChrono.RemainingTime >= 2 ? "s" : "")}");
            }
            else
            {
                text.gameObject.SetActive(false);
            }
        }
    }
}