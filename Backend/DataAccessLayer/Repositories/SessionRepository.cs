using DataAccessLayer.Models;
using DataAccessLayer.Database;
using System;
using System.Data.Entity;
using System.Linq;

namespace DataAccessLayer.Repositories
{
    public class SessionRepository
    {
        //returns null if no valid session is found in the sessions table, otherwise
        //  returns the current session
        public Session GetSession(DatabaseContext _db, string token, Guid userId)
        {
            var session = _db.Sessions
                .Where(s => s.Token == token && s.UserId == userId)
                .FirstOrDefault<Session>();

            return session;
        }
        
        public Session CreateSession(DatabaseContext _db, Session session)
        {
            _db.Entry(session).State = EntityState.Added;
            return session;
        }

        public Session ValidateSession(DatabaseContext _db, string token, Guid userId)
        {
            Session session = GetSession(_db, token, userId);

            if (session == null || session.Token != token)
            {
                return null;
            }
            else if (session.ExpiresAt < DateTime.UtcNow)
            {
                DeleteSession(_db, token, userId);
                return null;
            }
            else
            {
                return session;
            }
        }

        public Session UpdateSession(DatabaseContext _db, Session session)
        {
            session.UpdatedAt = DateTime.UtcNow;
            session.ExpiresAt = DateTime.UtcNow.AddMinutes(Session.MINUTES_UNTIL_EXPIRATION);
            _db.Entry(session).State = EntityState.Modified;
            return session;
        }

        public Session DeleteSession(DatabaseContext _db, string token, Guid userId)
        {
            var session = _db.Sessions
                .Where(s => s.Token == token && s.UserId == userId)
                .FirstOrDefault<Session>();
            if (session == null)
                return null;
            _db.Entry(session).State = EntityState.Deleted;
            return session;
        }
    }
}
