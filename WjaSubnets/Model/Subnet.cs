namespace WjaSubnets.Model
{
    /// <summary>
    /// Подсеть.
    /// </summary>
    class Subnet
    {
        /// <summary>
        /// ОСП.
        /// </summary>
        public string Osp { get; set; }

        /// <summary>
        /// Адрес.
        /// </summary>
        public string Address { get; }

        /// <summary>
        /// Маска подсети.
        /// </summary>
        public string Mask { get; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="address">Адрес</param>
        /// <param name="mask">Маска</param>
        public Subnet(string osp, string address, string mask)
        {
            Osp = osp;
            Address = address;
            Mask = mask;
        }
    }
}
