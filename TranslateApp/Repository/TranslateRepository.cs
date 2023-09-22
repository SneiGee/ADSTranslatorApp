using Microsoft.EntityFrameworkCore;
using TranslateApp.Data;
using TranslateApp.Interfaces;
using TranslateApp.Models;

namespace TranslateApp.Repository;

public class TranslateRepository : ITranslateRepository
{
    private readonly TranslateDbContext _context;
    public TranslateRepository(TranslateDbContext context)
    {
        _context = context;
    }

    public bool Add(Translation translation)
    {
        _context.Add(translation);
        return Save();
    }

    public async Task<IEnumerable<Translation>> GetAll()
    {
        return await _context.Translations!.ToListAsync();
    }

    public bool Save()
    {
        int saved = _context.SaveChanges();
        return saved > 0;
    }
}
