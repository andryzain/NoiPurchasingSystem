using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NoiPurchasingSystem.Areas.MasterData.Models;
using NoiPurchasingSystem.Areas.MasterData.Repository;
using NoiPurchasingSystem.Areas.MasterData.ViewModels;
using NoiPurchasingSystem.Data;
using System.Data;

namespace NoiPurchasingSystem.Areas.MasterData.Controllers
{
    [Area("MasterData")]
    [Route("MasterData/[Controller]/[Action]")]
    //[Authorize(Roles = "LevelUser")]
    public class LevelUserController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly ILevelUserRepository _levelUserRepository;
        private readonly IPenggunaRepository _penggunaRepository;

        public LevelUserController(
            ApplicationDbContext applicationDbContext,
            ILevelUserRepository LevelUserRepository,
            IPenggunaRepository penggunaRepository
        )
        {
            _applicationDbContext = applicationDbContext;
            _levelUserRepository = LevelUserRepository;
            _penggunaRepository = penggunaRepository;
        }

        //[Authorize(Roles = "IndexLevelUser")]
        public IActionResult Index()
        {
            var data = _levelUserRepository.GetAllLevelUser();
            return View(data);
        }

        //[Authorize(Roles = "CreateLevelUser")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<ViewResult> CreateLevelUser()
        {
            var LevelUser = new LevelUserViewModel();
            var dateNow = DateTimeOffset.Now;
            var setDateNow = DateTimeOffset.Now.ToString("yyMMdd");

            var lastCode = _levelUserRepository.GetAllLevelUser().Where(d => d.CreateDateTime.ToString("yyMMdd") == dateNow.ToString("yyMMdd")).OrderByDescending(k => k.KodeLevel).FirstOrDefault();
            if (lastCode == null)
            {
                LevelUser.KodeLevel = "LVL" + setDateNow + "0001";
            }
            else
            {
                var lastCodeTrim = lastCode.KodeLevel.Substring(3, 6);

                if (lastCodeTrim != setDateNow)
                {
                    LevelUser.KodeLevel = "LVL" + setDateNow + "0001";
                }
                else
                {
                    LevelUser.KodeLevel = "LVL" + setDateNow + (Convert.ToInt32(lastCode.KodeLevel.Substring(9, lastCode.KodeLevel.Length - 9)) + 1).ToString("D4");
                }
            }

            return View(LevelUser);
        }

        //[Authorize(Roles = "CreateLevelUser")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateLevelUser(LevelUserViewModel vm)
        {
            var dateNow = DateTimeOffset.Now;
            var setDateNow = DateTimeOffset.Now.ToString("yyMMdd");

            var lastCode = _levelUserRepository.GetAllLevelUser().Where(d => d.CreateDateTime.ToString("yyMMdd") == dateNow.ToString("yyMMdd")).OrderByDescending(k => k.KodeLevel).FirstOrDefault();
            if (lastCode == null)
            {
                vm.KodeLevel = "LVL" + setDateNow + "0001";
            }
            else
            {
                var lastCodeTrim = lastCode.KodeLevel.Substring(3, 6);

                if (lastCodeTrim != setDateNow)
                {
                    vm.KodeLevel = "LVL" + setDateNow + "0001";
                }
                else
                {
                    vm.KodeLevel = "LVL" + setDateNow + (Convert.ToInt32(lastCode.KodeLevel.Substring(9, lastCode.KodeLevel.Length - 9)) + 1).ToString("D4");
                }
            }

            if (ModelState.IsValid)
            {
                var level = new LevelUser
                {
                    CreateDateTime = DateTime.Now,
                    LevelId = vm.LevelId,
                    KodeLevel = vm.KodeLevel,
                    NamaLevel = vm.NamaLevel,
                    Persentase = vm.Persentase,
                    Keterangan = vm.Keterangan,
                };

                var resultLevelUser = _levelUserRepository.GetAllLevelUser().Where(c => c.NamaLevel == vm.NamaLevel).FirstOrDefault();

                if (resultLevelUser == null)
                {
                    _levelUserRepository.Tambah(level);
                    TempData["SuccessMessage"] = "LevelUser " + vm.NamaLevel + " Berhasil Disimpan";
                    return RedirectToAction("Index", "LevelUser");
                }
                else
                {
                    TempData["WarningMessage"] = "LevelUser " + vm.NamaLevel + " sudah ada !!!";
                    return View(vm);
                }
            }
            return View();
        }

