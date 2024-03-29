using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Microsoft.SharePoint.Client;
using System.Security;

using System.Net;
using System.Collections;
using System.Collections.Specialized;
using Jayrock.Json;
using Jayrock.Json.Conversion;
using System.IO;

namespace KMSharepointSync.Models
{
    public class SharepointFolder
    {
        public string TaskId { get; set; }
        public string Url { get; set; }
        public string ServerRelativeUrl { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string Parent { get; set; }
    }
    
}