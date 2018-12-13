using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Ergate
{
    public interface IEventPublisher : IDisposable
    {
        Task PublishAsync<TEvent>(TEvent @event) where TEvent : BaseEvent;

        void Publish<TEvent>(TEvent @event) where TEvent : BaseEvent;

        Task PublishAsync<TEvent>(TEvent @event, Action<MySqlConnection, IDbTransaction> action) where TEvent : BaseEvent;

        void Publish<TEvent>(TEvent @event, Action<MySqlConnection, IDbTransaction> action) where TEvent : BaseEvent;

        void Publish<TEvent>(List<TEvent> events, Action<MySqlConnection, IDbTransaction> action) where TEvent : BaseEvent;
    }
}
