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
    public class AttackComponent : MonoBehaviour, IGameUpdateListener
    {
        [SerializeField] private Transform shotPoint;
        [SerializeField] private GameObject laser;
        [SerializeField] private LayerMask laserLayerMask;

        private MainWeapon mainWeapon;
        private LaserWeapon laserWeapon;
        private DiContainer container;
        private CompositeDisposable disposables = new ();

        [Inject]
        private void Construct(DiContainer container)
        {
            this.container = container;
        }

        public void Initialize(
            GameConfiguration config,
            Transform bulletPoolParent,
            CharacterModel characterModel)
        {
            var bulletPool = new BulletPoolFacade(config.bulletPrefab, bulletPoolParent);
            mainWeapon = container.Instantiate<MainWeapon>();
            mainWeapon.Initialize(
                shotPoint,
                config.bulletSpeed,
                bulletPool);
            
            laserWeapon = container.Instantiate<LaserWeapon>();
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

        public void OnUpdate(float deltaTime)
        {
            laserWeapon.Update();
        }

        public void ResetWeapon()
        {
            laserWeapon.TurnOffLaser();
            mainWeapon.Reset();
            mainWeapon = null;
            laserWeapon = null;
            disposables.Clear();
        }
    }
}

