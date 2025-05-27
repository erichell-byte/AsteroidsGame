using System;
using Systems;
using UnityEngine;
using Zenject;

namespace GameSystem
{
    public class Timer : IGameUpdateListener
    {
        private GameCycle gameCycle;
        private Rigidbody2D rigibbody;
        
        private float duration;

        private float remainingTime;

        public Action TimerIsExpired;
        public Action<float> RemainingTimeChanged;

        [Inject]
        private void Construct(GameCycle gameCycle)
        {
            this.gameCycle = gameCycle;
        }
        
        public void Init(float duration)
        {
            this.duration = duration;
            gameCycle.AddListener(this);
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
        
        public class Factory : PlaceholderFactory<Timer>
        {
        }
    }
}