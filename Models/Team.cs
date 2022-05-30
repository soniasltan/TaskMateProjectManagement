using System;
namespace JWTProjectManagement.Models
{
	public partial class Team
	{
        public int Id { get; set; }
        public string TeamName { get; set; } = null!;
        public int? ProjectManagerId { get; set; }
    }
}

