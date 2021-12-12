﻿namespace Atata
{
    public class UseParentScopeStrategy : XPathComponentScopeFindStrategy
    {
        protected override string Build(ComponentScopeXPathBuilder builder, ComponentScopeFindOptions options)
        {
            return builder.Self.Any;
        }
    }
}
