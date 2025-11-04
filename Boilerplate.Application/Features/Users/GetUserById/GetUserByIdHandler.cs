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
            var usuario = await userRepository.GetById(query.Id) ?? throw new KeyNotFoundException($"{nameof(User)} not found");

            var response = mapper.Map<UserResponse>(usuario);

            return response;
        }
    }
}
