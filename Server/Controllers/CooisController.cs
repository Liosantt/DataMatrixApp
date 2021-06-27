using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scan.Server.Model;
using Scan.Shared;

using System.IO;

namespace Scan.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CooisController : ControllerBase {
        private readonly Connectors1Context _context;

        public CooisController(Connectors1Context context) {
            _context = context;
        }

        // GET: api/Coois/5
        [HttpGet("{order}")]
        public async Task<ActionResult<List<CooisComponent>>> GetCooisComponent(string order) {
            var cooisComponents = await _context.CooisComponents.Where(e => e.ProdOrder == long.Parse(order)).ToListAsync();

            if (cooisComponents == null || cooisComponents.Count() == 0)
            {
                return BadRequest();
            }

            System.IO.File.AppendAllText("access.log", "Accessed: " + order + " at (" + DateTime.Now.ToString("G") + ") by " + Request.Headers["User-Agent"].ToString() + "\n");

            foreach (CooisComponent item in cooisComponents)
            {
                if (String.IsNullOrEmpty(item.Batch))
                {
                    //This is done to enable editable field option on the Front-End Table
                    item.Batch = "None";
                }
            }

            return cooisComponents;
        }

        [HttpGet("log")]
        public async Task<ActionResult<string>> GetAccessLog()
        {
            return System.IO.File.ReadAllText("access.log");
        }
    }
}
