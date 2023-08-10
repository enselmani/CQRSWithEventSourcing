using SharedKernel.Events;
using System;

namespace Common.Events
{
    public class CommentAddedEvent : BaseEvent
    {
        public CommentAddedEvent()
            : base(nameof(CommentAddedEvent))
        {
            //
        }

        public Guid CommentId { get; set; }
        public string Content { get; set; }
        public string Username { get; set; }
        public DateTime CommentDate { get; set; }
    }
}