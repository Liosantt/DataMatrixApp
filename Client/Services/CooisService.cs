using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Net.Http;
using System.Net.Http.Json;

using Scan.Shared;

namespace Scan.Client
{
    public class CooisService : ICooisService
    {
        HttpClient httpClient;

        public CooisService(HttpClient _httpClient)
        {
            httpClient = _httpClient;
        }

        public async Task<List<CooisComponent>> GetCooisComponents(int prodOrder)
        {
            return await httpClient.GetFromJsonAsync<List<CooisComponent>>("api/coois/" + prodOrder);
        }
    }
}
