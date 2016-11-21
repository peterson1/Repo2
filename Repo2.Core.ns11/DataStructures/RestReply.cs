using System.Collections.Generic;

namespace Repo2.Core.ns11.DataStructures
{
    public class RestReply : Reply<Dictionary<string, object>>
    {
        public RestReply(Dictionary<string, object> result) : base(result)
        {
        }
    }
}
