using System.Runtime.Serialization;

namespace Open.Facebook
{
    [DataContract]
    public class Comment
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "from")]
        public User From { get; set; }
        [DataMember(Name = "message")]
        public string Message { get; set; }
        [DataMember(Name = "can_remove")]
        public bool CanRemove { get; set; }
        [DataMember(Name = "created_time")]
        public string CreatedTime { get; set; }
        [DataMember(Name = "like_count")]
        public long LikeCount { get; set; }
        [DataMember(Name = "user_likes")]
        public bool UserLikes { get; set; }
    }
}
