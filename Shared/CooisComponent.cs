using System;
using System.Collections.Generic;

using System.ComponentModel.DataAnnotations.Schema;
#nullable disable

namespace Scan.Shared
{
    public partial class CooisComponent : ICloneable
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
        [NotMapped]
        public virtual string Comment { get; set; }
        [NotMapped]
        public virtual bool Collected { get; set; }
        [NotMapped]
        public virtual bool Hidden { get; set; }

        public object Clone()
        {
            CooisComponent cmp = new CooisComponent();
            cmp.BaseUoM = this.BaseUoM;
            cmp.Batch = this.Batch;
            cmp.BulkMaterial = this.BulkMaterial;
            cmp.Collected = this.Collected;
            cmp.Comment = this.Comment;
            cmp.ComponentId = this.ComponentId;
            cmp.Hidden = this.Hidden;
            cmp.Material = this.Material;
            cmp.PhantomItem = this.PhantomItem;
            cmp.ProdOrder = this.ProdOrder;
            cmp.QtyWithdrawn = this.QtyWithdrawn;
            cmp.RequirementNo = this.RequirementNo;
            cmp.RequirementQty = this.RequirementQty;
            cmp.StatusId = this.StatusId;
            cmp.StorageLocation = this.StorageLocation;

            return cmp;
        }
    }
}
