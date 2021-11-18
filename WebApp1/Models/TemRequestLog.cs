using System;
using System.Collections.Generic;

#nullable disable

namespace WebApp1.Models
{
    public partial class TemRequestLog
    {
        public int Id { get; set; }
        public DateTime? OperationDate { get; set; }
        public string OperationKod { get; set; }
        public string IpAddress { get; set; }
        public string SessionGuid { get; set; }
        public string RequestJson { get; set; }
        public string ResponseJson { get; set; }
    }
}
