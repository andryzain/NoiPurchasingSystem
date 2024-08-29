using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NoiPurchasingSystem.Areas.MasterData.Models;
using NoiPurchasingSystem.Areas.MasterData.Repository;
using NoiPurchasingSystem.Areas.MasterData.ViewModels;
using NoiPurchasingSystem.Data;

namespace NoiPurchasingSystem.Areas.MasterData.Controllers
{
    [Area("MasterData")]
    [Route("MasterData/[Controller]/[Action]")]
    //[Authorize(Roles = "Berat")]
    public class BeratController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IBeratRepository _beratRepository;
        private readonly IProdukRepository _produkRepository;

        public BeratController(
            ApplicationDbContext applicationDbContext,
            IBeratRepository beratRepository,
            IProdukRepository produkRepository
        )
        {
            _applicationDbContext = applicationDbContext;
            _beratRepository = beratRepository;
            _produkRepository = produkRepository;
        }

        //[Authorize(Roles = "IndexBerat")]
        public IActionResult Index()
        {
            var data = _beratRepository.GetAllBerat();
            return View(data);
        }

        //[Authorize(Roles = "CreateBerat")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<ViewResult> CreateBerat()
        {
            var Berat = new BeratViewModel();
            var dateNow = DateTimeOffset.Now;
            var setDateNow = DateTimeOffset.Now.ToString("yyMMdd");

            var lastCode = _beratRepository.GetAllBerat().Where(d => d.CreateDateTime.ToString("yyMMdd") == dateNow.ToString("yyMMdd")).OrderByDescending(k => k.KodeBerat).FirstOrDefault();
            if (lastCode == null)
            {
                Berat.KodeBerat = "BRT" + setDateNow + "0001";
            }
            else
            {
                var lastCodeTrim = lastCode.KodeBerat.Substring(3, 6);

                if (lastCodeTrim != setDateNow)
                {
                    Berat.KodeBerat = "BRT" + setDateNow + "0001";
                }
                else
                {
                    Berat.KodeBerat = "BRT" + setDateNow + (Convert.ToInt32(lastCode.KodeBerat.Substring(9, lastCode.KodeBerat.Length - 9)) + 1).ToString("D4");
                }
            }

            return View(Berat);
        }

        //[Authorize(Roles = "CreateBerat")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateBerat(BeratViewModel vm)
        {
            var dateNow = DateTimeOffset.Now;
            var setDateNow = DateTimeOffset.Now.ToString("yyMMdd");

            var lastCode = _beratRepository.GetAllBerat().Where(d => d.CreateDateTime.ToString("yyMMdd") == dateNow.ToString("yyMMdd")).OrderByDescending(k => k.KodeBerat).FirstOrDefault();
            if (lastCode == null)
            {
                vm.KodeBerat = "BRT" + setDateNow + "0001";
            }
            else
            {
                var lastCodeTrim = lastCode.KodeBerat.Substring(3, 6);

                if (lastCodeTrim != setDateNow)
                {
                    vm.KodeBerat = "BRT" + setDateNow + "0001";
                }
                else
                {
                    vm.KodeBerat = "BRT" + setDateNow + (Convert.ToInt32(lastCode.KodeBerat.Substring(9, lastCode.KodeBerat.Length - 9)) + 1).ToString("D4");
                }
            }

            if (ModelState.IsValid)
            {
                var Berat = new Berat
                {
                    CreateDateTime = DateTime.Now,
                    BeratId = vm.BeratId,
                    KodeBerat = vm.KodeBerat,
                    Nilai = vm.Nilai
                };

                var resultBerat = _beratRepository.GetAllBerat().Where(c => c.Nilai == vm.Nilai).FirstOrDefault();

                if (resultBerat == null)
                {
                    _beratRepository.Tambah(Berat);
                    TempData["SuccessMessage"] = "Berat " + vm.Nilai + "Kg Berhasil Disimpan";
                    return RedirectToAction("Index", "Berat");
                }
                else
                {
                    TempData["WarningMessage"] = "Nilai Berat " + vm.Nilai + "Kg sudah ada !!!";
                    //ModelState.AddModelError("", "Maaf, nama Berat sudah ada !!!");
                    return View(vm);
                }
            }
            return View();
        }

        [HttpGet]
        //[Authorize(Roles = "DetailBerat")]
        [AllowAnonymous]
        public async Task<IActionResult> DetailBerat(Guid Id)
        {
            var Berat = await _beratRepository.GetBeratById(Id);

            if (Berat == null)
            {
                Response.StatusCode = 404;
                return View("BeratNotFound", Id);
            }

            BeratViewModel viewModel = new BeratViewModel
            {
                BeratId = Berat.BeratId,
                KodeBerat = Berat.KodeBerat,
                Nilai = Berat.Nilai
            };
            return View(viewModel);
        }

        [HttpPost]
        //[Authorize(Roles = "DetailBerat")]
        [AllowAnonymous]
        public async Task<IActionResult> DetailBerat(BeratViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                Berat Berat = await _beratRepository.GetBeratByIdNoTracking(viewModel.BeratId);

                var check = _beratRepository.GetAllBerat().Where(d => d.KodeBerat == viewModel.KodeBerat).FirstOrDefault();

                if (check != null)
                {
                    Berat.UpdateDateTime = DateTime.Now;
                    Berat.KodeBerat = viewModel.KodeBerat;
                    Berat.Nilai = viewModel.Nilai;

                    _beratRepository.Update(Berat);
                    _applicationDbContext.SaveChanges();

                    TempData["SuccessMessage"] = "Berat " + viewModel.Nilai + " Berhasil Diubah";
                    return RedirectToAction("Index", "Berat");
                }
                else
                {
                    TempData["WarningMessage"] = "Nilai Berat " + viewModel.Nilai + "Kg sudah ada !!!"; ;
                    return View(viewModel);
                }
            }
            return View();
        }

        [HttpGet]
        //[Authorize(Roles = "DeleteBerat")]
        [AllowAnonymous]
        //[Authorize]
        public async Task<IActionResult> DeleteBerat(Guid Id)
        {
            var Berat = await _beratRepository.GetBeratById(Id);
            if (Berat == null)
            {
                Response.StatusCode = 404;
                return View("BeratNotFound", Id);
            }

            BeratViewModel vm = new BeratViewModel
            {
                BeratId = Berat.BeratId,
                KodeBerat = Berat.KodeBerat,
                Nilai = Berat.Nilai
            };
            return View(vm);
        }

        [HttpPost]
        //[Authorize(Roles = "DeleteBerat")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteBerat(BeratViewModel vm)
        {
            //Cek Relasi Principal dengan Produk
            var produk = _produkRepository.GetAllProduk().Where(p => p.BeratId == vm.BeratId).FirstOrDefault();
            if (produk == null)
            {
                //Hapus Data
                var Berats = _applicationDbContext.Berats.FirstOrDefault(x => x.BeratId == vm.BeratId);
                _applicationDbContext.Attach(Berats);
                _applicationDbContext.Entry(Berats).State = EntityState.Deleted;
                _applicationDbContext.SaveChanges();

                TempData["SuccessMessage"] = "Berat " + vm.Nilai + " Berhasil Dihapus";
                return RedirectToAction("Index", "Berat");
            }
            else
            {
                TempData["WarningMessage"] = "Berat " + vm.Nilai + " terelasi dengan produk " + produk.NamaProduk;
                return View(vm);
            }
        }
    }
}
