using System;
using System.Collections.Generic;
using System.Linq;

public class BinaryTree<T> where T : IComparable {
    public TreeNode<T> Root { get; private set; }

    public void Add(T key) {
        if (Root == null) {
            Root = new TreeNode<T> { Value = key };
            return;
        }

        TreeNode<T> current = Root;
        while (true) {
            int comparisonResult = key.CompareTo(current.Value);
            if (comparisonResult < 0) {
                if (current.Left == null) {
                    current.Left = new TreeNode<T> { Value = key };
                    return;
                }
                current = current.Left;
            } else if (comparisonResult > 0) {
                if (current.Right == null) {
                    current.Right = new TreeNode<T> { Value = key };
                    return;
                }
                current = current.Right;
            } else {
                // If the key is equal to the current node's value, do nothing (assuming no duplicates are allowed).
                return;
            }
        }
    }

    public bool Contains(T key) {
        TreeNode<T> current = Root;
        while (current != null) {
            int comparisonResult = key.CompareTo(current.Value);
            if (comparisonResult < 0) {
                current = current.Left;
            } else if (comparisonResult > 0) {
                current = current.Right;
            } else {
                // Key found
                return true;
            }
        }

        // Key not found
        return false;
    }
}

public class TreeNode<T> {
    public T Value;
    public TreeNode<T> Left, Right;
}
