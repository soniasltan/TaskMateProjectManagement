using System;
using System.Collections.Generic;

namespace JWTProjectManagement.Models
{
    public partial class FollowUp
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public string Notes { get; set; } = null!;
        public string? CreatedDate { get; set; }
        public int? UpdatedById { get; set; }
    }
}
