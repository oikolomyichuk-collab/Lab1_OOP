using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Lab1_OOP;

namespace Lab1_OOP.Tests
{
    [TestClass]
    public class SmartFileManagerTests
    {
        private string csvPath;
        private string jsonPath;
        private List<Smart> testList;

        [TestInitialize]
        public void Setup()
        {
            csvPath = Path.GetTempFileName();
            jsonPath = Path.GetTempFileName();
            File.Delete(csvPath);
            File.Delete(jsonPath);

            SmartFileManager.SetFilePaths(csvPath, jsonPath);

            Smart.ResetCountForTests();
            testList = new List<Smart>
            {
                new Smart("Samsung", "S21", 8, 64),
                new Smart("Apple", "iPhone13", 6, 12)
            };
            Console.WriteLine($"Setup: csvPath={csvPath}, jsonPath={jsonPath}");
        }

        [TestCleanup]
        public void Cleanup()
        {
            try
            {
                Thread.Sleep(2000);
                if (File.Exists(csvPath))
                {
                    File.SetAttributes(csvPath, FileAttributes.Normal);
                    File.Delete(csvPath);
                    Console.WriteLine($"���� {csvPath} �������� � Cleanup");
                }
                if (File.Exists(jsonPath))
                {
                    File.SetAttributes(jsonPath, FileAttributes.Normal);
                    File.Delete(jsonPath);
                    Console.WriteLine($"���� {jsonPath} �������� � Cleanup");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"������� ��� ������� ����� � TestCleanup: {ex.Message}");
            }
        }

        private bool WaitForFile(string path, int retries = 15, int delayMs = 2000)
        {
            Console.WriteLine($"�������� ��������� �����: {path}");
            if (File.Exists(path))
                Console.WriteLine($"���� {path} ��� ���� ����� �������� WaitForFile");
            for (int i = 0; i < retries; i++)
            {
                if (File.Exists(path))
                {
                    Console.WriteLine($"���� {path} �������� �� ����� {i + 1}/{retries} � {DateTime.Now:HH:mm:ss.fff}");
                    return true;
                }
                Console.WriteLine($"���������� ����� {path}, ������ {i + 1}/{retries} � {DateTime.Now:HH:mm:ss.fff}");
                Thread.Sleep(delayMs);
            }
            Console.WriteLine($"���� {path} �� �������� ���� {retries} ����� � {DateTime.Now:HH:mm:ss.fff}");
            return false;
        }

        [TestMethod]
        public void SaveToCsv_CreatesFile_WithCorrectContent()
        {
            // Arrange

            // Act
            SmartFileManager.SaveToCsv(testList);

            // Assert
            Assert.IsTrue(WaitForFile(csvPath), $"CSV ���� �� ��������: {csvPath}");
            var content = File.ReadAllText(csvPath);
            Console.WriteLine($"���� CSV � ����: {content}");
            StringAssert.Contains(content, "Samsung;S21;8;64", "CSV �� ������ ��� Samsung");
            StringAssert.Contains(content, "Apple;iPhone13;6;12", "CSV �� ������ ��� Apple");
        }

        [TestMethod]
        public void LoadFromCsv_ReadsSmartphonesCorrectly()
        {
            // Arrange
            SmartFileManager.SaveToCsv(testList);
            var loadedList = new List<Smart>();

            // Act
            int count = SmartFileManager.LoadFromCsv(loadedList);

            // Assert
            Assert.AreEqual(2, count);
            Assert.AreEqual("Samsung", loadedList[0].Brand);
            Assert.AreEqual("iPhone13", loadedList[1].Model);
        }

        [TestMethod]
        public void LoadFromCsv_FileMissing_ReturnsZero()
        {
            // Arrange
            var list = new List<Smart>();
            File.Delete(csvPath);

            // Act
            int count = SmartFileManager.LoadFromCsv(list);

            // Assert
            Assert.AreEqual(0, count);
            Assert.AreEqual(0, list.Count);
        }

        [TestMethod]
        public void SaveToJson_CreatesFile_WithValidJson()
        {
            // Arrange

            // Act
            SmartFileManager.SaveToJson(testList);

            // Assert
            Assert.IsTrue(WaitForFile(jsonPath), $"JSON ���� �� ��������: {jsonPath}");
            string json = File.ReadAllText(jsonPath);
            Console.WriteLine($"���� JSON � ����: {json}");
            StringAssert.Contains(json, "\"Brand\": \"Samsung\"", "JSON �� ������ ��� Samsung");
            StringAssert.Contains(json, "\"Model\": \"iPhone13\"", "JSON �� ������ ��� iPhone13");
        }

        [TestMethod]
        public void LoadFromJson_ReadsSmartphonesCorrectly()
        {
            // Arrange
            SmartFileManager.SaveToJson(testList);
            var list = new List<Smart>();

            // Act
            int count = SmartFileManager.LoadFromJson(list);

            // Assert
            Assert.AreEqual(2, count);
            Assert.AreEqual("Samsung", list[0].Brand);
            Assert.AreEqual(8, list[0].OzyGB);
        }

        [TestMethod]
        public void LoadFromJson_FileMissing_ReturnsZero()
        {
            // Arrange
            var list = new List<Smart>();
            File.Delete(jsonPath);

            // Act
            int count = SmartFileManager.LoadFromJson(list);

            // Assert
            Assert.AreEqual(0, count);
        }

        [TestMethod]
        public void ClearCollection_RemovesItemsAndOverwritesFiles()
        {
            // Arrange
            SmartFileManager.SaveToCsv(testList);
            SmartFileManager.SaveToJson(testList);

            // Act
            SmartFileManager.ClearCollection(testList);
            Thread.Sleep(2000);

            // Assert
            Assert.AreEqual(0, testList.Count);
            string csvContent = WaitForFile(csvPath) ? File.ReadAllText(csvPath) : "";
            string jsonContent = WaitForFile(jsonPath) ? File.ReadAllText(jsonPath) : "[]";
            Console.WriteLine($"���� CSV ���� �������� � ����: {csvContent}");
            Console.WriteLine($"���� JSON ���� �������� � ����: {jsonContent}");
            StringAssert.Contains(csvContent, "�����;������;���;������", "CSV �� ������ ��������� ���� ��������");
            Assert.AreEqual("[]", jsonContent.Trim(), "JSON �� ������� ���� ��������");
        }
    }
}