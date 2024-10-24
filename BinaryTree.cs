using System;
using System.Collections;
using System.Collections.Generic;

namespace BinaryTrees {
    // обобщенный класс для бинарного дерева
    public class BinaryTree<T> : IEnumerable<T> where T : IComparable {
        // значение корневого узла
        private T rootNodeValue;
        // количество узлов в дереве
        private int nodeCount = 1;
        // левое поддерево
        private BinaryTree<T> leftSubtree;
        // правое поддерево
        private BinaryTree<T> rightSubtree;
        // флаг инициализации дерева
        private bool isInitialized;

        // конструктор по умолчанию
        public BinaryTree() { }

        // конструктор для инициализации дерева с заданным значением
        private BinaryTree(T value) {
            this.rootNodeValue = value;
            isInitialized = true;
        }

        // метод добавления элемента в дерево
        public void Add(T key) {
            // если дерево не инициализировано, инициализировать его
            if (!isInitialized) {
                InitializeTree(key);
                return;
            }

            // иначе добавляем элемент рекурсивно
            AddRecursive(this, key);
        }

        // метод инициализации дерева
        private void InitializeTree(T key) {
            rootNodeValue = key;
            isInitialized = true;
        }

        // рекурсивный метод добавления элемента в дерево
        private void AddRecursive(BinaryTree<T> parentNode, T key) {
            // увеличить счетчик узлов в дереве
            parentNode.nodeCount++;

            // cравнить значение корневого узла с добавляемым ключом
            int compareResult = parentNode.rootNodeValue.CompareTo(key);

            // значение текущего узла больше, чем добавляемое 
            if (compareResult > 0)
                // добавляем в левое поддерево
                AddToLeft(parentNode, key);
            else
                // добавляем в правое поддерево
                AddToRight(parentNode, key);
        }

        // метод добавления элемента в левое поддерево
        private void AddToLeft(BinaryTree<T> parentNode, T key) {
            // если левое поддерево существует
            if (parentNode.leftSubtree != null)
                // вызываем рекурсивное добавление
                AddRecursive(parentNode.leftSubtree, key);
            else
                // Иначе создаем новое левое поддерево с заданным ключом
                parentNode.leftSubtree = new BinaryTree<T>(key);
        }

        // Метод добавления элемента в правое поддерево
        private void AddToRight(BinaryTree<T> parentNode, T key) {
            // Если правое поддерево существует, вызываем рекурсивное добавление
            if (parentNode.rightSubtree != null)
                // вызываем рекурсивное добавление
                AddRecursive(parentNode.rightSubtree, key);
            else
                // Иначе создаем новое правое поддерево с заданным ключом
                parentNode.rightSubtree = new BinaryTree<T>(key);
        }

        // Метод проверки наличия элемента в дереве
        public bool Contains(T key) {
            // если дерево не инициализировано, элемент не может присутствовать
            if (!isInitialized)
                return false;

            // иначе проверяем наличие элемента рекурсивно
            return ContainsRecursive(this, key);
        }

        // рекурсивный метод проверки наличия элемента в дереве
        private bool ContainsRecursive(BinaryTree<T> parentNode, T key) {
            // сравнить значение корневого узла с заданным ключом
            int compareResult = parentNode.rootNodeValue.CompareTo(key);

            // если значение текущего узла равно искомому, элемент найден
            if (compareResult == 0)
                return true;

            // определяем, находится ли ключ в левом поддереве
            bool isLeftSubtree = compareResult > 0;
            // проверяем, существует ли левое поддерево
            bool hasLeftSubtree = parentNode.leftSubtree != null;
            // проверяем, существует ли правое поддерево
            bool hasRightSubtree = parentNode.rightSubtree != null;

            // если ключ в левом поддереве и левое поддерево существует
            if (isLeftSubtree && hasLeftSubtree)
                // рекурсивно ищем в левом поддереве
                return ContainsRecursive(parentNode.leftSubtree, key);

            // если ключ в правом поддереве и правое поддерево существует
            if (!isLeftSubtree && hasRightSubtree)
                // рекурсивно ищем в правом поддереве
                return ContainsRecursive(parentNode.rightSubtree, key);

            // иначе, ключ отсутствует
            return false;
        }

        // мндексатор для получения элемента по индексу
        public T this[int i] {
            get {
                // если дерево не инициализировано, выбрасываем исключение
                if (!isInitialized)
                    throw new InvalidOperationException();

                // иначе вызываем метод получения элемента по индексу
                return GetElementAtIndex(this, i);
            }
        }

        // метод получения элемента по индексу
        private T GetElementAtIndex(BinaryTree<T> parentNode, int index) {
            int parentWeight = 0;

            while (true) {
                // получаем количество узлов в левом поддереве, если оно существует
                int leftSubtreeNodeCount = (parentNode.leftSubtree != null) ? 
                    parentNode.leftSubtree.nodeCount : 0;

                // вычисляем индекс текущего узла,
                // учитывая количество узлов в левом поддереве и вес родительского узла
                int currentNodeIndex = leftSubtreeNodeCount + parentWeight;

                // если текущий индекс равен искомому
                if (index == currentNodeIndex)
                    // возвращаем значение текущего узла
                    return parentNode.rootNodeValue;

                // если искомый индекс меньше
                if (index < currentNodeIndex)
                    // ищем в левом поддереве
                    parentNode = parentNode.leftSubtree;
                else {
                    // иначе ищем в правом поддереве
                    parentNode = parentNode.rightSubtree;
                    parentWeight = currentNodeIndex + 1;
                }
            }
        }

        // реализация интерфейса IEnumerable<T> для поддержки итерации по дереву
        public IEnumerator<T> GetEnumerator() {
            // tсли дерево не инициализировано, завершаем итерацию
            if (!isInitialized)
                yield break;

            // рекурсивно обойти элементы в левом поддереве и вернуть их
            if (leftSubtree != null) {
                foreach (var element in TraverseInOrder(leftSubtree))
                    yield return element;
            }

            // возвращение значения текущего узла
            yield return rootNodeValue;

            // рекурсивно обойти элементы в правом поддереве и вернуть их
            if (rightSubtree != null) {
                foreach (var element in TraverseInOrder(rightSubtree))
                    yield return element;
            }
        }

        // метод для рекурсивного обхода элементов в порядке возрастания
        private IEnumerable<T> TraverseInOrder(BinaryTree<T> node) {
            // если узел пуст, завершить итерацию
            if (node == null)
                yield break;

            // рекурсивно обойти левое поддерево и вернуть его элементы
            foreach (var element in TraverseInOrder(node.leftSubtree))
                yield return element;

            // возвращение значения текущего узла
            yield return node.rootNodeValue;

            // рекурсивно обойти правое поддерево и вернуть его элементы
            foreach (var element in TraverseInOrder(node.rightSubtree))
                yield return element;
        }

        // реализация интерфейса IEnumerable для поддержки итерации по дереву
        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}
