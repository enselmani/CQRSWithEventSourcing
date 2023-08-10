using System;

namespace SharedKernel.Messages
{
    public abstract class Message
    {
        public Guid Id { get; set; }
    }
}