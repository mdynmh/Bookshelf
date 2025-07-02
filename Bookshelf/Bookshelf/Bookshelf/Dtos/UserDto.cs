using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookshelf.Dtos
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Patronymic { get; set; }
        public string Login { get; set; }
        public int RoleId { get; set; }

        public List<IssuedBookDto> IssuedBooks { get; set; } = new();
        public List<UserBookDto> UserBooks { get; set; } = new();
        public List<NotificationDto> Notifications { get; set; } = new();
    }
}
