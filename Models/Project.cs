using System;
using System.Collections.Generic;

namespace JWTProjectManagement.Models
{
    public partial class Project
    {
        public int Id { get; set; }
        public string ProjectName { get; set; } = null!;
        public string ProjectDescription { get; set; } = null!;
        public int? ClientId { get; set; }
        public int? ProjectManagerId { get; set; }
        public string? CreatedDate { get; set; }
        public string? Status { get; set; }
        public int? ProjectCost { get; set; }
    }
}
