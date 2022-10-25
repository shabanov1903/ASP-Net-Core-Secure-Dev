using FullTextSearchApp.Database;
using FullTextSearchApp.Services;
using FullTextSearchApp.Services.Impl;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FullTextSearchApp
{
    internal class Program01
    {
        static void Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
               .ConfigureServices(services => {

                   #region Configure EF DBContext Service (CardStorageService Database)

                   services.AddDbContext<DocumentDbContext>(options =>
                   {
                       options.UseSqlServer(@"data source=LAPTOP-KQ6S374D\SQLEXPRESS;initial catalog=DocumentStorageDB;user id=admin;password=admin;MultipleActiveResultSets=True;App=EntityFramework;trustServerCertificate=true");
                   });

                   #endregion

                   #region Configure Repositories

                   services.AddTransient<IDocumentRepository, DocumentRepository>();

                   #endregion
               
               }).Build();

            // Заполнение БД документами
            // host.Services.GetRequiredService<IDocumentRepository>().LoadDocuments();
            // Заполнение БД словами
            // new WordsDocuments(host.Services.GetService<DocumentDbContext>()).BuildIndex();
        }
    }
}
