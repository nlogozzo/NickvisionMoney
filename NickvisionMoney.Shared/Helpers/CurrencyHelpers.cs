using System.Globalization;

namespace NickvisionMoney.Shared.Helpers;

/// <summary>
/// Helper methods for working with currency
/// </summary>
public static class CurrencyHelpers
{
    public static string ToAmountString(this decimal amount, CultureInfo culture, bool showCurrencySymbol = true)
    {
        var result = amount.ToString("C", culture);
        if (culture.NumberFormat.CurrencyDecimalDigits == 99)
        {
            result = result.TrimEnd('0');
            if (result.EndsWith(culture.NumberFormat.CurrencyDecimalSeparator))
            {
                result = result.Remove(result.LastIndexOf(culture.NumberFormat.CurrencyDecimalSeparator));
            }
        }
        if (!showCurrencySymbol)
        {
            result = result.Remove(result.IndexOf(culture.NumberFormat.CurrencySymbol), culture.NumberFormat.CurrencySymbol.Length);
        }
        return result;
    }
}