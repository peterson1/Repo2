using Repo2.Core.ns11.Exceptions;
using Repo2.Core.ns11.Extensions.DecimalExtensions;
using System;
using System.Collections.Generic;

namespace Repo2.Core.ns11.DataStructures
{
    public class ActualVsExpected<T>
    {
        public ActualVsExpected(T actual, T expected)
        {
            Actual   = actual;
            Expected = expected;
            IsSame   = Compare(actual, expected);
        }


        public T      Actual     { get; }
        public T      Expected   { get; }
        public bool   IsSame     { get; }


        private bool Compare(T actual, T expected)
        {
            var methods = new Dictionary<Type, Func<object, object, bool>>
            {
                { typeof(string   ), (b1, b2) => CompareString  (b1, b2) },
                { typeof(int      ), (b1, b2) => CompareInt     (b1, b2) },
                { typeof(int?     ), (b1, b2) => CompareInt_    (b1, b2) },
                { typeof(DateTime ), (b1, b2) => CompareDate    (b1, b2) },
                { typeof(DateTime?), (b1, b2) => CompareDate_   (b1, b2) },
                { typeof(decimal  ), (b1, b2) => CompareDecimal (b1, b2) },
                { typeof(uint     ), (b1, b2) => CompareUInt    (b1, b2) },
                { typeof(ulong    ), (b1, b2) => CompareULong   (b1, b2) },
                { typeof(double   ), (b1, b2) => CompareDouble  (b1, b2) },
                { typeof(bool     ), (b1, b2) => CompareBool    (b1, b2) },
            };
            if (!methods.TryGetValue(typeof(T), out Func<object, object, bool> func))
                throw Fault.Unsupported(typeof(T));

            return func(actual, expected);
        }




        protected virtual bool CompareString(object value1, object value2)
            => value1?.ToString()?.Trim()?.ToLower()
            == value2?.ToString()?.Trim()?.ToLower();


        protected virtual bool CompareInt(object value1, object value2)
            => (int)value1 == (int)value2;


        protected virtual bool CompareInt_(object value1, object value2)
            => (int?)value1 == (int?)value2;


        protected virtual bool CompareDate(object value1, object value2)
            => (DateTime)value1 == (DateTime)value2;


        protected virtual bool CompareDate_(object value1, object value2)
            => (DateTime?)value1 == (DateTime?)value2;


        protected virtual bool CompareDecimal(object value1, object value2)
            => ((decimal)value1).AlmostEqualTo((decimal)value2);


        protected virtual bool CompareUInt(object value1, object value2)
            => (uint)value1 == (uint)value2;


        protected virtual bool CompareULong(object value1, object value2)
            => (ulong)value1 == (ulong)value2;


        protected virtual bool CompareDouble(object value1, object value2)
            => ((double)value1).AlmostEqualTo((double)value2);


        protected virtual bool CompareBool(object value1, object value2)
            => (bool)value1 == (bool)value2;
    }
}
