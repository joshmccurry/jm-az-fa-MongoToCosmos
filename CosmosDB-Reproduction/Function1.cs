using System;
using System.Diagnostics.Contracts;
using System.Text.Json;
using System.Threading.Tasks;
using CosmosDB_Reproduction.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace CosmosDB_Reproduction {

    public class Function1 {
        private readonly ILogger _logger;
        private static MongoClientSettings _settings = MongoClientSettings.FromConnectionString(Environment.GetEnvironmentVariable("MongoDB_ConnectionString"));
        private static MongoClient _client = new MongoClient(_settings);
       
        [FunctionName("GetFromCosmosDB")]
        public ActionResult GetFromCosmosDBAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]
            HttpRequest req, ILogger log) {
            log.LogInformation("C# HTTP trigger function processed a request.");
            var db = _client.GetDatabase("testdb");
            var items = db.GetCollection<TestEntity>("testtable");
            string name = req.Query["name"];
            var list = items.AsQueryable().Where(p => p.Name.Contains(name));
            return (ActionResult) new OkObjectResult(JsonSerializer.Serialize(list));
        }

        [FunctionName("PutFromCosmosDB")]
        public void PutFromCosmosDB(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]
            HttpRequest req, ILogger log) {
            log.LogInformation("C# HTTP trigger function processed a request.");
            var db = _client.GetDatabase("testdb");
            var items = db.GetCollection<TestEntity>("testtable");
            string name = req.Query["name"];
            int cat = 0;
            Int32.TryParse(req.Query["category"], out cat);
            if (name == null) {
                name = "null";
            }
            
            items.InsertOne(new TestEntity(Guid.NewGuid().ToString(), name, cat, 0, true));

        }

        [FunctionName("PostFromCosmosDB")]
        public void PostFromCosmosDB(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]
            HttpRequest req, ILogger log) {
            log.LogInformation("C# HTTP trigger function processed a request.");
        }


        [FunctionName("DeleteFromCosmosDB")]
        public void DeleteFromCosmosDB(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]
            HttpRequest req, ILogger log) {
            log.LogInformation("C# HTTP trigger function processed a request.");
            var db = _client.GetDatabase("testdb");
            var items = db.GetCollection<TestEntity>("testtable");

            int cat = 0;
            Int32.TryParse(req.Query["category"], out cat);
            items.DeleteMany<TestEntity>(c => c.Category.Equals(cat));

        }
    }
}
