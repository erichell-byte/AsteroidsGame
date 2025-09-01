using AssetsLoader;
using Character;
using Config;
using Pools;
using Systems;
using UniRx;
using UnityEngine;
using Weapon;
using Zenject;

namespace Components
{
	public class AttackComponent : MonoBehaviour, ITickable
	{
		[SerializeField] private Transform _shotPoint;
		[SerializeField] private GameObject _laser;
		[SerializeField] private LayerMask _laserLayerMask;

		private readonly CompositeDisposable _disposables = new();
		private BulletPoolFacade _bulletPool;
		private IGameEvents _gameEvents;

		private RemoteConfig _remoteConfig;

		public MainWeapon MainWeapon { get; private set; }

		public LaserWeapon LaserWeapon { get; private set; }

		public void Tick()
		{
			LaserWeapon.Tick();
		}

		[Inject]
		private void Construct(
			DiContainer container,
			IConfigProvider configProvider,
			GameConfiguration localConfig,
			Transform poolParent,
			IAssetLoader<Bullet> loader,
			IGameEvents gameEvents)
		{
			_remoteConfig = configProvider.GetRemoteConfig();
			MainWeapon = container.Instantiate<MainWeapon>();
			LaserWeapon = container.Instantiate<LaserWeapon>();
			_gameEvents = gameEvents;

			_bulletPool = new BulletPoolFacade(loader, localConfig.BulletId, poolParent);
		}

		public void Initialize(
			SpaceshipModel spaceshipModel)
		{
			MainWeapon.Initialize(
				_shotPoint,
				_remoteConfig.BulletSpeed,
				_bulletPool);

			LaserWeapon.Initialize(
				_shotPoint,
				_laser,
				_laserLayerMask,
				_remoteConfig.CountOfLaserShots);

			LaserWeapon.RemainingShots.Subscribe(spaceshipModel.SetLaserCount).AddTo(_disposables);
			LaserWeapon.TimeToRecovery.Subscribe(spaceshipModel.SetTimeToRecoveryLaser).AddTo(_disposables);
			MainWeapon.OnShot += OnShotPerformed;
		}

		public void AttackByMainShot()
		{
			MainWeapon.Attack();
		}

		public void AttackByLaserShot()
		{
			LaserWeapon.Attack();
		}

		public void ResetWeapon()
		{
			LaserWeapon.TurnOffLaser();
			MainWeapon.Reset();
			_disposables.Clear();
		}

		private void OnShotPerformed()
		{
			_gameEvents.NotifySpaceshipShot(_shotPoint.position);
		}
	}
}