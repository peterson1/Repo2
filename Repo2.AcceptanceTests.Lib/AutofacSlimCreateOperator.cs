using System;
using Autofac;
using Autofac.Core;
using fitSharp.Machine.Engine;
using fitSharp.Machine.Model;
using fitSharp.Slim.Operators;
using Repo2.AcceptanceTests.Lib.ComponentRegistry;

namespace Repo2.AcceptanceTests.Lib
{
    //public class AutofacCreateOperator : CellOperator, CreateOperator<Cell>
    //public class AutofacCreateOperator : Operator<string, SlimProcessor>
    //public class AutofacCreateOperator :  CellOperator
    public class AutofacSlimCreateOperator<T, P> : Operator<T, P>, CreateOperator<T> where P : class, Processor<T>
    {
        private static IContainer _container;

        public AutofacSlimCreateOperator()
        {
            _container = ContainerFactory.Build();
        }

        public bool CanCreate(NameMatcher memberName, Tree<T> parameters)
            //=> _container.ComponentRegistry.IsRegistered(new TypedService(Type.GetType(memberName.MatchName)));
        {
            var nme = $"{GetType().Namespace}.{memberName.MatchName}";
            var typ = Type.GetType(nme);
            if (typ == null)
                throw new ArgumentNullException($"cant create SLIM {nme} : typ == null");

            var svc = new TypedService(typ);

            if (!_container.ComponentRegistry.IsRegistered(svc))
                throw new ArgumentNullException($"cant create SLIM {nme} : not registered");

            return true;
        }


        public TypedValue Create(NameMatcher memberName, Tree<T> parameters)
        {
            //throw new NotImplementedException("+ + + + + + + +   tried to create SLIM  + + + + + + + + +");
            return new TypedValue(_container.Resolve(Type.GetType($"{GetType().Namespace}.{memberName.MatchName}")));
        }
    }


    public class AutofacSlimCreateOperator : AutofacSlimCreateOperator<string, SlimProcessor>
    {

    }
}
