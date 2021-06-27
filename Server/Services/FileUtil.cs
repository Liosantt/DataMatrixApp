using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.JSInterop;
using Scan.Shared;

namespace Scan
{
    //This class provides a helper method to save a file using JavaScript( as this functionality is not built in) 
    public class SFileUtil : IFileUtil
    {
        public virtual async Task SaveAs(object js, string filename, byte[] data)
        {
            await ((IJSRuntime)js).InvokeAsync<object>(
                "saveAsFile",
                filename,
                Convert.ToBase64String(data));
        }
    }
}
