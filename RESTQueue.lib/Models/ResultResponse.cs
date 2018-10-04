using System.Runtime.Serialization;

namespace RESTQueue.lib.Models
{
    [DataContract]
    public class ResultResponse
    {
        [DataMember]
        public string Result { get; set; }
    }
}