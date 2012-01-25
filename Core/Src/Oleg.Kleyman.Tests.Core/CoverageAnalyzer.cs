using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using Oleg.Kleyman.Core.Linq;

namespace Oleg.Kleyman.Tests.Core
{
    public class CoverageAnalyzer<T>
    {
        private readonly Type _targetType;
        private readonly BindingFlags _bindingFlags;

        public CoverageAnalyzer()
        {
            _targetType = typeof(T);
            _bindingFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static |
                            BindingFlags.DeclaredOnly;
        }

        public bool ValidateMethods(IDictionary<string, int> names)
        {
            ValidateObjectIsNotNull(names, true);
            var methods = _targetType.GetMethods(_bindingFlags);
            var filteredMethods = FilterProperties(methods);
            var allKnown = ValidateMembers(names, filteredMethods);

            return allKnown;
        }

        private static IEnumerable<MethodInfo> FilterProperties(IEnumerable<MethodInfo> methods)
        {
            const string propertySetPrefix = "set_";
            const string propertyGetPrefix = "get_";
            var filteredResult = from method in methods
                                 where !method.IsSpecialName 
                                    || !method.Name.StartsWith(propertySetPrefix) 
                                    && !method.Name.StartsWith(propertyGetPrefix)
                                 select method;
            return filteredResult;
        }


        private static bool ValidateObjectIsNotNull(object names, bool throwException)
        {
            //TODO: Figure out a better way to handle this issue
            if (names == null)
            {
                if (throwException)
                {
                    const string argumentName = "names";
                    const string argumentNullMessage = "Argument cannot be null or nothing.";
                    throw new ArgumentNullException(argumentName, argumentNullMessage);
                }

                return false;
            }

            return true;
        }

        public bool ValidateProperties(IDictionary<string, int> names)
        {
            ValidateObjectIsNotNull(names, true);
            var properties = _targetType.GetProperties(_bindingFlags);

            var allKnown = ValidateMembers(names, properties);

            return allKnown;
        }

        public bool ValidateMembers(IDictionary<string, int> names)
        {
            ValidateObjectIsNotNull(names, true);
            var methods = _targetType.GetMethods(_bindingFlags);
            var properties = _targetType.GetProperties(_bindingFlags);
            var filteredMethods = FilterProperties(methods);
            var members = ((IEnumerable<MemberInfo>) filteredMethods).Union(properties);
            var allKnown = ValidateMembers(names, members);

            return allKnown;
        }

        private bool ValidateMembers(IDictionary<string, int> names, IEnumerable<MemberInfo> members)
        {
            var namesIncluded = (from name in names
                                 where name.Value == members.Count(member => member.Name == name.Key)
                                 select name).Count() == names.Count;
            if(!namesIncluded)
            {
                return false;
            }

            return members.All(member => names.ContainsKey(member.Name));
        }
    }
}