using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;
using System;
using System.Threading;
using System.Threading.Tasks;
using static NetCoregRPC.Server.Users;

namespace NetCoregRPC.Client.ConsoleApp
{
    class Program
    {
        static async Task Main()
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new UsersClient(channel);

            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            Console.WriteLine($"Sending User Request via gRPC");

            using var streamingCall = client.GetAllUser(new Server.SearchRequest() { Name="R"}, cancellationToken: cts.Token);

            try
            {
                Console.WriteLine("Response Recived from gRPC \n");
                await foreach (var userData in streamingCall.ResponseStream.ReadAllAsync(cancellationToken: cts.Token))
                {
                    Console.WriteLine($" {userData.Id} | {userData.Name}");

                }

                Console.ReadLine();
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.Cancelled)
            {
                Console.WriteLine("Stream cancelled.");
            }
        }
    }
}
