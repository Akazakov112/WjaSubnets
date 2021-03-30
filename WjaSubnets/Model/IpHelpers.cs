using System.Globalization;

namespace WjaSubnets.Model
{
    /// <summary>
    /// Методы расширения для IP адресов.
    /// </summary>
    static class IpHelpers
    {
        /// <summary>
        /// Переводит uint IP адреса в строковое представление.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToIpString(this uint value)
        {
            var bitmask = 0xff000000;
            var parts = new string[4];
            for (var i = 0; i < 4; i++)
            {
                var masked = (value & bitmask) >> ((3 - i) * 8);
                bitmask >>= 8;
                parts[i] = masked.ToString(CultureInfo.InvariantCulture);
            }
            return string.Join(".", parts);
        }

        /// <summary>
        /// Пасрит строку с IP адресом в uint.
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public static uint ParseIp(this string ipAddress)
        {
            var splitted = ipAddress.Split('.');
            uint ip = 0;
            for (var i = 0; i < 4; i++)
            {
                ip = (ip << 8) + uint.Parse(splitted[i]);
            }
            return ip;
        }
    }
}
