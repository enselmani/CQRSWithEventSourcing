using System.Collections.Generic;
using System;

namespace Domain.Entities
{
    public class Post
    {
        public Post()
        {
            Comments = new List<Comment>();
        }

        public Guid PostId { get; set; }
        public string Author { get; set; }
        public DateTime DatePosted { get; set; }
        public string Message { get; set; }
        public int Likes { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}