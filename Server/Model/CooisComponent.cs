#nullable disable

namespace Scan.Server.Model
{
    public partial class CooisComponent
    {
        public string ComponentId { get; set; }
        public string Material { get; set; }
        public decimal RequirementQty { get; set; }
        public decimal QtyWithdrawn { get; set; }
        public string BaseUoM { get; set; }
        public long ProdOrder { get; set; }
        public int RequirementNo { get; set; }
        public string Batch { get; set; }
        public string StorageLocation { get; set; }
        public string StatusId { get; set; }
        public bool? PhantomItem { get; set; }
        public bool? BulkMaterial { get; set; }
    }
}
