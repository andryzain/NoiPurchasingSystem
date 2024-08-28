using NoiPurchasingSystem.Repositories;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoiPurchasingSystem.Areas.MasterData.Models
{
    [Table("MstrSatuan", Schema = "dbo")]
    public class Satuan : AktivitasPengguna
    {
        [Key]
        public Guid SatuanId { get; set; }
        public string KodeSatuan { get; set; }
        public string NamaSatuan { get; set; }
    }
}
