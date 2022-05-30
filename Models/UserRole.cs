using System;
using System.Collections.Generic;

namespace JWTProjectManagement.Models
{
    public partial class UserRole
    {
        public int Id { get; set; }
        public string Role { get; set; } = null!;
    }
}
