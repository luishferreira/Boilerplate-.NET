using AutoMapper;
using Bogus;
using Boilerplate.Application.Features.Users.Create;
using Boilerplate.Application.Mappings;
using Boilerplate.Domain.Entities;
using Boilerplate.Domain.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using System.Security.Cryptography;

namespace Boilerplate.Application.Tests.Unit.Features.Users.Create
{
    public class CreateUserHandlerTests
    {
        private readonly IUserRepository _userRepositoryMock;
        private readonly IUnitOfWork _unitOfWorkMock;
        private readonly IMapper _mapper;
        private readonly CreateUserHandler _handler;
        private readonly Faker _commandFaker;

        public CreateUserHandlerTests()
        {
            _userRepositoryMock = Substitute.For<IUserRepository>();
            _unitOfWorkMock = Substitute.For<IUnitOfWork>();

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            }, NullLoggerFactory.Instance);
            _mapper = mappingConfig.CreateMapper();

            _handler = new CreateUserHandler(_userRepositoryMock, _mapper, _unitOfWorkMock);

            _commandFaker = new Faker();
        }

        private static void SetEntityId(BaseEntity entity, int id)
        {
            var idProperty = typeof(BaseEntity).GetProperty("Id")!;
            idProperty.SetValue(entity, id);
        }

        [Fact]
        public async Task Handle_ShouldCreateUserAndReturnUserId_WhenCommandIsValid()
        {
            // Arrange
            var password = _commandFaker.Internet.Password(10);
            var command = new CreateUserCommand
            (
                _commandFaker.Internet.UserName(),
                password,
                password
            );

            var userIdExpected = RandomNumberGenerator.GetInt32(256);
            User? capturedUser = null;

            _userRepositoryMock.AddAsync(Arg.Any<User>()).Returns(callInfo =>
            {
                capturedUser = callInfo.Arg<User>();
                SetEntityId(capturedUser, userIdExpected);

                return Task.CompletedTask;
            });

            _unitOfWorkMock.SaveChangesAsync(CancellationToken.None).Returns(Task.FromResult(1));

            // Act
            var resultId = await _handler.Handle(command, CancellationToken.None);

            // Assert
            resultId.Should().Be(userIdExpected);

            await _userRepositoryMock.Received(1).AddAsync(Arg.Any<User>());
            await _unitOfWorkMock.Received(1).SaveChangesAsync(CancellationToken.None);

            capturedUser.Should().NotBeNull();
            capturedUser.Username.Should().Be(command.Username);
            capturedUser.PasswordHash.Should().NotBe(command.Password);
            capturedUser.PasswordHash.Should().StartWith("hashed_");
        }
    }
}
