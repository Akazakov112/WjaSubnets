using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace WjaSubnets.Model
{
    /// <summary>
    /// SQL построитель списка подсетей.
    /// </summary>
    class SqlSubnetsBuilder
    {
        /// <summary>
        /// Тип создаваемого списка.
        /// </summary>
        public enum Type : int
        {
            Subnets = 1,
            Groups = 2
        }

        /// <summary>
        /// Подключение.
        /// </summary>
        private readonly SqlConnection connection;

        /// <summary>
        /// Список подсетей.
        /// </summary>
        private readonly List<Subnet> subnets;

        /// <summary>
        /// Конструктор со строкой подключения и типом создаваемого списка.
        /// </summary>
        /// <param name="connectionString">Строка подключения</param>
        /// <param name="type">Тип создаваемого списка</param>
        public SqlSubnetsBuilder(string connectionString)
        {
            connection = new SqlConnection(connectionString);
            subnets = new List<Subnet>();
        }

        /// <summary>
        /// Возвращает список подсетей синхронно.
        /// </summary>
        /// <returns>Список подсетей</returns>
        public List<Subnet> GetSubnets(Type type)
        {
            // Команда выборки данных.
            string command = $"SELECT OSP, Subnet, Mask " +
                             $"FROM {App.Default.Table} " +
                             $"WHERE NOT (OSP IS NULL OR Subnet IS NULL OR Mask IS NULL) " +
                             $"Order by OSP";
            // Выполнение команды.
            using (var com = new SqlCommand(command, connection))
            {
                connection.Open();
                SqlDataReader read = com.ExecuteReader();
                while (read.Read())
                {
                    string osp = read.GetString(0);
                    string sub = read.GetString(1);
                    string mask = read.GetString(2);
                    var subnet = new Subnet(osp, sub, mask);
                    if (type == Type.Subnets)
                    {
                        CheckAddSubnet(subnet);
                    }
                    subnets.Add(subnet);
                }
            }
            return subnets;
        }

        /// <summary>
        /// Проверяет совпадение имени подсети и добавляет подсеть в список.
        /// </summary>
        /// <param name="subnet"></param>
        private void CheckAddSubnet(Subnet subnet)
        {
            int nameNum = subnets.Count(x=>x.Osp.Trim().ToLower().Contains(subnet.Osp.Trim().ToLower()));
            if (nameNum > 0)
            {
                subnet.Osp = $"{subnet.Osp} {nameNum}";
            }
        }
    }
}
