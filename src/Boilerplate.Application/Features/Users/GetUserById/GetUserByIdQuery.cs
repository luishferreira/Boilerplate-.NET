using Boilerplate.Application.Common.Interfaces;

namespace Boilerplate.Application.Features.Users.GetUserById
{
    public sealed record GetUserByIdQuery(int Id) : IQuery<UserResponse>;
}
