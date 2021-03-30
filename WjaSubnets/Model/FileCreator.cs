using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace WjaSubnets.Model
{
    /// <summary>
    /// Создает файлы для WJA.
    /// </summary>
    class FileCreator
    {
        /// <summary>
        /// Построитель списков подсетей.
        /// </summary>
        private readonly SqlSubnetsBuilder builder;

        /// <summary>
        /// Путь к папке сохранения.
        /// </summary>
        private readonly string saveDirectory;

        /// <summary>
        /// Инициализирует экземпляр создателя файлов для WJA.
        /// </summary>
        /// <param name="directory">Путь к папке сохранения файла</param>
        public FileCreator(string directory)
        {
            builder = new SqlSubnetsBuilder(App.Default.DbConnectionString);
            saveDirectory = directory;
        }

        /// <summary>
        /// Создает файл подсетей.
        /// </summary>
        public void CreateSubnetsFile()
        {
            // Путь сохранения.
            string filePath = Path.Combine(saveDirectory, "WjaSubnets.txt");
            // Получаем список ОСП для подсетей.
            List<Subnet> subnets = builder.GetSubnets(SqlSubnetsBuilder.Type.Subnets);
            // Запись в файл.
            using (var fileWriter = new StreamWriter(filePath, false))
            {
                foreach (var subnet in subnets)
                {
                    var ip = new IpSegment(subnet.Address, subnet.Mask);
                    string record = $"{ip.GetFirstHost()}-{ip.GetLastHost()}={subnet.Osp}";
                    fileWriter.WriteLine(record);
                }
            };
            Console.WriteLine($"Список подсетей успешно записан в файл WjaSubnets.txt по адресу {saveDirectory}.");
        }

        /// <summary>
        /// Создает файл групп.
        /// </summary>
        public void CreateGroupsFile()
        {
            // Путь сохранения.
            string filePath = Path.Combine(saveDirectory, "WjaGroups.xml");
            // Получаем список ОСП для групп.
            List<Subnet> subnets = builder.GetSubnets(SqlSubnetsBuilder.Type.Groups);
            // Список для создаваемых групп.
            List<XElement> groups = new List<XElement>();
            // Счетчик Id.
            int groupId = 1;
            // Неймспес для файла.
            XNamespace ns = "http://www.hp.com/schemas/imaging/con/devicemgmt/groupexport/2008/12/03";
            // Создаем группы.
            foreach (var subnet in subnets)
            {
                // Убираем неподдерживаемые символы.
                var symbols = new char[] { '(', ')', '[', ']' };
                foreach (var sym in symbols)
                {
                    subnet.Osp = subnet.Osp.Replace(sym.ToString(), string.Empty);
                }
                // Ищем уже созданную группу с названием ОСП.
                XElement existGroup = groups.FirstOrDefault(x => x.Element(ns + "Name").Value.ToLower() == subnet.Osp.ToLower());
                // Если такой группы нет, создаем.
                if (existGroup is null)
                {
                    var group = new XElement(ns + "Group");
                    var id = new XElement(ns + "Id", groupId++);
                    var parent = new XElement(ns + "Parent", 0);
                    var name = new XElement(ns + "Name", subnet.Osp);
                    var filter = new XElement(ns + "Filter", $"Contains([IPv4Address], [{subnet.Address.Remove(subnet.Address.Length - 1)}])");
                    var policies = new XElement(ns + "Policies");
                    group.Add(id, parent, name, filter, policies);
                    groups.Add(group);
                }
                // Иначе добавляем в фильтр группы адрес подсети.
                else
                {
                    string filter = existGroup.Element(ns + "Filter").Value;
                    existGroup.SetElementValue(ns + "Filter", filter + $" OR Contains([IPv4Address], [{subnet.Address.Remove(subnet.Address.Length - 1)}])");
                }
            }
            // Создаем документ.
            var xdoc = new XDocument();
            // Создаем корневой элемент.
            XElement root = new XElement(ns + "DeviceGroupExport");
            // Добавляем группы к корневому элементу.
            root.Add(groups);
            // Добавляем корневой элемент в документ.
            xdoc.Add(root);
            // Сохраняем документ.
            using (var fileWriter = new StreamWriter(filePath, false))
            {
                xdoc.Save(fileWriter);
            };
            Console.WriteLine($"Список групп записан в файл WjaGroups.xml по адресу {saveDirectory}.");
        }
    }
}
