using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab1SystemSoftware
{
    //узел дерева
    public class TreeNode
    {
        public TreeNode(string n)
        {
            this.Name = n;
            this.Children = new List<TreeNode>();
        }

        public string Name { get; set; } //имя узла
        public int X { get; set; } //горизонтальная координата для отображения (заполняется TreeCalcCoordinates)
        public int Y { get; set; } //вертикальная координата для отображения (заполняется TreeCalcCoordinates) 
        public List<TreeNode> Children { get; set; } //список дочерних узлов


        //получает суммарное количество всех дочерних узлов (высоту поддерева)
        public static int GetChildrenCountSum(TreeNode node)
        {
            if (node.Children.Count == 0) return 1;

            int c = 0;

            foreach (TreeNode child in node.Children) c += GetChildrenCountSum(child);

            return c;
        }

        //получает максимальную ширину узла в дереве
        public static void TreeGetMaxWidth(TreeNode root, ref int maxwidth)
        {
            if (root.Name.Length > maxwidth) maxwidth = root.Name.Length;

            foreach (TreeNode child in root.Children) TreeGetMaxWidth(child, ref maxwidth);

        }

        public static void TreeCalcCoordinates_Recursive(TreeNode node, int basex, int basey, int dx)
        {
            node.X = basex;
            node.Y = basey;

            int c = basey;
            foreach (TreeNode child in node.Children)
            {
                TreeCalcCoordinates_Recursive(child, basex + dx, c, dx);
                c += GetChildrenCountSum(child);
            }
        }

        //вычисляет координаты узлов в дереве
        public static void TreeCalcCoordinates(TreeNode root)
        {
            int maxwidth = 0;
            TreeGetMaxWidth(root, ref maxwidth);
            int dx = maxwidth + 1;

            TreeCalcCoordinates_Recursive(root, 0, 0, dx);
        }

        public static void TreePrint_Recursive(TreeNode node)
        {
            Console.SetCursorPosition(node.X, node.Y);
            Console.Write(node.Name);

            foreach (TreeNode child in node.Children)
            {
                TreePrint_Recursive(child);
            }
        }

        //выводит дерево в консоль, используя вычисленные координаты
        public static void TreePrint(TreeNode root)
        {
            Console.Clear();
            TreePrint_Recursive(root);
        }

    }

    internal class SyntaxAnalizer
    {
        public SyntaxAnalizer()
        {

        }
    }
}
