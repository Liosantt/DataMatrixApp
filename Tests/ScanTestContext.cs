using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using Bunit.JSInterop;
using System.Text.Json;
using Scan.Shared;

namespace Scan.Tests
{
    public class ScanTestContext
    {
        public TestContext Context { get; }
        public TestNavigationManager NavigationManager { get; }

        public ScanTestContext()
        {
            Context = new TestContext();
            NavigationManager = new TestNavigationManager();

            Context.Services.AddScoped<NavigationManager>(sp => NavigationManager);

            #region AntDesign Services
            Context.Services.AddAntDesign();
            AntDesign.JsInterop.Window testWindow = new AntDesign.JsInterop.Window();
            testWindow.innerWidth = 10000;
            testWindow.innerHeight = 10000;

            Context.JSInterop.Setup<AntDesign.JsInterop.Window>(AntDesign.JSInteropConstants.GetWindow).SetResult(testWindow);
            Context.JSInterop.SetupVoid(AntDesign.JSInteropConstants.AddFileClickEventListener, invocationMatcher: _ => true);
            Context.Services.AddScoped<AntDesign.JsInterop.DomEventService>(sp => new TestDomEventService(Context.JSInterop.JSRuntime));
            #endregion

            #region Scan Services
            Context.Services.AddScoped<ICooisService, TestCooisService>();
            Context.Services.AddSingleton<IFileUtil, TestFileUtil>(sp => new TestFileUtil());
            #endregion
        }

        public void Dispose()
        {
            Context?.Dispose();
        }
    }

    public class TestNavigationManager : NavigationManager
    {
        public delegate void NavigatedCallback(string uri, bool forceLoad);

        public TestNavigationManager()
        {
            Initialize("http://localhost/", "http://localhost/");
        }

        protected override void NavigateToCore(string uri, bool forceLoad)
        {
            Navigated?.Invoke(uri, forceLoad);
        }

        public event NavigatedCallback Navigated;
    }

    public class TestFileUtil : IFileUtil
    {
        public Task SaveAs(object js, string filename, byte[] data)
        {
            string testValid = System.Text.Encoding.UTF8.GetString(data);

            return Task.CompletedTask;
        }
    }

    public class TestDomEventService : AntDesign.JsInterop.DomEventService
    {
        public TestDomEventService(IJSRuntime _js) : base(_js)
        {

        }

        public override void AddEventListener<T>(object dom, string eventName, Action<T> callback, bool exclusive = true, bool preventDefault = false)
        {
            return;
        }
    }

