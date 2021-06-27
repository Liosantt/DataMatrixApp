using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scan.Shared
{
    public interface IFileUtil
    {
        public Task SaveAs(object js, string filename, byte[] data);
    }
}
