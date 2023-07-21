/*
    Author:     Alex Erwin
    Purpose:    Get/Set If we are doing enumeration
    Credit:     hamidmosalla.com
*/
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Tests.SQLTests
{
    public class SQLDataAdapterAttributeDiscoverer : DataDiscoverer
    {
        public override bool SupportsDiscoveryEnumeration(IAttributeInfo dataAttribute, IMethodInfo testMethod)
            => dataAttribute.GetNamedArgument<bool>("EnableDiscoveryEnumeration");
    }
}
