using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lab1_OOP;  // это твой основной проект

namespace Lab1_OOP.Tests   // исправил тут
{
    [TestClass]
    public class SmartTests
    {
        [TestMethod]
        public void UpgradeRAM_ShouldIncreaseRAM_WhenValidExtra()
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
}
