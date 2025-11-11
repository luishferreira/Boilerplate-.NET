using Boilerplate.Domain.Entities;
using Boilerplate.Domain.Interfaces;
using AutoMapper;
using MediatR;

namespace Boilerplate.Application.Features.Users.GetUserById
{
    public sealed class GetUserByIdHandler(
        IUserRepository userRepository,
        IMapper mapper) : IRequestHandler<GetUserByIdQuery, UserResponse>
    {
        public async Task<UserResponse> Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByIdAsync(query.Id) ?? throw new KeyNotFoundException($"Usuário não encontrado.");

            var response = mapper.Map<UserResponse>(user);

            return response;
        }
    }
}
