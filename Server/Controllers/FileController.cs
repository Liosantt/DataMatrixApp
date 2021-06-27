using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Scan.Shared;
using System.IO;
using System.Text.Json;

namespace Scan.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult> Post()
        {
            string filename = "";

            if (HttpContext.Request.Form.Files.Any())
            {
                IFormFile file = HttpContext.Request.Form.Files[0];
                filename = file.FileName;

                using (MemoryStream ms = new MemoryStream((int)file.Length))
                {
                    await file.CopyToAsync(ms);

                    return Ok(JsonSerializer.Serialize<String>(System.Text.Encoding.UTF8.GetString(ms.ToArray())));
                }
            }
            else
            {
                return BadRequest();
            }
        }

    }
}
