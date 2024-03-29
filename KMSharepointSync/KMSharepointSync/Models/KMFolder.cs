using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KMSharepointSync.Models
{
    public class KMFolder
    {
        public string TaskId { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string Parent { get; set; }
    }
}