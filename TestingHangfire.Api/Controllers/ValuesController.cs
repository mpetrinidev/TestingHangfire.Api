using System;
using System.Collections.Generic;
using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace TestingHangfire.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IBackgroundJobClient backgroundJobClient;
        private readonly ISingletonValue singletonValue;

        public ValuesController(IBackgroundJobClient backgroundJobClient, ISingletonValue singletonValue)
        {
            this.backgroundJobClient = backgroundJobClient;
            this.singletonValue = singletonValue;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            //backgroundJobClient.Enqueue(() => Console.WriteLine("Llamando a este evento ahora"));
            //backgroundJobClient.Schedule(() => Console.WriteLine("Llamando a este evento en 1 minuto"), TimeSpan.FromMinutes(1));

            //RecurringJob.AddOrUpdate("un-id", () => Console.WriteLine("Corriendo Siempre"), Cron.Daily(10, 30));
            //RecurringJob.RemoveIfExists("un-id");
            //RecurringJob.Trigger("un-id");

            //var id = backgroundJobClient.Enqueue(() => MethodWithRetry());
            //backgroundJobClient.ContinueJobWith(id, () => ContinueMethodWithRetry());

            return new string[] { "value1", "value2" };
        }

        [AutomaticRetry(DelaysInSeconds = new[] { 1, 10, 5 }, Attempts = 3)]
        public void MethodWithRetry()
        {
            if (singletonValue.Retries < 3)
            {
                singletonValue.Retries++;
                throw new Exception("MethodWithRetry exception");
            }

            singletonValue.Retries = 1;

            Console.WriteLine("Ejecución correcta de MethodWithRetry");
        }

        public void ContinueMethodWithRetry()
        {
            Console.WriteLine("Ejecución correcta de ContinueMethodWithRetry luego de MethodWithRetry");
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
