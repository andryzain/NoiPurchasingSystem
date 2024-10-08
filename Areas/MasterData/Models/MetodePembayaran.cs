﻿using NoiPurchasingSystem.Repositories;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoiPurchasingSystem.Areas.MasterData.Models
{
    [Table("MstrMetodePembayaran", Schema = "dbo")]
    public class MetodePembayaran : AktivitasPengguna
    {
        [Key]
        public Guid MetodePembayaranId { get; set; }
        public string KodeMetodePembayaran { get; set; }
        public string NamaMetodePembayaran { get; set; }
    }
}
