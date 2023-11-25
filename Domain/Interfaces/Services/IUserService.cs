using Common.DTO;
using Common.Helpers;

namespace Domain.Interfaces.Services;

public interface IUserService
{
    Task<UserResponse> Get(string userId, CancellationToken cancellationToken = default);
    Task<ServiceResult<string>> Save(UserRequest user, CancellationToken cancellationToken = default);
    Task<ServiceResult<bool>> Update(string userId, UserUpdateRequest user, CancellationToken cancellationToken = default);
    Task<ServiceResult<UserResponse>> ValidatePassword(LoginRequest sendedInfo, CancellationToken cancellationToken = default);
    Task<ServiceResult<bool>> UpdatePassword(string userId, UserUpdatePasswordRequest sendedInfo, CancellationToken cancellationToken = default);
    Task<ServiceResult<bool>> TempDelete(string userId, string password, CancellationToken cancellationToken = default);
    Task<bool> ActivateUser(string userId, string activationCode, CancellationToken cancellationToken = default);
}
