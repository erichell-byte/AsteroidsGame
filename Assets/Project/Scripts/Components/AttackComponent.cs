using AssetsLoader;
using Character;
using Config;
using Pools;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
using Weapon;
using Zenject;

namespace Components
{
    public class AttackComponent : MonoBehaviour, ITickable
    {
        [SerializeField] private Transform _shotPoint;
        [SerializeField] private GameObject _laser;
        [SerializeField] private LayerMask _laserLayerMask;

        private readonly CompositeDisposable _disposables = new ();
        
        private MainWeapon _mainWeapon;
        private LaserWeapon _laserWeapon;
        private BulletPoolFacade _bulletPool;
        private RemoteConfig _remoteConfig;
        
        public MainWeapon MainWeapon => _mainWeapon;
        public LaserWeapon LaserWeapon => _laserWeapon;

        [Inject]
        private void Construct(
            DiContainer container,
            IConfigProvider configProvider,
            GameConfiguration localConfig,
            Transform poolParent,
            IAssetLoader<Bullet> loader)
        {
            _remoteConfig = configProvider.GetRemoteConfig();
            _mainWeapon = container.Instantiate<MainWeapon>();
            _laserWeapon = container.Instantiate<LaserWeapon>();
            
            _bulletPool = new BulletPoolFacade(loader, localConfig.BulletId, poolParent);
        }

        public void Initialize(
            SpaceshipModel spaceshipModel)
        {
            _mainWeapon.Initialize(
                _shotPoint,
                _remoteConfig.BulletSpeed,
                _bulletPool);

            _laserWeapon.Initialize(
                _shotPoint,
                _laser,
                _laserLayerMask,
                _remoteConfig.CountOfLaserShots);

            _laserWeapon.RemainingShots.Subscribe(spaceshipModel.SetLaserCount).AddTo(_disposables);
            _laserWeapon.TimeToRecovery.Subscribe(spaceshipModel.SetTimeToRecoveryLaser).AddTo(_disposables);
        }

        public void AttackByMainShot()
        {
            _mainWeapon.Attack();
        }

        public void AttackByLaserShot()
        {
            _laserWeapon.Attack();
        }

        public void ResetWeapon()
        {
            _laserWeapon.TurnOffLaser();
            _mainWeapon.Reset();
            _disposables.Clear();
        }

        public void Tick()
        {
            _laserWeapon.Tick();
        }
    }
}

