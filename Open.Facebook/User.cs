using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Open.Facebook
{
    [DataContract]
    public class User
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "permissions")]
        public Permissions Permissions { get; set; }
    }

    [DataContract]
    public class Permissions
    {
        [DataMember(Name = "data")]
        public List<Permission> Data { get; set; }
    }

    [DataContract]
    public class Permission
    {
        [DataMember(Name = "permission")]
        public string Value { get; set; }
        [DataMember(Name = "status")]
        public string Status { get; set; }
        //[DataMember(Name = "installed")]
        //public bool Installed { get; set; }
        //[DataMember(Name = "status_update")]
        //public bool StatusUpdate { get; set; }
        //[DataMember(Name = "photo_upload")]
        //public bool photo_upload { get; set; }
        //[DataMember(Name = "video_upload")]
        //public bool video_upload { get; set; }
        //[DataMember(Name = "create_note")]
        //public bool create_note { get; set; }
        //[DataMember(Name = "share_item")]
        //public bool share_item { get; set; }
        //[DataMember(Name = "publish_stream")]
        //public bool publish_stream { get; set; }
        //[DataMember(Name = "publish_actions")]
        //public bool publish_actions { get; set; }
        //[DataMember(Name = "user_photos")]
        //public bool user_photos { get; set; }
        //[DataMember(Name = "user_videos")]
        //public bool user_videos { get; set; }
        //[DataMember(Name = "user_photo_video_tags")]
        //public bool user_photo_video_tags { get; set; }
    }
}
