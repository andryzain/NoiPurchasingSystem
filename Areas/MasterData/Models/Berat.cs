using NoiPurchasingSystem.Repositories;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoiPurchasingSystem.Areas.MasterData.Models
{
    [Table("MstrBerat", Schema = "dbo")]
    public class Berat : AktivitasPengguna
    {
        public Guid BeratId { get; set; }
        public string KodeBerat { get; set; }
        public int Nilai { get; set; }
    }
}
