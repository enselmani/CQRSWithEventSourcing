using SharedKernel.Events;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace SharedKernel.Domain
{
    public abstract class AggregateRoot
    {
        protected Guid _id;
        private readonly List<BaseEvent> _changes = new();

        public Guid Id
        {
            get { return _id; }
        }

        public int Version { get; set; } = -1;

        public IEnumerable<BaseEvent> GetUncommittedChanges()
        {
            return _changes;
        }

        public void MarkChangesAsCommitted()
        {
            _changes.Clear();
        }

        private void ApplyChange(BaseEvent @event, bool isNew)
        {
            MethodInfo method = this.GetType().GetMethod("Apply", new Type[] { @event.GetType() });

            if (method is null)
            {
                throw new ArgumentNullException(nameof(method),
                    $"The Apply method was not found in the aggregate for {@event.GetType().Name}!");
            }

            method.Invoke(this, new object[] { @event });

            if (isNew)
            {
                _changes.Add(@event);
            }
        }

        protected void RaiseEvent(BaseEvent @event)
        {
            ApplyChange(@event, true);
        }

        public void ReplayEvents(IEnumerable<BaseEvent> events)
        {
            foreach (BaseEvent @event in events)
            {
                ApplyChange(@event, false);
            }
        }
    }
}