using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using fitSharp.Fit.Operators;
using fitSharp.Machine.Engine;
using fitSharp.Machine.Model;
using Repo2.AcceptanceTests.Lib.ComponentRegistry;

namespace Repo2.AcceptanceTests.Lib
{
    public class AutofacFitCreateOperator : CellOperator, CreateOperator<Cell>
    {
        private static IContainer _container;

        public AutofacFitCreateOperator()
        {
            _container = ContainerFactory.Build();
        }

        public bool CanCreate(NameMatcher memberName, Tree<Cell> parameters)
        {
            var typ = Type.GetType(memberName.MatchName);
            if (typ == null)
                throw new ArgumentNullException($"cant create FIT {memberName.MatchName}");

            var svc = new TypedService(typ);

            if (!_container.ComponentRegistry.IsRegistered(svc))
                throw new ArgumentNullException($"cant create FIT {memberName.MatchName}");

            return true;
        }

        public TypedValue Create(NameMatcher memberName, Tree<Cell> parameters)
        {
            throw new NotImplementedException("+ + + + + + + +   tried to create FIT   + + + + + + + + +");
            //return new TypedValue(_container.Resolve(Type.GetType(memberName.MatchName)));
        }
    }
}
