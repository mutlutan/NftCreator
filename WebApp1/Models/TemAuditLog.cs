using System;
using System.Collections.Generic;

#nullable disable

namespace WebApp1.Models
{
    public partial class TemAuditLog
    {
        public int Id { get; set; }
        public DateTime? OperationDate { get; set; }
        public int UserId { get; set; }
        public string IpAddress { get; set; }
        public string OperationType { get; set; }
        public string TableName { get; set; }
        public string PrimaryKeyField { get; set; }
        public string PrimaryKeyValue { get; set; }
        public string CurrentValues { get; set; }
        public string OriginalValues { get; set; }
    }
}
