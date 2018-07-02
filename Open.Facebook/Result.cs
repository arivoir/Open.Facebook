using System.Runtime.Serialization;

namespace Open.Facebook
{
    [DataContract]
    public class Result
    {
        [DataMember(Name = "success")]
        public bool Success { get; set; }
    }
}
