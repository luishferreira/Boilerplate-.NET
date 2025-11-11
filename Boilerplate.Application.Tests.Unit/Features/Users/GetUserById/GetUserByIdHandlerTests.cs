using AutoMapper;
using Bogus;
using Boilerplate.Application.Features.Users.GetUserById;
using Boilerplate.Application.Mappings;
using Boilerplate.Domain.Entities;
using Boilerplate.Domain.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;

namespace Boilerplate.Application.Tests.Unit.Features.Users.GetUserById
{
    public class GetUserByIdHandlerTests
    {
        private readonly IUserRepository _userRepositoryMock;
        private readonly IMapper _mapper;
        private readonly GetUserByIdHandler _handler;
        private readonly Faker _faker;

        public GetUserByIdHandlerTests()
        {
            _userRepositoryMock = Substitute.For<IUserRepository>();

            var mappingConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            }, NullLoggerFactory.Instance);

            _mapper = mappingConfig.CreateMapper();

            _handler = new GetUserByIdHandler(_userRepositoryMock, _mapper);

            _faker = new Faker();
        }

        [Fact]
        public async Task Handler_ShouldReturnUserResponse_WhenUserExists()
        {
            // Arrange
            var userId = 1;
            var query = new GetUserByIdQuery(userId);

            var fakeUser = new User
            {
                Id = userId,
                Username = _faker.Internet.UserName(),
                PasswordHash = _faker.Internet.Password(),
                CreatedAt = DateTime.UtcNow
            };

            _userRepositoryMock.GetByIdAsync(userId).Returns(Task.FromResult<User?>(fakeUser));

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(userId);
            result.Username.Should().Be(fakeUser.Username);
        }

        [Fact]
        public async Task Handler_ShouldThrowKeyNotFoundException_WhenUserNotExist()
        {
            var userId = 99;
            var query = new GetUserByIdQuery(userId);

            _userRepositoryMock.GetByIdAsync(userId).Returns(Task.FromResult<User?>(null));

            Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage($"Usuário não encontrado."); // Verifica a mensagem da exceção
        }
    }
}
