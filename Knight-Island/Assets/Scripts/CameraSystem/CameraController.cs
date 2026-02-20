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

        public void SetNoise(float amplitude, float frequency)
        {
            _noise.AmplitudeGain = amplitude;
            _noise.FrequencyGain = frequency;
        }
    }
}
