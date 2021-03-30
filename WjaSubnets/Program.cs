using System;
using WjaSubnets.Model;

namespace WjaSubnets
{
    class Program
    {
        static void Main(string[] args)
        {
            ShowMenu();
            Main(args);
        }

        /// <summary>
        /// Отображает меню программы.
        /// </summary>
        private static void ShowMenu()
        {
            Console.WriteLine("=================================");
            Console.WriteLine("Выберите создаваемый файл:");
            Console.WriteLine("1. Список подсетей.");
            Console.WriteLine("2. Список групп.");
            Console.Write("Введите цифру выбранного пункта: ");
            if (int.TryParse(Console.ReadLine(), out int select))
            {
                var fileCreator = new FileCreator(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
                switch (select)
                {
                    case 1:
                        fileCreator.CreateSubnetsFile();
                        break;
                    case 2:
                        fileCreator.CreateGroupsFile();
                        break;
                    default:
                        Console.WriteLine("Введите номер из списка.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Необходимо ввести номер из списка.");
            }
        }
    }
}
