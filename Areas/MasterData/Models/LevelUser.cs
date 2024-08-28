using NoiPurchasingSystem.Repositories;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoiPurchasingSystem.Areas.MasterData.Models
{
    [Table("MstrLevelUser", Schema = "dbo")]
    public class LevelUser : AktivitasPengguna
    {
        [Key]
        public Guid LevelId { get; set; }
        public string KodeLevel { get; set; }
        public string NamaLevel { get; set; }
        public int Persentase { get; set; }
        public string? Keterangan { get; set; }
    }
}
