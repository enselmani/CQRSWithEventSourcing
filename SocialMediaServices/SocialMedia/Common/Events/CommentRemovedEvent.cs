using SharedKernel.Events;
using System;

namespace Common.Events
{
    public class CommentRemovedEvent : BaseEvent
    {
        public CommentRemovedEvent()
            : base(nameof(CommentRemovedEvent))
        {
            //
        }

        public Guid CommentId { get; set; }
    }
}