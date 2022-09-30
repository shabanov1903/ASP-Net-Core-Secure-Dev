using CardStorageService.Core.Crypto.Impl;

// Generate token for database
var cryptoFactory = new CryptoFatotySHA512();
const string password = "admin";

(string passwordSalt, string passwordHash) result = cryptoFactory.CreatePasswordHash(password);

Console.WriteLine($"Соль: {result.passwordSalt}");
Console.WriteLine($"Пароль: {result.passwordHash}");

const string salt = "TzYnBqlFKSI+e2yvSRvH0g==";
const string hash = "qtIwAFgq1QsQ8qwyR9aP1keF96kBgXogeS8+1yU5MlweRWUhEiVGTwYzQKUvfXh0dgApnps4J4VZJyZz5EoCJw==";

Console.WriteLine($"Проверка пароля: {cryptoFactory.VerifyPassword(password, salt, hash)}");
