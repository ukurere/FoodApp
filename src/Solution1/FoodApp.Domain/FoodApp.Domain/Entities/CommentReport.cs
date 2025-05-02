using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodApp.Domain.Entities
{
    public class CommentReport
    {
        public int Id { get; set; }

        public int CommentId { get; set; }
        public Comment Comment { get; set; }

        public int ReporterId { get; set; }
        public User Reporter { get; set; }

        public DateTime ReportedAt { get; set; }

        public string Reason { get; set; } = "Inappropriate";
    }

}
