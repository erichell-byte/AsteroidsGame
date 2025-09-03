using Character;
using Config;
using Systems;
using UnityEngine;
using Zenject;

namespace Components
{
	public class MoveComponent : MonoBehaviour,
		IGameFinishListener,
		IGamePauseListener,
		ITickable
	{
		private float _maxVelocityMagnitude;
		private float _moveСoefficient;

		private Rigidbody2D _rb2d;
		private RemoteConfig _remoteConfig;
		private float _rotateCoefficient;
		private SpaceshipModel _spaceshipModel;

		[Inject]
		private void Construct(
			GameCycle gameCycle,
			IConfigProvider configProvider)
		{
			_remoteConfig = configProvider.GetRemoteConfig();

			gameCycle.AddListener(this);
		}

		private void Awake()
		{
			_rb2d = GetComponent<Rigidbody2D>();
		}

		public void OnFinishGame()
		{
			_rb2d.linearVelocity = Vector2.zero;
			transform.rotation = Quaternion.identity;
			transform.position = Vector3.zero;
		}

		public void OnPauseGame()
		{
			_rb2d.linearVelocity = Vector2.zero;
		}

		public void Tick()
		{
			if (_rb2d.linearVelocity.magnitude > 0) _spaceshipModel?.SetPosition(transform.position);
		}


		public void Initialize(
			SpaceshipModel spaceshipModel)
		{
			_moveСoefficient = _remoteConfig.MoveCoefficient;
			_rotateCoefficient = _remoteConfig.RotateCoefficient;
			_maxVelocityMagnitude = _remoteConfig.MaxVelocityMagnitude;
			_spaceshipModel = spaceshipModel;
		}

		public void SetInitialPositionAndRotation(Vector3 position, float rotationZ)
		{
			transform.position = position;
			transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
		}

		public void MoveForward()
		{
			_rb2d.linearVelocity += (Vector2)transform.up * (Time.fixedDeltaTime * _moveСoefficient);
			_rb2d.linearVelocity = Vector2.ClampMagnitude(_rb2d.linearVelocity, _maxVelocityMagnitude);
			_spaceshipModel?.SetSpeed(_rb2d.linearVelocity.magnitude);
		}

		public void Rotate(int clockwiseDirection)
		{
			transform.Rotate(Vector3.back, clockwiseDirection * _rotateCoefficient * Time.deltaTime);
			_spaceshipModel?.SetRotation(transform.eulerAngles.z);
		}
	}
}