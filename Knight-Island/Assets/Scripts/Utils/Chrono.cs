using System;
using UnityEngine;
using UnityEngine.Events;

namespace Utils
{
    public class Chrono : MonoBehaviour
    {
        [SerializeField] private float chronoTime;

        private bool onGoing;
        private float startTime;

        public UnityEvent onChronoCanceled;
        public UnityEvent onChronoStarted;
        public UnityEvent onChronoEnded;
        
        public float ChronoTime
        {
            get => chronoTime;
            set => chronoTime = value;
        }
        
        public bool OnGoing => onGoing;
        public float RemainingTime => onGoing ? chronoTime - (Time.time  - startTime) : 0;
        public float OnGoingTime => onGoing ? Time.time - startTime : 0;
        
        // Update is called once per frame
        void Update()
        {
            if (onGoing &&  RemainingTime <= 0)
            {
                StopChrono();
                onChronoEnded?.Invoke();
            }
        }

        public void CancelChrono()
        {
            StopChrono();
            onChronoCanceled?.Invoke();
        }

        public void StartChrono()
        {
            if (onGoing) return;
            
            onGoing = true;
            startTime = Time.time;
            onChronoStarted?.Invoke();
        }

        public void StartChrono(float time)
        {
            chronoTime = time;
            StartChrono();
        }

        public void StopChrono()
        {
            onGoing = false;
            startTime = 0;
        }
    }
}
