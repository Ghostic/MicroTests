using System;
using System.Linq;
using Autofac;
using Common.EventBus;
using Common.EventBus.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Common.EventBusRabbitMQ
{
    public static class Extensions
    {
        public static string GetGenericTypeName(this Type type)
        {
            var typeName = string.Empty;

            if (type.IsGenericType)
            {
                var genericTypes = string.Join(",", type.GetGenericArguments().Select(t => t.Name).ToArray());
                typeName = $"{type.Name.Remove(type.Name.IndexOf('`'))}<{genericTypes}>";
            }
            else
            {
                typeName = type.Name;
            }

            return typeName;
        }

        public static string GetGenericTypeName(this object @object)
        {
            return @object.GetType().GetGenericTypeName();
        }

        public static void AddEventBus(this ContainerBuilder builder, IConfiguration configuration)
        {
            var subscriptionClientName = configuration["SubscriptionClientName"];

            builder.Register<IEventBus>(context => 
            {
                var rabbitMQPersistentConnection = context.Resolve<IRabbitMQPersistentConnection>();
                var iLifetimeScope = context.Resolve<ILifetimeScope>();
                var logger = context.Resolve<ILogger<EventBusRabbitMQ>>();
                var eventBusSubcriptionsManager = context.Resolve<IEventBusSubscriptionsManager>();
                var retryCount = 5;

                return new EventBusRabbitMQ(rabbitMQPersistentConnection, logger, iLifetimeScope, eventBusSubcriptionsManager, 
                                                            subscriptionClientName, retryCount);
            }).SingleInstance();

             builder.RegisterType<InMemoryEventBusSubscriptionsManager>()
                     .As<IEventBusSubscriptionsManager>().SingleInstance();          
        }

    }
}