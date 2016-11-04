using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repo2.Core.ns11.Authentication
{
    public abstract class R2D8CredentialsCheckerBase : IR2CredentialsChecker
    {
        public bool  CanRead   { get; private set; }
        public bool  CanWrite  { get; private set; }


        public Task Check(R2Credentials credentials)
        {
            throw new NotImplementedException();
        }
    }
}
