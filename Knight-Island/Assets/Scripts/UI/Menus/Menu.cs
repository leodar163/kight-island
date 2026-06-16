using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Menus
{
    public abstract class Menu : MonoBehaviour
    {
        [SerializeField] private Button restartButton;
        [SerializeField] private Button quiteButton;
        [SerializeField] private RectTransform root;
        
        protected virtual void Awake()
        {
            root.gameObject.SetActive(false);
            
            restartButton.onClick.AddListener(RestartLevel);
            quiteButton.onClick.AddListener(Quite);
        }

        public virtual void Open()
        {
            Time.timeScale = 0;
            root.gameObject.SetActive(true);
        }
        
        protected void RestartLevel()
        {
            Time.timeScale = 1;
            LevelManager.RestartLevel();
        }

        protected void Quite()
        {
            Time.timeScale = 1;
            LevelManager.Quite();
        }
    }
}