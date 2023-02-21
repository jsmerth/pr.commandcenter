using FluentValidation.TestHelper;
using PR.CommandCenter.Modules.Bots.Application;
using Xunit;

namespace PR.CommandCenter.Modules.Bots.Tests.Unit
{
  public class BotValidationTests
  {
    [Theory]
    [Trait("Bots", "ValidationRule")]
    [InlineData(null,null,null,null)]
    [InlineData("","","","")]
    public void Bot_Create_Requirements_ShouldNotBeNullOrEmpty(string business, string city, string region, string country)
    {
      //Arrange
      var model = new BotCreate.Request
      {
        Business = business,
        City = city,
        Region = region,
        Country = country
      };
      var validator = new BotCreate.Validator();

      //Act
      var result = validator.TestValidate(model);

      //Assert
      result.ShouldHaveValidationErrorFor(x => x.Business);
      result.ShouldHaveValidationErrorFor(x => x.City);
      result.ShouldHaveValidationErrorFor(x => x.Region);
      result.ShouldHaveValidationErrorFor(x => x.Country);
    }
  }
}