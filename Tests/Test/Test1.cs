using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Collections;
using MusicalInstruments;

namespace Test
{
    [TestClass]
    public class DoublyLinkedListTests
    {
        // 1. Тестирование метода Add
        [TestMethod]
        public void Add_ShouldAddElementsToTheList()
        {
            // Arrange
            var list = new DoublyLinkedList<MusicalInstrument>();
            var guitar = new Guitar { Name = "Guitar" };
            var piano = new Piano { Name = "Piano" };

            // Act
            list.Add(guitar);
            list.Add(piano);

            // Assert
            Assert.AreEqual(2, list.Count);
            Assert.AreEqual("Guitar", list.begin.Data.Name);
            Assert.AreEqual("Piano", list.end.Data.Name);
        }

        // 2. Тестирование метода PrintList (через перехват вывода в консоль)
        [TestMethod]
        public void PrintList_ShouldPrintElementsCorrectly()
        {
            // Arrange
            var list = new DoublyLinkedList<MusicalInstrument>();
            var guitar = new Guitar { Name = "Guitar" };
            var piano = new Piano { Name = "Piano" };
            list.Add(guitar);
            list.Add(piano);

            // Act & Assert
            using (var consoleOutput = new ConsoleOutput())
            {
                list.PrintList();
                string output = consoleOutput.GetOutput();
                StringAssert.Contains(output, "1: Guitar");
                StringAssert.Contains(output, "2: Piano");
            }
        }

        // 3. Тестирование метода AddOddIndexElements
        [TestMethod]
        public void AddOddIndexElements_ShouldAddRandomElementsAtOddIndexes()
        {
            // Arrange
            var list = new DoublyLinkedList<MusicalInstrument>();
            var guitar = new Guitar { Name = "Guitar" };
            var piano = new Piano { Name = "Piano" };
            list.Add(guitar);
            list.Add(piano);

            // Act
            list.AddOddIndexElements(2); // Добавляем 2 случайных элемента

            // Assert
            Assert.AreEqual(4, list.Count);
            Assert.IsTrue(list.begin.Next.Data.Name.Contains("(Rnd)")); // Проверяем, что добавленные элементы содержат "(Rnd)"
        }

        // 4. Тестирование метода RemoveFromElementToEnd
        [TestMethod]
        public void RemoveFromElementToEnd_ShouldRemoveElementsStartingFromGivenName()
        {
            // Arrange
            var list = new DoublyLinkedList<MusicalInstrument>();
            var guitar = new Guitar { Name = "Guitar" };
            var piano = new Piano { Name = "Piano" };
            var electroGuitar = new ElectroGuitar { Name = "ElectroGuitar" };
            list.Add(guitar);
            list.Add(piano);
            list.Add(electroGuitar);

            // Act
            list.RemoveFromElementToEnd("Piano");

            // Assert
            Assert.AreEqual(1, list.Count);
            Assert.AreEqual("Guitar", list.begin.Data.Name);
            Assert.IsNull(list.begin.Next);
        }

        // 5. Тестирование метода DeepClone
        [TestMethod]
        public void DeepClone_ShouldCreateDeepCopyOfTheList()
        {
            // Arrange
            var list = new DoublyLinkedList<MusicalInstrument>();
            var guitar = new Guitar { Name = "Guitar" };
            var piano = new Piano { Name = "Piano" };
            list.Add(guitar);
            list.Add(piano);

            // Act
            var clonedList = list.DeepClone();

            // Assert
            Assert.AreEqual(2, clonedList.Count);
            Assert.AreNotSame(list.begin.Data, clonedList.begin.Data); // Проверяем, что это разные объекты
            Assert.AreEqual(list.begin.Data.Name, clonedList.begin.Data.Name);
        }

        // 6. Тестирование метода Clear
        [TestMethod]
        public void Clear_ShouldRemoveAllElementsFromTheList()
        {
            // Arrange
            var list = new DoublyLinkedList<MusicalInstrument>();
            var guitar = new Guitar { Name = "Guitar" };
            var piano = new Piano { Name = "Piano" };
            list.Add(guitar);
            list.Add(piano);

            // Act
            list.Clear();

            // Assert
            Assert.AreEqual(0, list.Count);
            Assert.IsNull(list.begin);
            Assert.IsNull(list.end);
        }

        // 7. Тестирование пустого списка
        [TestMethod]
        public void EmptyList_ShouldHaveZeroCountAndNullPointers()
        {
            // Arrange
            var list = new DoublyLinkedList<MusicalInstrument>();

            // Assert
            Assert.AreEqual(0, list.Count);
            Assert.IsNull(list.begin);
            Assert.IsNull(list.end);
        }

        // 8. Тестирование метода CreateRandomInstr
        [TestMethod]
        public void CreateRandomInstr_ShouldReturnRandomInstrumentWithNameSuffix()
        {
            // Arrange
            var list = new DoublyLinkedList<MusicalInstrument>();

            // Act
            var randomInstrument = list.CreateRandomInstr();

            // Assert
            Assert.IsNotNull(randomInstrument);
            StringAssert.Contains(randomInstrument.Name, "(Rnd)");
        }
    }

    // Вспомогательный класс для перехвата вывода в консоль
    public class ConsoleOutput : IDisposable
    {
        private readonly StringWriter _stringWriter;
        private readonly TextWriter _originalOutput;

        public ConsoleOutput()
        {
            _stringWriter = new StringWriter();
            _originalOutput = Console.Out;
            Console.SetOut(_stringWriter);
        }

        public string GetOutput()
        {
            return _stringWriter.ToString();
        }

        public void Dispose()
        {
            Console.SetOut(_originalOutput);
            _stringWriter.Dispose();
        }
    }
}