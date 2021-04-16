using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageLibrary.Services
{
    public interface IMessageService
    {
        bool Enqueue(string message);
    }
}
