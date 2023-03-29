using StackExchange.Redis;
using System;

namespace Infrastructure.Cache
{
    public class ConnectionHelper
    {
        private static Lazy<ConnectionMultiplexer> lazyConnection;
        static ConnectionHelper()
        {
            lazyConnection = new Lazy<ConnectionMultiplexer>(() => {
                //return ConnectionMultiplexer.Connect(ConfigurationManager.AppSetting["RedisURL"]);
                return ConnectionMultiplexer.Connect("127.0.0.1:6379");
            });
        }
        
        public static ConnectionMultiplexer Connection
        {
            get
            {
                return lazyConnection.Value;
            }
        }
    }
}
