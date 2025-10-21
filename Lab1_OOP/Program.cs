using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Lab1_OOP
{
    public static class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            List<Smart> smartphones = new List<Smart>();
            int choice = -1;
            int maxCount = 0;

            do
            {
                Console.WriteLine("\n========== Меню =============");
                Console.WriteLine("1 - Додати об'єкт");
                Console.WriteLine("2 - Переглянути всі об'єкти");
                Console.WriteLine("3 - Знайти об'єкти за брендом");
                Console.WriteLine("4 - Продемонструвати поведінку");
                Console.WriteLine("5 - Видалити об'єкт за індексом");
                Console.WriteLine("6 - Порівняти камери двох смартфонів");
                Console.WriteLine("7 - Збільшити ОЗУ смартфону");
                Console.WriteLine("8 - Продемонструвати static-методи");
                Console.WriteLine("9 - Зберегти колекцію у файл");
                Console.WriteLine("10 - Зчитати колекцію з файлу");
                Console.WriteLine("11 - Очистити колекцію");
                Console.WriteLine("0 - Вийти з програми");
                Console.Write("Введіть вибір: ");

                if (!int.TryParse(Console.ReadLine(), out choice))
                {
                    Console.WriteLine("Помилка! Введіть число від 0 до 11.");
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        if (maxCount == 0)
                        {
                            Console.Write("Введіть максимальну кількість смартфонів: ");
                            while (!int.TryParse(Console.ReadLine(), out maxCount) || maxCount <= 0)
                            {
                                Console.WriteLine("Помилка! Введіть число більше за 0.");
                            }
                        }

                        if (smartphones.Count >= maxCount)
                        {
                            Console.WriteLine("Досягнуто максимальної кількості смартфонів!");
                            break;
                        }

                        Console.WriteLine("Введіть характеристики у форматі: brand;model;ozyGB;cameraMPx");
                        string input = Console.ReadLine();

                        try
                        {
                            AddSmartphone(smartphones, input);
                            Console.WriteLine("Смартфон додано!");
                        }
                        catch (ArgumentException ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;

                    case 2:
                        if (smartphones.Count == 0)
                            Console.WriteLine("Список порожній!");
                        else
                        {
                            Console.WriteLine($"\nВсього смартфонів (створено): {Smart.Count}");
                            Console.WriteLine("-------------------------------------------------------------------------------------------------");
                            Console.WriteLine($"{"#",3} {"Бренд",-12} {"Модель",-15} {"ОЗУ",5} {"Камера",8} {"Тип",-10} {"Батарея",8}");
                            Console.WriteLine("-------------------------------------------------------------------------------------------------");

                            for (int i = 0; i < smartphones.Count; i++)
                            {
                                Smart s = smartphones[i];
                                Console.WriteLine($"{i,3} {s.Brand,-12} {s.Model,-15} {s.OzyGB,5} {s.CameraMPx,8} {s.Type,-10} {s.BatteryLevel,8}%");
                            }
                        }
                        break;

                    case 3:
                        Console.Write("Введіть бренд для пошуку: ");
                        string searchBrand = Console.ReadLine();
                        var results = FindByBrand(smartphones, searchBrand);
                        if (results.Count == 0)
                            Console.WriteLine("Смартфонів з таким брендом не знайдено!");
                        else
                            results.ForEach(s => Console.WriteLine(s.GetInfo()));
                        break;

                    case 4:
                        if (smartphones.Count == 0)
                        {
                            Console.WriteLine("Немає смартфонів для демонстрації.");
                        }
                        else
                        {
                            Console.WriteLine("Оберіть індекс смартфону для демонстрації поведінки:");
                            for (int i = 0; i < smartphones.Count; i++)
                                Console.WriteLine($"{i} - {smartphones[i].Brand} {smartphones[i].Model}");

                            if (int.TryParse(Console.ReadLine(), out int demoIndex) &&
                                demoIndex >= 0 && demoIndex < smartphones.Count)
                            {
                                Console.WriteLine(smartphones[demoIndex].Call());
                                Console.WriteLine(smartphones[demoIndex].Call("123-456-789"));
                                Console.WriteLine(smartphones[demoIndex].Photo());
                                Console.WriteLine(smartphones[demoIndex].Internet());
                                Console.WriteLine(smartphones[demoIndex].Temperature());
                            }
                            else
                            {
                                Console.WriteLine("Невірний індекс!");
                            }
                        }
                        break;

                    case 5:
                        Console.Write("Введіть індекс смартфона для видалення: ");
                        if (int.TryParse(Console.ReadLine(), out int index))
                        {
                            if (RemoveSmartphone(smartphones, index))
                                Console.WriteLine("Смартфон видалено!");
                            else
                                Console.WriteLine("Невірний індекс!");
                        }
                        break;

                    case 6:
                        if (smartphones.Count < 2)
                        {
                            Console.WriteLine("Потрібно хоча б 2 смартфони для порівняння!");
                        }
                        else
                        {
                            Console.WriteLine("Оберіть індекс першого смартфона:");
                            for (int i = 0; i < smartphones.Count; i++)
                                Console.WriteLine($"{i} - {smartphones[i].Brand} {smartphones[i].Model}");

                            if (int.TryParse(Console.ReadLine(), out int first) && first >= 0 && first < smartphones.Count)
                            {
                                Console.WriteLine("Оберіть індекс другого смартфона:");
                                for (int i = 0; i < smartphones.Count; i++)
                                    if (i != first)
                                        Console.WriteLine($"{i} - {smartphones[i].Brand} {smartphones[i].Model}");

                                if (int.TryParse(Console.ReadLine(), out int second) &&
                                    second >= 0 && second < smartphones.Count && second != first)
                                {
                                    Console.WriteLine(smartphones[first].CompareCamera(smartphones[second]));
                                }
                                else
                                {
                                    Console.WriteLine("Невірний індекс другого смартфона!");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Невірний індекс першого смартфона!");
                            }
                        }
                        break;

                    case 7:
                        if (smartphones.Count == 0)
                        {
                            Console.WriteLine("Список порожній!");
                        }
                        else
                        {
                            Console.WriteLine("Оберіть індекс смартфону для збільшення ОЗУ:");
                            for (int i = 0; i < smartphones.Count; i++)
                                Console.WriteLine($"{i} - {smartphones[i].Brand} {smartphones[i].Model} ({smartphones[i].OzyGB} ГБ)");

                            if (int.TryParse(Console.ReadLine(), out int ramIndex) &&
                                ramIndex >= 0 && ramIndex < smartphones.Count)
                            {
                                Console.Write("На скільки ГБ збільшити: ");
                                if (int.TryParse(Console.ReadLine(), out int extraGB))
                                {
                                    Console.WriteLine(smartphones[ramIndex].UpgradeRAM(extraGB));
                                }
                                else
                                {
                                    Console.WriteLine("Помилка! Введіть число.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Невірний індекс!");
                            }
                        }
                        break;

                    case 8:
                        Console.WriteLine(Smart.GetCategory());
                        Console.WriteLine($"Кількість створених смартфонів: {Smart.Count}");
                        break;

                    case 9:
                        SmartFileManager.SaveCollection(smartphones);
                        break;

                    case 10:
                        SmartFileManager.LoadCollection(smartphones);
                        break;

                    case 11:
                        SmartFileManager.ClearCollection(smartphones);
                        break;

                    case 0:
                        Console.WriteLine("Вихід з програми...");
                        break;

                    default:
                        Console.WriteLine("Невірний вибір, спробуйте ще раз.");
                        break;
                }
            } while (choice != 0);
        }

        public static void AddSmartphone(List<Smart> smartphones, string input)
        {
            if (Smart.TryParse(input, out Smart parsedSmart))
                smartphones.Add(parsedSmart);
            else
                throw new ArgumentException("Некоректні дані для створення смартфона!");
        }

        public static List<Smart> FindByBrand(List<Smart> smartphones, string brand)
        {
            return smartphones.FindAll(s =>
                s.Brand.Equals(brand, StringComparison.OrdinalIgnoreCase));
        }

        public static bool RemoveSmartphone(List<Smart> smartphones, int index)
        {
            if (index >= 0 && index < smartphones.Count)
            {
                smartphones.RemoveAt(index);
                return true;
            }
            return false;
        }
    }
}
