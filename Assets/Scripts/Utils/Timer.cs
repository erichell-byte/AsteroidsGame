using System;
using Systems;

namespace GameSystem
{
    public class Timer : IGameUpdateListener
    {
        private float duration;

        public float remainingTime;

        public Action TimerIsExpired;
        public Action<float> RemainingTimeChanged;

        public Timer(float duration)
        {
            this.duration = duration;
            GameCycle.Instance.AddListener(this);
        }

        public void Play()
        {
            this.remainingTime = this.duration;
        }

        public bool IsPlaying()
        {
            return this.remainingTime > 0;
        }

        public void Tick(float deltaTime)
        {
            if (remainingTime > 0)
            {
                remainingTime -= deltaTime;
                RemainingTimeChanged?.Invoke(remainingTime);
            }
            else
            {
                TimerIsExpired?.Invoke();
            }
        }

        public void OnUpdate(float deltaTime)
        {
            Tick(deltaTime);
        }
    }
}