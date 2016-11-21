using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repo2.Core.ns11.ChangeNotification
{
    public interface IStatusChanger
    {
        event EventHandler<StatusText> StatusChanged;
    }
}
