﻿using Common.DTO;
using Common.Helpers;
using Domain.Entities;

namespace Domain.Interfaces.Infra;

public interface IUserRepository
{
    Task<User> GetById(string userId, CancellationToken cancellationToken = default);
    Task<User> GetByCpf(string Cpf, CancellationToken cancellationToken = default);
    Task<string> Save(UserRequest user, CancellationToken cancellationToken = default);
    Task<ServiceResult<bool>> Update(User modifiedUser, CancellationToken cancellationToken = default);
    Task<ServiceResult<bool>> Update(User userRecovered, UserUpdateRequest user, CancellationToken cancellationToken = default);
    Task<ServiceResult<UserResponse>> ValidatePassword(LoginRequest sendedInfo, CancellationToken cancellationToken = default);
    Task<ServiceResult<bool>> UpdatePassword(string userId, UserUpdatePasswordRequest sendedInfo, CancellationToken cancellationToken = default);
    Task<ServiceResult<bool>> TempDelete(string userId, string password, CancellationToken cancellationToken = default);
}
