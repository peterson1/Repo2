using System;
using System.Threading.Tasks;

namespace Repo2.Core.ns11.Threads
{
    public class NewThread
    {
        public static async Task WaitFor(Action action)
        {
            await Task.Run(async () =>
            {
                await Task.Delay(0);
                action.Invoke();
            }
            ).ConfigureAwait(false);
        }


        public static async Task<T> WaitFor<T>(Func<T> task)
        {
            return await Task.Run(async () =>
            {
                await Task.Delay(0);
                return task.Invoke();
            }
            ).ConfigureAwait(false);
        }


        public static void ParallelRun(Action action)
        {
            Task.Run(async () =>
            {
                await Task.Delay(0);
                action.Invoke();
            }
            ).ConfigureAwait(false);
        }
    }
}
