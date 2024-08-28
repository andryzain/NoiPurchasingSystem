using NoiPurchasingSystem.Areas.Purchasing.Models;

namespace NoiPurchasingSystem.Areas.Purchasing.ViewModels
{
    public class PurchaseRequestViewModel
    {
        public Guid PurchaseRequestId { get; set; }
        public string PurchaseRequestNumber { get; set; }
        public string UserAccessId { get; set; }
        public Guid? UserApprovalId { get; set; }
        public Guid? MetodePembayaranId { get; set; }
        public string Status { get; set; }
        public int QtyTotal { get; set; }
        public decimal GrandTotal { get; set; }
        public string? Note { get; set; }
        public List<PurchaseRequestDetail> PurchaseRequestDetails { get; set; } = new List<PurchaseRequestDetail>();
    }
}
