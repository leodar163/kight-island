using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

namespace CameraSystem
{
    public class CameraShake : MonoBehaviour
    {
        private IEnumerator _shakeCoroutine;
        [SerializeField] private CinemachineBasicMultiChannelPerlin noise;
        
        public void ShakeCamera(float intensity, float duration)
        {
            if (_shakeCoroutine != null) StopCoroutine(_shakeCoroutine);
            _shakeCoroutine = ShakeCoroutine(intensity, duration);
            StartCoroutine(_shakeCoroutine);
        }

        private IEnumerator ShakeCoroutine(float intensity, float duration)
        {
            noise.AmplitudeGain = intensity;
            yield return new WaitForSeconds(duration);
            noise.AmplitudeGain = 0;
            _shakeCoroutine = null;
        }
    }
}
