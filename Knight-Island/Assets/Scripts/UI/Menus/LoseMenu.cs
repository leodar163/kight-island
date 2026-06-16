using Managers;
using TMPro;
using UnityEngine;

namespace UI.Menus
{
    public class LoseMenu : Menu
    {
        [SerializeField] private TextMeshProUGUI deathContextText;


        protected override void Awake()
        {
            base.Awake();
            
            WinLoseManager.Instance.onLose.AddListener(Open);
        }

        public void Open(LoseContext context)
        {
            deathContextText.SetText(context.Explain());
            base.Open();
        }
    }
}