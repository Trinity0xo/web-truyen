using Humanizer.Localisation;
using Microsoft.EntityFrameworkCore;
using WEBTRUYEN.Models;

namespace WEBTRUYEN.Repository
{
    public class EFChapterRepository : IChapterRepository
    {
        private readonly ApplicationDbContext _context;

        public EFChapterRepository(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }

        public async Task AddAsync(Chapter chapter)
        {
            var comic = await _context.Comics.SingleOrDefaultAsync(c => c.Id == chapter.ComicId);

            comic.UpdatedDate = DateTime.UtcNow;

            _context.Chapters.Add(chapter);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var chapter = await _context.Chapters.FindAsync(id);
            _context.Chapters.Remove(chapter);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Chapter>> GetAllAsync(int id)
        {
            return await _context.Chapters.Where(c => c.ComicId == id).ToListAsync();
        }

        public async Task<Chapter> GetByIdAsync(int id)
        {
            return await _context.Chapters.Include(p => p.Pages.
                OrderByDescending(p => p.Id)).SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task UpdateAsync(Chapter chapter)
        {
            _context.Chapters.Update(chapter);
            await _context.SaveChangesAsync();
        }
    }
}
