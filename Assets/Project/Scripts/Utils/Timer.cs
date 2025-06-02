using System;
using UnityEngine;
using Zenject;

namespace GameSystem
{
    public class Timer : ITickable
    {
        private float duration;
        private float remainingTime;

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
        }

        public bool IsPlaying()
        {
            return remainingTime > 0;
        }
        
        public class Factory : PlaceholderFactory<Timer>
        {
        }

        public void Tick()
        {
            if (remainingTime > 0)
            {
                remainingTime -= Time.deltaTime;
                RemainingTimeChanged?.Invoke(remainingTime);
            }
            else
            {
                TimerIsExpired?.Invoke();
            }
        }
    }
}