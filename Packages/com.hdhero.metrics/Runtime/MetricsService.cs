using System.Collections.Generic;

namespace HDH.Metrics
{
    public class MetricsService<TEventName>
    {
        private readonly ObjectPool<Event<TEventName>> _eventsPool;
        private IMetricsAgent[] _agents;

        public MetricsService(IMetricsAgent[] agents)
        {
            _agents = agents;
            _eventsPool = new ObjectPool<Event<TEventName>>(() => new Event<TEventName>(this));
        }
        
        public Event<TEventName> Event(TEventName eventName) => _eventsPool.Get().SetName(eventName);

        public void ReturnEvent(Event<TEventName> @event) => _eventsPool.Return(@event);

        public void TrackEvent(TEventName eventName)
        {
            foreach (IMetricsAgent agent in _agents)
                agent.TrackEvent(eventName.ToString());
        }

        public void TrackEvent(TEventName eventName, Dictionary<string, object> @params)
        {
            foreach (IMetricsAgent agent in _agents)
                agent.TrackEvent(eventName.ToString(), @params);
        }
        
        public void TrackEventOnce(TEventName eventName)
        {
            foreach (IMetricsAgent agent in _agents)
                agent.TrackEventOnce(eventName.ToString());
        }

        public void TrackEventOnce(TEventName eventName, Dictionary<string, object> @params)
        {
            foreach (IMetricsAgent agent in _agents) 
                agent.TrackEventOnce(eventName.ToString(), @params);
        }
    }
}