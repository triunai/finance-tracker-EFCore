using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Supabase;
using DotNetEnv;

namespace FinanceTracker.Infrastructure.Supabase
{
    public class SupabaseClient
    {
        public Client Client { get; }

        public SupabaseClient()
        {
            Env.Load();
            var url = Environment.GetEnvironmentVariable("SUPABASE_URL");
            var key = Environment.GetEnvironmentVariable("SUPABASE_KEY");
            Client = new Client(url, key);
            Client.InitializeAsync().Wait();
        }
    }
}

