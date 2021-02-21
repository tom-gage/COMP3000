using System;
using System.Collections.Generic;
using System.Text;

namespace COMP3000Project.TestObjects
{
    class Message
    {
        public string ID { get; set; }
        public string Type { get; set; }
        public string Body { get; set; }

        public Message(string ID, string Type, string Body)
        {
            this.ID = ID;
            this.Type = Type;
            this.Body = Body;
        }
    }
}
