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
        IFileUtil fileService { get; set; }
        [Inject]
        MessageService messageService { get; set; }
        [Inject]
        ICooisService cooisService { get; set; }
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
        private int? productionOrder;
        int totalItems = 0;

        List<CooisComponent> PickDataHidden = new List<CooisComponent>();
        List<CooisComponent> PickDataCollected = new List<CooisComponent>();
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
            if (!productionOrder.HasValue ||
                productionOrder.Value.ToString().Length > 10)
            {
                _ = messageService.Error(productionOrder + " is not a vaild Production Order number.");
                loadingPO = false;
                return;
            }
            loadingPO = true;
            StateHasChanged();

            try
            {
                PickData = await cooisService.GetCooisComponents(productionOrder.Value);
            }
            catch (HttpRequestException e)
            {
                switch (e.StatusCode)
                {
                    case System.Net.HttpStatusCode.NotFound:
                        _ = messageService.Error("Error connecting to the server.");
                        loadingPO = false;
                        return;
                        
                    case System.Net.HttpStatusCode.BadRequest:
                        _ = messageService.Error(productionOrder + " does not exist on the server.");
                        loadingPO = false;
                        return;
                }

                if (e.Message.Contains("Failed to fetch"))
                {
                    _ = messageService.Error("Error connecting to the server.");
                    loadingPO = false;
                    return;
                }

                _ = messageService.Error("Unknown error has occured. Please contact Business Systems");
                loadingPO = false;
                return;
            }

            PickDataHidden.Clear();
            PickDataCollected.Clear();
            PickDataSorted.Clear();

            foreach (CooisComponent row in PickData)
            {
                //if batch and material already in, add quanitiy
                CooisComponent matchedRow = PickDataSorted.Find(e => e.Batch == row.Batch && e.Material == row.Material);
                if (matchedRow == null)
                {
                    PickDataSorted.Add(row);
                }
                else
                {
                    PickDataSorted.Remove(matchedRow);
                    matchedRow.RequirementQty += row.RequirementQty;
                    PickDataSorted.Add(matchedRow);
                }

                //If it's a bulk material, phantom item, or has no quanity - hide the component.
                if (row.BulkMaterial == true || row.PhantomItem == true || row.RequirementQty == 0)
                {
                    HideComponent(row);
                }
            }

            totalItems = PickDataSorted.Count + PickDataHidden.Count;
            loadingPO = false;

            Refresh();

            return;
        }

        #region SaveLoad
        /// <remarks>
        /// This function is automatically run when the API returns after the user uploads a file,
        /// Here we set all the lists in the object.
        /// </remarks>
        void OnLoadFile(UploadInfo fileinfo)
        {
            if (fileinfo.File.State == UploadState.Success)
            {
                string result = fileinfo.File.GetResponse<string>();

                PickDataHidden.Clear();
                PickDataCollected.Clear();
                PickDataSorted.Clear();

                //Each line contains a component in the picklist.
                foreach (string line in result.Split("\n"))
                {
                    string[] temp = line.Split(',');

                    //This makes sure the imported line contains data and isn't empty.
                    if (temp.Count() < 10)
                    {
                        continue;
                    }

                    CooisComponent coois = new CooisComponent();
                    coois.ComponentId = temp[0];
                    coois.Material = temp[1];
                    coois.RequirementQty = decimal.Parse(temp[2]);
                    coois.QtyWithdrawn = decimal.Parse(temp[3]);
                    coois.BaseUoM = temp[4];
                    coois.ProdOrder = long.Parse(temp[5]);
                    coois.RequirementNo = int.Parse(temp[6]);
                    coois.Batch = temp[7];
                    coois.StorageLocation = temp[8];
                    coois.StatusId = temp[9];
                    coois.Collected = bool.Parse(temp[10]);
                    coois.Comment = System.Web.HttpUtility.UrlDecode(temp[12]);

                    if (temp[11] == "True")
                    {
                        PickDataHidden.Add(coois);
                    }
                    else if (temp[10] == "True")
                    {
                        PickDataCollected.Add(coois);
                    }
                    else
                    {
                        PickDataSorted.Add(coois);
                    }
                }
                totalItems = PickDataSorted.Count + PickDataHidden.Count; //result.split("\n").count
            }
        }

        /// <remarks>
        ///     This function can be improved by merging the three lists and using one for loop.
        ///     However that would result in added complexity, but the same O notation ( O(N))
        /// </remarks>
        private async Task SaveListToDisk()
        {
            string csvData = "";

            foreach (CooisComponent row in PickDataSorted)
            {
                csvData += row.ComponentId + ",";
                csvData += row.Material + ",";
                csvData += row.RequirementQty + ",";
                csvData += row.QtyWithdrawn + ",";
                csvData += row.BaseUoM + ",";
                csvData += row.ProdOrder + ",";
                csvData += row.RequirementNo + ",";
                csvData += row.Batch + ",";
                csvData += row.StorageLocation + ",";
                csvData += row.StatusId + ",";
                csvData += row.Collected + ",";
                csvData += "False,";
                csvData += System.Web.HttpUtility.UrlEncode(row.Comment) + "\n";
            }

            foreach (CooisComponent row in PickDataHidden)
            {
                csvData += row.ComponentId + ",";
                csvData += row.Material + ",";
                csvData += row.RequirementQty + ",";
                csvData += row.QtyWithdrawn + ",";
                csvData += row.BaseUoM + ",";
                csvData += row.ProdOrder + ",";
                csvData += row.RequirementNo + ",";
                csvData += row.Batch + ",";
                csvData += row.StorageLocation + ",";
                csvData += row.StatusId + ",";
                csvData += row.Collected + ",";
                csvData += "True,";
                csvData += System.Web.HttpUtility.UrlEncode(row.Comment) + "\n";
            }

            foreach (CooisComponent row in PickDataCollected)
            {
                csvData += row.ComponentId + ",";
                csvData += row.Material + ",";
                csvData += row.RequirementQty + ",";
                csvData += row.QtyWithdrawn + ",";
                csvData += row.BaseUoM + ",";
                csvData += row.ProdOrder + ",";
                csvData += row.RequirementNo + ",";
                csvData += row.Batch + ",";
                csvData += row.StorageLocation + ",";
                csvData += row.StatusId + ",";
                csvData += row.Collected + ",";
                csvData += "False,";
                csvData += System.Web.HttpUtility.UrlEncode(row.Comment) + "\n";
            }

            await fileService.SaveAs(js, productionOrder + "-pickData" +".csv", System.Text.Encoding.UTF8.GetBytes(csvData));
        }
        #endregion

        #region HideLogic
        private void HideComponent(CooisComponent toHide)
        {
            toHide.Hidden = true;
            if (!hiddenItemsVisibleInMainList)
            {
                PickDataHidden.Add(toHide);
                PickDataSorted.Remove(toHide);
            }

            Refresh();
        }

        private void UnhideComponent (CooisComponent toUnHide)
        {
            toUnHide.Hidden = false;

            Refresh();
        }

        #endregion

        #region GroupLogic
        private void UnhideAllComponents()
        {
            hiddenItemsVisibleInMainList = true;
            PickDataSorted.AddRange(PickDataHidden);
            PickDataHidden.Clear();

            Refresh();
        }

        private void HideAllComponents()
        {
            hiddenItemsVisibleInMainList = false;
            List<CooisComponent> temp = new List<CooisComponent>();
            foreach (CooisComponent item in PickDataSorted)
            {
                if (item.Hidden)
                {
                    temp.Add(item);
                    PickDataHidden.Add(item);
                }
            }
            foreach (CooisComponent item in temp)
            {
                PickDataSorted.Remove(item);
            }

            Refresh();
        }

        private void ShowCollectedItems()
        {
            collectedItemsVisibleInMainList = true;
            PickDataSorted.AddRange(PickDataCollected);
            PickDataCollected.Clear();

            Refresh();
        }

        private void HideCollectedItems()
        {
            collectedItemsVisibleInMainList = false;
            List<CooisComponent> tempRems = new List<CooisComponent>();
            foreach (CooisComponent item in PickDataSorted)
            {
                if (item.Collected)
                {
                    tempRems.Add(item);
                    PickDataCollected.Add(item);
                }
            }
            foreach (CooisComponent item in tempRems)
            {
                PickDataSorted.Remove(item);
            }

            Refresh();
        }
        #endregion

        #region CommentsModal
        private void ShowCommentsModal(CooisComponent comp)
        {
            enteredCommentText = comp.Comment;
            
            commentsModalVisible = true;
            componentCommented = comp;
            StateHasChanged();
        }

        private void HideCommentsModal()
        {
            commentsModalVisible = false;

            StateHasChanged();
        }

        private void SaveCommentsToObject()
        {
            PickDataSorted.Find(e=>e.ComponentId==componentCommented.ComponentId).Comment = enteredCommentText;

            HideCommentsModal();
        }
        #endregion

        #region Collection Code

        private void CollectComponent(CooisComponent collected)
        {
            if (collected.Hidden)
            {
                _ = messageService.Error("Unable to collect hidden item, please unhide this item and then collect.");
                return;
            }
            //Here we remove it from the main list, if the setting is false.
            if (!collectedItemsVisibleInMainList)
            {
                PickDataSorted.Remove(collected);
                PickDataCollected.Add(collected);
            }
            collected.Collected = true;

            Refresh();
        }

        private void UncollectComponent(CooisComponent uncollected)
        {
            //List<>.Remove does not throw an exception if an object is not in the list before hand.
            //This means we can safely support all behaviours without a branch.
            PickDataSorted.Remove(uncollected);
            PickDataCollected.Remove(uncollected);
            uncollected.Collected = false;
            PickDataSorted.Add(uncollected);

            Refresh();
        }
        #endregion

        #region Batch Code Editor
        private void startEditingBatch(string id)
        {
            editingComponentID = id;
        }

        private void finishEditingBatch()
        {
            editingComponentID = null;
        }

        private string findStorLoc(string storCode)
        {
            string storName;
            try
            {
                storName = storageLocs[storCode];
            }
            catch (KeyNotFoundException)
            {
                storName = storCode;
            }
            catch (ArgumentNullException)
            {
                storName = "";
            }

            return storName;
        }
        #endregion
    }
}
