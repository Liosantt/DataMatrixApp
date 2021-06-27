using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Bunit;
using AngleSharp.Dom;
using MoreLinq;

namespace Scan.Tests
{
    public class IndexTests : ScanTestContext
    {
        [Fact]
        public void RendersEmptyTable()
        {
            var cut = Context.RenderComponent<Client.Pages.Index>();

            IElement tableElement = cut.Find(".ant-table");
            Assert.True(tableElement != null);
            Assert.True(tableElement.HasChildNodes);
            
            IElement emptyDataImage = cut.Find(".ant-empty");
            Assert.True(emptyDataImage != null);
        }

        [Fact]
        public void RendersMultipleMaterials()
        {
            var cut = Context.RenderComponent<Client.Pages.Index>();
            var msgholder = Context.RenderComponent<AntDesign.Message>();

            var inputCmp = cut.FindComponent<AntDesign.Input<int?>>(); //This finds the PO input box

            inputCmp.SetParametersAndRender(("DebounceMilliseconds", 0));

            inputCmp.Find(".ant-input").Input("5");
            inputCmp.Find(".ant-input").KeyUp("5");

            cut.Find(".ant-btn").Click();

            IElement tableElement = cut.Find(".ant-table");
            Assert.True(tableElement != null);
            Assert.True(tableElement.HasChildNodes);

            msgholder.Render();
            try
            {
                //No message
                Assert.Null(msgholder.Find(".ant-message"));
            }
            catch (Bunit.ElementNotFoundException) { }

            try
            {
                //No empty warning
                Assert.Null(cut.Find(".ant-empty"));
            }
            catch (Bunit.ElementNotFoundException) { }


            for (int i = 1; i < TestCooisService.coois[5].Count; i++)
            {
                IElement createdRow = cut.Find("tbody tr:nth-child(" + i.ToString() + ")");

                Assert.Equal(TestCooisService.coois[5][i - 1].Material, createdRow.Children[0].TextContent);
                Assert.Equal(TestCooisService.coois[5][i - 1].RequirementQty.ToString(), createdRow.Children[1].TextContent);
                Assert.Equal(TestCooisService.coois[5][i - 1].BaseUoM, createdRow.Children[2].TextContent);
                Assert.Equal(TestCooisService.coois[5][i - 1].Batch, createdRow.Children[3].TextContent);
                Assert.Equal(TestCooisService.coois[5][i - 1].StorageLocation, createdRow.Children[4].TextContent);
            }
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void RendersCorrectSingleMaterial(int PO)
        {
            var cut = Context.RenderComponent<Client.Pages.Index>();
            var msgholder = Context.RenderComponent<AntDesign.Message>();

            var inputCmp = cut.FindComponent<AntDesign.Input<int?>>(); //This finds the PO input box

            inputCmp.SetParametersAndRender(("DebounceMilliseconds", 0));

            inputCmp.Find(".ant-input").Input(PO.ToString());
            inputCmp.Find(".ant-input").KeyUp(PO.ToString());

            cut.Find(".ant-btn").Click();

            IElement tableElement = cut.Find(".ant-table");
            Assert.True(tableElement != null);
            Assert.True(tableElement.HasChildNodes);

            msgholder.Render();
            try
            {
                //No message
                Assert.Null(msgholder.Find(".ant-message"));
            }
            catch (Bunit.ElementNotFoundException) { }

            try
            {
                //No empty warning
                Assert.Null(cut.Find(".ant-empty"));
            }
            catch (Bunit.ElementNotFoundException) { }


            IElement createdRow = cut.Find("tbody tr:nth-child(1)");

            Assert.Equal(TestCooisService.coois[PO][0].Material, createdRow.Children[0].TextContent);
            Assert.Equal(TestCooisService.coois[PO][0].RequirementQty.ToString(), createdRow.Children[1].TextContent);
            Assert.Equal(TestCooisService.coois[PO][0].BaseUoM, createdRow.Children[2].TextContent);
            Assert.Equal(TestCooisService.coois[PO][0].Batch, createdRow.Children[3].TextContent);
            Assert.Equal(TestCooisService.coois[PO][0].StorageLocation, createdRow.Children[4].TextContent);
        }

        [Theory]
        [InlineData(404)]
        [InlineData(532)]
        [InlineData(100)]
        public void RendersMessageWhenNotFound(int PO)
        {
            var cut = Context.RenderComponent<Client.Pages.Index>();
            var msgholder = Context.RenderComponent<AntDesign.Message>();

            var inputCmp = cut.FindComponent<AntDesign.Input<int?>>(); //This finds the PO input box

            inputCmp.SetParametersAndRender(("DebounceMilliseconds", 0));

            inputCmp.Find(".ant-input").Input(PO.ToString());
            inputCmp.Find(".ant-input").KeyUp(PO.ToString());

            cut.Find(".ant-btn").Click();

            IElement tableElement = cut.Find(".ant-table");
            Assert.True(tableElement != null);
            Assert.True(tableElement.HasChildNodes);

            msgholder.Render();
            string messageValue = msgholder.Find(".ant-message-custom-content:last-child").TextContent;

            Assert.Contains("not exist", messageValue.ToLower());
        }
        
        /// <summary>
        /// Test if dictionary works for all storage locations and blanks and nulls
        /// </summary>
        /// <param name="storageCode">Code to be replaced</param>
        /// <param name="storageName">Expected Output</param>
        [Theory]
        [InlineData("NewLocation", "NewLocation")]
        [InlineData(null, "")]
        [InlineData("", "")]
        [InlineData("FMBA", "Main Store")]
        [InlineData("FMBB", "Finished Goods")]
        [InlineData("FMBC", "Clean Room")]
        [InlineData("FMBD", "Bar Store")]
        [InlineData("FMBE", "Mould Shop")]
        [InlineData("FMBF", "Wire Store")]
        [InlineData("FMBG", "Gas Store")]
        [InlineData("FMBH", "LCM Garage")]
        [InlineData("FMBI", "Machine Shop")]
        [InlineData("FMBM", "Quarantine")]
        [InlineData("FMBN", "Stock for Scrap")]
        [InlineData("FMBO", "Fault Investigations")]
        [InlineData("FMBR", "Kardex 1")]
        [InlineData("FMBS", "Kardex 2")]
        [InlineData("FMBT", "Production Engineering")]
        public void ReplacesStorageLocation(string storageCode, string storageName)
        {
            TestCooisService.coois[1][0].StorageLocation = storageCode;

            var cut = Context.RenderComponent<Client.Pages.Index>();
            var msgholder = Context.RenderComponent<AntDesign.Message>();

            var inputCmp = cut.FindComponent<AntDesign.Input<int?>>(); //This finds the PO input box

            inputCmp.SetParametersAndRender(("DebounceMilliseconds", 0));

            inputCmp.Find(".ant-input").Input("1");
            inputCmp.Find(".ant-input").KeyUp("1");

            cut.Find(".ant-btn").Click();

            IElement tableElement = cut.Find(".ant-table");
            Assert.True(tableElement != null);
            Assert.True(tableElement.HasChildNodes);

            msgholder.Render();
            try
            {
                //No message
                Assert.Null(msgholder.Find(".ant-message"));
            }
            catch (Bunit.ElementNotFoundException) { }

            try
            {
                //No empty warning
                Assert.Null(cut.Find(".ant-empty"));
            }
            catch (Bunit.ElementNotFoundException) { }


            IElement createdRow = cut.Find("tbody tr:nth-child(1)");

            Assert.Equal(storageName, createdRow.Children[4].TextContent);


            TestCooisService.coois[1][0].StorageLocation = "STORAGE";
        }

        [Fact]
        public void RendersMessageWhenLongerThan10Chars()
        {
            var cut = Context.RenderComponent<Client.Pages.Index>();
            var msgholder = Context.RenderComponent<AntDesign.Message>();

            var inputCmp = cut.FindComponent<AntDesign.Input<int?>>(); //This finds the PO input box

            inputCmp.SetParametersAndRender(("DebounceMilliseconds", 0));

            inputCmp.Find(".ant-input").Input("1234567891011");
            inputCmp.Find(".ant-input").KeyUp("1234567891011");

            cut.Find(".ant-btn").Click();

            IElement tableElement = cut.Find(".ant-table");
            Assert.True(tableElement != null);
            Assert.True(tableElement.HasChildNodes);

            msgholder.Render();
            string messageValue = msgholder.Find(".ant-message-custom-content:last-child").TextContent;

            Assert.Contains("not a vaild", messageValue.ToLower());
        }

        [Theory]
        [InlineData("TEST")]
        [InlineData("One")]
        [InlineData("TEST1")]
        [InlineData("1test")]
        [InlineData("")]
        public void RendersMessageWhenTextInPO(string PO)
        {
            var cut = Context.RenderComponent<Client.Pages.Index>();
            var msgholder = Context.RenderComponent<AntDesign.Message>();

            var inputCmp = cut.FindComponent<AntDesign.Input<int?>>(); //This finds the PO input box

            inputCmp.SetParametersAndRender(("DebounceMilliseconds", 0));

            inputCmp.Find(".ant-input").Input(PO);
            inputCmp.Find(".ant-input").KeyUp(PO);

            cut.Find(".ant-btn").Click();

            IElement tableElement = cut.Find(".ant-table");
            Assert.True(tableElement != null);
            Assert.True(tableElement.HasChildNodes);

            msgholder.Render();
            string messageValue = msgholder.Find(".ant-message-custom-content:last-child").TextContent;

            Assert.Contains("not a vaild", messageValue.ToLower());
        }

        [Theory]
        [InlineData("-1")]
        [InlineData("-2")]
        public void RendersMessageWhenOffline(string PO)
        {
            var cut = Context.RenderComponent<Client.Pages.Index>();
            var msgholder = Context.RenderComponent<AntDesign.Message>();

            var inputCmp = cut.FindComponent<AntDesign.Input<int?>>(); //This finds the PO input box

            inputCmp.SetParametersAndRender(("DebounceMilliseconds", 0));

            inputCmp.Find(".ant-input").Input(PO);
            inputCmp.Find(".ant-input").KeyUp(PO);

            cut.Find(".ant-btn").Click();

            IElement tableElement = cut.Find(".ant-table");
            Assert.True(tableElement != null);
            Assert.True(tableElement.HasChildNodes);

            msgholder.Render();
            string messageValue = msgholder.Find(".ant-message-custom-content:last-child").TextContent;

            Assert.Contains("connecting", messageValue.ToLower());
        }

        [Fact]
        public void RendersMessageWhenErrorIsUnknown()
        {
            var cut = Context.RenderComponent<Client.Pages.Index>();
            var msgholder = Context.RenderComponent<AntDesign.Message>();

            var inputCmp = cut.FindComponent<AntDesign.Input<int?>>(); //This finds the PO input box

            inputCmp.SetParametersAndRender(("DebounceMilliseconds", 0));

            inputCmp.Find(".ant-input").Input("-3");
            inputCmp.Find(".ant-input").KeyUp("-3");

            cut.Find(".ant-btn").Click();

            IElement tableElement = cut.Find(".ant-table");
            Assert.True(tableElement != null);
            Assert.True(tableElement.HasChildNodes);

            msgholder.Render();
            string messageValue = msgholder.Find(".ant-message-custom-content:last-child").TextContent;

            Assert.Contains("unknown", messageValue.ToLower());
        }

        [Fact]
        public void MaterialsAggregatedBasic()
        {
            var cut = Context.RenderComponent<Client.Pages.Index>();
            var msgholder = Context.RenderComponent<AntDesign.Message>();

            var inputCmp = cut.FindComponent<AntDesign.Input<int?>>(); //This finds the PO input box

            inputCmp.SetParametersAndRender(("DebounceMilliseconds", 0));

            inputCmp.Find(".ant-input").Input("3");
            inputCmp.Find(".ant-input").KeyUp("3");

            cut.Find(".ant-btn").Click();

            IElement tableElement = cut.Find(".ant-table");
            Assert.True(tableElement != null);
            Assert.True(tableElement.HasChildNodes);

            msgholder.Render();
            try
            {
                //No message
                Assert.Null(msgholder.Find(".ant-message"));
            }
            catch (Bunit.ElementNotFoundException) { }

            try
            {
                //No empty warning
                Assert.Null(cut.Find(".ant-empty"));
            }
            catch (Bunit.ElementNotFoundException) { }


            IElement createdRow = cut.Find("tbody tr:nth-child(1)");
            Assert.Equal(12.ToString(), createdRow.Children[1].TextContent);
        }

        [Fact]
        public void MaterialsAggregatedWhenNotNextToEachother()
        {
            var cut = Context.RenderComponent<Client.Pages.Index>();
            var msgholder = Context.RenderComponent<AntDesign.Message>();

            var inputCmp = cut.FindComponent<AntDesign.Input<int?>>(); //This finds the PO input box

            inputCmp.SetParametersAndRender(("DebounceMilliseconds", 0));

            inputCmp.Find(".ant-input").Input("4");
            inputCmp.Find(".ant-input").KeyUp("4");

            cut.Find(".ant-btn").Click();

            IElement tableElement = cut.Find(".ant-table");
            Assert.True(tableElement != null);
            Assert.True(tableElement.HasChildNodes);

            msgholder.Render();
            try
            {
                //No message
                Assert.Null(msgholder.Find(".ant-message"));
            }
            catch (Bunit.ElementNotFoundException) { }

            try
            {
                //No empty warning
                Assert.Null(cut.Find(".ant-empty"));
            }
            catch (Bunit.ElementNotFoundException) { }


            IElement createdRow = cut.Find("tbody tr:nth-child(2)");
            Assert.Equal(7.ToString(), createdRow.Children[1].TextContent);
        }

        [Theory]
        [InlineData("6")]
        [InlineData("7")]
        [InlineData("8")]
        [InlineData("9")]
        [InlineData("10")]
        public void NoShowPhanItemsBulkReqZero(string PO)
        {
            var cut = Context.RenderComponent<Client.Pages.Index>();
            var msgholder = Context.RenderComponent<AntDesign.Message>();

            var inputCmp = cut.FindComponent<AntDesign.Input<int?>>(); //This finds the PO input box

            inputCmp.SetParametersAndRender(("DebounceMilliseconds", 0));

            inputCmp.Find(".ant-input").Input(PO);
            inputCmp.Find(".ant-input").KeyUp(PO);

            cut.Find(".ant-btn").Click();

            IElement tableElement = cut.Find(".ant-table");
            Assert.True(tableElement != null);
            Assert.True(tableElement.HasChildNodes);

            msgholder.Render();
            try
            {
                //No message
                Assert.Null(msgholder.Find(".ant-message"));
            }
            catch (Bunit.ElementNotFoundException) { }

            try
            {
                //No empty warning
                Assert.Null(cut.Find(".ant-empty"));
            }
            catch (Bunit.ElementNotFoundException) { }

            try
            {
                //No two rows
                Assert.Null(cut.Find("tbody tr:nth-child(2)"));
            }
            catch (Bunit.ElementNotFoundException) { }
        }

        /*
        [Theory]
        [InlineData("11")]
        [InlineData("12")]
        [InlineData("13")]
        public void TotalMaterialsDisplayCorrect(string PO)
        {
            var cut = Context.RenderComponent<Client.Pages.Index>();
            var msgholder = Context.RenderComponent<AntDesign.Message>();

            var inputCmp = cut.FindComponent<AntDesign.Input<int?>>(); //This finds the PO input box

            inputCmp.SetParametersAndRender(("DebounceMilliseconds", 0));

            inputCmp.Find(".ant-input").Input(PO);
            inputCmp.Find(".ant-input").KeyUp(PO);

            cut.Find(".ant-btn").Click();

            IElement tableElement = cut.Find(".ant-table");
            Assert.True(tableElement != null);
            Assert.True(tableElement.HasChildNodes);

            msgholder.Render();
            try
            {
                //No message
                Assert.Null(msgholder.Find(".ant-message"));
            }
            catch (Bunit.ElementNotFoundException) { }

            try
            {
                //No empty warning
                Assert.Null(cut.Find(".ant-empty"));
            }
            catch (Bunit.ElementNotFoundException) { }

            //This gets the "out of" value
            int indexOfOut = cut.Markup.IndexOf(" out of ") + 8;

            Assert.Equal(TestCooisService.coois[int.Parse(PO)].DistinctBy(e => e.Material).Where(e => e.BulkMaterial == false &&
                                                                             e.PhantomItem == false &&
                                                                             e.RequirementQty > 0).Count().ToString()
                , cut.Markup.Substring(indexOfOut, cut.Markup.IndexOf("</span>", indexOfOut) - (indexOfOut)).Trim());
        }
        */

        [Fact]
        public void CanHideComponentAndDisappear()
        {
            var cut = Context.RenderComponent<Client.Pages.Index>();
            var msgholder = Context.RenderComponent<AntDesign.Message>();

            var inputCmp = cut.FindComponent<AntDesign.Input<int?>>(); //This finds the PO input box

            inputCmp.SetParametersAndRender(("DebounceMilliseconds", 0));

            inputCmp.Find(".ant-input").Input("11");
            inputCmp.Find(".ant-input").KeyUp("11");

            cut.Find(".ant-btn").Click();

            IElement tableElement = cut.Find(".ant-table");
            Assert.True(tableElement != null);
            Assert.True(tableElement.HasChildNodes);

            IElement rowToHide = cut.Find("tbody tr:nth-child(2)");

            string rowMaterial = rowToHide.Children[0].TextContent;

            cut.FindAll("tbody tr:nth-child(2) .ant-btn")[1].Click();

            IElement rowPushedDown = cut.Find("tbody tr:nth-child(2)");

            string rowDownMaterial = rowPushedDown.Children[0].TextContent;

            Assert.NotEqual(rowMaterial, rowDownMaterial);
        }
        /*
        [Fact]
        public async Task RendersCounterCorrectlyAfterHide()
        {
            var cut = Context.RenderComponent<Client.Pages.Index>();
            var msgholder = Context.RenderComponent<AntDesign.Message>();

            var inputCmp = cut.FindComponent<AntDesign.Input<int?>>(); //This finds the PO input box

            inputCmp.SetParametersAndRender(("DebounceMilliseconds", 0));

            inputCmp.Find(".ant-input").Input("11");
            inputCmp.Find(".ant-input").KeyUp("11");

            cut.Find(".ant-btn").Click();

            IElement tableElement = cut.Find(".ant-table");
            Assert.True(tableElement != null);
            Assert.True(tableElement.HasChildNodes);

            int indexOfOut = cut.Markup.IndexOf(" out of ") + 8;
            int beforeCount = int.Parse(cut.Markup.Substring(indexOfOut, cut.Markup.IndexOf("</span>", indexOfOut) - (indexOfOut)).Trim());

            cut.FindAll("tbody tr:nth-child(2) .ant-btn")[1].Click();

            cut.Render();

            //This gets the "out of" value
            indexOfOut = cut.Markup.IndexOf(" out of ") + 8;
            int afterCount = int.Parse(cut.Markup.Substring(indexOfOut, cut.Markup.IndexOf("</span>", indexOfOut) - (indexOfOut)).Trim());

            if (!(afterCount < beforeCount))
            {
                Console.WriteLine();
            }

            Assert.True(afterCount < beforeCount);
        }*/
    }
}
