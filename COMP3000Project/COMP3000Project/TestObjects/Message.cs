using System;
using System.Collections.Generic;
using System.Text;

namespace COMP3000Project.TestObjects
{
    public class Message
    {
        public string ID { get; set; }
        public string type { get; set; }
        public string Body { get; set; }

        public object[] Items;

        public Message(string ID, string type, string Body, object[] Items)
        {
            this.ID = ID;
            this.type = type;
            this.Body = Body;
            this.Items = Items;
        }
    }
}
