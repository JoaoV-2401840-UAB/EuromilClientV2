using System;
using System.Threading.Tasks;
using Grpc.Net.Client;
using Euromil;

//class Program
//{
//    static async Task Main(string[] args)
//    {
//        // O canal precisa ter a mesma porta usada pelo servidor
//        using var channel = GrpcChannel.ForAddress("https://localhost:5001");
//        var client = new Euromil.Euromil.EuromilClient(channel);
//        var reply = await client.RegisterEuroMilAsync(new RegisterRequest { Key = "some_key", Checkid = "some_checkid" });
//        Console.WriteLine("Mensagem recebida: " + reply.Message);
//    }
//}
class Program
{
    static async Task Main(string[] args)
    {
        // URL do server
        var serverUrl = "https://localhost:5001"; 

        // Criar um HttpClientHandler que ignora a validação do certificado SSL
        var httpHandler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true
        };

        // Criar o canal gRPC com o HttpClientHandler
        var channel = GrpcChannel.ForAddress(serverUrl, new GrpcChannelOptions
        {
            HttpClient = new HttpClient(httpHandler)
        });

        // Criar o cliente gRPC
        var client = new Euromil.Euromil.EuromilClient(channel);

        try
        {
            // Chamada gRPC
            var reply = await client.RegisterEuroMilAsync(new RegisterRequest { Key = "some_key", Checkid = "some_checkid" });

            Console.WriteLine("Mensagem recebida: " + reply.Message);
        }
        catch (Grpc.Core.RpcException ex)
        {
            // Captura de excepções de RPC, incluindo problemas de ligação
            Console.WriteLine($"Erro de RPC: {ex.Status.Detail}");
        }
    }
}