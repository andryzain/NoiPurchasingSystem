using NoiPurchasingSystem.Repositories;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoiPurchasingSystem.Areas.MasterData.Models
{
    [Table("MstrDiskon", Schema = "dbo")]
    public class Diskon : AktivitasPengguna
    {
        public Guid DiskonId { get; set; }
        public string KodeDiskon { get; set; }
        public int Nilai { get; set; }
    }
}
