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
using NoiPurchasingSystem.Data;
using NoiPurchasingSystem.Models;
using System.Data.SqlClient;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace NoiPurchasingSystem.Areas.Purchasing.Controllers
{
    [Area("Purchasing")]
    [Route("Purchasing/[Controller]/[Action]")]
    //[Authorize(Roles = "Purchasing")]
    public class PurchaseRequestController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IPurchaseRequestRepository _purchaseRequestRepository;
        private readonly IPenggunaRepository _penggunaRepository;
        private readonly IProdukRepository _produkRepository;
        private readonly IMetodePembayaranRepository _metodePembayaranRepository;
        private readonly IBengkelRepository _bengkelRepository;
        private readonly IPurchaseOrderRepository _purchaseOrderRepository;

        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;

        public PurchaseRequestController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext applicationDbContext,
            IPurchaseRequestRepository purchaseRequestRepository,
            IPenggunaRepository penggunaRepository,
            IProdukRepository produkRepository,
            IMetodePembayaranRepository metodePembayaranRepository,
            IBengkelRepository bengkelRepository,
            IPurchaseOrderRepository PurchaseOrderRepository,

            IHostingEnvironment hostingEnvironment,
            IWebHostEnvironment webHostEnvironment,
            IConfiguration configuration
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _applicationDbContext = applicationDbContext;
            _purchaseRequestRepository = purchaseRequestRepository;
            _penggunaRepository = penggunaRepository;
            _produkRepository = produkRepository;
            _metodePembayaranRepository = metodePembayaranRepository;
            _bengkelRepository = bengkelRepository;
            _purchaseOrderRepository = PurchaseOrderRepository;

            _hostingEnvironment = hostingEnvironment;
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
        }

        public JsonResult LoadProduk(Guid Id)
        {
            var produk = _applicationDbContext.Produks.Include(p => p.Principal).Include(s => s.Satuan).Include(d => d.Diskon).Include(b => b.Berat).Where(p => p.ProdukId == Id).FirstOrDefault();
            return new JsonResult(produk);
        }

        //[Authorize(Roles = "IndexPurchaseRequest")]
        public IActionResult Index()
        {
            var data = _purchaseRequestRepository.GetAllPurchaseRequest();
            return View(data);
        }

        //[Authorize(Roles = "CreatePurchaseRequest")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> CreatePurchaseRequest()
        {
            _signInManager.IsSignedIn(User);
            var getUser = _penggunaRepository.GetAllUserLogin().Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            ViewBag.Produk = new SelectList(await _produkRepository.GetProduks(), "ProdukId", "NamaProduk", SortOrder.Ascending);
            ViewBag.MetodePembayaran = new SelectList(await _metodePembayaranRepository.GetMetodePembayarans(), "MetodePembayaranId", "NamaMetodePembayaran", SortOrder.Ascending);            
            ViewBag.Approve = new SelectList(await _penggunaRepository.GetPenggunas(), "PenggunaId", "NamaLengkap", SortOrder.Ascending);

            var PurchaseRequest = new PurchaseRequest()
            {
                UserAccessId = getUser.Id,
            };
            PurchaseRequest.PurchaseRequestDetails.Add(new PurchaseRequestDetail() { PurchaseRequestDetailId = Guid.NewGuid() });

            var dateNow = DateTimeOffset.Now;
            var setDateNow = DateTimeOffset.Now.ToString("yyMMdd");

            var lastCode = _purchaseRequestRepository.GetAllPurchaseRequest().Where(d => d.CreateDateTime.ToString("yyMMdd") == dateNow.ToString("yyMMdd")).OrderByDescending(k => k.PurchaseRequestNumber).FirstOrDefault();
            if (lastCode == null)
            {
                PurchaseRequest.PurchaseRequestNumber = "PM" + setDateNow + "0001";
            }
            else
            {
                var lastCodeTrim = lastCode.PurchaseRequestNumber.Substring(2, 6);

                if (lastCodeTrim != setDateNow)
                {
                    PurchaseRequest.PurchaseRequestNumber = "PM" + setDateNow + "0001";
                }
                else
                {
                    PurchaseRequest.PurchaseRequestNumber = "PM" + setDateNow + (Convert.ToInt32(lastCode.PurchaseRequestNumber.Substring(9, lastCode.PurchaseRequestNumber.Length - 9)) + 1).ToString("D4");
                }
            }

            return View(PurchaseRequest);
        }

        //[Authorize(Roles = "CreatePurchaseRequest")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreatePurchaseRequest(PurchaseRequest model)
        {
            ViewBag.Produk = new SelectList(await _produkRepository.GetProduks(), "ProdukId", "ProductName", SortOrder.Ascending);
            ViewBag.MetodePembayaranId = new SelectList(await _metodePembayaranRepository.GetMetodePembayarans(), "MetodePembayaranId", "NamaMetodePembayaran", SortOrder.Ascending);
            ViewBag.Mengetahui = new SelectList(await _penggunaRepository.GetPenggunas(), "PenggunaId", "NamaLengkap", SortOrder.Ascending);
            ViewBag.Bengkel = new SelectList(await _bengkelRepository.GetBengkels(), "BengkelId", "NamaBengkel", SortOrder.Ascending);

            var dateNow = DateTimeOffset.Now;
            var setDateNow = DateTimeOffset.Now.ToString("yyMMdd");

            var lastCode = _purchaseRequestRepository.GetAllPurchaseRequest().Where(d => d.CreateDateTime.ToString("yyMMdd") == dateNow.ToString("yyMMdd")).OrderByDescending(k => k.PurchaseRequestNumber).FirstOrDefault();
            if (lastCode == null)
            {
                model.PurchaseRequestNumber = "PM" + setDateNow + "0001";
            }
            else
            {
                var lastCodeTrim = lastCode.PurchaseRequestNumber.Substring(2, 6);

                if (lastCodeTrim != setDateNow)
                {
                    model.PurchaseRequestNumber = "PM" + setDateNow + "0001";
                }
                else
                {
                    model.PurchaseRequestNumber = "PM" + setDateNow + (Convert.ToInt32(lastCode.PurchaseRequestNumber.Substring(9, lastCode.PurchaseRequestNumber.Length - 9)) + 1).ToString("D4");
                }
            }

            var getUser = _penggunaRepository.GetAllUserLogin().Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            //var getApproval = _penggunaRepository.GetAllUserLogin().Where(n => n.Id == model.Approval).FirstOrDefault();

            if (ModelState.IsValid)
            {
                var PurchaseRequest = new PurchaseRequest
                {
                    CreateDateTime = DateTime.Now,
                    CreateBy = new Guid(getUser.Id), //Convert Guid to String
                    PurchaseRequestId = model.PurchaseRequestId,
                    PurchaseRequestNumber = model.PurchaseRequestNumber,
                    UserAccessId = getUser.Id,
                    UserApprovalId = model.UserApprovalId,
                    MetodePembayaranId = model.MetodePembayaranId,
                    Status = model.Status,
                    QtyTotal = model.QtyTotal,
                    GrandTotal = model.GrandTotal,
                    Note = model.Note,
                };

                var ItemsList = new List<PurchaseRequestDetail>();

                foreach (var item in model.PurchaseRequestDetails)
                {
                    ItemsList.Add(new PurchaseRequestDetail
                    {
                        CreateDateTime = DateTime.Now,
                        CreateBy = new Guid(getUser.Id),
                        ProductNumber = item.ProductNumber,
                        ProductName = item.ProductName,
                        Principal = item.Principal,
                        Measurement = item.Measurement,
                        Weight = item.Weight,
                        Qty = item.Qty,
                        Price = item.Price,
                        Discount = item.Discount,
                        SubTotal = item.SubTotal
                    });
                }

                PurchaseRequest.PurchaseRequestDetails = ItemsList;
                _purchaseRequestRepository.Tambah(PurchaseRequest);

                TempData["SuccessMessage"] = "No. PurchaseRequest " + model.PurchaseRequestNumber + " Berhasil Disimpan";
                return Json(new { redirectToUrl = Url.Action("Index", "PurchaseRequest") });
            }
            else
            {
                ViewBag.Produk = new SelectList(await _produkRepository.GetProduks(), "ProdukId", "ProductName", SortOrder.Ascending);
                ViewBag.MetodePembayaranId = new SelectList(await _metodePembayaranRepository.GetMetodePembayarans(), "MetodePembayaranId", "NamaMetodePembayaran", SortOrder.Ascending);
                ViewBag.Mengetahui = new SelectList(await _penggunaRepository.GetPenggunas(), "PenggunaId", "NamaLengkap", SortOrder.Ascending);
                ViewBag.Bengkel = new SelectList(await _bengkelRepository.GetBengkels(), "BengkelId", "NamaBengkel", SortOrder.Ascending);
                TempData["WarningMessage"] = "Terdapat data yang masih kosong !!!";
                return View(model);
            }
        }

        [HttpGet]
        //[Authorize(Roles = "DetailPurchaseRequest")]
        [AllowAnonymous]
        public async Task<IActionResult> DetailPurchaseRequest(Guid Id)
        {
            ViewBag.Produk = new SelectList(await _produkRepository.GetProduks(), "ProdukId", "ProductName", SortOrder.Ascending);
            ViewBag.MetodePembayaranId = new SelectList(await _metodePembayaranRepository.GetMetodePembayarans(), "MetodePembayaranId", "NamaMetodePembayaran", SortOrder.Ascending);
            ViewBag.Mengetahui = new SelectList(await _penggunaRepository.GetPenggunas(), "PenggunaId", "NamaLengkap", SortOrder.Ascending);
            ViewBag.Bengkel = new SelectList(await _bengkelRepository.GetBengkels(), "BengkelId", "NamaBengkel", SortOrder.Ascending);

            var PurchaseRequest = await _purchaseRequestRepository.GetPurchaseRequestById(Id);

            if (PurchaseRequest == null)
            {
                Response.StatusCode = 404;
                return View("PurchaseRequestNotFound", Id);
            }

            PurchaseRequest model = new PurchaseRequest
            {
                PurchaseRequestId = PurchaseRequest.PurchaseRequestId,
                PurchaseRequestNumber = PurchaseRequest.PurchaseRequestNumber,
                UserAccessId = PurchaseRequest.UserAccessId,
                UserApprovalId = PurchaseRequest.UserApprovalId,
                MetodePembayaranId = PurchaseRequest.MetodePembayaranId,
                Status = PurchaseRequest.Status,
                QtyTotal = PurchaseRequest.QtyTotal,
                GrandTotal = Math.Truncate(PurchaseRequest.GrandTotal),
                Note = PurchaseRequest.Note
            };

            var ItemsList = new List<PurchaseRequestDetail>();

            foreach (var item in PurchaseRequest.PurchaseRequestDetails)
            {
                ItemsList.Add(new PurchaseRequestDetail
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

            model.PurchaseRequestDetails = ItemsList;
            return View(model);
        }

        [HttpPost]
        //[Authorize(Roles = "DetailPurchaseRequest")]
        [AllowAnonymous]
        public async Task<IActionResult> DetailPurchaseRequest(PurchaseRequest model)
        {
            var getUser = _penggunaRepository.GetAllUserLogin().Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            if (ModelState.IsValid)
            {
                PurchaseRequest PurchaseRequest = _purchaseRequestRepository.GetAllPurchaseRequest().Where(p => p.PurchaseRequestNumber == model.PurchaseRequestNumber).FirstOrDefault();

                if (PurchaseRequest != null)
                {
                    PurchaseRequest.UpdateDateTime = DateTime.Now;
                    PurchaseRequest.UpdateBy = new Guid(getUser.Id);
                    PurchaseRequest.MetodePembayaranId = model.MetodePembayaranId;
                    PurchaseRequest.UserApprovalId = model.UserApprovalId;
                    PurchaseRequest.Status = "Diproses";
                    PurchaseRequest.Note = model.Note;
                    PurchaseRequest.PurchaseRequestDetails = model.PurchaseRequestDetails;
                    PurchaseRequest.QtyTotal = model.QtyTotal;
                    PurchaseRequest.GrandTotal = model.GrandTotal;

                    if (PurchaseRequest.UserApprovalId == null)
                    {
                        TempData["SuccessMessage"] = "No. PurchaseRequest " + model.PurchaseRequestNumber + " Berhasil Diubah";
                        return Json(new { redirectToUrl = Url.Action("Index", "PurchaseRequest") });
                    }
                    else
                    {
                        _purchaseRequestRepository.Update(PurchaseRequest);
                        TempData["SuccessMessage"] = "No. PurchaseRequest " + model.PurchaseRequestNumber + " Berhasil Diubah";
                        return Json(new { redirectToUrl = Url.Action("Index", "PurchaseRequest") });
                    }
                }
                else
                {
                    TempData["WarningMessage"] = "No. PurchaseRequest " + model.PurchaseRequestNumber + " sudah ada !!!";
                    return View(model);
                }
            }
            TempData["WarningMessage"] = "No. PurchaseRequest " + model.PurchaseRequestNumber + " Gagal Disimpan";
            return Json(new { redirectToUrl = Url.Action("Index", "PurchaseRequest") });
        }

        [HttpGet]
        //[Authorize(Roles = "GeneratePurchaseOrder-PurchaseRequest")]
        [AllowAnonymous]
        public async Task<IActionResult> GeneratePurchaseOrder(Guid Id)
        {
            ViewBag.Produk = new SelectList(await _produkRepository.GetProduks(), "ProdukId", "ProductName", SortOrder.Ascending);
            ViewBag.MetodePembayaranId = new SelectList(await _metodePembayaranRepository.GetMetodePembayarans(), "MetodePembayaranId", "NamaMetodePembayaran", SortOrder.Ascending);
            ViewBag.Mengetahui = new SelectList(await _penggunaRepository.GetPenggunas(), "PenggunaId", "NamaLengkap", SortOrder.Ascending);
            ViewBag.Bengkel = new SelectList(await _bengkelRepository.GetBengkels(), "BengkelId", "NamaBengkel", SortOrder.Ascending);

            PurchaseRequest PurchaseRequest = _applicationDbContext.PurchaseRequests
                .Include(d => d.PurchaseRequestDetails)
                .Include(u => u.ApplicationUser)
                .Include(p => p.MetodePembayaran)
                .Where(p => p.PurchaseRequestId == Id).FirstOrDefault();

            _signInManager.IsSignedIn(User);

            var getUser = _penggunaRepository.GetAllUserLogin().Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            var po = new PurchaseOrder();

            var dateNow = DateTimeOffset.Now;
            var setDateNow = DateTimeOffset.Now.ToString("yyMMdd");

            var lastCode = _purchaseOrderRepository.GetAllPurchaseOrder().Where(d => d.CreateDateTime.ToString("yyMMdd") == dateNow.ToString("yyMMdd")).OrderByDescending(k => k.PurchaseOrderNumber).FirstOrDefault();
            if (lastCode == null)
            {
                po.PurchaseOrderNumber = "ORD" + setDateNow + "0001";
            }
            else
            {
                var lastCodeTrim = lastCode.PurchaseOrderNumber.Substring(3, 6);

                if (lastCodeTrim != setDateNow)
                {
                    po.PurchaseOrderNumber = "ORD" + setDateNow + "0001";
                }
                else
                {
                    po.PurchaseOrderNumber = "ORD" + setDateNow + (Convert.ToInt32(lastCode.PurchaseRequestNumber.Substring(9, lastCode.PurchaseRequestNumber.Length - 9)) + 1).ToString("D4");
                }
            }

            ViewBag.PurchaseOrderNumber = po.PurchaseOrderNumber;

            var getPr = new PurchaseRequest()
            {
                PurchaseRequestId = PurchaseRequest.PurchaseRequestId,
                PurchaseRequestNumber = PurchaseRequest.PurchaseRequestNumber,
                UserAccessId = PurchaseRequest.UserAccessId,
                UserApprovalId = PurchaseRequest.UserApprovalId,
                MetodePembayaranId = PurchaseRequest.MetodePembayaranId,
                Status = PurchaseRequest.Status,
                QtyTotal = PurchaseRequest.QtyTotal,
                GrandTotal = PurchaseRequest.GrandTotal,
                Note = PurchaseRequest.Note,
                PurchaseRequestDetails = PurchaseRequest.PurchaseRequestDetails,
            };
            return View(getPr);
        }

        [HttpPost]
        //[Authorize(Roles = "GeneratePurchaseOrder-PurchaseRequest")]
        [AllowAnonymous]
        public async Task<IActionResult> GeneratePurchaseOrder(PurchaseRequest model)
        {
            PurchaseRequest PurchaseRequest = await _purchaseRequestRepository.GetPurchaseRequestByIdNoTracking(model.PurchaseRequestId);

            _signInManager.IsSignedIn(User);

            var getUser = _penggunaRepository.GetAllUserLogin().Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            string getPurchaseOrderNumber = Request.Form["PMBNumber"];

            var updatePurchaseRequest = _purchaseRequestRepository.GetAllPurchaseRequest().Where(c => c.PurchaseRequestId == model.PurchaseRequestId).FirstOrDefault();
            if (updatePurchaseRequest != null)
            {
                {
                    updatePurchaseRequest.Status = getPurchaseOrderNumber;
                };
                _applicationDbContext.Entry(updatePurchaseRequest).State = EntityState.Modified;
            }

            var newPurchaseOrder = new PurchaseOrder
            {
                CreateDateTime = DateTime.Now,
                CreateBy = new Guid(getUser.Id),
                PurchaseRequestId = PurchaseRequest.PurchaseRequestId,
                PurchaseRequestNumber = PurchaseRequest.PurchaseRequestNumber,
                UserAccessId = getUser.Id.ToString(),
                UserApprovalId = PurchaseRequest.UserApprovalId,
                MetodePembayaranId = PurchaseRequest.MetodePembayaranId,
                Status = "Diproses",
                QtyTotal = PurchaseRequest.QtyTotal,
                GrandTotal = PurchaseRequest.GrandTotal,
                Note = PurchaseRequest.Note,
            };

            newPurchaseOrder.PurchaseOrderNumber = getPurchaseOrderNumber;

            var ItemsList = new List<PurchaseOrderDetail>();

            foreach (var item in PurchaseRequest.PurchaseRequestDetails)
            {
                ItemsList.Add(new PurchaseOrderDetail
                {
                    CreateDateTime = DateTime.Now,
                    CreateBy = new Guid(getUser.Id),
                    ProductNumber = item.ProductNumber,
                    ProductName = item.ProductName,
                    Principal = item.Principal,
                    Measurement = item.Measurement,
                    Weight = item.Weight,
                    Qty = item.Qty,
                    Price = item.Price,
                    Discount = item.Discount,
                    SubTotal = item.SubTotal
                });
            }

            newPurchaseOrder.PurchaseOrderDetails = ItemsList;

            _purchaseOrderRepository.Tambah(newPurchaseOrder);

            TempData["SuccessMessage"] = "No. PurchaseOrder " + newPurchaseOrder.PurchaseOrderNumber + " Berhasil Disimpan";
            return RedirectToAction("Index", "PurchaseOrder");
        }

        public async Task<IActionResult> PrintPurchaseRequest(Guid Id)
        {
            var PurchaseRequest = await _purchaseRequestRepository.GetPurchaseRequestById(Id);     

            var getPmNumber = PurchaseRequest.PurchaseRequestNumber;
            //var getPoNumber = PurchaseRequest.PurchaseOrderNumber;
            var getMetodePembayaranId = PurchaseRequest.MetodePembayaranId; ;
            var getDateNow = DateTime.Now.ToString("dd MMMM yyyy");
            var getGrandTotal = PurchaseRequest.GrandTotal;
            var getTax = (getGrandTotal / 100) * 11;
            var getGrandTotalAfterTax = (getGrandTotal + getTax);
            var getDibuatOleh = PurchaseRequest.ApplicationUser.NamaUser;
            var getDisetujuiOleh = PurchaseRequest.UserApproval.NamaLengkap;

            WebReport web = new WebReport();
            var path = $"{_webHostEnvironment.WebRootPath}\\Reporting\\PurchaseRequest.frx";
            web.Report.Load(path);

            var msSqlDataConnection = new MsSqlDataConnection();
            msSqlDataConnection.ConnectionString = _configuration.GetConnectionString("DefaultConnection");
            var Conn = msSqlDataConnection.ConnectionString;

            web.Report.SetParameterValue("Conn", Conn);
            web.Report.SetParameterValue("PurchaseRequestId", Id.ToString());
            web.Report.SetParameterValue("PurchaseRequestNumber", getPmNumber);
            //web.Report.SetParameterValue("PoNumber", getPoNumber);
            web.Report.SetParameterValue("PmDateNow", getDateNow);
            web.Report.SetParameterValue("MetodePembayaranId", getMetodePembayaranId);
            web.Report.SetParameterValue("GrandTotal", getGrandTotal);
            web.Report.SetParameterValue("Tax", getTax);
            web.Report.SetParameterValue("GrandTotalAfterTax", getGrandTotalAfterTax);
            web.Report.SetParameterValue("CreateBy", getDibuatOleh);
            web.Report.SetParameterValue("ApproveBy", getDisetujuiOleh);

            web.Report.Prepare();
            Stream stream = new MemoryStream();
            web.Report.Export(new PDFSimpleExport(), stream);
            stream.Position = 0;
            return File(stream, "application/zip", (getPmNumber + ".pdf"));
        }
    }
}
