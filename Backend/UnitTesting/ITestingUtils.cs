using DataAccessLayer.Database;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTesting
{
    interface ITestingUtils
    {
        User createUser();
        Session createSession(User user);
        bool isEqual(string[] arr1, string[] arr2);

    }
}
