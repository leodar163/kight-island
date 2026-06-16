using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace Managers
{
    public class LevelManager : Singleton<LevelManager>
    {
        public static void Quite()
        {
            print("quite");
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();            
#endif
        }

        public static void RestartLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
