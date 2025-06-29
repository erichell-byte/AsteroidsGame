using System;
using UnityEngine;
using Zenject;

namespace GameSystem
{
    public class Timer : ITickable
    {
        private float duration;
        private float remainingTime;
        private bool isPaused;

        public Action TimerIsExpired;
        public Action<float> RemainingTimeChanged;

        
        [Inject]
        private void Construct(TickableManager tickableManager)
        {
            tickableManager.Add(this);
        }

        public void Init(float duration)
        {
            this.duration = duration;
        }

        public void Play()
        {
            remainingTime = duration;
            isPaused = false;
        }

        public bool IsPlaying()
        {
            return remainingTime > 0;
        }

        public void Pause()
        {
            isPaused = true;
        }
        
        public void Resume()
        {
            isPaused = false;
        }
        
        public class Factory : PlaceholderFactory<Timer>
        {
        }

        public void Tick()
        {
            if (remainingTime > 0 && isPaused == false)
            {
                remainingTime -= Time.deltaTime;
                RemainingTimeChanged?.Invoke(remainingTime);

                if (remainingTime <= 0f)
                {
                    remainingTime = 0f;
                    TimerIsExpired?.Invoke();
                }
            }
        }
    }
}