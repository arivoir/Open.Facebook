using System.Runtime.Serialization;

namespace Open.Facebook
{
    [DataContract]
    public class Error
    {
        [DataMember(Name = "code")]
        public int Code { get; set; }
        [DataMember(Name = "message")]
        public string Message { get; set; }
        [DataMember(Name = "type")]
        public string Type { get; set; }
    }

    [DataContract]
    public class ErrorResponse
    {
        [DataMember(Name = "error")]
        public Error Error { get; set; }
    }
}
