namespace Allors.Excel
{
    using System;

    using NLog;

    public partial class Mediator
    {
        public delegate void ChangeEventHandler(object sender, EventArgs eventArgs);

        public event ChangeEventHandler StateChanged;

        protected Logger Logger { get; } = LogManager.GetCurrentClassLogger();

        public virtual void OnStateChanged() => this.StateChanged?.Invoke(this, new EventArgs());
    }
}
