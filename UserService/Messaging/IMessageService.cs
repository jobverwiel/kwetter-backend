using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Messaging
{
    public interface IMessageService
    {
        bool Enqueue(string message);
    }
}
