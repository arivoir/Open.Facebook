using System.Runtime.Serialization;

namespace Open.Facebook
{
    [DataContract]
    public class Video
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "description")]
        public string Description { get; set; }
        [DataMember(Name = "updated_time")]
        public string UpdatedTime { get; set; }
        [DataMember(Name = "picture")]
        public string Picture { get; set; }
        [DataMember(Name = "source")]
        public string Source { get; set; }
        [DataMember(Name = "created_time")]
        public string CreatedTime { get; set; }
        [DataMember(Name = "permalink_url")]
        public string PermalinkUrl { get; set; }
    }
}