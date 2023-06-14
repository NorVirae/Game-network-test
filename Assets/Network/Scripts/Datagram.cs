using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Network
{
    public class Datagram
    {
        public EventType type;
        public object key;
        public object body;
        public object clientCallabckId;

        public Datagram(EventType type, object body, object id = null)
        {
            this.type = type;
            this.body = body;
            this.clientCallabckId = id;
        }

        public override string ToString()
        {
            //object data = new { type = this.type, body = this.body };
            string sd = JsonConvert.SerializeObject(this);
            return sd;
        }
    }

}
