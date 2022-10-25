using CardStorageService.Protos;
using Grpc.Net.Client;
using static CardStorageService.Protos.ClientService;

AppContext.SetSwitch("System.Net.Http.SocketHttpHandler.Http2UnencryptedSupport", true);

using var channel = GrpcChannel.ForAddress("http://localhost:5001");

ClientServiceClient clientService = new ClientServiceClient(channel);

var createClientResponse = clientService.Create(new ClientRPC
{
    FirstName = "Шабанов",
    Surname = "Данил",
    Patronymic = "Валерьевич"
});

Console.WriteLine($"Client {createClientResponse.ClientId} created successfully");

int id = createClientResponse.ClientId;
var getByIdClientResponse = clientService.GetById(new IdRPC { ClientId = id });
Console.WriteLine($"Client with Id={id} has parameters: FirstName: {getByIdClientResponse.FirstName}, Surname: {getByIdClientResponse.Surname}, Patronymic: {getByIdClientResponse.Patronymic}");

var getAllClientResponse = clientService.GetAll(new GetAllPRC());
Console.WriteLine($"All clients:");
foreach (var client in getAllClientResponse.Clients)
{
    Console.WriteLine($"Client with Id={client.ClientId} has parameters: FirstName: {client.FirstName}, Surname: {client.Surname}, Patronymic: {client.Patronymic}");
}

Console.ReadKey();
