using System.Collections.Generic;
using System.Reflection;
using System;
using System.Linq;
using System.Diagnostics.CodeAnalysis;

namespace Hubtel.ECommerce.API.Core.Domain
{
    public abstract class ValueObject : IEqualityComparer<ValueObject>
    {
        private List<PropertyInfo>? _properties;

        private List<FieldInfo>? _fields;

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            return GetProperties().All(p => PropertiesAreEqual(obj, p))
                   && GetFields().All(f => FieldsAreEqual(obj, f));
        }

        private bool PropertiesAreEqual(object obj, PropertyInfo p)
        {
            return Equals(p.GetValue(this, null), p.GetValue(obj, null));
        }

        private bool FieldsAreEqual(object obj, FieldInfo f)
        {
            return Equals(f.GetValue(this), f.GetValue(obj));
        }

        private IEnumerable<PropertyInfo> GetProperties()
        {
            if (_properties == null)
            {
                _properties = GetType()
                    .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    .Where(p => p.GetCustomAttribute(typeof(IgnoreMemberAttribute)) == null)
                    .ToList();
            }

            return _properties;
        }

        private IEnumerable<FieldInfo> GetFields()
        {
            if (_fields == null)
            {
                _fields = GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                    .Where(p => p.GetCustomAttribute(typeof(IgnoreMemberAttribute)) == null)
                    .ToList();
            }

            return _fields;
        }

        private int HashValue(int seed, object? value)
        {
            var currentHash = value?.GetHashCode() ?? 0;

            return seed * 23 + currentHash;
        }

        public bool Equals([AllowNull] ValueObject x, [AllowNull] ValueObject y)
        {
            return x.Equals(y);
        }

        public int GetHashCode([DisallowNull] ValueObject obj)
        {
            unchecked
            {
                int hash = 17;
                foreach (var prop in GetProperties())
                {
                    var value = prop.GetValue(obj, null);
                    hash = HashValue(hash, value);
                }

                foreach (var field in GetFields())
                {
                    var value = field.GetValue(this);
                    hash = HashValue(hash, value);
                }

                return hash;
            }
        }

        public override int GetHashCode()
        {
            var properties = GetProperties();
            var fields = GetFields();
            return HashCode.Combine(properties, fields);
        }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class IgnoreMemberAttribute : Attribute
    {
    }
}
