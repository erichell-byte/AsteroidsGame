using UnityEngine;
using Zenject;

namespace Analytics
{
    public class AnalyticsMediator : MonoBehaviour
    {
        private IAnalyticsHandler analyticsHandler;
    
        [Inject]
        private void Construct(IAnalyticsHandler analyticsHandler)
        {
            this.analyticsHandler = analyticsHandler;
        }
    
    
    }
}