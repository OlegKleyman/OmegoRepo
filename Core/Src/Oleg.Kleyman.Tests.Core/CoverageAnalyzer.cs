using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

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

        public bool ValidateMethods(IEnumerable<string> names)
        {
            ValidateNames(names, true);
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


        private static bool ValidateNames(object names, bool throwException)
        {
            if (names == null)
            {
                if (throwException)
                {
                    const string argumentName = "names";
                    const string argumentNullMessage = "Argument cannot be null or nothing.";
                    throw new ArgumentNullException(argumentNullMessage, argumentName);
                }

                return false;
            }

            return true;
        }

        public bool ValidateProperties(IEnumerable<string> names)
        {
            ValidateNames(names, true);
            var properties = _targetType.GetProperties(_bindingFlags);

            var allKnown = ValidateMembers(names, properties);

            return allKnown;
        }

        private bool ValidateMembers(IEnumerable<string> names, IEnumerable<MemberInfo> members)
        {
            return members.All(member => names.Contains(member.Name));
        }
    }
}