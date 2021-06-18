using COMP3000Project.TestObjects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace COMP3000Project.Interfaces
{
    public interface Subscriber
    {
        void Update(Message message);
    }
}
