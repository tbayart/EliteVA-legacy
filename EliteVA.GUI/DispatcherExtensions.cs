using System;
using System.Runtime.CompilerServices;
using System.Windows.Threading;

namespace EliteVA.GUI
{
    public static class DispatcherExtensions
    {
        /// <summary>
        /// Awaitable method to switch to <paramref name="dispatcher"/> thread.
        /// https://medium.com/criteo-engineering/switching-back-to-the-ui-thread-in-wpf-uwp-in-modern-c-5dc1cc8efa5e
        /// </summary>
        /// <param name="dispatcher">The <see cref="Dispatcher"/> to switch the thread to.</param>
        /// <returns>A <see cref="SwitchThreadAwaiter"/> instance.</returns>
        public static SwitchThreadAwaiter SwitchThread(this Dispatcher dispatcher)
        {
            return new SwitchThreadAwaiter(dispatcher);
        }

        public struct SwitchThreadAwaiter : INotifyCompletion
        {
            private readonly Dispatcher _dispatcher;

            public SwitchThreadAwaiter(Dispatcher dispatcher)
            {
                _dispatcher = dispatcher;
            }

            public SwitchThreadAwaiter GetAwaiter() => this;

            public void GetResult() { }

            public bool IsCompleted => _dispatcher.CheckAccess();

            public void OnCompleted(Action continuation)
            {
                _dispatcher.BeginInvoke(continuation);
            }
        }
    }
}
