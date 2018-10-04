using System;
using System.Runtime.Serialization;

namespace RESTQueue.lib.Models
{
    [DataContract]
    public class QueryHashResponse
    {
        [DataMember]
        public Guid Guid { get; set; }
    }
}