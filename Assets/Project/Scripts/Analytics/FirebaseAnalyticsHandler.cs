namespace Analytics
{
    public class FirebaseAnalyticsHandler : IAnalyticsHandler
    {
        public void StartGame()
        {
            Firebase.Analytics.FirebaseAnalytics
                .LogEvent("StartGame");
        }

        public void FinishGame(int shotCount, int laserCount, int asteroidsDestroyedCount, int ufoDestroedCount)
        {
            Firebase.Analytics.FirebaseAnalytics.LogEvent(
                "EndGame",
                new Firebase.Analytics.Parameter("shot_count", shotCount),
                new Firebase.Analytics.Parameter("laser_count", laserCount),
                new Firebase.Analytics.Parameter("asteroids_destroyed_count", asteroidsDestroyedCount),
                new Firebase.Analytics.Parameter("ufo_destroyed_count", ufoDestroedCount)
            );
        }

        public void LaserShotFired()
        {
            Firebase.Analytics.FirebaseAnalytics.LogEvent("LaserShotFired");
        }
    }
}