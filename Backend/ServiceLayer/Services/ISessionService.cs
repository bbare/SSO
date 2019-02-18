using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceLayer.Services
{
    public interface ISessionService
    {
        string GenerateSession();
    }
}
