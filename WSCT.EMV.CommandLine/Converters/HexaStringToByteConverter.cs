using System.ComponentModel;
using System.Globalization;
using WSCT.Helpers;

namespace WSCT.EMV.CommandLine.Converters;

/// <summary>
/// Converts a string containing a 2 digits hexa value to a <c>byte</c>.
/// </summary>
public class HexaStringToByteConverter : TypeConverter
{
    /// <inheritdoc/>
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

    /// <inheritdoc/>
    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        if (value is string str)
        {
            return str.FromHexa()[0];
        }

        return base.ConvertFrom(context, culture, value);
    }
}