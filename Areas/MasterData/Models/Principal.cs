﻿using NoiPurchasingSystem.Repositories;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoiPurchasingSystem.Areas.MasterData.Models
{
    [Table("MstrPrincipal", Schema = "dbo")]
    public class Principal : AktivitasPengguna
    {
        [Key]
        public Guid PrincipalId { get; set; }
        public string KodePrincipal { get; set; }
        public string NamaPrincipal { get; set; }
        public string Alamat { get; set; }
        public string NomorTelepon { get; set; }
        public string Email { get; set; }        
        public string? Keterangan { get; set; }
    }
}
