using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace AccountingSoftware.Areas.Identity.Data;

// Add profile data for application users by adding properties to the AccountingSoftwareUser class
public class AccountingSoftwareUser : IdentityUser
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string? Patronymic { get; set; }
}

