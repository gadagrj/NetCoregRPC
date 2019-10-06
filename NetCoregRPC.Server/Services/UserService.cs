using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoregRPC.Server
{
    public class UserService:Users.UsersBase
    {
        private readonly ILogger<UserService> _logger;
        public UserService(ILogger<UserService> logger)
        {
            _logger = logger;
        }

        public override async Task GetAllUser(SearchRequest request, IServerStreamWriter<Response> responseStream, ServerCallContext context)
        {
            var rng = new Random();
            var now = DateTime.UtcNow;

            var i = 0;
            while (!context.CancellationToken.IsCancellationRequested && i < 2)
            {
                await Task.Delay(200); 

                var forecast = new Response
                {
                    Id=1,
                    Name="User 1 From gRPC Service"
                };
                _logger.LogInformation("Sending User response");
                await responseStream.WriteAsync(forecast);
            }
        }
    }
}
