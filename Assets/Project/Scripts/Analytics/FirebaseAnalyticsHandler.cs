using Firebase.Analytics;

namespace Analytics
{
	public class FirebaseAnalyticsHandler : IAnalyticsHandler
	{
		public void StartGame()
		{
			FirebaseAnalytics
				.LogEvent("StartGame");
		}

		public void FinishGame(int shotCount, int laserCount, int asteroidsDestroyedCount, int ufoDestroedCount)
		{
			FirebaseAnalytics.LogEvent(
				"EndGame",
				new Parameter("shot_count", shotCount),
				new Parameter("laser_count", laserCount),
				new Parameter("asteroids_destroyed_count", asteroidsDestroyedCount),
				new Parameter("ufo_destroyed_count", ufoDestroedCount)
			);
		}

		public void LaserShotFired()
		{
			FirebaseAnalytics.LogEvent("LaserShotFired");
		}
	}
}