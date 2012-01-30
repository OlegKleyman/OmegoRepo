using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using NUnit.Framework;
using Oleg.Kleyman.Core.Linq;

namespace Oleg.Kleyman.Tests.Core
{
    public class CoverageAnalyzer
    {
        private readonly Type _targetType;
        private const BindingFlags BINDING_FLAGS = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
        private const string NAMES_ARGUMENT_NAME = "names";

        public CoverageAnalyzer(Type targetType)
        {
            _targetType = targetType;
        }

        public bool ValidateMethods(IDictionary<string, int> names)
        {
            ThrowArgumentNullExceptionIfObjectIsNull(names, NAMES_ARGUMENT_NAME);
            var methods = _targetType.GetMethods(BINDING_FLAGS);
            var filteredMethods = FilterProperties(methods);
            var allKnown = ValidateMembers(names, filteredMethods);

            return allKnown;
        }

        private static IEnumerable<MethodBase> FilterProperties(IEnumerable<MethodBase> methods)
        {
            var filteredResult = from method in methods
                                 where !IsMemberPropertyAAcessor(method)
                                 select method;
            return filteredResult;
        }

        private static bool IsMemberPropertyAAcessor(MethodBase method)
        {
            const string propertySetPrefix = "set_";
            const string propertyGetPrefix = "get_";

            return method.IsSpecialName
                && (method.Name.StartsWith(propertySetPrefix)
                || method.Name.StartsWith(propertyGetPrefix));
        }

        private static void ThrowArgumentNullExceptionIfObjectIsNull(object target, string argumentName)
        {
            //TODO: Figure out a better way to handle this issue
            if (target == null)
            {
                const string argumentNullMessage = "Argument cannot be null or nothing.";
                throw new ArgumentNullException(argumentName, argumentNullMessage);

            }
        }

        public bool ValidateProperties(IDictionary<string, int> names)
        {
            ThrowArgumentNullExceptionIfObjectIsNull(names, NAMES_ARGUMENT_NAME);
            var properties = _targetType.GetProperties(BINDING_FLAGS);

            var allKnown = ValidateMembers(names, properties);

            return allKnown;
        }

        public bool ValidateMembers(IDictionary<string, int> names)
        {
            ThrowArgumentNullExceptionIfObjectIsNull(names, NAMES_ARGUMENT_NAME);
            var members = _targetType.GetMembers(BINDING_FLAGS);

            var filteredMembers = (from mem in members
                                   where mem is PropertyInfo ||
                                        (mem is MethodBase
                                      && !IsMemberPropertyAAcessor((MethodBase)mem))
                                   select mem);

            var allKnown = ValidateMembers(names, filteredMembers);

            return allKnown;
        }

        private bool ValidateMembers(IDictionary<string, int> names, IEnumerable<MemberInfo> members)
        {
            var namesIncluded = (from name in names
                                 where name.Value == members.Count(member => member.Name == name.Key)
                                 select name).Count() == names.Count;
            if (!namesIncluded)
            {
                return false;
            }

            return members.All(member => names.ContainsKey(member.Name));
        }

        public static bool ValidateMembersNoCoverage<T>(IDictionary<string, int> names, bool assertInconclusive)
        {
            ThrowArgumentNullExceptionIfObjectIsNull(names, NAMES_ARGUMENT_NAME);
            var coverageAnalyzer = new CoverageAnalyzer(typeof(T));

            var result = coverageAnalyzer.ValidateMembers(names);
            if (!result)
            {
                if (assertInconclusive)
                {
                    const string membersNotCoveredMessage = "All members not covered";
                    Assert.Inconclusive(membersNotCoveredMessage);
                }
                else
                {
                    return false;
                }
            }

            return true;
        }
    }
}