using NoiPurchasingSystem.Repositories;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoiPurchasingSystem.Areas.MasterData.Models
{
    [Table("MstrBank", Schema = "dbo")]
    public class Bank : AktivitasPengguna
    {
        [Key]
        public Guid BankId { get; set; }
        public string KodeBank { get; set; }
        public string NamaBank { get; set; }
    }    
}
