using System;
using System.Collections.Generic;
using Managers;
using UnityEngine;

namespace UI.Health
{
    public class BuildingsHealthBar : MonoBehaviour
    {
        [SerializeField] private RectTransform healthBarFill;
        [SerializeField] private RectTransform healthBarBackground;
        private List<Characters.Health> buildingHealths;
        private float currentHealth;
        private float maxHealth;
        
        void Awake()
        {
            buildingHealths = BuildingManager.Instance.GetBuildingsHealth();
            ResetMaxHealth();
        }

        // Update is called once per frame
        void Update()
        {
            GetBuildingHealth();
            ResizeHealthBar();
        }

        private void ResizeHealthBar()
        {
            float ratio = currentHealth / maxHealth;
            healthBarFill.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, healthBarBackground.rect.width * ratio);
        }

        private void GetBuildingHealth()
        {
            currentHealth = 0;
            foreach (Characters.Health health in buildingHealths)
            {
                currentHealth += health.CurrentHealth;
            }
        }

        public void ResetMaxHealth()
        {
            maxHealth = 0;
            foreach (Characters.Health health in buildingHealths)
            {
                maxHealth +=  health.MaxHealth;
            }
        }
    }
}
