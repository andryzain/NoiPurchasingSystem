using NoiPurchasingSystem.Repositories;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoiPurchasingSystem.Areas.MasterData.Models
{
    [Table("MstrItemReimbursment", Schema = "dbo")]
    public class ItemReimbursment : AktivitasPengguna
    {
        [Key]
        public Guid ItemReimbursmentId { get; set; }
        public string KodeItemReimbursment { get; set; }
        public string NamaItemReimbursment { get; set; }
    }
}
