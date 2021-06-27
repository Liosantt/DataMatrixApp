#nullable disable

namespace Scan.Server.Model
{
    public partial class MaterialMaster
    {
        public string Material { get; set; }
        public string Description { get; set; }
        public string CostControl { get; set; }
        public string Mrpcontroller { get; set; }
        public string ProdnSuperv { get; set; }
        public bool Configurable { get; set; }
    }
}
