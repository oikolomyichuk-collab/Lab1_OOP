using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;

namespace Lab1_OOP
{
    public static class SmartFileManager
    {
        private static ThreadLocal<string> CsvFile = new ThreadLocal<string>(() => Path.Combine(Directory.GetCurrentDirectory(), "smartphones.csv"));
        private static ThreadLocal<string> JsonFile = new ThreadLocal<string>(() => Path.Combine(Directory.GetCurrentDirectory(), "smartphones.json"));

        public static void SetFilePaths(string csvPath, string jsonPath)
        {
            if (string.IsNullOrWhiteSpace(csvPath)) throw new ArgumentException(nameof(csvPath));
            if (string.IsNullOrWhiteSpace(jsonPath)) throw new ArgumentException(nameof(jsonPath));

            CsvFile.Value = Path.GetFullPath(csvPath);
            JsonFile.Value = Path.GetFullPath(jsonPath);
            Console.WriteLine($"SetFilePaths: CSV={CsvFile.Value}, JSON={JsonFile.Value}");
        }

        public static void SaveCollection(List<Smart> smartphones)
        {
            Console.WriteLine("1 - Зберегти у *.csv");
            Console.WriteLine("2 - Зберегти у *.json");
            Console.Write("Ваш вибір: ");

            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.WriteLine("Помилка введення!");
                return;
            }

            if (smartphones == null || smartphones.Count == 0)
            {
                Console.WriteLine("Немає смартфонів для збереження!");
                return;
            }

            switch (choice)
            {
                case 1: SaveToCsv(smartphones); break;
                case 2: SaveToJson(smartphones); break;
                default: Console.WriteLine("Невірний вибір!"); break;
            }
        }

        public static void LoadCollection(List<Smart> smartphones)
        {
            Console.WriteLine("1 - Зчитати з *.csv");
            Console.WriteLine("2 - Зчитати з *.json");
            Console.Write("Ваш вибір: ");

            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.WriteLine("Помилка введення!");
                return;
            }

