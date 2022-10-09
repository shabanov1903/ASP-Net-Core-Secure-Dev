using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordStorage
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string appName = Properties.Settings.Default.AppName;
            Console.WriteLine($"Вас приветствует приложение {appName}");

            string userName = Properties.Settings.Default.UserName;
            if (string.IsNullOrEmpty(userName))
            {
                Console.WriteLine($"Поздравляем! Вы первый пользователь!");
            }
            else
            {
                Console.WriteLine($"Предыдущий пользователь назвал себя: {userName}");
            }
            Console.WriteLine($"Пожалуйста, представьтесь:");
            string newUserName = Console.ReadLine();
            while (string.IsNullOrEmpty(newUserName))
            {
                Console.WriteLine($"Вы ввели пустое имя. Введите хотя бы один символ:");
                newUserName = Console.ReadLine();
            }
            
            Properties.Settings.Default.UserName = newUserName;
            Properties.Settings.Default.Save();
            Console.WriteLine($"Следующий пользователь увидит ваше имя, {newUserName}. Спасибо!");
            Console.ReadLine();
        }
    }
}
