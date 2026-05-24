using System;
using UnityEngine;
using UnityEngine.UI;

namespace WaveSystem.UI
{
    public class WaveTriggerBar : MonoBehaviour
    {
        [SerializeField] private RectTransform fill;
        [SerializeField] private RectTransform border;
        [SerializeField] [Range(0, 1)] private float fillAmount = 1; 
        [SerializeField] private WaveTrigger waveTrigger;
        
        private void OnValidate()
        {
            if (fill == null || border == null) return;
            ResizeFill();
        }

        private void Update()
        {
            fillAmount = Mathf.Clamp(waveTrigger.TrirgerProgression, 0 ,1);
            ResizeFill();
        }

        private void ResizeFill()
        {
            fill.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, border.rect.width * fillAmount);
        }
    }
}
