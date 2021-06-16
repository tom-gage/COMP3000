using System;
using System.Collections.Generic;
using System.Text;

namespace COMP3000Project.Interfaces
{
    public interface Publisher
    {
        void registerSubscriber(Subscriber subscriber);
        void removeSubsciber(Subscriber subscriber);
        void updateSubscribers(string jsonData);
    }
}
