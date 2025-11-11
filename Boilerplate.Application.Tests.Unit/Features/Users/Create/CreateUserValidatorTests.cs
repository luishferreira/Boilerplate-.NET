using Bogus;
using Boilerplate.Application.Features.Users.Create;
using Boilerplate.Domain.Interfaces;
using FluentAssertions;
using NSubstitute;

namespace Boilerplate.Application.Tests.Unit.Features.Users.Create
{
    public class CreateUserValidatorTests
    {
        private readonly IUserRepository _userRepositoryMock;
        private readonly CreateUserValidator _validator;
        private readonly Faker _faker = new();

        public CreateUserValidatorTests()
        {
            _userRepositoryMock = Substitute.For<IUserRepository>();
            _validator = new CreateUserValidator(_userRepositoryMock);
        }

        [Fact]
        public async Task Validator_ShouldBeValid_WhenCommandIsOk()
        {
            // Arrange
            var validUsername = _faker.Internet.UserName();
            var validPassword = _faker.Internet.Password(8);
            var command = new CreateUserCommand(validUsername, validPassword, validPassword);

            _userRepositoryMock.UsernameExistsAsync(validUsername).Returns(Task.FromResult(false));

            // Act
            var result = await _validator.ValidateAsync(command);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public async Task Validator_ShouldHaveError_WhenUsernameIsEmpty()
        {
            // Arrange
            var validPassword = _faker.Internet.Password(8);
            var command = new CreateUserCommand("", validPassword, validPassword);

            // Act
            var result = await _validator.ValidateAsync(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e =>
                e.PropertyName == nameof(CreateUserCommand.Username) &&
                e.ErrorMessage.Contains("O nome é obrigatório."));
        }

        [Fact]
        public async Task Validator_ShouldHaveError_WhenPasswordsNotMatch()
        {
            // Arrange
            var username = _faker.Internet.UserName();
            var password1 = _faker.Internet.Password(10, prefix: "A");
            var password2 = _faker.Internet.Password(10, prefix: "B");
            var command = new CreateUserCommand(username, password1, password2);

            _userRepositoryMock.UsernameExistsAsync(username).Returns(Task.FromResult(false));

            // Act
            var result = await _validator.ValidateAsync(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e =>
                e.PropertyName == nameof(CreateUserCommand.ConfirmPassword) &&
                e.ErrorMessage.Contains("As senhas não coincidem."));
        }

        [Fact]
        public async Task Validator_ShouldHaveError_WhenUsernameAlreadyExists()
        {
            // Arrange
            var existingUsername = _faker.Internet.UserName();
            var password = _faker.Internet.Password(10);
            var command = new CreateUserCommand(existingUsername, password, password);

            _userRepositoryMock.UsernameExistsAsync(existingUsername).Returns(Task.FromResult(true));

            // Act
            var result = await _validator.ValidateAsync(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e =>
                e.PropertyName == nameof(CreateUserCommand.Username) &&
                e.ErrorMessage.Contains("Este nome já está a ser utilizado."));
        }

        [Fact]
        public async Task Validator_ShouldHaveError_WhenPasswordTooShort()
        {
            // Arrange
            var username = _faker.Internet.UserName();
            var shortPassword = _faker.Internet.Password(5); // Less than 8 characters
            var command = new CreateUserCommand(username, shortPassword, shortPassword);

            _userRepositoryMock.UsernameExistsAsync(username).Returns(Task.FromResult(false));

            // Act
            var result = await _validator.ValidateAsync(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e =>
                e.PropertyName == nameof(CreateUserCommand.Password) &&
                e.ErrorMessage.Contains("A senha deve ter pelo menos 8 caracteres."));
        }

        [Fact]
        public async Task Validator_ShouldHaveError_WhenUsernameTooLong()
        {
            // Arrange
            var longUsername = _faker.Internet.UserName().PadRight(151, 'a');
            var password = _faker.Internet.Password(10);
            var command = new CreateUserCommand(longUsername, password, password);

            _userRepositoryMock.UsernameExistsAsync(longUsername).Returns(Task.FromResult(false));

            // Act
            var result = await _validator.ValidateAsync(command);
            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e =>
                e.PropertyName == nameof(CreateUserCommand.Username) &&
                e.ErrorMessage.Contains("O nome não pode exceder 150 caracteres."));
        }

        [Fact]
        public async Task Validator_ShouldHaveError_WhenPasswordIsEmpty()
        {
            var username = _faker.Internet.UserName();
            var command = new CreateUserCommand(username, "", "");

            _userRepositoryMock.UsernameExistsAsync(username).Returns(Task.FromResult(false));

            var result = await _validator.ValidateAsync(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e =>
                e.PropertyName == nameof(CreateUserCommand.Password) &&
                e.ErrorMessage.Contains("A senha é obrigatória."));
        }

        [Fact]
        public async Task Validator_ShouldHaveMultipleErrors_WhenMultipleRulesViolated()
        {
            // Arrange
            var longUsername = _faker.Internet.UserName().PadRight(151, 'a');
            var shortPassword = _faker.Internet.Password(5);
            var differentPassword = _faker.Internet.Password(6);
            var command = new CreateUserCommand(longUsername, shortPassword, differentPassword);

            _userRepositoryMock.UsernameExistsAsync(longUsername).Returns(Task.FromResult(true));

            // Act
            var result = await _validator.ValidateAsync(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCountGreaterThan(1);
        }
    }
}
