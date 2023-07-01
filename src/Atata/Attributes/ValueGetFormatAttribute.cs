﻿namespace Atata;

/// <summary>
/// Specifies the data format to use for getting the value of the control.
/// Can override control's <see cref="FormatAttribute"/>, but only for getting the value.
/// Format value is similar to format of <see cref="string.Format(string, object[])"/> method.
/// For example: <c>"start {0} end"</c>, <c>"{0:C2}"</c>, <c>"{0:MM/dd/yyyy}"</c>.
/// In order to output a <c>{</c> use <c>{{</c>, and to output a <c>}</c> use <c>}}</c>.
/// </summary>
public class ValueGetFormatAttribute : MulticastAttribute
{
    public ValueGetFormatAttribute(string value) =>
        Value = value;

    /// <summary>
    /// Gets the format value.
    /// </summary>
    public string Value { get; }
}
