using FluentAssertions;
using Repo2.Core.ns11.Randomizers;
using Repo2.Core.ns11.ReflectionTools;
using Xunit;

namespace Repo2.UnitTests.Lib.Core.ns11.Tests.ReflectionTools
{
    [Trait("Core", "Unit")]
    public class PropertyValueCopierFacts
    {
        private static FakeFactory _fke = new FakeFactory();


        [Fact(DisplayName = "CopyByName : same types")]
        public void Case1()
        {
            var o1 = new SampleClass1
            {
                Text1 = _fke.Text,
                Number1 = _fke.Int(1, 1000),
            };
            var o2 = new SampleClass2();

            o2.CopyByNameFrom(o1);
            o2.Text1.Should().Be(o1.Text1);
            o2.Number1.Should().Be(o1.Number1);
        }


        [Fact(DisplayName = "CopyByName : nullable int")]
        public void Case2()
        {
            var o1 = new SampleClass1
            {
                Number4 = _fke.Int(1, 1000),
                Number5 = _fke.Int(1, 1000),
            };
            var o2 = new SampleClass2();

            o2.CopyByNameFrom(o1);
            o2.Number4.Should().Be(o1.Number4);
            o2.Number5.Should().Be(o1.Number5);
        }


        [Fact(DisplayName = "CopyByName : int to dec")]
        public void Case3()
        {
            var o1 = new SampleClass1
            {
                Number6 = _fke.Int(1, 1000),
            };
            var o2 = new SampleClass2();

            o2.CopyByNameFrom(o1);
            o2.Number6.Should().Be(0);
        }


        [Fact(DisplayName = "Inherited properties")]
        public void Case4()
        {
            var src = new SampleClass1A
            {
                Number1 = _fke.Int(1, 1000)
            };
            var dest = new SampleClass2A();

            dest.CopyByNameFrom(src);
            dest.Number1.Should().Be(src.Number1);
        }



        [Fact(DisplayName = "from Interface")]
        public void Case5()
        {
            ISampleInterface1 src = new SampleClass1
            {
                Number1 = _fke.Int(1, 1000)
            };
            var dest = new SampleClass2A();

            dest.CopyByNameFrom(src);
            dest.Number1.Should().Be(src.Number1);
        }



        [Fact(DisplayName = "from nested Interface")]
        public void Case6()
        {
            ISampleInterface1 src = new SampleClass1
            {
                ParentInterfaceProp = _fke.Int(1, 1000)
            };
            var dest = new SampleClass2A();

            dest.CopyByNameFrom(src);
            dest.ParentInterfaceProp.Should().Be(src.ParentInterfaceProp);
        }


        class SampleParentClass1 : ISampleParentInterface1
        {
            public int ParentInterfaceProp { get; set; }
        }


        class SampleClass1 : SampleParentClass1, ISampleInterface1
        {
            public string Text1 { get; set; }
            public int Number1 { get; set; }
            public int Number3 { get; set; }
            public int? Number4 { get; set; }
            public int Number5 { get; set; }
            public int Number6 { get; set; }
        }


        class SampleClass2 : SampleParentClass1
        {
            public string Text1 { get; set; }
            public string Text2 { get; set; }
            public int Number1 { get; set; }
            public int Number2 { get; set; }
            public int Number4 { get; set; }
            public int? Number5 { get; set; }
            public decimal Number6 { get; set; }
        }


        interface ISampleInterface1 : ISampleParentInterface1
        {
            int Number1 { get; }
        }


        interface ISampleParentInterface1
        {
            int ParentInterfaceProp { get; }
        }


        class SampleClass1A : SampleClass1
        {
            public int MyProperty { get; set; }
        }


        class SampleClass2A : SampleClass2
        {
            public int MyProperty { get; set; }
        }
    }
}
