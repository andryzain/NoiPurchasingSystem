using NoiPurchasingSystem.Repositories;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoiPurchasingSystem.Areas.MasterData.Models
{
    [Table("MstrPengguna", Schema = "dbo")]
    public class Pengguna : AktivitasPengguna
    {
        [Key]
        public Guid PenggunaId { get; set; }
        public string KodePengguna { get; set; }
        public string NamaLengkap { get; set; }
        public string NomorIdentitas{ get; set; }
        public Guid? LevelId { get; set; }        
        public string TempatLahir { get; set; }
        public DateTime TanggalLahir { get; set; }
        public string JenisKelamin { get; set; }
        public string AlamatLengkap { get; set; }
        public string NomorHandphone { get; set; }
        public string Email { get; set; }
        public string? Foto { get; set; }

        //Relationship
        [ForeignKey("LevelId")]
        public LevelUser? LevelUser { get; set; }
    }
}
