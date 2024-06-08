using System.Collections.Generic;

namespace HDH.Metrics
{
    public class DummyMetricsAgent : IMetricsAgent
    {
        public void TrackEvent(string eventName)
        {
        }

        public void TrackEvent(string eventName, Dictionary<string, object> @params)
        {
        }

        public void TrackEventOnce(string eventName)
        {
        }

        public void TrackEventOnce(string eventName, Dictionary<string, object> @params)
        {
        }
    }
}