            switch (choice)
            {
                case 1: LoadFromCsv(smartphones); break;
                case 2: LoadFromJson(smartphones); break;
                default: Console.WriteLine("Невірний вибір!"); break;
            }
        }

        private static void RetryFileAction(Action action, int retries = 15, int delayMs = 2000)
        {
            for (int i = 0; i < retries; i++)
            {
                try
                {
                    action();
                    return;
                }
                catch (IOException ex)
                {
                    if (i == retries - 1) throw;
                    Console.WriteLine($"Повторна спроба {i + 1}/{retries} через помилку: {ex.Message}");
                    Thread.Sleep(delayMs);
                }
            }
        }

        private static void PrepareFileForWrite(string path)
        {
            if (File.Exists(path))
            {
                File.SetAttributes(path, FileAttributes.Normal);
            }
        }

        public static void SaveToCsv(List<Smart> smartphones)
        {
            try
            {
                if (string.IsNullOrEmpty(CsvFile.Value))
                    throw new InvalidOperationException("Шлях до CSV-файлу не ініціалізовано.");

                PrepareFileForWrite(CsvFile.Value);
                RetryFileAction(() =>
                {
                    using (var fs = new FileStream(CsvFile.Value, FileMode.Create, FileAccess.Write, FileShare.Read))
                    using (var writer = new StreamWriter(fs))
                    {
                        writer.WriteLine("Бренд;Модель;ОЗУ;Камера");
                        foreach (var s in smartphones)
                            writer.WriteLine(s.ToString());
                    }
                });
                Console.WriteLine("Колекцію збережено у CSV файл!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при збереженні CSV: {ex.Message}");
                throw;
            }
        }

        public static int LoadFromCsv(List<Smart> smartphones)
        {
            if (string.IsNullOrEmpty(CsvFile.Value))
            {
                Console.WriteLine("Шлях до CSV-файлу не ініціалізовано!");
                return 0;
            }

            if (!File.Exists(CsvFile.Value))
            {
                Console.WriteLine("Файл CSV не знайдено!");
                return 0;
            }

            int count = 0;
            try
            {
                RetryFileAction(() =>
                {
                    using (var fs = new FileStream(CsvFile.Value, FileMode.Open, FileAccess.Read, FileShare.Read))
                    using (var reader = new StreamReader(fs))
                    {
                        reader.ReadLine(); 
                        string? line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            try
                            {
                                var smart = Smart.Parse(line);
                                bool exists = smartphones.Any(s =>
                                    s.Brand == smart.Brand &&
                                    s.Model == smart.Model &&
                                    s.OzyGB == smart.OzyGB &&
                                    s.CameraMPx == smart.CameraMPx);
                                if (!exists)
                                {
                                    smartphones.Add(smart);
                                    count++;
                                }
                            }
                            catch { }
                        }
                    }
                });

                Console.WriteLine($"Десеріалізовано {count} смартфонів із CSV файлу.");
                foreach (var s in smartphones)
                    Console.WriteLine(s.GetInfo());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при зчитуванні CSV: {ex.Message}");
            }

            return count;
        }

        public static void SaveToJson(List<Smart> smartphones)
        {
            try
            {
                if (string.IsNullOrEmpty(JsonFile.Value))
                    throw new InvalidOperationException("Шлях до JSON-файлу не ініціалізовано.");

                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    IncludeFields = true
                };
                string json = JsonSerializer.Serialize(smartphones, options);

                PrepareFileForWrite(JsonFile.Value);
                RetryFileAction(() =>
                {
                    File.WriteAllText(JsonFile.Value, json);
                });
                Console.WriteLine("Колекцію збережено у JSON файл!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при збереженні JSON: {ex.Message}");
                throw;
            }
        }

        public static int LoadFromJson(List<Smart> smartphones)
        {
            if (string.IsNullOrEmpty(JsonFile.Value))
            {
                Console.WriteLine("Шлях до JSON-файлу не ініціалізовано!");
                return 0;
            }

            if (!File.Exists(JsonFile.Value))
            {
                Console.WriteLine("Файл JSON не знайдено!");
                return 0;
            }

            int loadedCount = 0;
            try
            {
                RetryFileAction(() =>
                {
                    string json = File.ReadAllText(JsonFile.Value);
                    var options = new JsonSerializerOptions
                    {
                        IncludeFields = true
                    };
                    var loaded = JsonSerializer.Deserialize<List<Smart>>(json, options);

                    if (loaded != null)
                    {
                        foreach (var smart in loaded)
                        {
                            bool exists = smartphones.Any(s =>
                                s.Brand == smart.Brand &&
                                s.Model == smart.Model &&
                                s.OzyGB == smart.OzyGB &&
                                s.CameraMPx == smart.CameraMPx);

                            if (!exists)
                            {
                                smartphones.Add(smart);
                                loadedCount++;
                            }
                        }
                        Console.WriteLine($"Десеріалізовано {loadedCount} смартфонів із JSON файлу.");
                        foreach (var s in smartphones)
                            Console.WriteLine(s.GetInfo());
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при зчитуванні JSON: {ex.Message}");
            }

            return loadedCount;
        }

        public static void ClearCollection(List<Smart> smartphones)
        {
            smartphones.Clear();

            try
            {
                if (string.IsNullOrEmpty(CsvFile.Value) || string.IsNullOrEmpty(JsonFile.Value))
                    throw new InvalidOperationException("Шляхи до файлів не ініціалізовано.");

                PrepareFileForWrite(CsvFile.Value);
                PrepareFileForWrite(JsonFile.Value);
                RetryFileAction(() =>
                {
                    File.WriteAllText(CsvFile.Value, "Бренд;Модель;ОЗУ;Камера" + Environment.NewLine);
                });
                RetryFileAction(() =>
                {
                    File.WriteAllText(JsonFile.Value, "[]");
                });
                Console.WriteLine("Колекцію очищено. Файли оновлено!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка очищення файлів: {ex.Message}");
                throw;
            }
        }
    }
}