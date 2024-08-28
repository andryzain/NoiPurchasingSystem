using NoiPurchasingSystem.Repositories;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoiPurchasingSystem.Areas.MasterData.Models
{
    [Table("MstrMekanik", Schema = "dbo")]
    public class Mekanik : AktivitasPengguna
    {
        [Key]
        public Guid MekanikId { get; set; }
        public string KodeMekanik { get; set; }
        public string NamaMekanik { get; set; }
        public Guid? BengkelId { get; set; }

        //Relationship
        [ForeignKey("BengkelId")]
        public Bengkel? Bengkel { get; set; }
    }
}
