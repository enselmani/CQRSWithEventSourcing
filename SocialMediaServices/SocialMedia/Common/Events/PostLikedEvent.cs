using SharedKernel.Events;

namespace Common.Events
{
    public class PostLikedEvent : BaseEvent
    {
        public PostLikedEvent()
            : base(nameof(PostLikedEvent))
        {
            //
        }
    }
}