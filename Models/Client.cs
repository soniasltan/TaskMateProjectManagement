using System;
namespace JWTProjectManagement.Models
{
	public partial class Client
	{
        public int Id { get; set; }
        public string ClientName { get; set; } = null!;
        public string? Address { get; set; }
        public string? Phonenumber { get; set; }
    }
}

