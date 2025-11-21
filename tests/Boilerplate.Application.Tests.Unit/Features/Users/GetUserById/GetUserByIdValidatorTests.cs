using Boilerplate.Application.Features.Users.GetUserById;
using FluentAssertions;

namespace Boilerplate.Application.Tests.Unit.Features.Users.GetUserById
{
    public class GetUserByIdValidatorTests
    {
        private readonly GetUserByIdValidator _validator;

        public GetUserByIdValidatorTests()
        {
            _validator = new GetUserByIdValidator();
        }

        [Fact]
        public async Task Validator_ShouldHaveError_WhenIdIsNotPositive()
        {
            // Arrange
            var command = new GetUserByIdQuery(0);

            // Act
            var result = await _validator.ValidateAsync(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e =>
                e.PropertyName == nameof(GetUserByIdQuery.Id) &&
                e.ErrorMessage == "O Id do utilizador deve ser um número positivo.");
        }
    }
}
