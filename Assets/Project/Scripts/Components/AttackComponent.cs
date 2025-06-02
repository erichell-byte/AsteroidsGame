using Character;
using Config;
using Pools;
using UniRx;
using UnityEngine;
using Weapon;
using Zenject;

namespace Components
{
    public class AttackComponent : MonoBehaviour, ITickable
    {
        [SerializeField] private Transform shotPoint;
        [SerializeField] private GameObject laser;
        [SerializeField] private LayerMask laserLayerMask;

        private MainWeapon mainWeapon;
        private LaserWeapon laserWeapon;
        private DiContainer container;
        private CompositeDisposable disposables = new ();
        private BulletPoolFacade bulletPool;
        private GameConfiguration config;

        [Inject]
        private void Construct(
            DiContainer container,
            GameConfiguration config,
            Transform poolParent)
        {
            this.container = container;
            this.config = config;
            mainWeapon = container.Instantiate<MainWeapon>();
            laserWeapon = container.Instantiate<LaserWeapon>();
            
            bulletPool = new BulletPoolFacade(config.bulletPrefab, poolParent);
        }

        public void Initialize(
            CharacterModel characterModel)
        {
            mainWeapon.Initialize(
                shotPoint,
                config.bulletSpeed,
                bulletPool);

            laserWeapon.Initialize(
                shotPoint,
                laser,
                laserLayerMask,
                config.countOfLaserShots);

            laserWeapon.RemainingShots.Subscribe(characterModel.SetLaserCount).AddTo(disposables);
            laserWeapon.TimeToRecovery.Subscribe(characterModel.SetTimeToRecoveryLaser).AddTo(disposables);
        }

        public void AttackByMainShot()
        {
            mainWeapon.Attack();
        }

        public void AttackByLaserShot()
        {
            laserWeapon.Attack();
        }

        public void ResetWeapon()
        {
            laserWeapon.TurnOffLaser();
            mainWeapon.Reset();
            disposables.Clear();
        }

        public void Tick()
        {
            laserWeapon.Tick();
        }
    }
}

