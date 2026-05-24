using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace WaveSystem
{
    public class WaveTrigger : MonoBehaviour
    {
        [SerializeField] private float triggerProgressionSpeed = 0.2f;
        
        private PlayerInputs _playerInputs;
        private bool isTriggering;
        private float triggerProgression;
        
        private void Awake()
        {
            _playerInputs = new PlayerInputs();
        }

        private void OnEnable()
        {
            if (_playerInputs != null)
            {
                _playerInputs.Actions.TriggerWave.started += EnableProgression;
                _playerInputs.Actions.TriggerWave.canceled += DisableProgression;
                _playerInputs.Enable();
            }
        }

        private void OnDisable()
        {
            if (_playerInputs != null)
            {
                _playerInputs.Actions.TriggerWave.started -= EnableProgression;
                _playerInputs.Actions.TriggerWave.canceled -= DisableProgression;
                _playerInputs.Disable();
            }
        }

        private void EnableProgression(InputAction.CallbackContext context)
        {
            if (!context.started) return;
            isTriggering = true;
        }

        private void DisableProgression(InputAction.CallbackContext context)
        {
            if (!context.canceled) return;
            isTriggering = false;
        }

        private void Update()
        {
            if (isTriggering) 
                triggerProgression += triggerProgressionSpeed * Time.deltaTime;
            else 
                triggerProgression -= triggerProgressionSpeed * Time.deltaTime * 2;
            
            triggerProgression = Mathf.Clamp(triggerProgression, 0f, 1f);
            
            if (triggerProgression >= 1f)
                WaveManager.Instance.TriggerNextWave();
                
        }
    }
}