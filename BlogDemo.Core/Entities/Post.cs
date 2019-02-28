using System;
using System.Collections.Generic;
using System.Text;

namespace BlogDemo.Core.Entities
{
    public class Post:Entity
    {
        public string Titles { get; set; }
        public string Body { get; set; }
        public string Author { get; set; }
        public DateTime LastModified { get; set; }

    }
}
