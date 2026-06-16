using Managers;
using TMPro;
using UnityEngine;

namespace UI.Menus
{
    public class WinMenu : Menu
    {
        [SerializeField] private TextMeshProUGUI batCountText;
            
        protected override void Awake()
        {
            base.Awake();
            
            WinLoseManager.Instance.onWin.AddListener(Open);
        }

        public override void Open()
        {
            batCountText.SetText($"il reste {BuildingManager.Instance.Buildings.Count} bâtiments debouts");
            base.Open();
        }
    }
}