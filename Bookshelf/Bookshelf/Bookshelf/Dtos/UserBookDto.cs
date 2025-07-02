using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookshelf.Dtos
{
    public class UserBookDto
    {
        public int UserId { get; set; }
        public int BookId { get; set; }
        public int CurrentPage { get; set; }

        public BookDto Book { get; set; }
        public List<BookmarkDto> Bookmarks { get; set; }
    }
}
