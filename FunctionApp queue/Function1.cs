using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace FunctionApp_queue
{
    public class Function1
    {
        private readonly ILogger<Function1> _logger;

        public Function1(ILogger<Function1> logger)
        {
            _logger = logger;
        }

        [Function("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            [Queue("myqueue", Connection = "AzureWebJobsStorage")] IAsyncCollector<string> queueCollector,
            ILogger log)
        {
            string message = await new StreamReader(req.Body).ReadToEndAsync();
            await queueCollector.AddAsync(message);
            return new OkObjectResult("Message added to queue");
        }
