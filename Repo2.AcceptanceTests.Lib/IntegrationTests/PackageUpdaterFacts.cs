using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Repo2.AcceptanceTests.Lib.IntegrationTests
{
    [Trait("Local:453", "Write")]
    public class PackageUpdaterFacts
    {
        [Fact(DisplayName = "Update Running Exe")]
        public void UpdateRunningExe()
        {
            // run test exe
            // isOutdated should be false
            // upload newer exe
            // isOutdated should be true
            // update running exe
            // isOutdated should be false

            // cleanup: kill running exe
        }
    }
}
