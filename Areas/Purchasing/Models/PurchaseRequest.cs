﻿using NoiPurchasingSystem.Areas.MasterData.Models;
using NoiPurchasingSystem.Models;
using NoiPurchasingSystem.Repositories;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoiPurchasingSystem.Areas.Purchasing.Models
{
    [Table("PrpoPurchaseRequest", Schema = "dbo")]
    public class PurchaseRequest : AktivitasPengguna
    {
        [Key]
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

        //Relationship
        [ForeignKey("UserAccessId")]
        public ApplicationUser? ApplicationUser { get; set; }
        [ForeignKey("TermOfPaymentId")]
        public MetodePembayaran? MetodePembayaran { get; set; }
        [ForeignKey("UserApprovalId")]
        public Pengguna? UserApproval { get; set; }
    }

    [Table("PrpoPurchaseRequestDetail", Schema = "dbo")]
    public class PurchaseRequestDetail : AktivitasPengguna
    {
        [Key]
        public Guid PurchaseRequestDetailId { get; set; }
        public Guid? PurchaseRequestId { get; set; }
        public string ProductNumber { get; set; }
        public string ProductName { get; set; }
        public string Measurement { get; set; }
        public string Principal { get; set; }
        public int Qty { get; set; }
        public decimal Price { get; set; }
        public int Discount { get; set; }
        public decimal SubTotal { get; set; }

        //Relationship
        [ForeignKey("PurchaseRequestId")]
        public PurchaseRequest? PurchaseRequest { get; set; }
    }
}
