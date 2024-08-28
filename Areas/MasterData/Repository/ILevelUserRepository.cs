using Microsoft.EntityFrameworkCore;
using NoiPurchasingSystem.Areas.MasterData.Models;
using NoiPurchasingSystem.Data;

namespace NoiPurchasingSystem.Areas.MasterData.Repository
{
    public class ILevelUserRepository
    {
        private readonly ApplicationDbContext _context;

        public ILevelUserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public LevelUser Tambah(LevelUser LevelUser)
        {
            _context.LevelUsers.Add(LevelUser);
            _context.SaveChanges();
            return LevelUser;
        }

        public async Task<LevelUser> GetLevelUserById(Guid Id)
        {
            var LevelUser = await _context.LevelUsers.FindAsync(Id);

            if (LevelUser != null)
            {
                var LevelUserDetail = new LevelUser()
                {
                    LevelId = LevelUser.LevelId,
                    KodeLevel = LevelUser.KodeLevel,
                    NamaLevel = LevelUser.NamaLevel,
                    Persentase = LevelUser.Persentase,
                    Keterangan = LevelUser.Keterangan,
                };
                return LevelUserDetail;
            }
            return null;
        }

        public async Task<LevelUser> GetLevelUserByIdNoTracking(Guid Id)
        {
            return await _context.LevelUsers.AsNoTracking().FirstOrDefaultAsync(a => a.LevelId == Id);
        }

        public async Task<List<LevelUser>> GetLevelUsers()
        {
            return await _context.LevelUsers.OrderBy(p => p.CreateDateTime).Select(LevelUser => new LevelUser()
            {
                LevelId = LevelUser.LevelId,
                KodeLevel = LevelUser.KodeLevel,
                NamaLevel = LevelUser.NamaLevel,
                Persentase = LevelUser.Persentase,
                Keterangan = LevelUser.Keterangan,
            }).ToListAsync();
        }

        public IEnumerable<LevelUser> GetAllLevelUser()
        {
            return _context.LevelUsers.AsNoTracking();
        }

        public LevelUser Update(LevelUser update)
        {
            var LevelUser = _context.LevelUsers.Attach(update);
            LevelUser.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return update;
        }

        public LevelUser Delete(Guid Id)
        {
            var LevelUser = _context.LevelUsers.Find(Id);
            if (LevelUser != null)
            {
                _context.LevelUsers.Remove(LevelUser);
                _context.SaveChanges();
            }
            return LevelUser;
        }
    }
}
