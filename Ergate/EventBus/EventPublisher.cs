using DotNetCore.CAP;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ergate
{
    public class EventPublisher : IEventPublisher
    {
        private readonly ICapPublisher publisher;
        private readonly IConfigurationRoot config;

        public EventPublisher(ICapPublisher cap)
        {
            publisher = cap;

            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            config = builder.Build();
        }

        public void Dispose()
        {

        }

        public async Task PublishAsync<TEvent>(TEvent @event) where TEvent : BaseEvent
        {
            using (var conn = new MySqlConnection(config.GetSection("ConnectionString:Database").Value))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        var name = @event.GetType().FullName;
                        await publisher.PublishAsync(name, @event, tran);
                        tran.Commit();
                    }
                    catch (Exception exp)
                    {
                        tran.Rollback();
                    }
                }
            }
        }

        public async Task PublishAsync<TEvent>(TEvent @event, Action<MySqlConnection, IDbTransaction> action) where TEvent : BaseEvent
        {
            using (var conn = new MySqlConnection(config.GetSection("ConnectionString:Database").Value))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        var name = @event.GetType().FullName;

                        action?.Invoke(conn, tran);

                        await publisher.PublishAsync(name, @event, tran);
                        tran.Commit();
                    }
                    catch (Exception exp)
                    {
                        tran.Rollback();
                    }
                }
            }
        }

        public void Publish<TEvent>(TEvent @event) where TEvent : BaseEvent
        {
            using (var conn = new MySqlConnection(config.GetSection("ConnectionString:Database").Value))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        var name = @event.GetType().FullName;
                        publisher.Publish(name, @event, tran);
                        tran.Commit();
                    }
                    catch (Exception exp)
                    {
                        tran.Rollback();
                    }
                }
            }
        }

        public void Publish<TEvent>(TEvent @event, Action<MySqlConnection, IDbTransaction> action) where TEvent : BaseEvent
        {
            using (var conn = new MySqlConnection(config.GetSection("ConnectionString:Database").Value))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        var name = @event.GetType().FullName;

                        action?.Invoke(conn, tran);

                        publisher.Publish(name, @event,tran);
                        tran.Commit();
                    }
                    catch (Exception exp)
                    {
                        tran.Rollback();
                    }
                }
            }
        }

        public void Publish<TEvent>(List<TEvent> events, Action<MySqlConnection, IDbTransaction> action) where TEvent : BaseEvent
        {
            using (var conn = new MySqlConnection(config.GetSection("ConnectionString:Database").Value))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        action?.Invoke(conn, tran);
                        events.AsParallel().ForAll(async x =>
                        {
                            var name = x.GetType().FullName;
                            await publisher.PublishAsync(name, x, tran);
                        });
                        tran.Commit();
                    }
                    catch (Exception exp)
                    {
                        tran.Rollback();
                    }
                }
            }
        }
    }
}
