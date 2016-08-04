using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Barcelona.Models
{
    public class Rootobject
    {
        public Class1[] Property1 { get; set; }
    }

    public class Class1
    {
        public string faceId { get; set; }
        public Facerectangle faceRectangle { get; set; }
        public Attributes attributes { get; set; }
    }

    public class Facerectangle
    {
        public int top { get; set; }
        public int left { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }

    public class Attributes
    {
        public Headpose headPose { get; set; }
        public string gender { get; set; }
        public int age { get; set; }
    }

    public class Headpose
    {
        public float pitch { get; set; }
        public float roll { get; set; }
        public float yaw { get; set; }
    }
}