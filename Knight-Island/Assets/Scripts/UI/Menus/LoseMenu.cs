using System;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Menus
{
    public class LoseMenu : MonoBehaviour
    {
        [SerializeField] private Button restartButton;
        [SerializeField] private Button quiteButton;
        [SerializeField] private TextMeshProUGUI deathContextText;
        [SerializeField] private RectTransform root;

        
        private void Awake()
        {
            root.gameObject.SetActive(false);
            
            WinLoseManager.Instance.onLose.AddListener(Open);
            
            restartButton.onClick.AddListener(RestartLevel);
            quiteButton.onClick.AddListener(Quite);
        }

        public void Open(LoseContext context)
        {
            Time.timeScale = 0;
            deathContextText.SetText(context.Explain());
            root.gameObject.SetActive(true);
        }

        private void RestartLevel()
        {
            Time.timeScale = 1;
            LevelManager.RestartLevel();
        }

        private void Quite()
        {
            Time.timeScale = 1;
            LevelManager.Quite();
        }
        
    }
}