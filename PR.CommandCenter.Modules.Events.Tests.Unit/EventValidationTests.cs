using FluentValidation.TestHelper;
using PR.CommandCenter.Modules.Events.Application;
using Xunit;

namespace PR.CommandCenter.Modules.Events.Tests.Unit
{
  public class EventValidationTests
  {
    [Theory]
    [Trait("Events", "ValidationRule")]
    [InlineData(null)]
    [InlineData("")]
    public void Event_Create_Requirements_ShouldNotBeNullOrEmpty(string name)
    {
      //Arrange
      var model = new EventCreate.Request
      {
        Name = name
      };
      var validator = new EventCreate.Validator();

      //Act
      var result = validator.TestValidate(model);

      //Assert
      result.ShouldHaveValidationErrorFor(x => x.Name);
    }
  }
}