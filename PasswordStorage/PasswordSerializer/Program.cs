// See https://aka.ms/new-console-template for more information

using PasswordSerializer.Models;
using PasswordSerializer.Utils;

var conectionStringCardStorage = new ConnectionString
{
    DatabaseName = "CardStorageDB",
    Host = "localhost",
    Password = "qwwerty123",
    UserName = "admin"
};

var conectionStringPasswordStorage = new ConnectionString
{
    DatabaseName = "PasswordStorageDB",
    Host = "localhost",
    Password = "12345678",
    UserName = "user"
};

List<ConnectionString> connections = new List<ConnectionString>();
connections.Add(conectionStringCardStorage);
connections.Add(conectionStringPasswordStorage);

Serializer serializer= new();
serializer.Serialize(connections);

connections = serializer.Deserialize();

foreach (var connection in connections)
    Console.WriteLine(connection);

Console.ReadKey(true);
