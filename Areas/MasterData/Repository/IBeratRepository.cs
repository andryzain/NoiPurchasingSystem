using Microsoft.EntityFrameworkCore;
using NoiPurchasingSystem.Areas.MasterData.Models;
using NoiPurchasingSystem.Data;

namespace NoiPurchasingSystem.Areas.MasterData.Repository
{
    public class IBeratRepository
    {
        private readonly ApplicationDbContext _context;

        public IBeratRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Berat Tambah(Berat Berat)
        {
            _context.Berats.Add(Berat);
            _context.SaveChanges();
            return Berat;
        }

        public async Task<Berat> GetBeratById(Guid Id)
        {
            var Berat = await _context.Berats.FindAsync(Id);

            if (Berat != null)
            {
                var BeratDetail = new Berat()
                {
                    BeratId = Berat.BeratId,
                    KodeBerat = Berat.KodeBerat,
                    Nilai = Berat.Nilai
                };
                return BeratDetail;
            }
            return null;
        }

        public async Task<Berat> GetBeratByIdNoTracking(Guid Id)
        {
            return await _context.Berats.AsNoTracking().FirstOrDefaultAsync(a => a.BeratId == Id);
        }

        public async Task<List<Berat>> GetBerats()
        {
            return await _context.Berats.OrderBy(p => p.CreateDateTime).Select(Berat => new Berat()
            {
                BeratId = Berat.BeratId,
                KodeBerat = Berat.KodeBerat,
                Nilai = Berat.Nilai,
            }).ToListAsync();
        }

        public IEnumerable<Berat> GetAllBerat()
        {
            return _context.Berats.AsNoTracking();
        }

        public Berat Update(Berat update)
        {
            var Berat = _context.Berats.Attach(update);
            Berat.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return update;
        }

        public Berat Delete(Guid Id)
        {
            var Berat = _context.Berats.Find(Id);
            if (Berat != null)
            {
                _context.Berats.Remove(Berat);
                _context.SaveChanges();
            }
            return Berat;
        }
    }
}
