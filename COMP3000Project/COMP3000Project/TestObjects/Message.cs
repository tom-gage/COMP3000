using System;
using System.Collections.Generic;
using System.Text;

namespace COMP3000Project.TestObjects
{
    class Message
    {
        public string ID { get; set; }
        public string type { get; set; }
        public string Body { get; set; }

        public Message(string ID, string type, string Body)
        {
            this.ID = ID;
            this.type = type;
            this.Body = Body;
        }
    }
}
