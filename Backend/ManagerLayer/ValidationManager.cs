using ServiceLayer.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace ManagerLayer
{
    public class ValidationManager
    {
        /// <summary>
        ///     This is not ready for review
        ///     Not part of our sprint 3
        /// </summary>
        /// <param name="password"></param>
        public void CheckPassword(string password)
        {
            IPasswordService _passwordService = new PasswordService();
            object passwordResponse = _passwordService.CheckPasswordPwned(password);
        }
    }
}
