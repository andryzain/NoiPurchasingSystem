using Microsoft.AspNetCore.Identity;
using NoiPurchasingSystem.Areas.MasterData.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoiPurchasingSystem.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string KodeUser { get; set; }
        public string NamaUser { get; set; }
        public Guid? LevelId { get; set; }

        //Relationship
        [ForeignKey("LevelId")]
        public LevelUser? LevelUser { get; set; }
    }
}
