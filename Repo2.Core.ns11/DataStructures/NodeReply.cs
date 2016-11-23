using System;
using System.Collections.Generic;
using Repo2.Core.ns11.Exceptions;
using Repo2.Core.ns11.Extensions.StringExtensions;

namespace Repo2.Core.ns11.DataStructures
{
    public class NodeReply : RestReply
    {
        public NodeReply(Dictionary<string, object> result) : base(result)
        {
            if (result != null)
                Nid = GetNodeID(result);
        }


        public int Nid { get; }


        public static NodeReply Fail(Exception ex)
        {
            var rep = new NodeReply(null);
            rep.Errors.Add(ex.Info(true, true));
            return rep;
        }


        private int GetNodeID(Dictionary<string, object> dict)
        {
            if (Result == null) throw 
                Fault.NullRef<Dictionary<string, object>>(nameof(Result));

            object obj;
            if (!Result.TryGetValue("nid", out obj))
                throw Fault.NoMember("nid");

            if (obj == null)
                throw Fault.NullRef<object>("nid");

            var json = obj.ToString();
            if (json.IsBlank())
                throw Fault.BlankText("json nid");

            var numbr = json.Between("[{value:", "}]");
            if (!numbr.IsNumeric())
                throw Fault.BadCast<int>(numbr);

            return numbr.ToInt();
        }
    }
}
