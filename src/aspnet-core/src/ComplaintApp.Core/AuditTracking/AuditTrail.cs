using System;
using System.Collections.Generic;
using System.Text;

namespace ComplaintApp.Core.AuditTracking
{
    public class AuditTrail
    {
        public int Id { get; set; }
        public Guid AuditID { get; set; }
        public string UserName { get; set; }
        public string IPAddress { get; set; }
        public string AreaAccessed { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
