using Common.DTO;
using Common.Helpers;
using Domain.Entities;
using Domain.Interfaces.Infra;
using Domain.Interfaces.Services;

namespace Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IActivationCodeService _activationCodeService;

    public UserService(
        IUserRepository userRepository,
        IActivationCodeService activationCodeService)
    {
        _userRepository = userRepository;
        _activationCodeService = activationCodeService;
    }

    public async Task<UserResponse> Get(string userId, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetById(userId, cancellationToken);

        if (user is null)
            return new();

        return new()
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            CompanyRef = user.CompanyRef
        };
    }

    public async Task<ServiceResult<string>> Save(UserRequest user, CancellationToken cancellationToken = default)
    {
        var companyRef = ValidateCompanyRef(user.CompanyRef);

        if (string.IsNullOrEmpty(companyRef))
            return ServiceResult<string>.MakeErrorResult("Invalid Cnpj (CompanyRef).");

        var getResult = await _userRepository.GetByCpf(user.Cpf, cancellationToken);

        if (getResult is not null)
            return ServiceResult<string>.MakeErrorResult("User already exists");

        var userId = string.Empty;

        user.CompanyRef = companyRef;

        if (companyRef == "Default")
        {
            userId = await _userRepository.Save(user, cancellationToken);

            await MakeActivationCode(userId, cancellationToken);

            return MakeReturn(userId);
        }

        userId = await _userRepository.Save(user, cancellationToken);

        await MakeActivationCode(userId, cancellationToken);

        return MakeReturn(userId);
    }

    public async Task<ServiceResult<bool>> Update(string userId, UserUpdateRequest request, CancellationToken cancellationToken = default)
    {
        var userRecovered = await _userRepository.GetById(userId, cancellationToken);

        if (userRecovered is null)
            return ServiceResult<bool>.MakeErrorResult("User not found.");

        if (userRecovered.CompanyRef == request.CompanyRef)
            return await _userRepository.Update(userRecovered, request, cancellationToken);

        return await _userRepository.Update(userRecovered, request, cancellationToken);
    }

    public async Task<ServiceResult<UserResponse>> ValidatePassword(LoginRequest sendedInfo, CancellationToken cancellationToken = default)
    {
        return await _userRepository.ValidatePassword(sendedInfo, cancellationToken);
    }

    public async Task<ServiceResult<bool>> UpdatePassword(string userId, UserUpdatePasswordRequest sendedInfo, CancellationToken cancellationToken = default)
    {
        return await _userRepository.UpdatePassword(userId, sendedInfo, cancellationToken);
    }

    private static string ValidateCompanyRef(string companyRef)
    {
        if (string.IsNullOrEmpty(companyRef))
            return "Default";

        if (DocumentValidator.IsValidCnpj(companyRef))
            return companyRef;

        return string.Empty;
    }

    private static ServiceResult<string> MakeReturn(string userId)
    {
        if (string.IsNullOrEmpty(userId))
            return ServiceResult<string>.MakeErrorResult("Error on save process.");

        return ServiceResult<string>.MakeSuccessResult(userId);
    }

    public async Task<ServiceResult<bool>> TempDelete(string userId, string password, CancellationToken cancellationToken = default)
    {
        return await _userRepository.TempDelete(userId, password, cancellationToken);
    }

    public async Task<bool> ActivateUser(string userId, string activationCodeId, CancellationToken cancellationToken = default)
    {
        var userRecovered = await _userRepository.GetById(userId, cancellationToken);

        if (userRecovered is null)
            return false;

        userRecovered.IsActivated = true;

        var inactivateCodeResult = await _activationCodeService.Update(activationCodeId, cancellationToken);

        if (!inactivateCodeResult)
            return false;

        return _userRepository.Update(userRecovered, cancellationToken).Result.Success;
    }

    private async Task MakeActivationCode(string userId, CancellationToken cancellationToken)
    {
        var random = new Random();

        var activationCode = new ActivationCode()
        {
            Id = Guid.NewGuid().ToString(),
            UserId = userId,
            Code = random.Next(100000, 1000000).ToString()
        };

        _ = await _activationCodeService.Save(activationCode, cancellationToken);
    }
}
