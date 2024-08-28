using NoiPurchasingSystem.Areas.MasterData.Models;

namespace NoiPurchasingSystem.Areas.MasterData.ViewModels
{
    public class BankViewModel
    {
        public Guid BankId { get; set; }
        public string KodeBank { get; set; }
        public string NamaBank { get; set; }
    }
}
