using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Scribble.Functions.Functions
{
    public class WordEntity : TableEntity
    {
        public string Word { get; set; }
    }

    public static class WordImportTask
    {
        [FunctionName("WordImportTask")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            [Table("ScribbleWords", Connection = "AzureWebJobsStorage")] CloudTable cloudTable,
            ILogger log)
        {
            log.LogInformation("Importing words");

            using var client = new HttpClient();

            var response = await client.GetAsync("https://skribbliohints.github.io/words.json");
            response.EnsureSuccessStatusCode();
            var resp = await response.Content.ReadAsStringAsync();

            var wordList = JsonConvert.DeserializeObject<Dictionary<string, object>>(resp).Select(x => x.Key).ToList();
            
            cloudTable.CreateIfNotExists();
            int id = 1;
            while(id <= wordList.Count())
            {
                List<WordEntity> batch = new List<WordEntity>();

                var batchOperation = new TableBatchOperation();
                int ti = id;
                for (; id < Math.Min(wordList.Count() + 1, ti + 100); id++)
                {
                    batchOperation.InsertOrReplace(new WordEntity { Word = wordList[id - 1], PartitionKey = $"Words", RowKey = id.ToString() });
                }
                cloudTable.ExecuteBatch(batchOperation);
            }

            return new OkObjectResult(wordList);

        }
    }
}
