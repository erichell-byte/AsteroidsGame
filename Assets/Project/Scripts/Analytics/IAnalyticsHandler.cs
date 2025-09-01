namespace Analytics
{
	public interface IAnalyticsHandler
	{
		public void StartGame();

		public void FinishGame(int shotCount, int laserCount, int asteroidsDestroyedCount, int ufoDestroedCount);

		public void LaserShotFired();
	}
}