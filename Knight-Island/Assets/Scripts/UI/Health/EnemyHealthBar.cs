using System;
using UnityEngine;

namespace UI.Health
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class EnemyHealthBar : MonoBehaviour
    {
        [SerializeField] [HideInInspector] private SpriteRenderer spriteRenderer;
        [SerializeField] private Characters.Health enemyHealth;
        [SerializeField] private Color barColo = new Color32(172, 50, 50, 255);

        private float _initialWidth;
        private Transform _barTransform;

        private void OnValidate()
        {
            if (spriteRenderer == null) TryGetComponent(out spriteRenderer);
            else
            {
                spriteRenderer.color = barColo;
            }
        }

        private void Start()
        {
            _barTransform = spriteRenderer.gameObject.transform;
            _initialWidth = _barTransform.localScale.x;
        }

        private void Update()
        {
            SetBarSize();

            spriteRenderer.enabled = !Mathf.Approximately(enemyHealth.CurrentHealth, enemyHealth.MaxHealth);
        }

        private void SetBarSize()
        {
            float ratio = enemyHealth.CurrentHealth / enemyHealth.MaxHealth;
            
            _barTransform.localScale = new Vector3(_initialWidth * ratio, _barTransform.localScale.y, 1);
        }
    }
}