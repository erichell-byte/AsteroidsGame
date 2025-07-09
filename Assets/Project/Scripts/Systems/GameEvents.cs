using System;
using UniRx;

namespace Systems
{
    public class GameEvents : IGameEvents
    {
        private readonly Subject<Unit> spaceshipCollidedWithEnemy = new();
        
        public IObservable<Unit> OnSpaceshipCollidedWithEnemy => spaceshipCollidedWithEnemy;
        public void NotifySpaceshipCollidedWithEnemy() => spaceshipCollidedWithEnemy.OnNext(Unit.Default);
    }
}