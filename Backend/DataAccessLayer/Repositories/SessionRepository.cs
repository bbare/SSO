using DataAccessLayer.Models;
using DataAccessLayer.Database;
using System;
using System.Data.Entity;
using System.Linq;

namespace DataAccessLayer.Repositories
{
    public class SessionRepository : ISessionRepository
    {
        public Session GetSession(DatabaseContext _db, string token)
        {
            var session = _db.Sessions
                .Where(s => s.Token == token)
                .FirstOrDefault<Session>();

            return session;
        }
        
        public Session CreateSession(DatabaseContext _db, Session session)
        {
            _db.Entry(session).State = EntityState.Added;
            return session;
        }

        public Session UpdateSession(DatabaseContext _db, Session session)
        {
            session.UpdatedAt = DateTime.UtcNow;
            session.ExpiresAt = DateTime.UtcNow.AddMinutes(Session.MINUTES_UNTIL_EXPIRATION);
            _db.Entry(session).State = EntityState.Modified;
            return session;
        }

        public Session DeleteSession(DatabaseContext _db, string token)
        {
            var session = _db.Sessions
                .Where(s => s.Token == token)
                .FirstOrDefault<Session>();
            if (session == null)
                return null;
            _db.Entry(session).State = EntityState.Deleted;
			_db.SaveChanges();
            return session;
        }
    }
}
