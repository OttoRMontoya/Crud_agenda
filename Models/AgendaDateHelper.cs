using System;
using System.Globalization;

namespace Crud_agenda.Models
{
    /// <summary>
    /// Fechas de agenda como hora de pared (sin zona horaria).
    /// Evita desfases al publicar en servidores con UTC distinto al cliente.
    /// </summary>
    public static class AgendaDateHelper
    {
        private static readonly string[] WallClockFormats =
        {
            "yyyy-MM-ddTHH:mm:ss",
            "yyyy-MM-ddTHH:mm",
            "yyyy-MM-dd HH:mm:ss",
            "yyyy-MM-dd HH:mm"
        };

        public static string FormatWallClock(DateTime value)
        {
            return value.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture);
        }

        public static DateTime ParseWallClock(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new FormatException("Fecha vacía.");

            value = NormalizeWallClockString(value.Trim());
            return DateTime.ParseExact(value, WallClockFormats, CultureInfo.InvariantCulture, DateTimeStyles.None);
        }

        public static DateTime EnsureWallClock(DateTime value)
        {
            return DateTime.SpecifyKind(value, DateTimeKind.Unspecified);
        }

        public static bool TryParseWallClock(string value, out DateTime result)
        {
            result = default(DateTime);
            if (string.IsNullOrWhiteSpace(value))
                return false;

            try
            {
                value = NormalizeWallClockString(value.Trim());
                return DateTime.TryParseExact(value, WallClockFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out result);
            }
            catch
            {
                return false;
            }
        }

        private static string NormalizeWallClockString(string value)
        {
            if (value.EndsWith("Z", StringComparison.OrdinalIgnoreCase))
                value = value.Substring(0, value.Length - 1);

            var dot = value.IndexOf('.');
            if (dot > 0)
                value = value.Substring(0, dot);

            var tIndex = value.IndexOf('T');
            if (tIndex < 0)
                tIndex = value.IndexOf(' ');

            if (tIndex > 0)
            {
                var plus = value.IndexOf('+', tIndex);
                var minus = value.LastIndexOf('-');
                if (minus > tIndex && minus > 10)
                {
                    var afterMinus = value.Substring(minus + 1);
                    if (afterMinus.Length >= 4 && afterMinus.Contains(":"))
                        value = value.Substring(0, minus);
                }
                if (plus > tIndex)
                    value = value.Substring(0, plus);
            }

            return value;
        }
    }
}
