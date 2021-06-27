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
        #endregion

        #region HardcodedStorageCodes
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

        #region CommentsModal
        private bool commentsModalVisible = false;
        private CooisComponent componentCommented;
        private string enteredCommentText;
        #endregion

        Table<CooisComponent> pickTable { get; set; }

        private bool loadingPO = false;
        private bool collectedItemsVisibleInMainList = false;
        private bool hiddenItemsVisibleInMainList = false;
        private string? tempUserInput;
        int totalItems = 0;

        List<CooisComponent> PickDataHidden = new List<CooisComponent>();
        List<CooisComponent> PickDataSorted = new List<CooisComponent>();
        List<CooisComponent> PickData = new List<CooisComponent>();
        private string editingComponentID;

        private void Refresh()
        {
            StateHasChanged();
            pickTable.ReloadData();
        }

        private async Task SubmitProdOrder()
        {
            if (string.IsNullOrEmpty(tempUserInput))
            {
                _ = messageService.Error("Oopsie whoopsie, Wumpus sad :(");
                loadingPO = false;
                return;
            }

            string serialNo = tempUserInput.Split(';')[0];
            string material = tempUserInput.Split(';')[0];
            // ask patryk which way best TODO
            // string[] uInputs = tempUserInput.Split(';');
            // serialNo = uInputs[0];
            // material = uInputs[1];
            
            loadingPO = true;
            StateHasChanged();
        }
    }
}
