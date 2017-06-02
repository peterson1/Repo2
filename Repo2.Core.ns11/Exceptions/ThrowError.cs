using Repo2.Core.ns11.DataStructures;

namespace Repo2.Core.ns11.Exceptions
{
    public class ThrowError
    {
        public static void IfFailed(Reply reply, string operationDesc)
        {
            if (reply.Failed)
                throw Fault.Failed(operationDesc, reply);
        }
    }
}
