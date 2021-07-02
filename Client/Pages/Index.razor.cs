using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Scan.Shared;
using System.Net.Http;
using System.Net.Http.Json;
using AntDesign;
using AntDesign.TableModels;

using Microsoft.AspNetCore.Components;

namespace Scan.Client.Pages
{
    public partial class Index
    {
        #region Services
        [Inject]
        MessageService messageService { get; set; }
       
        private Dictionary<string, string> storageLocs = new Dictionary<string, string>() 
        {
            { "FMBA", "Main Store" },
            { "FMBB", "Finished Goods" },
            { "FMBC", "Clean Room" },
            { "FMBD", "Bar Store" },
            { "FMBE", "Mould Shop" },
            { "FMBF", "Wire Store" },
            { "FMBG", "Gas Store" },
            { "FMBH", "LCM Garage" },
            { "FMBI", "Machine Shop" },
            { "FMBM", "Quarantine" },
            { "FMBN", "Stock for Scrap" },
            { "FMBO", "Fault Investigations" },
            { "FMBR", "Kardex 1" },
            { "FMBS", "Kardex 2" },
            { "FMBT", "Production Engineering" }
        };
        #endregion
        
        private bool loadingPO = false;
        private string? tempUserInput;
        private async Task SubmitProdOrder()
        {
            if (string.IsNullOrEmpty(tempUserInput))
            {
                _ = messageService.Error("Oopsie whoopsie, Wumpus sad :(");
                loadingPO = false;
                return;
            }

            int index = tempUserInput.IndexOf(';');
            string sn = tempUserInput.Substring(3, index-3);
            string mat = tempUserInput.Substring(index + 5, tempUserInput.Length - (index - 5));
            
            
            
            loadingPO = true;
            StateHasChanged();
        }
    }
}
