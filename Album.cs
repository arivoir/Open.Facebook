using System.Runtime.Serialization;

namespace Open.Facebook
{
    [DataContract]
    public class Album
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "count")]
        public long Count { get; set; }
        [DataMember(Name = "picture")]
        public string Picture { get; set; }
        [DataMember(Name = "can_upload")]
        public bool CanUpload { get; set; }
        [DataMember(Name = "cover_photo")]
        public string CoverPhoto { get; set; }
        [DataMember(Name = "link")]
        public string Link { get; set; }
        [DataMember(Name = "privacy")]
        public string Privacy { get; set; }
        [DataMember(Name = "type")]
        public string Type { get; set; }
    }
}
