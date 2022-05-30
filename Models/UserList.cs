using System;
using System.Collections.Generic;

namespace JWTProjectManagement.Models
{
    public partial class UserList
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? Email { get; set; }
        public string? Phonenumber { get; set; }
        public string? Address { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int? UserRolesId { get; set; }
    }
}
