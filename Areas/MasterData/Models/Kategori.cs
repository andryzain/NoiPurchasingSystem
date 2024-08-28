using NoiPurchasingSystem.Repositories;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoiPurchasingSystem.Areas.MasterData.Models
{
    [Table("MstrKategori", Schema = "dbo")]
    public class Kategori : AktivitasPengguna
    {
        [Key]
        public Guid KategoriId { get; set; }
        public string KodeKategori { get; set; }
        public string NamaKategori { get; set; }
    }
}
