using Collections;
using MusicalInstruments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    [TestClass]
    public class MyCollectionTests
    {
        private MyCollection<MusicalInstrument> collection;

        // Инициализируем коллекцию перед каждым тестом — создаём с 5 случайными элементами
        [TestInitialize]
        public void Initialize()
        {
            collection = new MyCollection<MusicalInstrument>(5);
        }

        // Проверяем, что конструктор по умолчанию создаёт коллекцию с ёмкостью 10 и нулевым количеством элементов
        [TestMethod]
        public void MyCollection_DefaultConstructor_CreatesEmptyCollectionWithDefaultCapacity()
        {
            var collection = new MyCollection<MusicalInstrument>();

            Assert.IsNotNull(collection); // объект успешно создан
            Assert.AreEqual(0, collection.Count); // коллекция пустая
            Assert.AreEqual(10, collection.Capacity); // стандартная ёмкость равна 10
        }

        // Проверяем, что Clear() обнуляет количество элементов
        [TestMethod]
        public void ClearCount_ReturnsZero()
        {
            ((ICollection<MusicalInstrument>)collection).Clear();
            Assert.AreEqual(0, collection.Count); // после очистки должно быть 0 элементов
        }

        // Проверяем, что Add() корректно добавляет элемент и увеличивает Count
        [TestMethod]
        public void Add_Item_ShouldIncreaseCount()
        {
            var item = new Guitar();
            item.RandomInit();

            collection.Add(item);

            Assert.IsTrue(collection.Contains(item)); // элемент должен находиться в коллекции
            Assert.AreEqual(6, ((ICollection<MusicalInstrument>)collection).Count); // количество элементов увеличилось до 6
        }

        // Проверяем, что IsReadOnly возвращает false
        [TestMethod]
        public void IsReadOnly_ReturnsFalse()
        {
            Assert.IsFalse(((ICollection<MusicalInstrument>)collection).IsReadOnly); // коллекция не только для чтения
        }

        // Проверяем, что Add через интерфейс ICollection<T> тоже работает
        [TestMethod]
        public void Add_UsingICollectionInterface_ShouldIncreaseCount()
        {
            var item = new Guitar();
            item.RandomInit();

            ((ICollection<MusicalInstrument>)collection).Add(item);

            Assert.IsTrue(collection.Contains(item));
            Assert.AreEqual(6, ((ICollection<MusicalInstrument>)collection).Count);
        }

        // Проверяем, что Contains через интерфейс ICollection<T> находит существующий элемент
        [TestMethod]
        public void Contains_UsingICollectionInterface_ReturnsTrueForExistingItem()
        {
            var item = new Piano();
            item.RandomInit();
            ((ICollection<MusicalInstrument>)collection).Add(item);

            Assert.IsTrue(((ICollection<MusicalInstrument>)collection).Contains(item));
        }

        // Проверяем, что CopyTo копирует все элементы в массив при достаточном размере
        [TestMethod]
        public void CopyTo_ArrayWithSufficientSpace_CopiesAllItems()
        {
            var array = new MusicalInstrument[collection.Count];
            ((ICollection<MusicalInstrument>)collection).CopyTo(array, 0);

            foreach (var item in array)
            {
                Assert.IsNotNull(item);
                Assert.IsTrue(collection.Contains(item));
            }
        }

        // Проверяем, что CopyTo выбрасывает ArgumentNullException, если передан null
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CopyTo_NullArray_ThrowsArgumentNullException()
        {
            ((ICollection<MusicalInstrument>)collection).CopyTo(null, 0);
        }

        // Проверяем, что отрицательный индекс вызывает ArgumentOutOfRangeException
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void CopyTo_NegativeArrayIndex_ThrowsArgumentOutOfRangeException()
        {
            var array = new MusicalInstrument[collection.Count];
            ((ICollection<MusicalInstrument>)collection).CopyTo(array, -1);
        }

        // Проверяем, что если массив слишком маленький — бросается ArgumentException
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CopyTo_NotEnoughSpaceInArray_ThrowsArgumentException()
        {
            var array = new MusicalInstrument[collection.Count - 1];
            ((ICollection<MusicalInstrument>)collection).CopyTo(array, 0);
        }

        // Проверяем, что Equals возвращает true, если сравниваем с тем же объектом
        [TestMethod]
        public void Equals_SameReference_ReturnsTrue()
        {
            var other = collection;
            Assert.IsTrue(collection.Equals(other));
        }

        

        // Проверяем, что Contains возвращает true для существующего элемента
        [TestMethod]
        public void Contains_Item_ReturnsTrueIfExists()
        {
            var item = new Piano();
            item.RandomInit();
            collection.Add(item);

            Assert.IsTrue(collection.Contains(item));
        }

        // Проверяем, что попытка присвоить значение по несуществующему ключу вызывает KeyNotFoundException
        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void Indexer_SetItemWithNonExistingKey_ShouldThrow()
        {
            var key = new Piano();
            key.RandomInit();

            var value = new Piano();
            value.RandomInit();

            collection[key] = value; // должно выбросить исключение
        }

        // Проверяем, что GetEnumerator перебирает все элементы коллекции
        [TestMethod]
        public void GetEnumerator_ShouldIterateAllItems()
        {
            int count = 0;
            foreach (var item in collection)
            {
                count++;
                Assert.IsNotNull(item);
            }

            Assert.AreEqual(collection.Count, count); // количество элементов совпадает с Count
        }

        // Проверяем, что конструктор копирования создаёт новый объект с теми же данными
        [TestMethod]
        public void Clone_CopyCollection_ShouldHaveSameItems()
        {
            var copy = new MyCollection<MusicalInstrument>(collection);

            Assert.AreNotSame(collection, copy); // объекты разные

            var originalEnumerator = collection.GetEnumerator();
            var copyEnumerator = copy.GetEnumerator();

            while (originalEnumerator.MoveNext() && copyEnumerator.MoveNext())
            {
                Assert.AreNotSame(originalEnumerator.Current, copyEnumerator.Current); // ссылки разные
                Assert.AreEqual(originalEnumerator.Current.ToString(), copyEnumerator.Current.ToString()); // данные одинаковые
            }
        }

        // Проверяем, что при заполнении коллекции выше LoadFactor происходит Resize
        [TestMethod]
        public void Resize_Collection_AfterExceedingLoadFactor()
        {
            int initialCapacity = collection.Capacity;

            for (int i = 0; i < initialCapacity; i++)
            {
                var item = new Piano();
                item.RandomInit();
                collection.Add(item);
            }

            Assert.IsTrue(collection.Capacity > initialCapacity); // ёмкость должна увеличиться
        }

        // Проверяем, что Equals(null) возвращает false
        [TestMethod]
        public void Equals_Null_ReturnsFalse()
        {
            Assert.IsFalse(collection.Equals(null));
        }

        // Проверяем, что Remove через интерфейс ICollection<T> удаляет элемент
        [TestMethod]
        public void Remove_ItemUsingICollectionInterface_ReturnsTrueAndRemovesItem()
        {
            var item = new Guitar();
            item.RandomInit();
            collection.Add(item);

            bool removed = collection.Remove(item);

            Assert.IsTrue(removed);
            Assert.IsFalse(collection.Contains(item));
        }

        // Проверяем, что GetHashCode стабильный для одного и того же объекта
        [TestMethod]
        public void GetHashCode_SameReference_ReturnsSameHashCode()
        {
            int hash1 = collection.GetHashCode();
            int hash2 = collection.GetHashCode();

            Assert.AreEqual(hash1, hash2);
        }

        // Проверяем, что удаление несуществующего элемента возвращает false
        [TestMethod]
        public void Remove_NonExistingItemUsingICollectionInterface_ReturnsFalse()
        {
            var item = new Piano();
            item.RandomInit();

            bool removed = (collection).Remove(item);

            Assert.IsFalse(removed);
        }

        // Проверяем, что удаление из пустой коллекции возвращает false
        [TestMethod]
        public void Remove_ItemFromEmptyCollectionUsingICollectionInterface_ReturnsFalse()
        {
            var emptyCollection = new MyCollection<MusicalInstrument>(0);
            var item = new Piano();
            item.RandomInit();

            bool removed = (emptyCollection.Remove(item));

            Assert.IsFalse(removed);
        }
    }
}