using ExceptionHandling.Test.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExceptionHandling.Test.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public Task<IResult> GetAsync()
        {
            if (Request.Query.Any())
                throw new OException("test expected exception");

            throw new InvalidOperationException("test unexpected exception");
        }

        [HttpGet("{code}")]
        [OExceptionFilter]
        public Task<IResult> GetAsync(int code)
        {
            if (code < 0)
                throw new OException("expected exception");

            if (code > 0)
                return Task.FromResult<IResult>(
                    new Result<string>("test exception filter attribute"));

            throw new Exception("unexpected exception");
        }

        [HttpPost]
        [OExceptionFilter]
        public Task PostAsync([FromBody] Person person)
        {
            throw new OException("test body parameter exception");
        }

        [HttpPut]
        [OExceptionFilter]
        public Task PutAsync([FromForm] Student student)
        {
            throw new OException("test body parameter exception");
        }
    }
}
