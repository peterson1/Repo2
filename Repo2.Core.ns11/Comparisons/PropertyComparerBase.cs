using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Repo2.Core.ns11.Exceptions;
using Repo2.Core.ns11.Extensions.StringExtensions;
using Repo2.Core.ns11.ReflectionTools;

namespace Repo2.Core.ns11.Comparisons
{
    public abstract class PropertyComparerBase<T>
    {
        public PropertyComparerBase(T obj1, T obj2)
        {
            _1 = obj1;
            _2 = obj2;
            _methods = new Dictionary<Type, Func<object, object, bool>>
            {
                { typeof(string   ), (b1, b2) => CompareString  (b1, b2) },
                { typeof(int      ), (b1, b2) => CompareInt     (b1, b2) },
                { typeof(DateTime ), (b1, b2) => CompareDate    (b1, b2) },
                { typeof(DateTime?), (b1, b2) => CompareDate_   (b1, b2) },
                { typeof(decimal  ), (b1, b2) => CompareDecimal (b1, b2) },
                { typeof(uint     ), (b1, b2) => CompareUInt    (b1, b2) },
                { typeof(double   ), (b1, b2) => CompareDouble  (b1, b2) },
                { typeof(bool     ), (b1, b2) => CompareBool    (b1, b2) },
            };
        }

        protected T _1;
        protected T _2;
        private List<string> _diff = new List<string>();
        protected string _diffText;
        protected Dictionary<Type, Func<object, object, bool>> _methods;

        protected abstract List<string> GetPropertiesToCompare();

        public bool SameValues => _diff.Count == 0;
        public string DiffText => string.Join(Environment.NewLine, _diff);

        public bool Only1HasValue  => (_1 != null) && (_2 == null);
        public bool Only2HasValue  => (_1 == null) && (_2 != null);
        public bool BothHaveValues => (_1 != null) && (_2 != null);
        public bool BothAreNull    => (_1 == null) && (_2 == null);


        public bool CompareValues(out string diffText)
        {
            _diff.Clear();
            if (BothAreNull) goto Return;
            if (_1 == null) AddDiffText("self", "NULL", "has_value");
            if (_2 == null) AddDiffText("self", "has_value", "NULL");
            if (!SameValues) goto Return;
            var comparableProps = GetPropertiesToCompare();

            foreach (var propName in comparableProps)
            {
                var prop = GetRuntimePropertyByName(propName);
                var typ = GetRuntimePropertyType(prop);
                var val1 = prop.GetValue(_1);
                var val2 = prop.GetValue(_2);
                if (val1 == null && val2 == null) continue;
                if (BothBlank(typ, val1, val2)) continue;

                if (val1 == null)
                {
                    AddDiffText(propName, "NULL", val2);
                    continue;
                }
                if (val2 == null)
                {
                    AddDiffText(propName, val1, "NULL");
                    continue;
                }

                var b1 = val1;
                var b2 = val2;

                if (typ.Namespace == "System")
                {
                    try   { b1 = Convert.ChangeType(val1, typ); }
                    catch { }
                    try   { b2 = Convert.ChangeType(val2, typ); }
                    catch { }
                }

                if (!_methods[typ](b1, b2)) AddDiffText(propName, b1, b2);
            }

            CompareNonListedProperties();

            Return:
            diffText = DiffText;
            return SameValues;
        }


        private static PropertyInfo GetRuntimePropertyByName(string propName)
        {
            var prop = typeof(T).GetRuntimeProperty(propName);

            if (prop == null)
                prop = typeof(T).GetRuntimeProperties()
                    .SingleOrDefault(x => x.Name == propName);

            if (prop == null)
            {
                var props = typeof(T).GetRuntimeProperties();
                prop = props.SingleOrDefault(x => x.Name == propName);
            }
            if (prop == null) throw Fault.NoMember(propName);
            return prop;
        }


        private Type GetRuntimePropertyType(PropertyInfo prop)
        {
            var typ = prop.PropertyType.IsNullableType()
                    ? Nullable.GetUnderlyingType(prop.PropertyType)
                    : prop.PropertyType;

            if (typ.GetTypeInfo().IsEnum) typ = typeof(int);

            if (!_methods.ContainsKey(typ))
                throw Fault.Unsupported(typ);

            return typ;
        }


        private bool BothBlank(Type typ, object val1, object val2)
        {
            if (typ != typeof(string)) return false;
            var s1 = val1?.ToString() ?? string.Empty;
            var s2 = val2?.ToString() ?? string.Empty;
            return s1.IsBlank()
                && s2.IsBlank();
        }


        protected virtual void CompareNonListedProperties()
        {
        }


        protected void AddDiffText(string propName, object value1, object value2)
            => _diff.Add($"{propName} : [{value1}] -vs- [{value2}]");



        protected bool Compare(string propName, int value1, int value2)
        {
            var same = CompareInt(value1, value2);
            if (!same) AddDiffText(propName, value1, value2);
            return same;
        }

        protected bool Compare(string propName, string value1, string value2)
        {
            var same = CompareString(value1, value2);
            if (!same) AddDiffText(propName, value1, value2);
            return same;
        }




        /*
         *
         * 
         *   Overridable Compare methods 
         *   
         * 
         */


        protected virtual bool CompareString(object value1, object value2)
            => value1?.ToString()?.Trim()?.ToLower()
            == value2?.ToString()?.Trim()?.ToLower();


        protected virtual bool CompareInt(object value1, object value2)
            => (int)value1 == (int)value2;


        protected virtual bool CompareDate(object value1, object value2)
            => (DateTime)value1 == (DateTime)value2;


        protected virtual bool CompareDate_(object value1, object value2)
            => (DateTime?)value1 == (DateTime?)value2;


        protected virtual bool CompareDecimal(object value1, object value2)
            => (decimal)value1 == (decimal)value2;


        protected virtual bool CompareUInt(object value1, object value2)
            => (uint)value1 == (uint)value2;


        protected virtual bool CompareDouble(object value1, object value2)
            => (double)value1 == (double)value2;


        protected virtual bool CompareBool(object value1, object value2)
            => (bool)value1 == (bool)value2;
    }
}
