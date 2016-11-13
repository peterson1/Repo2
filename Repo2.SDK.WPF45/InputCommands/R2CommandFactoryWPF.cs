using System;
using System.Threading.Tasks;

namespace Repo2.SDK.WPF45.InputCommands
{
    public class R2Command
    {
        public static R2AsyncCommandWPF Async(Func<Task> task, Predicate<object> canExecute = null, string buttonLabel = null)
            => new R2AsyncCommandWPF(task, canExecute, buttonLabel);



        public static R2AsyncCommandWPF Relay(Action action, Predicate<object> canExecute = null, string buttonLabel = null)
            => new R2AsyncCommandWPF(async () =>
            {
                await Task.Delay(1);
                action?.Invoke();
            },
            canExecute, buttonLabel);
    }
}
