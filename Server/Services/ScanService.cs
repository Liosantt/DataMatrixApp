using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Scan.Server.Model;
using Scan.Shared;
using CooisComponent = Scan.Shared.CooisComponent;

namespace Scan.Server.Services
{
    public class ScanService : ScanServices.ScanServicesBase
    {
        private readonly Connectors1Context _dbContext;

        public ScanService(Connectors1Context dbContext)
        {
            _dbContext = dbContext;
        }

        public override Task<ScanData> GetScanData(Tag request, ServerCallContext context)
        {
            long order = _dbContext.SerialNos.First(e => e.SerialNo1 == request.SNum).ProdOrder;
            ScanData response = new ScanData();
            response.Components.AddRange(_dbContext.CooisComponents.Where(e => e.ProdOrder == order).ToProtobuf());
            response.Description = _dbContext.MaterialMasters.First(e => e.Material == request.Material).Description;
            response.ProdOrder = order;
            response.Class = _dbContext.ProductClassifications.First(e => e.ClassMaterial == request.Material)
                .ToProtobuf();
            return Task.FromResult(response);
        }
    }

    public static class Extentions
    {
        public static IEnumerable<CooisComponent> ToProtobuf(this IEnumerable<Scan.Server.Model.CooisComponent> self)
        {
            foreach (Scan.Server.Model.CooisComponent component in self)
            {
                yield return new CooisComponent
                {
                    Material = component.Material,
                    RequirementQty = Convert.ToSingle(component.RequirementQty),
                    QtyWithdrawn = Convert.ToSingle(component.QtyWithdrawn),
                    BaseUoM = component.BaseUoM,
                    Batch = (string.IsNullOrEmpty(component.Batch) ? "Missing" : component.Batch),
                    StorageLocation = component.StorageLocation
                };
            }
        }

        public static Classification ToProtobuf(this ProductClassification prodClass)
        {
            return new Classification
            {
                ClassMaxRatedVoltage = prodClass.ClassRatedVoltage,
                ClassMinTemp = Convert.ToInt32(prodClass.ClassMinTemp),
                ClassMaxTemp = Convert.ToInt32(prodClass.ClassMaxTemp),
                ClassMaxWaterDepth = Convert.ToInt32(prodClass.ClassMaxWaterDepth),
                ClassMAWP = Convert.ToInt32(prodClass.ClassMawp),
                ClassPSL = (string.IsNullOrEmpty(prodClass.ClassPsl) ? "Missing" : prodClass.ClassPsl),
                ClassMass = Convert.ToInt32(prodClass.ClassMass),
                ClassApplicableStandard = (string.IsNullOrEmpty(prodClass.ClassApplicableStandard) ? "Missing" : prodClass.ClassApplicableStandard),
                ClassIsCEMarked = prodClass.ClassIsCemarked.GetValueOrDefault(),
                ClassModifiedBy = prodClass.ClassModifiedBy,
                ClassValidatedBy = prodClass.ClassValidatedBy,
                ClassValidationDate =
                    Timestamp.FromDateTime(DateTime.SpecifyKind(prodClass.ClassValidationDate.GetValueOrDefault(),
                        DateTimeKind.Utc)),
                ClassPartNoAliased = prodClass.ClassPartNoAliased.GetValueOrDefault()
            };
        }
    }
}