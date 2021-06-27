using System;

#nullable disable

namespace Scan.Server.Model
{
    public partial class ProductClassification
    {
        public string ClassMaterial { get; set; }
        public string ClassRangeId { get; set; }
        public string ClassTypeId { get; set; }
        public string ClassStatusId { get; set; }
        public string ClassRatedVoltage { get; set; }
        public short? ClassMinTemp { get; set; }
        public short? ClassMaxTemp { get; set; }
        public short? ClassMaxWaterDepth { get; set; }
        public short? ClassMawp { get; set; }
        public string ClassPsl { get; set; }
        public short? ClassMass { get; set; }
        public string ClassTemplateId { get; set; }
        public string ClassApplicableStandard { get; set; }
        public bool? ClassIsCemarked { get; set; }
        public byte[] ClassTimestamp { get; set; }
        public string ClassDatasheetRef { get; set; }
        public string ClassModifiedBy { get; set; }
        public string ClassValidatedBy { get; set; }
        public DateTime? ClassValidationDate { get; set; }
        public bool? ClassPartNoAliased { get; set; }
    }
}
