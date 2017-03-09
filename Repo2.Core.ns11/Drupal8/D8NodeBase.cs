using System;

namespace Repo2.Core.ns11.Drupal8
{
    public abstract class D8NodeBase : ID8Node
    {
        public int nid { get; set; }
        public int uid { get; set; }
        //public int vid { get; set; }
        public DateTime? created { get; set; }

        public abstract string D8TypeName { get; }
    }
}
