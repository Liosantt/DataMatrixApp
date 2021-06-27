using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Scan.Server.Model;

using System.Net.Http;
using Scan.Shared;

namespace Scan.Server.Services
{
    public class SCooisService : ICooisService
    {
        private readonly Connectors1Context _context;

        public SCooisService(Connectors1Context context)
        {
            _context = context;
        }

        public async Task<List<CooisComponent>> GetCooisComponents(int prodOrder)
        {
            var cooisComponents = await _context.CooisComponents.Where(e => e.ProdOrder == prodOrder).ToListAsync();

            if (cooisComponents == null || cooisComponents.Count() == 0)
            {
                throw new HttpRequestException("", new Exception(), System.Net.HttpStatusCode.BadRequest);
            }

            List<CooisComponent> returnVal = new List<CooisComponent>();

            foreach (CooisComponent item in cooisComponents)
            {
                if (String.IsNullOrEmpty(item.Batch))
                {
                    //This is done to enable editable field option on the Front-End Table
                    item.Batch = "None";
                }

                returnVal.Add((CooisComponent)item.Clone());
            }

            return returnVal;
        }
    }
}