    public class TestCooisService : ICooisService
    {
        public static Dictionary<int, List<CooisComponent>> coois = new Dictionary<int, List<CooisComponent>>()
        {
            {
                1,
                new List<CooisComponent>()
                {
                    new CooisComponent()
                    {
                        Material = "test",
                        BaseUoM = "PC",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 5,
                        StatusId = "status",
                        StorageLocation = "STORAGE",
                        Batch = "test"
                    }
                }
            },
            {
                2,
                new List<CooisComponent>()
                {
                    new CooisComponent()
                    {
                        Material = "test2",
                        BaseUoM = "PC",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 5,
                        StatusId = "status",
                        StorageLocation = "STORAGE",
                        Batch = "test"
                    }
                }
            },
            {
                3,
                new List<CooisComponent>()
                {
                    new CooisComponent()
                    {
                        Material = "materialmerge",
                        BaseUoM = "PC",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 8,
                        StatusId = "status",
                        StorageLocation = "STORAGE",
                        Batch = "samebatch"
                    },
                    new CooisComponent()
                    {
                        Material = "materialmerge",
                        BaseUoM = "PC",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 4,
                        StatusId = "status",
                        StorageLocation = "STORAGE",
                        Batch = "samebatch"
                    }
                }
            },
            {
                4,
                new List<CooisComponent>()
                {
                    new CooisComponent()
                    {
                        Material = "materialmerge",
                        BaseUoM = "PC",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 3,
                        StatusId = "status",
                        StorageLocation = "STORAGE",
                        Batch = "samebatch"
                    },
                    new CooisComponent()
                    {
                        Material = "nomerge",
                        BaseUoM = "PC",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 4,
                        StatusId = "status",
                        StorageLocation = "STORAGE",
                        Batch = "samebatch"
                    },
                    new CooisComponent()
                    {
                        Material = "materialmerge",
                        BaseUoM = "PC",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 4,
                        StatusId = "status",
                        StorageLocation = "STORAGE",
                        Batch = "samebatch"
                    }
                }
            },
            {
                5,
                new List<CooisComponent>()
                {
                    new CooisComponent()
                    {
                        Material = "materialone",
                        BaseUoM = "PC",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 3,
                        StatusId = "status",
                        StorageLocation = "STORAGE",
                        Batch = "samebatch"
                    },
                    new CooisComponent()
                    {
                        Material = "materialtwo",
                        BaseUoM = "PC",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 4,
                        StatusId = "status",
                        StorageLocation = "STORAGE",
                        Batch = "samebatch"
                    },
                }
            },
            {
                6,
                new List<CooisComponent>()
                {
                    new CooisComponent()
                    {
                        Material = "noshow",
                        BaseUoM = "uomone",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = true,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 1,
                        StatusId = "status",
                        StorageLocation = "storone",
                        Batch = "batchone"
                    },
                    new CooisComponent()
                    {
                        Material = "mattwo",
                        BaseUoM = "uomtwo",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 2,
                        StatusId = "status",
                        StorageLocation = "stortwo",
                        Batch = "batchtwo"
                    }
                }
            },
            {
                7,
                new List<CooisComponent>()
                {
                    new CooisComponent()
                    {
                        Material = "noshow",
                        BaseUoM = "uomone",
                        BulkMaterial = true,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 1,
                        StatusId = "status",
                        StorageLocation = "storone",
                        Batch = "batchone"
                    },
                    new CooisComponent()
                    {
                        Material = "mattwo",
                        BaseUoM = "uomtwo",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 2,
                        StatusId = "status",
                        StorageLocation = "stortwo",
                        Batch = "batchtwo"
                    }
                }
            },
            {
                8,
                new List<CooisComponent>()
                {
                    new CooisComponent()
                    {
                        Material = "noshow",
                        BaseUoM = "uomone",
                        BulkMaterial = true,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = true,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 1,
                        StatusId = "status",
                        StorageLocation = "storone",
                        Batch = "batchone"
                    },
                    new CooisComponent()
                    {
                        Material = "mattwo",
                        BaseUoM = "uomtwo",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 1,
                        StatusId = "status",
                        StorageLocation = "stortwo",
                        Batch = "batchtwo"
                    }
                }
            },
            {
                9,
                new List<CooisComponent>()
                {
                    new CooisComponent()
                    {
                        Material = "noshow",
                        BaseUoM = "uomone",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = true,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 0,
                        StatusId = "status",
                        StorageLocation = "storone",
                        Batch = "batchone"
                    },
                    new CooisComponent()
                    {
                        Material = "mattwo",
                        BaseUoM = "uomtwo",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 2,
                        StatusId = "status",
                        StorageLocation = "stortwo",
                        Batch = "batchtwo"
                    }
                }
            },
            {
                10,
                new List<CooisComponent>()
                {
                    new CooisComponent()
                    {
                        Material = "noshow",
                        BaseUoM = "uomone",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = true,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 0,
                        StatusId = "status",
                        StorageLocation = "storone",
                        Batch = "batchone"
                    },
                    new CooisComponent()
                    {
                        Material = "noshowmetoo",
                        BaseUoM = "uomtwo",
                        BulkMaterial = true,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 2,
                        StatusId = "status",
                        StorageLocation = "stortwo",
                        Batch = "batchtwo"
                    },
                    new CooisComponent()
                    {
                        Material = "showmetho",
                        BaseUoM = "uomtwo",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 2,
                        StatusId = "status",
                        StorageLocation = "stortwo",
                        Batch = "batchtwo"
                    }
                }
            },
            {
                11,
                new List<CooisComponent>()
                {
                    new CooisComponent()
                    {
                        Material = "countmeup",
                        BaseUoM = "uomone",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 1,
                        StatusId = "status",
                        StorageLocation = "storone",
                        Batch = "batchone"
                    },
                    new CooisComponent()
                    {
                        Material = "countmeuptoo",
                        BaseUoM = "uomtwo",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 2,
                        StatusId = "status",
                        StorageLocation = "stortwo",
                        Batch = "batchtwo"
                    },
                    new CooisComponent()
                    {
                        Material = "countmeupthrice",
                        BaseUoM = "uomtwo",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 2,
                        StatusId = "status",
                        StorageLocation = "stortwo",
                        Batch = "batchtwo"
                    }
                }
            },
            {
                12,
                new List<CooisComponent>()
                {
                    new CooisComponent()
                    {
                        Material = "countmeup",
                        BaseUoM = "uomone",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 1,
                        StatusId = "status",
                        StorageLocation = "storone",
                        Batch = "batchone"
                    },
                    new CooisComponent()
                    {
                        Material = "dontcountme",
                        BaseUoM = "uomtwo",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = true,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 2,
                        StatusId = "status",
                        StorageLocation = "stortwo",
                        Batch = "batchtwo"
                    },
                    new CooisComponent()
                    {
                        Material = "countmeupthrice",
                        BaseUoM = "uomtwo",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 2,
                        StatusId = "status",
                        StorageLocation = "stortwo",
                        Batch = "batchtwo"
                    },
                    new CooisComponent()
                    {
                        Material = "orme",
                        BaseUoM = "uomtwo",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 0,
                        StatusId = "status",
                        StorageLocation = "stortwo",
                        Batch = "batchtwo"
                    },
                    new CooisComponent()
                    {
                        Material = "neither",
                        BaseUoM = "uomtwo",
                        BulkMaterial = true,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 1,
                        StatusId = "status",
                        StorageLocation = "stortwo",
                        Batch = "batchtwo"
                    }
                }
            },
            {
                13,
                new List<CooisComponent>()
                {
                    new CooisComponent()
                    {
                        Material = "countmeup",
                        BaseUoM = "uomone",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 1,
                        StatusId = "status",
                        StorageLocation = "storone",
                        Batch = "batchone"
                    },
                    new CooisComponent()
                    {
                        Material = "dontcountme",
                        BaseUoM = "uomtwo",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = true,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 2,
                        StatusId = "status",
                        StorageLocation = "stortwo",
                        Batch = "batchtwo"
                    },
                    new CooisComponent()
                    {
                        Material = "joinme",
                        BaseUoM = "uomtwo",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 2,
                        StatusId = "status",
                        StorageLocation = "stortwo",
                        Batch = "batchtwo"
                    },
                    new CooisComponent()
                    {
                        Material = "orme",
                        BaseUoM = "uomtwo",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 0,
                        StatusId = "status",
                        StorageLocation = "stortwo",
                        Batch = "batchtwo"
                    },
                    new CooisComponent()
                    {
                        Material = "joinme",
                        BaseUoM = "uomtwo",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 1,
                        StatusId = "status",
                        StorageLocation = "stortwo",
                        Batch = "batchtwo"
                    }
                }
            },
            {
                15,
                new List<CooisComponent>()
                {
                    new CooisComponent()
                    {
                        Material = "1",
                        BaseUoM = "uomtwo",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 1,
                        StatusId = "status",
                        StorageLocation = "stortwo",
                        Batch = "batchtwo"
                    },
                    new CooisComponent()
                    {
                        Material = "2",
                        BaseUoM = "uomtwo",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 1,
                        StatusId = "status",
                        StorageLocation = "stortwo",
                        Batch = "batchtwo"
                    },
                    new CooisComponent()
                    {
                        Material = "3",
                        BaseUoM = "uomtwo",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 1,
                        StatusId = "status",
                        StorageLocation = "stortwo",
                        Batch = "batchtwo"
                    },
                    new CooisComponent()
                    {
                        Material = "4",
                        BaseUoM = "uomtwo",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 1,
                        StatusId = "status",
                        StorageLocation = "stortwo",
                        Batch = "batchtwo"
                    },
                    new CooisComponent()
                    {
                        Material = "5",
                        BaseUoM = "uomtwo",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 1,
                        StatusId = "status",
                        StorageLocation = "stortwo",
                        Batch = "batchtwo"
                    },
                    new CooisComponent()
                    {
                        Material = "6",
                        BaseUoM = "uomtwo",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 1,
                        StatusId = "status",
                        StorageLocation = "stortwo",
                        Batch = "batchtwo"
                    },
                    new CooisComponent()
                    {
                        Material = "7",
                        BaseUoM = "uomtwo",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 1,
                        StatusId = "status",
                        StorageLocation = "stortwo",
                        Batch = "batchtwo"
                    },
                    new CooisComponent()
                    {
                        Material = "8",
                        BaseUoM = "uomtwo",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 1,
                        StatusId = "status",
                        StorageLocation = "stortwo",
                        Batch = "batchtwo"
                    },
                    new CooisComponent()
                    {
                        Material = "9",
                        BaseUoM = "uomtwo",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 1,
                        StatusId = "status",
                        StorageLocation = "stortwo",
                        Batch = "batchtwo"
                    },
                    new CooisComponent()
                    {
                        Material = "10",
                        BaseUoM = "uomtwo",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 1,
                        StatusId = "status",
                        StorageLocation = "stortwo",
                        Batch = "batchtwo"
                    },
                    new CooisComponent()
                    {
                        Material = "11",
                        BaseUoM = "uomtwo",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 1,
                        StatusId = "status",
                        StorageLocation = "stortwo",
                        Batch = "batchtwo"
                    },
                    new CooisComponent()
                    {
                        Material = "12",
                        BaseUoM = "uomtwo",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 1,
                        StatusId = "status",
                        StorageLocation = "stortwo",
                        Batch = "batchtwo"
                    },

                    new CooisComponent()
                    {
                        Material = "13",
                        BaseUoM = "uomtwo",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 1,
                        StatusId = "status",
                        StorageLocation = "stortwo",
                        Batch = "batchtwo"
                    },
                    new CooisComponent()
                    {
                        Material = "14",
                        BaseUoM = "uomtwo",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 1,
                        StatusId = "status",
                        StorageLocation = "stortwo",
                        Batch = "batchtwo"
                    },
                    new CooisComponent()
                    {
                        Material = "15",
                        BaseUoM = "uomtwo",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 1,
                        StatusId = "status",
                        StorageLocation = "stortwo",
                        Batch = "batchtwo"
                    },
                    new CooisComponent()
                    {
                        Material = "16",
                        BaseUoM = "uomtwo",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 1,
                        StatusId = "status",
                        StorageLocation = "stortwo",
                        Batch = "batchtwo"
                    },
                    new CooisComponent()
                    {
                        Material = "17",
                        BaseUoM = "uomtwo",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 1,
                        StatusId = "status",
                        StorageLocation = "stortwo",
                        Batch = "batchtwo"
                    },
                    new CooisComponent()
                    {
                        Material = "18",
                        BaseUoM = "uomtwo",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 1,
                        StatusId = "status",
                        StorageLocation = "stortwo",
                        Batch = "batchtwo"
                    },
                    new CooisComponent()
                    {
                        Material = "1",
                        BaseUoM = "uomtwo",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 1,
                        StatusId = "status",
                        StorageLocation = "stortwo",
                        Batch = "batchtwo"
                    },
                    new CooisComponent()
                    {
                        Material = "2",
                        BaseUoM = "uomtwo",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 1,
                        StatusId = "status",
                        StorageLocation = "stortwo",
                        Batch = "batchtwo"
                    },
                    new CooisComponent()
                    {
                        Material = "3",
                        BaseUoM = "uomtwo",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 1,
                        StatusId = "status",
                        StorageLocation = "stortwo",
                        Batch = "batchtwo"
                    },
                    new CooisComponent()
                    {
                        Material = "4",
                        BaseUoM = "uomtwo",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 1,
                        StatusId = "status",
                        StorageLocation = "stortwo",
                        Batch = "batchtwo"
                    },
                    new CooisComponent()
                    {
                        Material = "5",
                        BaseUoM = "uomtwo",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 1,
                        StatusId = "status",
                        StorageLocation = "stortwo",
                        Batch = "batchtwo"
                    },
                    new CooisComponent()
                    {
                        Material = "6",
                        BaseUoM = "uomtwo",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 1,
                        StatusId = "status",
                        StorageLocation = "stortwo",
                        Batch = "batchtwo"
                    },
                    new CooisComponent()
                    {
                        Material = "7",
                        BaseUoM = "uomtwo",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 1,
                        StatusId = "status",
                        StorageLocation = "stortwo",
                        Batch = "batchtwo"
                    },
                    new CooisComponent()
                    {
                        Material = "8",
                        BaseUoM = "uomtwo",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 1,
                        StatusId = "status",
                        StorageLocation = "stortwo",
                        Batch = "batchtwo"
                    },
                    new CooisComponent()
                    {
                        Material = "9",
                        BaseUoM = "uomtwo",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 1,
                        StatusId = "status",
                        StorageLocation = "stortwo",
                        Batch = "batchtwo"
                    },
                    new CooisComponent()
                    {
                        Material = "10",
                        BaseUoM = "uomtwo",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 1,
                        StatusId = "status",
                        StorageLocation = "stortwo",
                        Batch = "batchtwo"
                    },
                    new CooisComponent()
                    {
                        Material = "11",
                        BaseUoM = "uomtwo",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 1,
                        StatusId = "status",
                        StorageLocation = "stortwo",
                        Batch = "batchtwo"
                    },
                    new CooisComponent()
                    {
                        Material = "12",
                        BaseUoM = "uomtwo",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 1,
                        StatusId = "status",
                        StorageLocation = "stortwo",
                        Batch = "batchtwo"
                    },

                    new CooisComponent()
                    {
                        Material = "13",
                        BaseUoM = "uomtwo",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 1,
                        StatusId = "status",
                        StorageLocation = "stortwo",
                        Batch = "batchtwo"
                    },
                    new CooisComponent()
                    {
                        Material = "14",
                        BaseUoM = "uomtwo",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 1,
                        StatusId = "status",
                        StorageLocation = "stortwo",
                        Batch = "batchtwo"
                    },
                    new CooisComponent()
                    {
                        Material = "15",
                        BaseUoM = "uomtwo",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 1,
                        StatusId = "status",
                        StorageLocation = "stortwo",
                        Batch = "batchtwo"
                    },
                    new CooisComponent()
                    {
                        Material = "16",
                        BaseUoM = "uomtwo",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 1,
                        StatusId = "status",
                        StorageLocation = "stortwo",
                        Batch = "batchtwo"
                    },
                    new CooisComponent()
                    {
                        Material = "17",
                        BaseUoM = "uomtwo",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 1,
                        StatusId = "status",
                        StorageLocation = "stortwo",
                        Batch = "batchtwo"
                    },
                    new CooisComponent()
                    {
                        Material = "18",
                        BaseUoM = "uomtwo",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 1,
                        StatusId = "status",
                        StorageLocation = "stortwo",
                        Batch = "batchtwo"
                    },
                    new CooisComponent()
                    {
                        Material = "1",
                        BaseUoM = "uomtwo",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 1,
                        StatusId = "status",
                        StorageLocation = "stortwo",
                        Batch = "batchtwo"
                    },
                    new CooisComponent()
                    {
                        Material = "2",
                        BaseUoM = "uomtwo",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 1,
                        StatusId = "status",
                        StorageLocation = "stortwo",
                        Batch = "batchtwo"
                    },
                    new CooisComponent()
                    {
                        Material = "3",
                        BaseUoM = "uomtwo",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 1,
                        StatusId = "status",
                        StorageLocation = "stortwo",
                        Batch = "batchtwo"
                    },
                    new CooisComponent()
                    {
                        Material = "4",
                        BaseUoM = "uomtwo",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 1,
                        StatusId = "status",
                        StorageLocation = "stortwo",
                        Batch = "batchtwo"
                    },
                    new CooisComponent()
                    {
                        Material = "5",
                        BaseUoM = "uomtwo",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 1,
                        StatusId = "status",
                        StorageLocation = "stortwo",
                        Batch = "batchtwo"
                    },
                    new CooisComponent()
                    {
                        Material = "6",
                        BaseUoM = "uomtwo",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 1,
                        StatusId = "status",
                        StorageLocation = "stortwo",
                        Batch = "batchtwo"
                    },
                    new CooisComponent()
                    {
                        Material = "7",
                        BaseUoM = "uomtwo",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 1,
                        StatusId = "status",
                        StorageLocation = "stortwo",
                        Batch = "batchtwo"
                    },
                    new CooisComponent()
                    {
                        Material = "8",
                        BaseUoM = "uomtwo",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 1,
                        StatusId = "status",
                        StorageLocation = "stortwo",
                        Batch = "batchtwo"
                    },
                    new CooisComponent()
                    {
                        Material = "9",
                        BaseUoM = "uomtwo",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 1,
                        StatusId = "status",
                        StorageLocation = "stortwo",
                        Batch = "batchtwo"
                    },
                    new CooisComponent()
                    {
                        Material = "10",
                        BaseUoM = "uomtwo",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 1,
                        StatusId = "status",
                        StorageLocation = "stortwo",
                        Batch = "batchtwo"
                    },
                    new CooisComponent()
                    {
                        Material = "11",
                        BaseUoM = "uomtwo",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 1,
                        StatusId = "status",
                        StorageLocation = "stortwo",
                        Batch = "batchtwo"
                    },
                    new CooisComponent()
                    {
                        Material = "12",
                        BaseUoM = "uomtwo",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 1,
                        StatusId = "status",
                        StorageLocation = "stortwo",
                        Batch = "batchtwo"
                    },

                    new CooisComponent()
                    {
                        Material = "13",
                        BaseUoM = "uomtwo",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 1,
                        StatusId = "status",
                        StorageLocation = "stortwo",
                        Batch = "batchtwo"
                    },
                    new CooisComponent()
                    {
                        Material = "14",
                        BaseUoM = "uomtwo",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 1,
                        StatusId = "status",
                        StorageLocation = "stortwo",
                        Batch = "batchtwo"
                    },
                    new CooisComponent()
                    {
                        Material = "15",
                        BaseUoM = "uomtwo",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 1,
                        StatusId = "status",
                        StorageLocation = "stortwo",
                        Batch = "batchtwo"
                    },
                    new CooisComponent()
                    {
                        Material = "16",
                        BaseUoM = "uomtwo",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 1,
                        StatusId = "status",
                        StorageLocation = "stortwo",
                        Batch = "batchtwo"
                    },
                    new CooisComponent()
                    {
                        Material = "17",
                        BaseUoM = "uomtwo",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 1,
                        StatusId = "status",
                        StorageLocation = "stortwo",
                        Batch = "batchtwo"
                    },
                    new CooisComponent()
                    {
                        Material = "18",
                        BaseUoM = "uomtwo",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 1,
                        StatusId = "status",
                        StorageLocation = "stortwo",
                        Batch = "batchtwo"
                    },
                    new CooisComponent()
                    {
                        Material = "1",
                        BaseUoM = "uomtwo",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 1,
                        StatusId = "status",
                        StorageLocation = "stortwo",
                        Batch = "batchtwo"
                    },
                    new CooisComponent()
                    {
                        Material = "2",
                        BaseUoM = "uomtwo",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 1,
                        StatusId = "status",
                        StorageLocation = "stortwo",
                        Batch = "batchtwo"
                    },
                    new CooisComponent()
                    {
                        Material = "3",
                        BaseUoM = "uomtwo",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 1,
                        StatusId = "status",
                        StorageLocation = "stortwo",
                        Batch = "batchtwo"
                    },
                    new CooisComponent()
                    {
                        Material = "4",
                        BaseUoM = "uomtwo",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 1,
                        StatusId = "status",
                        StorageLocation = "stortwo",
                        Batch = "batchtwo"
                    },
                    new CooisComponent()
                    {
                        Material = "5",
                        BaseUoM = "uomtwo",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 1,
                        StatusId = "status",
                        StorageLocation = "stortwo",
                        Batch = "batchtwo"
                    },
                    new CooisComponent()
                    {
                        Material = "6",
                        BaseUoM = "uomtwo",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 1,
                        StatusId = "status",
                        StorageLocation = "stortwo",
                        Batch = "batchtwo"
                    },
                    new CooisComponent()
                    {
                        Material = "7",
                        BaseUoM = "uomtwo",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 1,
                        StatusId = "status",
                        StorageLocation = "stortwo",
                        Batch = "batchtwo"
                    },
                    new CooisComponent()
                    {
                        Material = "8",
                        BaseUoM = "uomtwo",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 1,
                        StatusId = "status",
                        StorageLocation = "stortwo",
                        Batch = "batchtwo"
                    },
                    new CooisComponent()
                    {
                        Material = "9",
                        BaseUoM = "uomtwo",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 1,
                        StatusId = "status",
                        StorageLocation = "stortwo",
                        Batch = "batchtwo"
                    },
                    new CooisComponent()
                    {
                        Material = "10",
                        BaseUoM = "uomtwo",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 1,
                        StatusId = "status",
                        StorageLocation = "stortwo",
                        Batch = "batchtwo"
                    },
                    new CooisComponent()
                    {
                        Material = "11",
                        BaseUoM = "uomtwo",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 1,
                        StatusId = "status",
                        StorageLocation = "stortwo",
                        Batch = "batchtwo"
                    },
                    new CooisComponent()
                    {
                        Material = "12",
                        BaseUoM = "uomtwo",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 1,
                        StatusId = "status",
                        StorageLocation = "stortwo",
                        Batch = "batchtwo"
                    },

                    new CooisComponent()
                    {
                        Material = "13",
                        BaseUoM = "uomtwo",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 1,
                        StatusId = "status",
                        StorageLocation = "stortwo",
                        Batch = "batchtwo"
                    },
                    new CooisComponent()
                    {
                        Material = "14",
                        BaseUoM = "uomtwo",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 1,
                        StatusId = "status",
                        StorageLocation = "stortwo",
                        Batch = "batchtwo"
                    },
                    new CooisComponent()
                    {
                        Material = "15",
                        BaseUoM = "uomtwo",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 1,
                        StatusId = "status",
                        StorageLocation = "stortwo",
                        Batch = "batchtwo"
                    },
                    new CooisComponent()
                    {
                        Material = "16",
                        BaseUoM = "uomtwo",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 1,
                        StatusId = "status",
                        StorageLocation = "stortwo",
                        Batch = "batchtwo"
                    },
                    new CooisComponent()
                    {
                        Material = "17",
                        BaseUoM = "uomtwo",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 1,
                        StatusId = "status",
                        StorageLocation = "stortwo",
                        Batch = "batchtwo"
                    },
                    new CooisComponent()
                    {
                        Material = "18",
                        BaseUoM = "uomtwo",
                        BulkMaterial = false,
                        Collected = false,
                        Comment = "test",
                        ComponentId = "1",
                        Hidden = false,
                        PhantomItem = false,
                        ProdOrder = 1,
                        QtyWithdrawn = 5,
                        RequirementNo = 5,
                        RequirementQty = 1,
                        StatusId = "status",
                        StorageLocation = "stortwo",
                        Batch = "batchtwo"
                    },
                }
            }
        };

        async Task<List<CooisComponent>> ICooisService.GetCooisComponents(int prodOrder)
        {
            //OFFLINE TEST
            if (prodOrder == -1)
            {
                throw new System.Net.Http.HttpRequestException("Failed to fetch", null, System.Net.HttpStatusCode.NotFound);
            }

            if (prodOrder == -2)
            {
                throw new System.Net.Http.HttpRequestException("Failed to fetch", null, System.Net.HttpStatusCode.ServiceUnavailable);
            }

            if (prodOrder == -3)
            {
                throw new System.Net.Http.HttpRequestException("Other error", null, System.Net.HttpStatusCode.SeeOther);
            }

            if (coois.ContainsKey(prodOrder))
            {
                return coois[prodOrder];
            }
            else
            {
                throw new System.Net.Http.HttpRequestException("Not in test data", null, System.Net.HttpStatusCode.BadRequest);
            }
        }
    }
}
