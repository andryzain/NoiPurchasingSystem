﻿using NoiPurchasingSystem.Repositories;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoiPurchasingSystem.Areas.MasterData.Models
{
    [Table("MstrBengkel", Schema = "dbo")]
    public class Bengkel : AktivitasPengguna
    {
        [Key]
        public Guid BengkelId { get; set; }
        public string KodeBengkel { get; set; }
        public string NamaBengkel { get; set; }
        public string PenanggungJawab { get; set; }
        public string NamaPemilik { get; set; }
        public string Alamat { get; set; }
        public string NomorTelepon { get; set; }
        public string Email { get; set; }
        public string? Keterangan { get; set; }
        public string? Foto { get; set; }
    }
}
