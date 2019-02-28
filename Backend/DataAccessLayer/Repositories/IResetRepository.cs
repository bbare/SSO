using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    interface IResetRepository
    {
        void addResetID(string userName, string resetID, DateTime expirationTime);
        void deleteResetID(string resetID);
        DateTime getExpirationTime(string resetID);
        Guid getUserID(string resetID);
        string getResetID(Guid userID);
        bool existingResetIDGivenResetID(string resetID);
        bool existingResetIDGivenUsername(string resetID);
        void updatePassword(Guid userIDToChangePassword, string newPasswordHash);
        List<string> getSecurityQuestions(Guid userIDToGetQuestionsFrom);
        bool checkSecurityAnswers(Guid userIDToGetQuestionsFrom, List<string> userSubmittedSecurityAnswers);
    }
}
