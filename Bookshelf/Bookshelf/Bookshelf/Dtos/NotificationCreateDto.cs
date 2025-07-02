using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookshelf.Dtos
{
    public class NotificationCreateDto
    {
        public string Message { get; set; }
        public List<BookDto> Books { get; set; } = new();
        public List<UserDto> Users { get; set; } = new();
    }
}
