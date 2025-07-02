using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookshelf.Dtos
{
    public class IssuedBookDto
    {
        public int IssueId { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public DateTime IssuedAt { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnedAt { get; set; }

        public BookDto Book { get; set; } = null!;
    }
}
