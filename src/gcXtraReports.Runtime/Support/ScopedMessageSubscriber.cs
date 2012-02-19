using System;
using System.Linq;
using Caliburn.Micro;
using DevExpress.XtraReports.UI;
using GeniusCode.XtraReports.Runtime.Messaging;

namespace GeniusCode.XtraReports.Runtime.Support
{
    internal class ScopedMessageSubscriber : IHandle<ScopedControlBeforePrintMessage>, IDisposable
    {
        private readonly IEventAggregator _aggregator;
        private readonly int _rootHashCode;
        private readonly Action<XRControl> _handler;

        public ScopedMessageSubscriber(int rootHashCode, Action<XRControl> handler)
            : this(EventAggregatorSingleton.Instance, rootHashCode, handler)
        {
        }

        public ScopedMessageSubscriber(IEventAggregator aggregator, int rootHashCode, Action<XRControl> handler)
        {
            _aggregator = aggregator;
            _rootHashCode = rootHashCode;
            _handler = handler;

            _aggregator.Subscribe(this);
        }

        public void Handle(ScopedControlBeforePrintMessage message)
        {
            if (message.RootReportHashcode == _rootHashCode)
            {
                _handler(message.Control);
            }
        }

        public void Dispose()
        {
            _aggregator.Unsubscribe(this);
        }
    }
}