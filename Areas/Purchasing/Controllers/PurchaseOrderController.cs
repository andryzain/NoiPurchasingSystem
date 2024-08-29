using FastReport.Data;
using FastReport.Export.PdfSimple;
using FastReport.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NoiPurchasingSystem.Areas.MasterData.Repository;
using NoiPurchasingSystem.Areas.Purchasing.Models;
using NoiPurchasingSystem.Areas.Purchasing.Repositories;
using NoiPurchasingSystem.Areas.Purchasing.ViewModels;
using NoiPurchasingSystem.Data;
using NoiPurchasingSystem.Models;
using System.Data.SqlClient;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace NoiPurchasingSystem.Areas.Purchasing.Controllers
{
    [Area("Purchasing")]
    [Route("Purchasing/[Controller]/[Action]")]
    //[Authorize(Roles = "PurchaseOrder")]
    public class PurchaseOrderController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IPenggunaRepository _penggunaRepository;
        private readonly IProdukRepository _produkRepository;
        private readonly IMetodePembayaranRepository _metodePembayaranRepository;
        private readonly IPurchaseRequestRepository _PurchaseRequestRepository;
        private readonly IPurchaseOrderRepository _purchaseOrderRepository;
        private readonly IBankRepository _bankRepository;
        //private readonly IPembayaranBarangRepository _pembayaranBarangRepository;
        //private readonly IHutangUsahaRepository _hutangUsahaRepository;

        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;

        public PurchaseOrderController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext applicationDbContext,
            IPenggunaRepository penggunaRepository,
            IProdukRepository produkRepository,
            IMetodePembayaranRepository metodePembayaranRepository,
            IPurchaseRequestRepository PurchaseRequestRepository,
            IPurchaseOrderRepository PurchaseOrderRepository,
            IBankRepository bankRepository,
            //IPembayaranBarangRepository pembayaranBarangRepository,
            //IHutangUsahaRepository hutangUsahaRepository,

            IHostingEnvironment hostingEnvironment,
            IWebHostEnvironment webHostEnvironment,
            IConfiguration configuration
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _applicationDbContext = applicationDbContext;
            _penggunaRepository = penggunaRepository;
            _produkRepository = produkRepository;
            _metodePembayaranRepository = metodePembayaranRepository;
            _PurchaseRequestRepository = PurchaseRequestRepository;
            _purchaseOrderRepository = PurchaseOrderRepository;
            _bankRepository = bankRepository;
            //_pembayaranBarangRepository = pembayaranBarangRepository;
            //_hutangUsahaRepository = hutangUsahaRepository;

            _hostingEnvironment = hostingEnvironment;
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
        }

        //[Authorize(Roles = "IndexPurchaseOrder")]
        public IActionResult Index()
        {
            var data = _purchaseOrderRepository.GetAllPurchaseOrder();
            return View(data);
        }

        [HttpGet]
        //[Authorize(Roles = "DetailPurchaseOrder")]
        [AllowAnonymous]
        public async Task<IActionResult> DetailPurchaseOrder(Guid Id)
        {
            ViewBag.PurchaseRequest = new SelectList(await _PurchaseRequestRepository.GetPurchaseRequests(), "PurchaseRequestId", "PurchaseRequestNumber", SortOrder.Ascending);
            ViewBag.Users = new SelectList(_userManager.Users, nameof(ApplicationUser.Id), nameof(ApplicationUser.NamaUser), SortOrder.Ascending);
            ViewBag.MetodePembayaranId = new SelectList(await _metodePembayaranRepository.GetMetodePembayarans(), "MetodePembayaranId", "NamaMetodePembayaran", SortOrder.Ascending);
            ViewBag.Mengetahui = new SelectList(await _penggunaRepository.GetPenggunas(), "PenggunaId", "NamaLengkap", SortOrder.Ascending);

            var PurchaseOrder = await _purchaseOrderRepository.GetPurchaseOrderById(Id);

            if (PurchaseOrder == null)
            {
                Response.StatusCode = 404;
                return View("PurchaseOrderNotFound", Id);
            }

            PurchaseOrder model = new PurchaseOrder
            {
                PurchaseOrderId = PurchaseOrder.PurchaseOrderId,
                PurchaseOrderNumber = PurchaseOrder.PurchaseOrderNumber,
                PurchaseRequestId = PurchaseOrder.PurchaseRequestId,
                PurchaseRequestNumber = PurchaseOrder.PurchaseRequestNumber,
                UserAccessId = PurchaseOrder.UserAccessId,
                UserApprovalId = PurchaseOrder.UserApprovalId,
                MetodePembayaranId = PurchaseOrder.MetodePembayaranId,
                Status = PurchaseOrder.Status,
                QtyTotal = PurchaseOrder.QtyTotal,
                GrandTotal = Math.Truncate(PurchaseOrder.GrandTotal),
                Note = PurchaseOrder.Note,
                PurchaseOrderDetails = PurchaseOrder.PurchaseOrderDetails,
            };

            var ItemsList = new List<PurchaseOrderDetail>();

            foreach (var item in PurchaseOrder.PurchaseOrderDetails)
            {
                ItemsList.Add(new PurchaseOrderDetail
                {
                    ProductNumber = item.ProductNumber,
                    ProductName = item.ProductName,
                    Principal = item.Principal,
                    Measurement = item.Measurement,
                    Weight = item.Weight,
                    Qty = item.Qty,
                    Price = Math.Truncate(item.Price),
                    Discount = item.Discount,
                    SubTotal = Math.Truncate(item.SubTotal)
                });
            }

            model.PurchaseOrderDetails = ItemsList;
            return View(model);
        }

        [HttpGet]
        //[Authorize(Roles = "GeneratePayment-PurchaseOrder")]
        [AllowAnonymous]
        public async Task<IActionResult> GeneratePayment(Guid Id)
        {
            ViewBag.Bank = new SelectList(await _bankRepository.GetBanks(), "BankId", "NamaBank", SortOrder.Ascending);
            ViewBag.MetodePembayaranId = new SelectList(await _metodePembayaranRepository.GetMetodePembayarans(), "MetodePembayaranId", "NamaMetodePembayaran", SortOrder.Ascending);
            ViewBag.Mengetahui = new SelectList(await _penggunaRepository.GetPenggunas(), "PenggunaId", "NamaLengkap", SortOrder.Ascending);

            var purchaseOrder = _applicationDbContext.PurchaseOrders
                .Include(u => u.ApplicationUser)
                .Include(r => r.PurchaseRequest)
                .Include(p => p.MetodePembayaran)
                .Where(p => p.PurchaseOrderId == Id).FirstOrDefault();

            _signInManager.IsSignedIn(User);

            var getUser = _penggunaRepository.GetAllUserLogin().Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            //PembayaranBarang payment = new PembayaranBarang();

            //var dateNow = DateTimeOffset.Now;
            //var setDateNow = DateTimeOffset.Now.ToString("yyMMdd");

            //var lastCode = _pembayaranBarangRepository.GetAllPembayaranBarang().Where(d => d.CreateDateTime.ToString("yyMMdd") == dateNow.ToString("yyMMdd")).OrderByDescending(k => k.PaymentNumber).FirstOrDefault();
            //if (lastCode == null)
            //{
            //    payment.PaymentNumber = "PBR" + setDateNow + "0001";
            //}
            //else
            //{
            //    var lastCodeTrim = lastCode.PaymentNumber.Substring(3, 6);

            //    if (lastCodeTrim != setDateNow)
            //    {
            //        payment.PaymentNumber = "PBR" + setDateNow + "0001";
            //    }
            //    else
            //    {
            //        payment.PaymentNumber = "PBR" + setDateNow + (Convert.ToInt32(lastCode.PaymentNumber.Substring(9, lastCode.PaymentNumber.Length - 9)) + 1).ToString("D4");
            //    }
            //}

            //ViewBag.PaymentNumber = payment.PaymentNumber;

            var getPmbVM = new PurchaseOrderViewModel()
            {
                PurchaseOrderId = purchaseOrder.PurchaseOrderId,
                PurchaseOrderNumber = purchaseOrder.PurchaseOrderNumber,
                PurchaseRequestId = purchaseOrder.PurchaseRequestId,
                PurchaseRequestNumber = purchaseOrder.PurchaseRequestNumber,
                UserAccessId = purchaseOrder.UserAccessId,
                UserApprovalId = purchaseOrder.UserApprovalId,
                MetodePembayaranId = purchaseOrder.MetodePembayaranId,
                Status = purchaseOrder.Status,
                QtyTotal = purchaseOrder.QtyTotal,
                GrandTotal = Math.Truncate(purchaseOrder.GrandTotal),
                Note = purchaseOrder.Note,
            };

            return View(getPmbVM);
        }

        [HttpPost]
        //[Authorize(Roles = "GeneratePayment-Invoice")]
        [AllowAnonymous]
        public async Task<IActionResult> GeneratePayment(PurchaseOrder model, PurchaseOrderViewModel vm)
        {
            ViewBag.Bank = new SelectList(await _bankRepository.GetBanks(), "BankId", "NamaBank", SortOrder.Ascending);

            PurchaseOrder PurchaseOrder = await _purchaseOrderRepository.GetPurchaseOrderByIdNoTracking(model.PurchaseOrderId);

            _signInManager.IsSignedIn(User);

            var getUser = _penggunaRepository.GetAllUserLogin().Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            string getPaymentNumber = Request.Form["PBRNumber"];

            var updatePurchaseOrder = _purchaseOrderRepository.GetAllPurchaseOrder().Where(c => c.PurchaseOrderId == model.PurchaseOrderId).FirstOrDefault();
            if (updatePurchaseOrder != null)
            {
                {
                    updatePurchaseOrder.Status = "Selesai";
                };
                _applicationDbContext.Entry(updatePurchaseOrder).State = EntityState.Modified;
            }

            //// Generate ke Table Pembayaran
            //var newPayment = new PembayaranBarang
            //{
            //    CreateDateTime = DateTime.Now,
            //    CreateBy = new Guid(getUser.Id),
            //    PurchaseOrderId = PurchaseOrder.PurchaseOrderId,
            //    PurchaseOrderNumber = PurchaseOrder.PurchaseOrderNumber,
            //    BankId = vm.BankId,
            //    UserAccessId = getUser.Id.ToString(),
            //    PenggunaId = PurchaseOrder.PenggunaId,
            //    UserApprovalId = PurchaseOrder.UserApprovalId,
            //    MetodePembayaranId = PurchaseOrder.MetodePembayaranId,
            //    Status = "Lunas",
            //    GrandTotal = PurchaseOrder.GrandTotal,
            //    Note = PurchaseOrder.Note,
            //};

            //newPayment.PaymentNumber = getPaymentNumber;

            //_pembayaranBarangRepository.Tambah(newPayment);

            //// Generate ke Table Hutang Usaha            
            //var newHutang = new HutangUsaha
            //{
            //    CreateDateTime = DateTime.Now,
            //    CreateBy = new Guid(getUser.Id),
            //    TransaksiId = PurchaseOrder.PurchaseOrderId,
            //    TransaksiNumber = PurchaseOrder.PurchaseOrderNumber,
            //    BankId = vm.BankId,
            //    UserAccessId = getUser.Id.ToString(),
            //    Nominal = PurchaseOrder.GrandTotal
            //};

            //var dateNow = DateTimeOffset.Now;
            //var setDateNow = DateTimeOffset.Now.ToString("yyMMdd");

            //var lastCode = _hutangUsahaRepository.GetAllHutangUsaha().Where(d => d.CreateDateTime.ToString("yyMMdd") == dateNow.ToString("yyMMdd")).OrderByDescending(k => k.HutangNumber).FirstOrDefault();
            //if (lastCode == null)
            //{
            //    newHutang.HutangNumber = "HTG" + setDateNow + "0001";
            //}
            //else
            //{
            //    var lastCodeTrim = lastCode.HutangNumber.Substring(3, 6);

            //    if (lastCodeTrim != setDateNow)
            //    {
            //        newHutang.HutangNumber = "HTG" + setDateNow + "0001";
            //    }
            //    else
            //    {
            //        newHutang.HutangNumber = "HTG" + setDateNow + (Convert.ToInt32(lastCode.HutangNumber.Substring(9, lastCode.HutangNumber.Length - 9)) + 1).ToString("D4");
            //    }
            //}

            //_hutangUsahaRepository.Tambah(newHutang);

            ViewBag.Bank = new SelectList(await _bankRepository.GetBanks(), "BankId", "NamaBank", SortOrder.Ascending);
            //TempData["SuccessMessage"] = "No. Pembayaran " + newPayment.PaymentNumber + " Berhasil Disimpan";
            return RedirectToAction("Index", "PurchaseOrder");
        }

        public async Task<IActionResult> PrintPurchaseOrder(Guid Id)
        {
            var PurchaseOrder = await _purchaseOrderRepository.GetPurchaseOrderById(Id);           

            var getPbrNumber = PurchaseOrder.PurchaseOrderNumber;
            var getPmNumber = PurchaseOrder.PurchaseRequestNumber;
            var getMetodePembayaranId = PurchaseOrder.MetodePembayaranId;
            var getDateNow = DateTime.Now.ToString("dd MMMM yyyy");
            var getGrandTotal = PurchaseOrder.GrandTotal;
            var getTax = (getGrandTotal / 100) * 11;
            var getGrandTotalAfterTax = (getGrandTotal + getTax);
            var getDibuatOleh = PurchaseOrder.ApplicationUser.NamaUser;
            var getDisetujuiOleh = PurchaseOrder.UserApproval.NamaLengkap;

            WebReport web = new WebReport();
            var path = $"{_webHostEnvironment.WebRootPath}\\Reporting\\PurchaseOrder.frx";
            web.Report.Load(path);

            var msSqlDataConnection = new MsSqlDataConnection();
            msSqlDataConnection.ConnectionString = _configuration.GetConnectionString("DefaultConnection");
            var Conn = msSqlDataConnection.ConnectionString;

            web.Report.SetParameterValue("Conn", Conn);
            web.Report.SetParameterValue("PurchaseOrderId", Id.ToString());
            web.Report.SetParameterValue("PurchaseOrderNumber", getPbrNumber);
            web.Report.SetParameterValue("PurchaseRequestNumber", getPmNumber);
            web.Report.SetParameterValue("PbrDateNow", getDateNow);
            web.Report.SetParameterValue("MetodePembayaranId", getMetodePembayaranId);
            web.Report.SetParameterValue("GrandTotal", getGrandTotal);
            web.Report.SetParameterValue("Tax", getTax);
            web.Report.SetParameterValue("GrandTotalAfterTax", getGrandTotalAfterTax);
            web.Report.SetParameterValue("DibuatOleh", getDibuatOleh);
            web.Report.SetParameterValue("DisetujuiOleh", getDisetujuiOleh);

            web.Report.Prepare();
            Stream stream = new MemoryStream();
            web.Report.Export(new PDFSimpleExport(), stream);
            stream.Position = 0;
            return File(stream, "application/zip", (getPbrNumber + ".pdf"));
        }
    }
}
