using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ProcessingInformationFunction
{
    public class InfoEF
    {
        private const string Route = "efcomputers";
        private readonly ComputerContext _computerContext;

        public InfoEF(ComputerContext computerContext)
        {
            this._computerContext = computerContext;
        }

        [FunctionName("EntityFramework_GetComputers")]
        public async Task<IActionResult> GetComputers(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = Route)]
        HttpRequest req, ILogger log)
        {
            log.LogInformation("Getting list of computers");
            var computers = await _computerContext.ComputerInfoModels.ToListAsync();
            return new OkObjectResult(computers);
        }

        [FunctionName("EntityFramework_GetComputerById")]
        public async Task<IActionResult> GetComputerById(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = Route + "/{id}")]
        HttpRequest req, ILogger log, string id)
        {
            log.LogInformation("Getting todo item by id");
            var computer = await _computerContext.ComputerInfoModels.FindAsync(Guid.Parse(id));
            if (computer == null)
            {
                log.LogInformation($"Item {id} not found");
                return new NotFoundResult();
            }
            return new OkObjectResult(computer);
        }

        [FunctionName("EntityFramework_CreateComputer")]
        public async Task<IActionResult> CreateComputer(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = Route)]
        HttpRequest req, ILogger log)
        {
            log.LogInformation("Creating a new todo list item");
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var input = JsonConvert.DeserializeObject<ComputerInfoModel>(requestBody);
            var computer = new ComputerInfoModel { ComputerName = input.ComputerName };
            await _computerContext.ComputerInfoModels.AddAsync(computer);
            await _computerContext.SaveChangesAsync();
            return new OkObjectResult(computer);
        }

        [FunctionName("EntityFramework_UpdateComputer")]
        public async Task<IActionResult> UpdateComputer(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = Route + "/{id}")]
        HttpRequest req, ILogger log, string id)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var updated = JsonConvert.DeserializeObject<ComputerInfoModel>(requestBody);
            var computer = await _computerContext.ComputerInfoModels.FindAsync(Guid.Parse(id));
            if (computer == null)
            {
                log.LogWarning($"Item {id} not found");
                return new NotFoundResult();
            }

            if (computer.LastConnectedTime != updated.LastConnectedTime)
            {
                computer.LastConnectedTime = updated.LastConnectedTime;
            }
            
            await _computerContext.SaveChangesAsync();

            return new OkObjectResult(computer);
        }

        [FunctionName("EntityFramework_DeleteComputer")]
        public async Task<IActionResult> DeleteComputer(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = Route + "/{id}")]
        HttpRequest req, ILogger log, string id)
        {
            var computer = await _computerContext.ComputerInfoModels.FindAsync(Guid.Parse(id));
            if (computer == null)
            {
                log.LogWarning($"Item {id} not found");
                return new NotFoundResult();
            }

            _computerContext.ComputerInfoModels.Remove(computer);
            await _computerContext.SaveChangesAsync();
            return new OkResult();
        }
    }
}
