using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookshelf.Services
{
    public interface INavigationAware
    {
        public void OnNavigatedTo(object? parameter);
    }
}
