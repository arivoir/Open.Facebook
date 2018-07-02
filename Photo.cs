using System.Runtime.Serialization;

namespace Open.Facebook
{
    [DataContract]
    public class Photo
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "picture")]
        public string Picture { get; set; }
        [DataMember(Name = "link")]
        public string Link { get; set; }
        [DataMember(Name = "source")]
        public string Source { get; set; }
        [DataMember(Name = "width")]
        public long Width { get; set; }
        [DataMember(Name = "height")]
        public long Height { get; set; }
        [DataMember(Name = "can_delete")]
        public bool CanDelete { get; set; }
        [DataMember(Name = "created_time")]
        public string CreatedTime { get; set; }

        [DataMember(Name = "from")]
        public User From { get; set; }
    }

    [DataContract]
    public class Place
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "location")]
        public Location Location { get; set; }
    }

    [DataContract]
    public class Location
    {
        [DataMember(Name = "latitude")]
        public double Latitude { get; set; }
        [DataMember(Name = "longitude")]
        public double Longitude { get; set; }
        [DataMember(Name = "street")]
        public string Street { get; set; }
        [DataMember(Name = "zip")]
        public string Zip { get; set; }
    }

    [DataContract]
    public class UploadedPhoto
    {
        [DataMember(Name = "post_id")]
        public string PostId { get; set; }
        [DataMember(Name = "id")]
        public string Id { get; set; }
    }
}
