using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TranslateApp.Data;
using TranslateApp.Models;
using TranslateApp.Repository;

namespace TranslateApp.Tests.Repository;

public class TranslationRepositoryTests
{
    private async Task<TranslateDbContext> GetDbContext()
    {
        var options = new DbContextOptionsBuilder<TranslateDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        var databaseContext = new TranslateDbContext(options);
        databaseContext.Database.EnsureCreated();
        if (await databaseContext.Translations!.CountAsync() < 0)
        {
            for (int i = 0; i < 10; i++)
            {
                databaseContext.Translations!.Add(
                  new Translation()
                  {
                      Text = "Hello World!",
                      Translated = "23asd nasser",
                      Timestamp = DateTime.Now
                  });
                await databaseContext.SaveChangesAsync();
            }
        }
        return databaseContext;
    }

    [Fact]
    public async void TranslateRepository_Add_ReturnsBool()
    {
        //Arrange
        var club = new Translation()
        {
            Text = "Hello World!",
            Translated = "23asd nasser",
            Timestamp = DateTime.Now
        };
        var dbContext = await GetDbContext();
        var translateRepository = new TranslateRepository(dbContext);

        //Act
        bool result = translateRepository.Add(club);

        //Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async void TranslateRepository_GetAll_ReturnsList()
    {
        //Arrange
        var dbContext = await GetDbContext();
        var translateRepository = new TranslateRepository(dbContext);

        //Act
        var result = await translateRepository.GetAll();

        //Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<List<Translation>>();
    }
}
