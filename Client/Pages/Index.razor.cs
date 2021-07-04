using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Scan.Shared;
using AntDesign;
using Google.Protobuf;
using Grpc.Core;
using Microsoft.AspNetCore.Components;
using Tag = Scan.Shared.Tag;

namespace Scan.Client.Pages
{
    public partial class Index
    {
        [Inject] private MessageService MessageService { get; set; }
        [Inject] private ScanServices.ScanServicesClient ScanClient { get; set; }

        private Dictionary<string, string> _storageLocations = new() 
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
        
        private bool _loadingData;
        private string _scannerText;
        private Input<string> _scannerInputReference;
        private ScanData dataInTag;
        
        private async Task SubmitScanData()
        {
            if (string.IsNullOrEmpty(_scannerText))
            {
                _ = MessageService.Error("Oopsie whoopsie, Wumpus sad :("); //Input can not be null
                _loadingData = false;
                return;
            }

            Tag tagData;

            try
            {
                tagData = GenerateTag();
            }
            catch (InvalidOperationException)
            {
                _ = MessageService.Error("Oopsie whoopsie, Wumpus Client sad :("); //Invalid Tag Data
                _loadingData = false;
                return;
            }

            _loadingData = true;
            StateHasChanged();
            
            ScanData data;
            try
            {
                data = await ScanClient.GetScanDataAsync(tagData);
            }
            catch (RpcException e)
            {
                _ = MessageService.Error(e.Status.Detail);
                _loadingData = false;
                return;
            }

            _loadingData = false;
            dataInTag = data;
        }
        
        private Tag GenerateTag()
        {
            int indexOfDelimiter = _scannerText.IndexOf(';');
            string serialNumber = _scannerText.Substring(3, indexOfDelimiter - 3);
            string material = _scannerText.Substring(indexOfDelimiter + 5);

            Tag tagData = new()
            {
                Material = material, 
                SNum = serialNumber
            };

            return tagData;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await Task.Yield();
                await _scannerInputReference.Focus();
            }
            
            await base.OnAfterRenderAsync(firstRender);
        }
    }
}
