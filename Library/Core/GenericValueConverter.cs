using System;
using System.ComponentModel;

namespace CollectionConfig.net.Common.Core;

/// <summary>
/// Converts values from string to specific type
/// </summary>
public static class GenericValueConverter
{
    /// <summary>
    /// Converts values from string to specific type
    /// </summary>
    /// <param name="input">String to convert</param>
    /// <param name="result">out T value as converted type if successful</param>
    /// <typeparam name="T">Type to attempt to convert to</typeparam>
    /// <returns>True if conversion was successful</returns>
    public static bool TryParse<T>(this string input, out T result) where T : new()
    {
        var isConversionSuccessful = false;

        result = default(T) ?? new T();
        
        if (result is null) throw new ArgumentNullException(nameof(result));
        
        var converter = TypeDescriptor.GetConverter(typeof(T));

        try
        {
            result = (T?)converter.ConvertFromString(input ?? throw new ArgumentNullException(nameof(input)))
                     ?? throw new ArgumentNullException();

            isConversionSuccessful = true;
        }
        catch (ArgumentException) { }

        return isConversionSuccessful;
    }
}