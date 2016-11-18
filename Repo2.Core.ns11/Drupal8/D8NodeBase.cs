using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repo2.Core.ns11.Drupal8
{
    public abstract class D8NodeBase
    {
        public int nid { get; set; }
        public int uid { get; set; }
        public int vid { get; set; }

        public abstract string D8TypeName { get; }
    }
}
