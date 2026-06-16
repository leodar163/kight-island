using UnityEngine.Events;
using Utils;
using WaveSystem;

namespace Managers
{
    public class WinLoseManager : Singleton<WinLoseManager>
    {
        private BuildingManager buildingManager;
        private WaveManager waveManager;
        public enum State
        {
            ONGOING,
            LOST,
            WON,
        }
        
        private  State state = State.ONGOING;
        public State CurrentState => state;
        
        public UnityEvent onWin;
        public UnityEvent<LoseContext> onLose;
        
        private void Start()
        {
            buildingManager = BuildingManager.Instance;
            waveManager = WaveManager.Instance;
            waveManager.onWaveEnds.AddListener(TryWin);
        }

        private void Update()
        {
            if (state == State.ONGOING && buildingManager.Buildings.Count <= 0)
            {
                Lose();
            }
        }

        public void Lose()
        {
            print("Lose");
            state = State.LOST;
            onLose?.Invoke(new LoseOnLoseBuildings());
        }

        private void TryWin()
        {
            if (waveManager.IsEndOfSequence)
            {
                Win();
            }
        }
        
        public void Win()
        {
            print("Win");
            state = State.WON;
            onWin?.Invoke();
        }
    }

    public class LoseContext
    {
        public virtual string Explain()
        {
            return "Vous avez mal joué";
        } 
    }

    public class LoseOnLoseBuildings : LoseContext
    {
        public override string Explain()
        {
            return "Tous les bâtiments ont été détruits";
        }
    }
}
