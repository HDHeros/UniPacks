using System.Collections.Generic;
using UnityEngine;

namespace HDH.Metrics
{
    public class DummyMetricsAgent : IMetricsAgent
    {
        public void TrackEvent(string eventName)
        {
            Debug.Log($"Event {eventName} tracked");
        }

        public void TrackEvent(string eventName, Dictionary<string, object> @params)
        {
            Debug.Log($"Event {eventName} tracked");
        }

        public void TrackEventOnce(string eventName)
        {
            Debug.Log($"Event {eventName} tracked once");
        }

        public void TrackEventOnce(string eventName, Dictionary<string, object> @params)
        {
            Debug.Log($"Event {eventName} tracked once");
        }
    }
}