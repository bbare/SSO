using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    interface IResetRepository
    {
        void AddResetID(string email, string resetID, DateTime expirationTime);
        void DeleteResetID(string resetID);
        DateTime GetExpirationTime(string resetID);
        Guid GetUserID(string resetID);
        string GetResetID(Guid userID);
        string GetResetID(string email);
        bool ExistingResetIDGivenResetID(string resetID);
        bool ExistingResetIDGivenEmail(string email);
        bool UpdatePassword(Guid userIDToChangePassword, string newPasswordHash);
        List<string> GetSecurityQuestions(Guid userIDToGetQuestionsFrom);
        bool CheckSecurityAnswers(Guid userIDToGetQuestionsFrom, List<string> userSubmittedSecurityAnswers);
        void LockOut(string resetID);
        bool CheckLockOut(string resetID);
        int GetAttemptsPerResetID(string resetID);
        int GetResetIDCountPerEmail(string email);
    }
}
