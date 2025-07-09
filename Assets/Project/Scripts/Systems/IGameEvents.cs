using System;
using UniRx;

namespace Systems
{
    public interface IGameEvents
    {
        IObservable<Unit> OnSpaceshipCollidedWithEnemy { get; }
        void NotifySpaceshipCollidedWithEnemy();
    }
}