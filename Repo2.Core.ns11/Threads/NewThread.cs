using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repo2.Core.ns11.Threads
{
    public class NewThread
    {
        public static async Task Run(Action action)
        {
            await Task.Run(async () =>
            {
                await Task.Delay(1);
                action.Invoke();
            }
            ).ConfigureAwait(false);
        }


        public static async Task<T> Run<T>(Func<T> task)
        {
            return await Task.Run(async () =>
            {
                await Task.Delay(1);
                return task.Invoke();
            }
            ).ConfigureAwait(false);
        }
    }
}
