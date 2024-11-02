using System;
using System.Collections.Generic;

namespace Test_Account.Models;

public partial class Account
{
    public int AccId { get; set; }

    public string? FullName { get; set; }

    public string? Password { get; set; }

    public DateOnly? Dob { get; set; }

    public string? Email { get; set; }

    public int? Role { get; set; }

    public string? Address { get; set; }

    public int? Sex { get; set; }

    public int? AccStatus { get; set; }

    public virtual RoleAccount? RoleNavigation { get; set; }
}
