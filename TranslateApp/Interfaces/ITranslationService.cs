namespace TranslateApp.Interfaces;

public interface ITranslationService
{
     Task<string> TranslateAsync(string text);
}