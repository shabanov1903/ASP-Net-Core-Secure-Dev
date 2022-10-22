using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using FullTextSearchApp.Database;
using FullTextSearchApp.Services;
using FullTextSearchApp.Services.Impl;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FullTextSearchApp
{
    internal class Program02
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

               }).Build();

            BenchmarkSwitcher.FromAssembly(typeof(Program02).Assembly).Run(args, new BenchmarkDotNet.Configs.DebugInProcessConfig());
            BenchmarkRunner.Run<SearchBenchmark>();
        }
    }

    [MemoryDiagnoser]
    [WarmupCount(1)]
    [IterationCount(5)]
    public class SearchBenchmark
    {
        private readonly FullTextSearch _searcher;
        private readonly string[] _documentsSet;

        [Params("target", "know", "for")]
        public string Query { get; set; }

        public SearchBenchmark()
        {
            _documentsSet = DocumentExtractor.DocumentsSet().Take(10000).ToArray();
            _searcher = new FullTextSearch();
            foreach (var item in _documentsSet) _searcher.AddStringToIndex(item);
        }

        [Benchmark(Baseline = true)]
        public void SimpleSearch()
        {
            new SimpleSearcher().SearchV3(Query, _documentsSet).ToArray();
        }

        [Benchmark]
        public void FullTextIndexSearch()
        {
            _searcher.SearchTest(Query).ToArray();
        }
    }
}
