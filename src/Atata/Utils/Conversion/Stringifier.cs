﻿namespace Atata;

/// <summary>
/// Provides a set of static methods for object conversion to readable string.
/// The methods are useful for the formatting of objects for log messages.
/// </summary>
public static class Stringifier
{
    public const string NullString = "null";

    private static readonly Lazy<Func<WebElement, string>> s_elementIdRetrieveFunction = new(() =>
    {
        var idProperty = typeof(WebElement).GetPropertyWithThrowOnError(
            "Id",
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        var parameterExpression = Expression.Parameter(typeof(WebElement), "item");

        var propertyExpression = Expression.Property(parameterExpression, idProperty);

        return Expression.Lambda<Func<WebElement, string>>(propertyExpression, parameterExpression)
            .Compile();
    });

    public static string ToString(IEnumerable collection) =>
        ToString(collection?.Cast<object>());

    public static string ToString<T>(IEnumerable<T> collection)
    {
        if (collection == null)
            return NullString;

        string[] itemStringValues = collection.Select(x => ToString(x)).ToArray();

        return itemStringValues.Any(x => x.Contains('\n'))
            ? $"[{Environment.NewLine}{string.Join($",{Environment.NewLine}", itemStringValues)}]"
            : $"[{string.Join(", ", itemStringValues)}]";
    }

    public static string ToString(Expression expression) =>
        $"({ObjectExpressionStringBuilder.ExpressionToString(expression)})";

    public static string ToString(object value)
    {
        if (Equals(value, null))
            return NullString;
        else if (value is string)
            return $"\"{value}\"";
        else if (value is bool)
            return value.ToString().ToLowerInvariant();
        else if (value is ValueType)
            return value.ToString();
        else if (value is IEnumerable enumerableValue)
            return ToString(enumerableValue);
        else if (value is Expression expressionValue)
            return ToString(expressionValue);
        else if (value is WebElement asWebElement)
            return $"Element {{ Id={s_elementIdRetrieveFunction.Value.Invoke(asWebElement)} }}";
        else
            return $"{{ {value} }}";
    }

    public static string ToStringInFormOfOneOrMany<T>(IEnumerable<T> collection)
    {
        if (collection == null)
            return NullString;

        int count = collection.Count();

        if (count == 1)
            return ToString(collection.First());

        return ToString(collection);
    }

    public static string ToStringInSimpleStructuredForm(object value, Type excludeBaseType = null)
    {
        if (Equals(value, null))
            return NullString;

        Type valueType = value.GetType();

        IEnumerable<PropertyInfo> properties = valueType.GetProperties(
            BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.Public);

        if (excludeBaseType != null)
            properties = properties.Where(
                x => excludeBaseType.IsAssignableFrom(x.DeclaringType) && x.DeclaringType != excludeBaseType);

        string[] propertyStrings = (from prop in properties
                                    let val = prop.GetValue(value)
                                    where TakeValueForSimpleStructuredForm(val)
                                    select $"{prop.Name}={ToString(val)}").ToArray();

        string simplifiedTypeName = ResolveSimplifiedTypeName(valueType);
        StringBuilder builder = new StringBuilder(simplifiedTypeName);

        if (propertyStrings.Length > 0)
            builder.Append($" {{ {string.Join(", ", propertyStrings)} }}");

        return builder.ToString();
    }

    private static string ResolveSimplifiedTypeName(Type type)
    {
        string name = type.Name;

        if (type.IsGenericType)
        {
            Type[] genericArgumentTypes = type.GetGenericArguments();
            string genericArgumentsString = string.Join(", ", genericArgumentTypes.Select(ResolveSimplifiedTypeName));

            return $"{name.Substring(0, name.IndexOf('`'))}<{genericArgumentsString}>";
        }

        return name;
    }

    private static bool TakeValueForSimpleStructuredForm(object value)
    {
        if (Equals(value, null))
            return false;
        else if (value is Array valueAsArray)
            return valueAsArray.Length > 0;
        else if (value is IEnumerable<object> valueAsGenericEnumerable)
            return valueAsGenericEnumerable.Any();
        else if (value is IEnumerable valueAsEnumerable)
            return valueAsEnumerable.Cast<object>().Any();
        else
            return true;
    }
}
