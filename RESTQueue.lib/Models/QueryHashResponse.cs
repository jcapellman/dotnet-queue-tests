using System;
using System.Runtime.Serialization;

using RESTQueue.lib.Enums;

namespace RESTQueue.lib.Models
{
    [DataContract]
    public class QueryHashResponse
    {
        [DataMember]
        public Guid Guid { get; set; }

        [DataMember]
        public ResponseStatus Status { get; set; }

        [DataMember]
        public string MD5Hash { get; set; }

        [DataMember]
        public string ErrorMessage { get; set; }

        [DataMember]
        public bool IsMalicious { get; set; }
    }
}