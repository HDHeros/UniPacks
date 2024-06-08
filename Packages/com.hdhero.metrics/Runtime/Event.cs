using System.Collections.Generic;

namespace HDH.Metrics
{
    public class Event<TEventName>
    {
        private readonly MetricsService<TEventName> _metricsService;
        
        public TEventName Name {get; private set;}
        public Dictionary<string, object> Params {get; private set;}

        public Event(MetricsService<TEventName> metricsService, int paramsCapacity = 5)
        {
            _metricsService = metricsService;
            Params = new Dictionary<string, object>(paramsCapacity);
        }

        public Event<TEventName> SetName(TEventName name)
        {
            Name = name;
            return this;
        }

        public Event<TEventName> AddParam(string paramName, object paramValue)
        {
            Params.Add(paramName, paramValue);
            return this;
        }

        public void TrackOnce()
        {
            if (Params.Count > 0) 
                _metricsService.TrackEventOnce(Name, Params);
            else
                _metricsService.TrackEventOnce(Name);
            Reset();
            _metricsService.ReturnEvent(this);
        }

        public void Track()
        {
            if (Params.Count > 0) 
                _metricsService.TrackEvent(Name, Params);
            else
                _metricsService.TrackEvent(Name);
            Reset();
            _metricsService.ReturnEvent(this);
        }

        private void Reset()
        {
            Name = default;
            Params.Clear();
        }
    }
}