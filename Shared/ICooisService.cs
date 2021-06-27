using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scan.Shared
{
    public interface ICooisService
    {
        public Task<List<CooisComponent>> GetCooisComponents(int prodOrder);
    }
}
