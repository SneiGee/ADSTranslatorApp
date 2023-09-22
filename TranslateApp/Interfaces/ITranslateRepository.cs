using TranslateApp.Models;

namespace TranslateApp.Interfaces;

public interface ITranslateRepository
{
    Task<IEnumerable<Translation>> GetAll();
    bool Add(Translation translation);
    bool Save();
}
