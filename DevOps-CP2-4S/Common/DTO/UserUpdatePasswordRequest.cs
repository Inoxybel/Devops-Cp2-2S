﻿namespace Common.DTO;

public class UserUpdatePasswordRequest
{
    public string Email { get; set; }
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }
    public string ConfirmPassword { get; set; }
}
