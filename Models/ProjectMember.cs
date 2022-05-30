using System;
namespace JWTProjectManagement.Models
{
	public partial class ProjectMember
	{
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int UserListId { get; set; }
    }
}

