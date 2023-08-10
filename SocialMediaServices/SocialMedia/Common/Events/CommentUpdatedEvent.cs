using SharedKernel.Events;
using System;

namespace Common.Events
{
    public class CommentUpdatedEvent : BaseEvent
    {
        public CommentUpdatedEvent()
            : base(nameof(CommentUpdatedEvent))
        {
            //
        }

        public Guid CommentId { get; set; }
        public string Content { get; set; }
        public string Username { get; set; }
        public DateTime EditDate { get; set; }
    }
}