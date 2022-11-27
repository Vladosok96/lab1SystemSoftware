using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace lab1SystemSoftware
{
    //узел дерева
    public class TreeNode
    {
        public List<TreeNode> Children { get; set; } //список дочерних узлов
        public GrammaticalComponent.Component type; // имя узла
        public TokenType.Type sub_type;
        public VariableType.Type variable_type;
        public int value { get; set; } // Значение
        public int X { get; set; } //горизонтальная координата для отображения (заполняется TreeCalcCoordinates)
        public int Y { get; set; } //вертикальная координата для отображения (заполняется TreeCalcCoordinates) 
        
        public TreeNode(GrammaticalComponent.Component _Type)
        {
            type = _Type;
            Children = new List<TreeNode>();
        }
        public TreeNode(GrammaticalComponent.Component _Type, TokenType.Type _sub_type)
        {
            type = _Type;
            sub_type = _sub_type;
            Children = new List<TreeNode>();
        }

        public TreeNode(GrammaticalComponent.Component _Type, TokenType.Type _sub_type, VariableType.Type _variable_type, int _value)
        {
            type = _Type;
            sub_type = _sub_type;
            variable_type = _variable_type;
            value = _value;
            Children = new List<TreeNode>();
        }

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
            if (root.type.ToString().Length > maxwidth) maxwidth = root.type.ToString().Length;
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
            Console.Write(node.type.ToString());

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

        private static TreeNode root;

        public SyntaxAnalizer() { }

        public SyntaxAnalizer(TreeNode _root)
        {
            root = _root;
        }

        public static SyntaxAnalizer Process(LexicalAnalizer lexical_analizer)
        {
            List<Tuple<TokenType.Type, int>> tokens_thread = lexical_analizer.GetTokensThread();
            
            TreeNode _root = new TreeNode(GrammaticalComponent.Component.empty_set);

            GrammaticalComponent.State current_state = GrammaticalComponent.State.None;
            List<TreeNode> nodes_buffer = new List<TreeNode>();

            for (int i = 0; i < tokens_thread.Count; i++)
            {
                switch (current_state)
                {
                    case GrammaticalComponent.State.None:
                        if (tokens_thread[i].Item1 == TokenType.Type.Identifier)
                        {
                            if (TokenType.isAssignType(tokens_thread[i + 1].Item1))
                            {
                                nodes_buffer.Add(new TreeNode(GrammaticalComponent.Component.stmts, TokenType.Type.OperatorAssign));
                                nodes_buffer[nodes_buffer.Count - 1].Children.Add(new TreeNode(GrammaticalComponent.Component.identifier, TokenType.Type.Identifier, VariableType.Type._int, 0));

                            }
                            else
                            {
                                // TODO: Assign error handler
                            }
                        }
                        //nodes_buffer.Add(new TreeNode(GrammaticalComponent.Component.stmts));
                        break;
                }
            }

            return new SyntaxAnalizer(_root);
        }
    }
}
