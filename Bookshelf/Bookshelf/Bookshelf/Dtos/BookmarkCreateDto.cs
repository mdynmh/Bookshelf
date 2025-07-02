using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookshelf.Dtos
{
    public class BookmarkCreateDto
    {
        public int UserId { get; set; }
        public int BookId { get; set; }
        public int PageNumber { get; set; }
        public string Note { get; set; }
    }
}