        //[Authorize(Roles = "DetailLevelUser")]
        [AllowAnonymous]
        public async Task<IActionResult> DetailLevelUser(Guid Id)
        {
            var level = await _levelUserRepository.GetLevelUserById(Id);

            if (level == null)
            {
                Response.StatusCode = 404;
                return View("LevelUserNotFound", Id);
            }

            LevelUserViewModel viewModel = new LevelUserViewModel
            {
                LevelId = level.LevelId,
                KodeLevel = level.KodeLevel,
                NamaLevel = level.NamaLevel,
                Persentase = level.Persentase,
                Keterangan = level.Keterangan,
            };
            return View(viewModel);
        }

        [HttpPost]
        //[Authorize(Roles = "DetailLevelUser")]
        [AllowAnonymous]
        public async Task<IActionResult> DetailLevelUser(LevelUserViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                LevelUser level = await _levelUserRepository.GetLevelUserByIdNoTracking(viewModel.LevelId);

                var check = _levelUserRepository.GetAllLevelUser().Where(d => d.KodeLevel == viewModel.KodeLevel).FirstOrDefault();

                if (check != null)
                {
                    level.UpdateDateTime = DateTime.Now;
                    level.KodeLevel = viewModel.KodeLevel;
                    level.NamaLevel = viewModel.NamaLevel;
                    level.Persentase = viewModel.Persentase;
                    level.Keterangan = viewModel.Keterangan;

                    _levelUserRepository.Update(level);
                    _applicationDbContext.SaveChanges();

                    TempData["SuccessMessage"] = "Level " + viewModel.NamaLevel + " Berhasil Diubah";
                    return RedirectToAction("Index", "LevelUser");
                }
                else
                {
                    TempData["WarningMessage"] = "Level " + viewModel.NamaLevel + " sudah ada !!!";
                    return View(viewModel);
                }
            }
            return View();
        }

        [HttpGet]
        //[Authorize(Roles = "DeleteLevelUser")]
        [AllowAnonymous]
        //[Authorize]
        public async Task<IActionResult> DeleteLevelUser(Guid Id)
        {
            var level = await _levelUserRepository.GetLevelUserById(Id);
            if (level == null)
            {
                Response.StatusCode = 404;
                return View("LevelUserNotFound", Id);
            }

            LevelUserViewModel vm = new LevelUserViewModel
            {
                LevelId = level.LevelId,
                KodeLevel = level.KodeLevel,
                NamaLevel = level.NamaLevel,
                Persentase = level.Persentase,
                Keterangan = level.Keterangan,
        };
            return View(vm);
        }

        [HttpPost]
        //[Authorize(Roles = "DeleteLevelUser")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteLevelUser(LevelUserViewModel vm)
        {
            //Cek Relasi Principal dengan Produk
            var pengguna = _penggunaRepository.GetAllPengguna().Where(p => p.LevelId == vm.LevelId).FirstOrDefault();
            if (pengguna == null)
            {
                //Hapus Data
                var level = _applicationDbContext.LevelUsers.FirstOrDefault(x => x.LevelId == vm.LevelId);
                _applicationDbContext.Attach(level);
                _applicationDbContext.Entry(level).State = EntityState.Deleted;
                _applicationDbContext.SaveChanges();

                TempData["SuccessMessage"] = "Level Pengguna " + vm.NamaLevel + " Berhasil Dihapus";

                return RedirectToAction("Index", "Level Pengguna");
            }
            else
            {
                TempData["WarningMessage"] = "Level Pengguna " + vm.NamaLevel + " terelasi dengan pengguna " + pengguna.NamaLengkap;
                return View(vm);
            }            
        }
    }
}
