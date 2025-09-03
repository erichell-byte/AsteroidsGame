using System;
using UnityEngine;
using Zenject;

namespace Utils
{
	public class Timer : ITickable
	{
		private float _duration;
		private bool _isPaused;
		private float _remainingTime;
		public Action<float> RemainingTimeChanged;

		public Action TimerIsExpired;

		[Inject]
		private void Construct(TickableManager tickableManager)
		{
			tickableManager.Add(this);
		}

		public void Tick()
		{
			if (_remainingTime > 0 && _isPaused == false)
			{
				_remainingTime -= Time.deltaTime;
				RemainingTimeChanged?.Invoke(_remainingTime);

				if (_remainingTime <= 0f)
				{
					_remainingTime = 0f;
					TimerIsExpired?.Invoke();
				}
			}
		}

		public void Init(float duration)
		{
			_duration = duration;
		}

		public void Play()
		{
			_remainingTime = _duration;
			_isPaused = false;
		}

		public bool IsPlaying()
		{
			return _remainingTime > 0;
		}

		public void Pause()
		{
			_isPaused = true;
		}

		public void Resume()
		{
			_isPaused = false;
		}

		public class Factory : PlaceholderFactory<Timer>
		{
		}
	}
}