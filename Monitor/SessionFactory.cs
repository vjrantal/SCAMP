using System;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.Framework.DependencyInjection;
using Microsoft.AspNet.WebSockets.Server;
using Microsoft.AspNet.WebSockets.Protocol;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using ScampTypes.Messages;
using System.Collections.Generic;
using System.Net.WebSockets;

namespace Monitor
{
    public class SessionFactory
    {
        ConcurrentDictionary<Guid, Session> sessions = new ConcurrentDictionary<Guid, Session>();
        ConcurrentDictionary<Session, List<string>> perSessionSubs = new ConcurrentDictionary<Session, List<string>>();
        ConcurrentDictionary<string, HashSet<Session>> perUserSubs = new ConcurrentDictionary<string, HashSet<Session>>();

        public Session Create(WebSocket ws)
        {
            var session = new Session(this, ws);
            sessions[session.Id] = session;
            return session;
        }

        public void Remove(Session session)
        {
            sessions.TryRemove(session.Id, out session);
            foreach (var u in perSessionSubs[session])
                perUserSubs[u].Remove(session);
        }

        public void AddSubscription(Session session, Subscribe sub)
        {
            HashSet<Session> subs;
            if (!perUserSubs.TryGetValue(sub.User, out subs))
                subs = perUserSubs[sub.User] = new HashSet<Session>();

            lock (subs)
            {
                subs.Add(session);
            }

            List<string> list;
            if (!perSessionSubs.TryGetValue(session, out list))
                list = perSessionSubs[session] = new List<string>();

            list.Add(sub.User);
        }

        public List<string> GetSubscriptions(Session session)
        {
            List<string> list;
            if (perSessionSubs.TryGetValue(session, out list))
                return list;
            return null;
        }
    }
}