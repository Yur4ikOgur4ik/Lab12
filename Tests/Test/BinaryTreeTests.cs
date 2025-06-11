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
    public class BinaryTreeTests
    {
        private BinaryTree<MusicalInstrument> tree;

        [TestInitialize]
        public void Initialize()
        {
            // Создаем идеальное дерево из 7 элементов
            tree = new BinaryTree<MusicalInstrument>();
            tree.BuildPerfectTree(7);
        }

        [TestMethod]
        public void BuildPerfectTree_CreatesNonEmptyTree()
        {
            Assert.IsNotNull(tree.Root);            
        }

        [TestMethod]
        public void FindMaxId_ReturnsNodeWithMaxId()
        {
            var maxNode = tree.FindMaxId();
            Assert.IsNotNull(maxNode);

            // Пройдемся по всему дереву и найдём реальный максимум для проверки
            int expectedMaxId = GetMaxIdFromTree(tree.Root);
            Assert.AreEqual(expectedMaxId, maxNode.ID.Id);
        }

        private int GetMaxIdFromTree(TreeNode<MusicalInstrument> node)
        {
            if (node == null) return -1;

            int currentId = node.Data.ID.Id;
            int leftMax = GetMaxIdFromTree(node.Left);
            int rightMax = GetMaxIdFromTree(node.Right);

            return Math.Max(currentId, Math.Max(leftMax, rightMax));
        }

        [TestMethod]
        public void ConvertToSearchTree_ConvertsCorrectly()
        {
            var searchTree = tree.ConvertToSearchTree();

            Assert.IsNotNull(searchTree.Root);
            Assert.IsTrue(IsBinarySearchTree(searchTree.Root));
        }

        private bool IsBinarySearchTree(TreeNode<MusicalInstrument> node, int min = int.MinValue, int max = int.MaxValue)
        {
            if (node == null) return true;

            int currentId = node.Data.ID.Id;

            if (currentId <= min || currentId >= max)
                return false;

            return IsBinarySearchTree(node.Left, min, currentId) &&
                   IsBinarySearchTree(node.Right, currentId, max);
        }

        [TestMethod]
        public void AddToSearchTree_AddsCorrectly()
        {
            var testTree = new BinaryTree<MusicalInstrument>();

            var instr1 = new Guitar { ID = new IdNumber(100), Name = "Guitar 1" };
            var instr2 = new ElectroGuitar { ID = new IdNumber(50), Name = "EGuitar 1" };
            var instr3 = new Piano { ID = new IdNumber(150), Name = "Piano 1" };

            testTree.AddToSearchTree(instr1);
            testTree.AddToSearchTree(instr2);
            testTree.AddToSearchTree(instr3);

            Assert.IsTrue(testTree.Contains(new IdNumber(100)));
            Assert.IsTrue(testTree.Contains(new IdNumber(50)));
            Assert.IsTrue(testTree.Contains(new IdNumber(150)));
        }

        [TestMethod]
        public void DeleteById_RemovesNode()
        {
            var testTree = new BinaryTree<MusicalInstrument>();
            var instr1 = new Guitar { ID = new IdNumber(100), Name = "Guitar 1" };
            var instr2 = new Guitar { ID = new IdNumber(50), Name = "Guitar 2" };
            var instr3 = new Guitar { ID = new IdNumber(150), Name = "Guitar 3" };

            testTree.AddToSearchTree(instr1);
            testTree.AddToSearchTree(instr2);
            testTree.AddToSearchTree(instr3);

            testTree.DeleteById(new IdNumber(50));

            Assert.IsFalse(testTree.Contains(new IdNumber(50)));
            Assert.IsTrue(testTree.Contains(new IdNumber(100)));
        }

        [TestMethod]
        public void Contains_ReturnsTrueForExistingId()
        {
            var idToCheck = tree.Root.Data.ID;
            bool contains = tree.Contains(idToCheck);
            Assert.IsTrue(contains);
        }

        [TestMethod]
        public void Contains_ReturnsFalseForNonExistingId()
        {
            var fakeId = new IdNumber(101010100);
            bool contains = tree.Contains(fakeId);
            Assert.IsFalse(contains);
        }

        [TestMethod]
        public void ToString_ReturnsNullString_WhenDataIsNull()
        {
            // Arrange
            var node = new TreeNode<MusicalInstrument>(null);

            // Act
            string result = node.ToString();

            // Assert
            Assert.AreEqual("null", result);
        }
        [TestMethod]
        public void ToString_ReturnsCorrect_WhenDataIsNotNull()
        {
            // Arrange
            Piano piano = new Piano();
            piano.RandomInit();
            var node = new TreeNode<MusicalInstrument>(piano);

            // Act
            string result = node.ToString();

            // Assert
            Assert.AreEqual(piano.ToString(), result);
        }




        // Ветка: Удаление в пустом дереве (node == null)
        [TestMethod]
        public void DeleteNode_NodeIsNull_ReturnsNull()
        {
            var result = tree.DeleteNode(null, new IdNumber(1));
            Assert.IsNull(result, "В пустом дереве должен вернуться null");
        }

        // Ветка: Идем налево (compare < 0)
        [TestMethod]
        public void DeleteNode_GoesLeft()
        {
            // Создаем дерево: Root(10), Left(5)
            var root = new TreeNode<MusicalInstrument>(new Guitar { ID = new IdNumber(10) });
            root.Left = new TreeNode<MusicalInstrument>(new Guitar { ID = new IdNumber(5) });

            // Удаляем ID=5 — он слева от корня
            var newRoot = tree.DeleteNode(root, new IdNumber(5));

            Assert.IsNotNull(newRoot); // корень остаётся
            Assert.IsNull(newRoot.Left, "Левый дочерний узел должен быть удален");
        }

        // Ветка: Идем направо (compare > 0)
        [TestMethod]
        public void DeleteNode_GoesRight()
        {
            // Создаем дерево: Root(10), Right(15)
            var root = new TreeNode<MusicalInstrument>(new Guitar { ID = new IdNumber(10) });
            root.Right = new TreeNode<MusicalInstrument>(new Guitar { ID = new IdNumber(15) });

            // Удаляем ID=15 — он справа от корня
            var newRoot = tree.DeleteNode(root, new IdNumber(15));

            Assert.IsNotNull(newRoot);
            Assert.IsNull(newRoot.Right, "Правый дочерний узел должен быть удален");
        }

        // Ветка: Узел является листом (нет потомков)
        [TestMethod]
        public void DeleteNode_LeafNode()
        {
            var root = new TreeNode<MusicalInstrument>(new Guitar { ID = new IdNumber(10) });

            var newRoot = tree.DeleteNode(root, new IdNumber(10));

            Assert.IsNull(newRoot, "Корень должен быть удален, так как это лист");
        }

        // Ветка: Узел с одним правым потомком
        [TestMethod]
        public void DeleteNode_OneRightChild()
        {
            // Строим дерево:
            //     10
            //      \
            //       20
            var root = new TreeNode<MusicalInstrument>(new Guitar { ID = new IdNumber(10) });
            root.Right = new TreeNode<MusicalInstrument>(new Guitar { ID = new IdNumber(20) });

            var newRoot = tree.DeleteNode(root, new IdNumber(10));

            Assert.IsNotNull(newRoot);
            Assert.AreEqual(20, newRoot.Data.ID.Id, "После удаления корня должен стать узел с ID=20");
            Assert.IsNull(newRoot.Left);
            Assert.IsNull(newRoot.Right);
        }

        // Ветка: Узел с одним левым потомком
        [TestMethod]
        public void DeleteNode_OneLeftChild()
        {
            // Строим дерево:
            //     10
            //    /
            //   5
            var root = new TreeNode<MusicalInstrument>(new Guitar { ID = new IdNumber(10) });
            root.Left = new TreeNode<MusicalInstrument>(new Guitar { ID = new IdNumber(5) });

            var newRoot = tree.DeleteNode(root, new IdNumber(10));

            Assert.IsNotNull(newRoot);
            Assert.AreEqual(5, newRoot.Data.ID.Id, "После удаления корня должен стать узел с ID=5");
            Assert.IsNull(newRoot.Left);
            Assert.IsNull(newRoot.Right);
        }

        // Ветка: Узел с двумя потомками
        [TestMethod]
        public void DeleteNode_TwoChildren()
        {
            // Строим дерево:
            //      10
            //     /  \
            //    5    15
            //         \
            //          20
            var root = new TreeNode<MusicalInstrument>(new Guitar { ID = new IdNumber(10) });
            root.Left = new TreeNode<MusicalInstrument>(new Guitar { ID = new IdNumber(5) });
            root.Right = new TreeNode<MusicalInstrument>(new Guitar { ID = new IdNumber(15) });
            root.Right.Right = new TreeNode<MusicalInstrument>(new Guitar { ID = new IdNumber(20) });

            var newRoot = tree.DeleteNode(root, new IdNumber(10));

            Assert.IsNotNull(newRoot);
            Assert.AreEqual(15, newRoot.Data.ID.Id, "Новый корень должен быть с ID=15");

            // Проверяем, что старый правый узел (20) остался
            Assert.IsNotNull(newRoot.Right);
            Assert.AreEqual(20, newRoot.Right.Data.ID.Id);

            // Проверяем, что левый потомок (5) сохранился
            Assert.IsNotNull(newRoot.Left);
            Assert.AreEqual(5, newRoot.Left.Data.ID.Id);
        }
    }
}
