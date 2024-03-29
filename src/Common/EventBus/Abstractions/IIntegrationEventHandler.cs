using System.Threading.Tasks;
using Common.EventBus.Events;

namespace Common.EventBus.Abstractions
{

    public interface IIntegrationEventHandler<in TIntegrationEvent> : IIntegrationEventHandler 
        where TIntegrationEvent: IntegrationEvent
    {
        Task Handle(TIntegrationEvent @event);
    }
    
    public interface IIntegrationEventHandler
    {
         
    }
}