using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

namespace CameraSystem
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private CameraShake shake;
        private CinemachineBasicMultiChannelPerlin _noise;
        
        private void Awake()
        {
            var cam = GetComponent<CinemachineCamera>();
            _noise = cam.GetCinemachineComponent(CinemachineCore.Stage.Noise)
                as CinemachineBasicMultiChannelPerlin;
        }
        
        public void StartShake(float intensity, float duration)
        {
            shake.ShakeCamera(intensity, duration);
        }
        
        private void Start()
        {
            StartCoroutine(TestShake());
        }
        
        private IEnumerator TestShake()
        {
            SetNoise(5f, 1f); // gros shake lent
            yield return new WaitForSeconds(10f);
            
            SetNoise(1f, 10f); // shake rapide
            yield return new WaitForSeconds(1f);

            SetNoise(0f, 0f); // stop
        }
        
        public void SetNoise(float amplitude, float frequency)
        {
            _noise.AmplitudeGain = amplitude;
            _noise.FrequencyGain = frequency;
        }
    }
}
