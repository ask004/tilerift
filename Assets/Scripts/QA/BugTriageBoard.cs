using System;
using System.Collections.Generic;
using System.Linq;

namespace TileRift.QA
{
    public enum BugSeverity
    {
        P0,
        P1,
        P2
    }

    [Serializable]
    public sealed class BugTicket
    {
        public string id;
        public string title;
        public BugSeverity severity;
        public bool isClosed;
    }

    public sealed class BugTriageBoard
    {
        private readonly List<BugTicket> _tickets = new();

        public IReadOnlyList<BugTicket> Tickets => _tickets;

        public void Add(string id, string title, BugSeverity severity)
        {
            _tickets.Add(new BugTicket
            {
                id = id,
                title = title,
                severity = severity,
                isClosed = false,
            });
        }

        public bool Close(string id)
        {
            var ticket = _tickets.FirstOrDefault(t => t.id == id);
            if (ticket == null)
            {
                return false;
            }

            ticket.isClosed = true;
            return true;
        }

        public int OpenCount(BugSeverity severity)
        {
            return _tickets.Count(t => t.severity == severity && !t.isClosed);
        }

        public bool IsReleaseClear()
        {
            return OpenCount(BugSeverity.P0) == 0 && OpenCount(BugSeverity.P1) == 0;
        }
    }
}
