using UnityEngine;
using Characters;
using CameraSystem;

namespace CameraSystem 
{
    public class CameraDamageFeedback : MonoBehaviour 
    {
        [SerializeField] private CharacterHealth characterHealth;
        [SerializeField] private CameraShake cameraShake; 

        private void Start() 
        {
            characterHealth.OnDamageTaken += Shake;
        }

        private void Shake() 
        {
            cameraShake?.ShakeCamera(2.0f, 0.2f); 
            
            print("[FEEDBACK] Dégâts détectés : Tremblement caméra lancé.");
        }

        private void OnDestroy() 
        {
            if (characterHealth != null) 
                characterHealth.OnDamageTaken -= Shake;
        }
    }
}