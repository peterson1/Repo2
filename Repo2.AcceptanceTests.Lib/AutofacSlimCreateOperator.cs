using System;
using Autofac;
using Autofac.Core;
using fitSharp.Machine.Engine;
using fitSharp.Machine.Model;
using fitSharp.Slim.Operators;
using Repo2.AcceptanceTests.Lib.ComponentRegistry;
using Repo2.Core.ns11.ReflectionTools;
using Repo2.SDK.WPF45.Authentication;
using Repo2.Uploader.Lib45.Configuration;

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
            var cfg = UploaderConfigFile.Parse(UploaderCfg.Localhost);

            Certificator.AllowFrom(cfg.CertificateThumb);

            _container = ContainerFactory.Build(cfg);
        }

        public bool CanCreate(NameMatcher memberName, Tree<T> parameters)
        {
            var typ = ToType(memberName);
            if (typ == null)
            {
                Console.WriteLine($"Can't resolve {memberName.MatchName}");
                Console.WriteLine(GetType().Assembly.PrependNamespace(memberName.MatchName));
                return false;
            }
            var svc = new TypedService(ToType(memberName));
            return _container.ComponentRegistry.IsRegistered(svc);
        }


        private Type ToType(NameMatcher memberName)
            => GetType().Assembly.FindTypeByName(memberName.MatchName, false);


        public TypedValue Create(NameMatcher memberName, Tree<T> parameters)
            => new TypedValue(_container.Resolve(ToType(memberName)));
    }


    public class AutofacSlimCreateOperator : AutofacSlimCreateOperator<string, SlimProcessor>
    {

    }
}
