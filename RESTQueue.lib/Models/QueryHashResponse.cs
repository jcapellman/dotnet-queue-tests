using System;
using System.Runtime.Serialization;

using Newtonsoft.Json;

using RESTQueue.lib.Common;
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

        public override string ToString() => 
            $"{Guid}::{Status}::{MD5Hash ?? Constants.RESPONSE_HASH_NOT_SET}::IsMalicious={IsMalicious}::{ErrorMessage ?? Constants.RESPONSE_NO_ERROR}";

        public string ToJSON() => JsonConvert.SerializeObject(this);
    }
}