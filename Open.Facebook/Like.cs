using System.Runtime.Serialization;

namespace Open.Facebook
{
    [DataContract]
    public class Like
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
    }
}
