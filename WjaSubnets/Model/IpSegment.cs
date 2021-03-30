namespace WjaSubnets.Model
{
    /// <summary>
    /// Сегмент сети.
    /// </summary>
    class IpSegment
    {
        /// <summary>
        /// Адрес подсети.
        /// </summary>
        private readonly uint ip;
        /// <summary>
        /// Маска подсети.
        /// </summary>
        private readonly uint mask;

        /// <summary>
        /// Количество хостов сети.
        /// </summary>
        public uint NumberOfHosts
        {
            get { return ~mask + 1; }
        }

        /// <summary>
        /// Сетевой адрес.
        /// </summary>
        public uint NetworkAddress
        {
            get { return ip & mask; }
        }

        /// <summary>
        /// Широковещательный адрес.
        /// </summary>
        public uint BroadcastAddress
        {
            get { return NetworkAddress + ~mask; }
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="ip">Адрес подсети</param>
        /// <param name="mask">Маска подсети</param>
        public IpSegment(string ip, string mask)
        {
            this.ip = ip.ParseIp();
            this.mask = mask.ParseIp();
        }

        /// <summary>
        /// Возвращает адрес первого хоста в подсети.
        /// </summary>
        /// <returns>Первый хост в подсети</returns>
        public string GetFirstHost()
        {
            var host = NetworkAddress + 1;
            return host.ToIpString();
        }

        /// <summary>
        /// Возвращает адрес последнего хоста в подсети.
        /// </summary>
        /// <returns>Последний хост в подсети</returns>
        public string GetLastHost()
        {
            var host = BroadcastAddress - 1;
            return host.ToIpString();
        }
    }
}
