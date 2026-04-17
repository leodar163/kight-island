using System;
using UnityEngine;
using Characters;
using CameraSystem;

namespace CameraSystem 
{
    public class CameraDamageFeedback : MonoBehaviour 
    {
        [SerializeField] private Health health;
        [SerializeField] private CameraShake cameraShake; 

        private void OnEnable() 
        {
            health.onDamageTaken.AddListener(Shake);
        }

        private void OnDisable()
        {
            health.onDamageTaken.RemoveListener(Shake);
        }

        private void Shake() 
        {
            cameraShake?.ShakeCamera(2.0f, 0.2f); 
            
            print("[FEEDBACK] Dégâts détectés : Tremblement caméra lancé.");
        }
    }
}