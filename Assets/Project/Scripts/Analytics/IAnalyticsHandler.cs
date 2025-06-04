
namespace Analytics
{
    public interface IAnalyticsHandler
    {
        public void StartGame();

        public void EndGame(int shotCount, int laserCount, int asteroidsDestroyedCount, int ufoDestroedCount);
    
        public void LaserShotFired();
    }
}