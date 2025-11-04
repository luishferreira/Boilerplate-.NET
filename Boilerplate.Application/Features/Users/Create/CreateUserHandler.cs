using Boilerplate.Domain.Entities;
using Boilerplate.Domain.Interfaces;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boilerplate.Application.Features.Users.Create
{
    public sealed class CreateUserHandler(IUserRepository userRepository, IMapper mapper, IUnitOfWork unitOfWork) : IRequestHandler<CreateUserCommand, int>
    {
        public async Task<int> Handle(CreateUserCommand command, CancellationToken cancellationToken)
        {
            var user = mapper.Map<User>(command);

            user.PasswordHash = GeneratePasswordHash(command.Password);

            await userRepository.AddAsync(user);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return user.Id;
        }

        private static string GeneratePasswordHash(string password)
        {
            //fake hash just to simplify the example
            return $"hashed_{password}_placeholder";
        }
    }
}


