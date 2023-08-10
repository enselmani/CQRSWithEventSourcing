using System;
using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class Comment
    {
        public Guid CommentId { get; set; }
        public string Username { get; set; }
        public DateTime CommentDate { get; set; }
        public string Content { get; set; }
        public bool Edited { get; set; }

        [JsonIgnore]    // To avoid object cycle
        public Post Post { get; set; }

        public Guid PostId { get; set; }
    }
}