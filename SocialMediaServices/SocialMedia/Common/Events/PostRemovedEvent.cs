using SharedKernel.Events;

namespace Common.Events
{
    public class PostRemovedEvent : BaseEvent
    {
        public PostRemovedEvent()
            : base(nameof(PostRemovedEvent))
        {
            //
        }
    }
}