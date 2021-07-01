using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MoreLinq;
using Scan.Server.Model;

namespace Scan.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class testController
    {
        private Connectors1Context testContext;

        public testController(Connectors1Context testContext)
        {
            this.testContext = testContext;
        }

        [HttpGet]
        public int test()
        {
            return testContext.CooisComponents.Count();
        }
    }
}