using System;
using System.Collections;
using UnityEngine;

namespace CameraSystem.Test
{
    public class TestCamShake : MonoBehaviour
    {
        [SerializeField] private CameraShake cameraShake;

        private void Start()
        {
            StartCoroutine(ShakeCoroutine());
        }

        private IEnumerator ShakeCoroutine()
        {
            cameraShake.ShakeCamera(1, 20);
            yield return new WaitForSeconds(1);
            cameraShake.ShakeCamera(4, 1);
        }
    }
}