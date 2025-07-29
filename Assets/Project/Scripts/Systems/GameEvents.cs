using System;
using UniRx;

namespace Systems
{
    public class GameEvents : IGameEvents
    {
        private readonly Subject<Unit> _spaceshipCollidedWithEnemy = new();
        
        public IObservable<Unit> OnSpaceshipCollidedWithEnemy => _spaceshipCollidedWithEnemy;
        public void NotifySpaceshipCollidedWithEnemy() => _spaceshipCollidedWithEnemy.OnNext(Unit.Default);
    }
}