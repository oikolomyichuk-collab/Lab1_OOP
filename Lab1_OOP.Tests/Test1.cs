using Lab1_OOP;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lab1_OOP.Tests
{
    [TestClass]
    public class RamTests
    {
        [TestMethod]
        public void UpgradeRAM()
        {
            // Arrange
            var smart = new Smart("Samsung", "Galaxy", 8, 64);

            // Act
            var result = smart.UpgradeRAM(4);

            // Assert
            Assert.AreEqual("ОЗУ збільшено! Тепер 12 ГБ", result);
            Assert.AreEqual(12, smart.OzyGB);
        }
    }

    [TestClass]
    public class BrandTests
    {
        [TestMethod]
        public void Brand()
        {
            // Arrange
            var smart = new Smart("Samsung", "Galaxy", 8, 64);

            // Assert
            Assert.AreEqual("Samsung", smart.Brand);
        }
    }

    [TestClass]
    public class ModelTests
    {
        [TestMethod]
        public void Model()
        {
            // Arrange
            var smart = new Smart("Samsung", "Galaxy", 8, 64);

            // Assert
            Assert.AreEqual("Galaxy", smart.Model);
        }
    }

    [TestClass]
    public class OzyGbTests
    {
        [TestMethod]
        public void OzyGb()
        {
            // Arrange
            var smart = new Smart("Samsung", "Galaxy", 8, 64);

            // Assert
            Assert.AreEqual(8, smart.OzyGB);
        }
    }

    [TestClass]
    public class CameraTests
    {
        [TestMethod]
        public void Camera()
        {
            // Arrange
            var smart = new Smart("Samsung", "Galaxy", 8, 64);

            // Assert
            Assert.AreEqual(64, smart.CameraMPx);
        }
    }

    [TestClass]
    public class PhotoTests
    {
        [TestMethod]
        public void Photo()
        {
            // Arrange
            var smart = new Smart("Samsung", "Galaxy", 8, 64);

            // Act
            var result = smart.Photo();

            // Assert
            Assert.AreEqual("Samsung Galaxy може фотографувати", result);
        }
    }

    [TestClass]
    public class InternetTests
    {
        [TestMethod]
        public void Internet()
        {
            // Arrange
            var smart = new Smart("Samsung", "Galaxy", 8, 64);

            // Act
            var result = smart.Internet();

            // Assert
            Assert.AreEqual("Samsung Galaxy може виходити в інтернет", result);
        }
    }

    [TestClass]
    public class TemperatureTests
    {
        [TestMethod]
        public void Temperature()
        {
            // Arrange
            var smart = new Smart("Samsung", "Galaxy", 8, 64);

            // Act
            var result = smart.Temperature();

            // Assert
            Assert.AreEqual("Samsung Galaxy може нагріватися", result);
        }
    }

    [TestClass]
    public class CallTests
    {
        [TestMethod]
        public void Call()
        {
            // Arrange
            var smart = new Smart("Samsung", "Galaxy", 8, 64);

            // Act
            var result = smart.Call();

            // Assert
            Assert.AreEqual("Samsung Galaxy може дзвонити", result);
        }

        [TestMethod]
        public void CallWithNumber()
        {
            // Arrange
            var smart = new Smart("Samsung", "Galaxy", 8, 64);

            // Act
            var result = smart.Call("123-456-789");

            // Assert
            Assert.AreEqual("Samsung Galaxy телефонує на номер 123-456-789", result);
        }
    }

    [TestClass]
    public class CompareCameraTests
    {
        [TestMethod]
        public void CompareCamera1()
        {
            // Arrange
            var smart1 = new Smart("Samsung", "Galaxy", 8, 64);
            var smart2 = new Smart("Xiaomi", "A23", 2, 32);

            // Act
            var result = smart1.CompareCamera(smart2);

            // Assert
            Assert.AreEqual("Samsung Galaxy має кращу камеру (64 Мп) ніж Xiaomi A23 (32 Мп).", result);
        }

        [TestMethod]
        public void CompareCamera2()
        {
            // Arrange
            var smart1 = new Smart("Samsung", "Galaxy", 8, 64);
            var smart2 = new Smart("Xiaomi", "A23", 2, 132);

            // Act
            var result = smart1.CompareCamera(smart2);

            // Assert
            Assert.AreEqual("Xiaomi A23 має кращу камеру (132 Мп) ніж Samsung Galaxy (64 Мп).", result);
        }

        [TestMethod]
        public void CompareCamera3()
        {
            // Arrange
            var smart1 = new Smart("Samsung", "Galaxy", 8, 132);
            var smart2 = new Smart("Xiaomi", "A23", 2, 132);

            // Act
            var result = smart1.CompareCamera(smart2);

            // Assert
            Assert.AreEqual("У обох смартфонів однакова камера.", result);
        }
    }

    [TestClass]
    public class ToStringTest
    {
        [TestMethod]
        public void ToString_ReturnsCorrectFormat()
        {
            // Arrange
            var smart = new Smart("Samsung", "Galaxy", 8, 64);

            // Act
            var result = smart.ToString();

            // Assert
            Assert.AreEqual("Samsung;Galaxy;8;64", result);
        }
    }

    [TestClass]
    public class ParseTest
    {
        [TestMethod]
        public void Parse1()
        {
            // Arrange
            string input = "Samsung;Galaxy;8;64";

            // Act
            var result = Smart.Parse(input);

            // Assert
            Assert.AreEqual("Samsung", result.Brand);
            Assert.AreEqual("Galaxy", result.Model);
            Assert.AreEqual(8, result.OzyGB);
            Assert.AreEqual(64, result.CameraMPx);
        }

        [TestMethod]
        public void Parse2()
        {
            // Arrange
            string input = "НекорректнаСтрока";

            // Act + Assert
            Assert.ThrowsException<FormatException>(() => Smart.Parse(input));
        }
    }

    [TestClass]
    public class TryParseTests
    {
        [TestMethod]
        public void TryParse1()
        {
            // Arrange
            string input = "Samsung;Galaxy;8;64";

            // Act
            bool success = Smart.TryParse(input, out Smart result);

            // Assert
            Assert.IsTrue(success);
            Assert.IsNotNull(result);
            Assert.AreEqual("Samsung", result.Brand);
            Assert.AreEqual("Galaxy", result.Model);
            Assert.AreEqual(8, result.OzyGB);
            Assert.AreEqual(64, result.CameraMPx);
        }

        [TestMethod]
        public void TryParse2()
        {
            // Arrange
            string input = "НекорректнаСтрока";

            // Act
            bool success = Smart.TryParse(input, out Smart result);

            // Assert
            Assert.IsFalse(success);
            Assert.IsNull(result);
        }
    }

    [TestClass]
    public class GetCategoryTests
    {
        [TestMethod]
        public void GetCategory()
        {
            // Arrange
            var smart = new Smart("Samsung", "Galaxy", 8, 64);

            // Act
            var result = Smart.GetCategory();

            // Assert
            Assert.AreEqual("Категорія товару: Електроніка", result);
        }
    }

    [TestClass]
    public class UpdateTypeTests
    {
        [TestMethod]
        public void UpdateType_Weak()
        {
            // Arrange
            var smart = new Smart("Test", "Phone1", 2, 12);

            // Act
            smart.UpgradeRAM(5);
            var type = smart.Type;

            // Assert
            Assert.AreEqual(SmartphoneType.Average, type);
        }

        [TestMethod]
        public void UpdateType_Average()
        {
            // Arrange
            var smart = new Smart("Test", "Phone2", 8, 12);

            // Act
            smart.UpgradeRAM(5);
            var type = smart.Type;

            // Assert
            Assert.AreEqual(SmartphoneType.Powerfull, type);
        }

        [TestMethod]
        public void UpdateType_Powerfull()
        {
            // Arrange
            var smart = new Smart("Test", "Phone3", 16, 12);

            // Act
            smart.UpgradeRAM(4);
            var type = smart.Type;

            // Assert
            Assert.AreEqual(SmartphoneType.Powerfull, type);
        }
    }

    [TestClass]
    public class UpdateType1Tests
    {
        private Smart smart;

        [TestInitialize]
        public void Init()
        {
            smart = new Smart("TestBrand", "TestModel", 1, 8);
        }

        [TestCleanup]
        public void Cleanup()
        {
            smart = null;
        }

        [TestMethod]
        public void UpdateType1()
        {
            // Arrange
            smart.OzyGB = 2;

            // Act
            var type = smart.Type;

            // Assert
            Assert.AreEqual(SmartphoneType.Weak, type);
        }

        [TestMethod]
        public void UpdateType2()
        {
            // Arrange
            smart.OzyGB = 8;

            // Act
            var type = smart.Type;

            // Assert
            Assert.AreEqual(SmartphoneType.Average, type);
        }

        [TestMethod]
        public void UpdateType3()
        {
            // Arrange
            smart.OzyGB = 16;

            // Act
            var type = smart.Type;

            // Assert
            Assert.AreEqual(SmartphoneType.Powerfull, type);
        }
    }


    [TestClass]
    [DoNotParallelize]
    public class CountTests
    {
        [TestInitialize]
        public void Init()
        {
            Smart.ResetCountForTests();
        }

        [TestMethod]
        public void Count()
        {
            // Act
            var s1 = new Smart("Samsung", "Galaxy", 8, 64);
            var s2 = new Smart("Xiaomi", "A23", 4, 32);

            // Assert
            Assert.AreEqual(2, Smart.Count);
        }
    }

    [TestClass]
    public class ProgramListMethodsTests
    {
        private List<Smart> smartphones;

        [TestInitialize]
        public void Init()
        {
            smartphones = new List<Smart>
            {
                new Smart("Samsung", "Galaxy", 8, 64),
                new Smart("Xiaomi", "Redmi", 4, 48),
                new Smart("Apple", "iPhone", 6, 12)
            };
        }

        [TestMethod]
        public void AddSmartphone_ValidInput_AddsObject()
        {
            // Arrange
            string input = "Nokia;3310;1;2";

            // Act
            Program.AddSmartphone(smartphones, input);

            // Assert
            Assert.AreEqual(4, smartphones.Count);
            Assert.AreEqual("Nokia", smartphones[3].Brand);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void AddSmartphone_InvalidInput_ThrowsException()
        {
            // Arrange
            string input = "НекорректнаСтрока";

            // Act
            Program.AddSmartphone(smartphones, input);
        }

        [TestMethod]
        public void FindByBrand_ExistingBrand_ReturnsList()
        {
            // Act
            var result = Program.FindByBrand(smartphones, "Samsung");

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Galaxy", result[0].Model);
        }

        [TestMethod]
        public void FindByBrand_NonExistingBrand_ReturnsEmptyList()
        {
            // Act
            var result = Program.FindByBrand(smartphones, "Motorola");

            // Assert
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void RemoveSmartphone_ValidIndex_RemovesObject()
        {
            // Act
            bool removed = Program.RemoveSmartphone(smartphones, 1);

            // Assert
            Assert.IsTrue(removed);
            Assert.AreEqual(2, smartphones.Count);
            Assert.AreEqual("Samsung", smartphones[0].Brand);
        }

        [TestMethod]
        public void RemoveSmartphone_InvalidIndex_ReturnsFalse()
        {
            // Act
            bool removed = Program.RemoveSmartphone(smartphones, 10);

            // Assert
            Assert.IsFalse(removed);
            Assert.AreEqual(3, smartphones.Count);
        }

    }
}


