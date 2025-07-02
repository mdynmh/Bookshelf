using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookshelf.Dtos
{
    public class SectionDto
    {
        public int SectionId { get; set; }
        public string Title { get; set; }
        public int StartPage { get; set; }
    }
}
