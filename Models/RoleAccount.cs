using System;
using System.Collections.Generic;

namespace Test_Account.Models;

public partial class RoleAccount
{
    public int RoleId { get; set; }

    public string? RoleName { get; set; }

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
}
