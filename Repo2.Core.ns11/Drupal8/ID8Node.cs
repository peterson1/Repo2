using System;

namespace Repo2.Core.ns11.Drupal8
{
    public interface ID8Node
    {
        int        nid        { get; }
        int        uid        { get; }
        DateTime?  created    { get; }

        string     D8TypeName { get; }
    }
}
