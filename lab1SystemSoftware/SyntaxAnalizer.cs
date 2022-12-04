using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Linq;

namespace lab1SystemSoftware
{
    //узел дерева
    public class TreeNode
    {
        public List<TreeNode> Children { get; set; } //список дочерних узлов
        public GrammaticalComponent.Component type; // имя узла
        public TokenType.Type sub_type;
        public VariableType.Type variable_type;
        public int value = -69696969; // Значение
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

        public TreeNode(GrammaticalComponent.Component _Type, int _value)
        {
            type = _Type;
            value = _value;
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
            if (root.type.ToString().Length > maxwidth)
            {
                maxwidth = root.type.ToString().Length;
            }
            foreach (TreeNode child in root.Children)
            {
                TreeGetMaxWidth(child, ref maxwidth);
            }
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

            TreeCalcCoordinates_Recursive(root, 0, 0, 25);
        }

        public static void TreePrint_Recursive(TreeNode node, Dictionary<int, string> table)
        {
            Console.SetCursorPosition(node.X, node.Y * 3);
            Console.Write(node.type.ToString());
            if (TokenType.isAssignType(node.sub_type) || TokenType.isAryphmeticType(node.sub_type) || node.sub_type == TokenType.Type.KeywordIf || TokenType.isComparisionType(node.sub_type))
            {
                Console.SetCursorPosition(node.X, node.Y * 3 + 1);
                Console.Write(node.sub_type.ToString());
            }
            else if (node.sub_type == TokenType.Type.Identifier)
            {
                Console.SetCursorPosition(node.X, node.Y * 3 + 1);
                Console.Write(table[node.value]);
            }
            else if (node.value != -69696969)
            {
                Console.SetCursorPosition(node.X, node.Y * 3 + 1);
                Console.Write(node.value.ToString());
            }

            foreach (TreeNode child in node.Children)
            {
                TreePrint_Recursive(child, table);
            }
        }

        //выводит дерево в консоль, используя вычисленные координаты
        public static void TreePrint(TreeNode root, Dictionary<int, string> table)
        {
            Console.Clear();
            TreePrint_Recursive(root, table);
        }

    }

    internal class SyntaxAnalizer
    {

        private TreeNode root;

        public SyntaxAnalizer() { }

        public SyntaxAnalizer(TreeNode _root)
        {
            root = _root;
        }

        private static TreeNode parseExpression (List<Tuple<TokenType.Type, int>> expression_thread)
        {
            List<TreeNode> nodes_buffer = new List<TreeNode>();
            List<TreeNode> last_nodes_buffer = new List<TreeNode>();

            for (int i = 0; i < expression_thread.Count; i++)
            {
                nodes_buffer.Add(new TreeNode(GrammaticalComponent.Component.value, expression_thread[i].Item1, VariableType.Type._int, expression_thread[i].Item2));
            }

            while (!nodes_buffer.SequenceEqual(last_nodes_buffer))
            {
                last_nodes_buffer = new List<TreeNode>(nodes_buffer);
                for (int i = 1; i < nodes_buffer.Count; i += 2) // low priority (not dota)
                {
                    if (nodes_buffer[i].sub_type == TokenType.Type.OperatorMultiplication || nodes_buffer[i].sub_type == TokenType.Type.OperatorDivision)
                    {
                        nodes_buffer[i].Children.Add(nodes_buffer[i + 1]);
                        nodes_buffer[i].Children.Add(nodes_buffer[i - 1]);
                        nodes_buffer.RemoveAt(i + 1);
                        nodes_buffer.RemoveAt(i - 1);
                        i--;
                    }
                }
            }

            last_nodes_buffer = new List<TreeNode>(nodes_buffer);
            for (int i = 1; i < nodes_buffer.Count; i++) // meddium priority (not dota)
            {
                if (nodes_buffer[i].sub_type == TokenType.Type.OperatorAddition || nodes_buffer[i].sub_type == TokenType.Type.OperatorSubtraction)
                {
                    nodes_buffer[i].Children.Add(nodes_buffer[i + 1]);
                    nodes_buffer[i].Children.Add(nodes_buffer[i - 1]);
                    nodes_buffer.RemoveAt(i + 1);
                    nodes_buffer.RemoveAt(i - 1);
                    i--;
                }
            }

            last_nodes_buffer = new List<TreeNode>(nodes_buffer);
            for (int i = 1; i < nodes_buffer.Count; i++) // meddium priority (not dota)
            {
                if (TokenType.isComparisionType(nodes_buffer[i].sub_type))
                {
                    nodes_buffer[i].Children.Add(nodes_buffer[i + 1]);
                    nodes_buffer[i].Children.Add(nodes_buffer[i - 1]);
                    nodes_buffer.RemoveAt(i + 1);
                    nodes_buffer.RemoveAt(i - 1);
                    i--;
                }
            }

            return nodes_buffer[0];
        }

        public static SyntaxAnalizer Process(LexicalAnalizer lexical_analizer)
        {
            List<Tuple<TokenType.Type, int>> tokens_thread = lexical_analizer.GetTokensThread();
            Dictionary<int, string> identifiers_table = lexical_analizer.GetIdentifiresTable();
            

            List<TreeNode> nodes_buffer = new List<TreeNode>();
            nodes_buffer.Add(new TreeNode(GrammaticalComponent.Component.empty_set));

            GrammaticalComponent.State state = GrammaticalComponent.State.none;
            
            for (int i = 0; i < tokens_thread.Count; i++)
            {
                if (state == GrammaticalComponent.State.none)
                {
                    if (tokens_thread[i].Item1 == TokenType.Type.KeywordIf)
                    {
                        // TODO: stmtif handler
                        nodes_buffer.Add(new TreeNode(GrammaticalComponent.Component.stmts, tokens_thread[i].Item1));
                        nodes_buffer[nodes_buffer.Count - 1].Children.Add(new TreeNode(GrammaticalComponent.Component.condition));

                        int counter = 0;
                        while (tokens_thread[i + 2 + counter].Item1 != TokenType.Type.BracketClose)
                        {
                            counter++;
                        }

                        nodes_buffer[nodes_buffer.Count - 1].Children[0].Children.Add(parseExpression(tokens_thread.GetRange(i + 2, counter)));
                        state = GrammaticalComponent.State.wait_condition;
                    }
                    else if (tokens_thread[i].Item1 == TokenType.Type.Identifier)
                    {
                        TreeNode tmpNode = new TreeNode(GrammaticalComponent.Component.stmts, tokens_thread[i + 1].Item1);
                        tmpNode.Children.Add(new TreeNode(GrammaticalComponent.Component.identifier, TokenType.Type.Identifier, VariableType.Type._int, tokens_thread[i].Item2));
                        

                        int counter = 0;
                        while (tokens_thread[i + 2 + counter].Item1 != TokenType.Type.Semicolon)
                        {
                            counter++;
                        }


                        TreeNode tmpExpression = parseExpression(tokens_thread.GetRange(i + 2, counter));
                        tmpNode.Children.Add(parseExpression(tokens_thread.GetRange(i + 2, counter)));
                        
                        if (nodes_buffer[nodes_buffer.Count - 1].sub_type == TokenType.Type.KeywordIf)
                        {
                            nodes_buffer[nodes_buffer.Count - 1].Children[nodes_buffer[nodes_buffer.Count - 1].Children.Count - 1].Children.Add(tmpNode);
                        }
                        else
                        {
                            nodes_buffer[nodes_buffer.Count - 1].Children.Add(tmpNode);
                        }

                        state = GrammaticalComponent.State.wait_expression;
                    }
                    else if (tokens_thread[i].Item1 == TokenType.Type.BraceClose)
                    {
                        if (nodes_buffer[nodes_buffer.Count - 2].sub_type == TokenType.Type.KeywordIf)
                        {
                            nodes_buffer[nodes_buffer.Count - 2].Children[nodes_buffer[nodes_buffer.Count - 1].Children.Count - 1].Children.Add(nodes_buffer[nodes_buffer.Count - 1]);
                        }
                        else
                        {
                            nodes_buffer[nodes_buffer.Count - 2].Children.Add(nodes_buffer[nodes_buffer.Count - 1]);
                        }
                        nodes_buffer.RemoveAt(nodes_buffer.Count - 1);
                    }
                    else
                    {
                        // TODO: syntax error handler
                    }
                }
                else if (state == GrammaticalComponent.State.wait_expression)
                {
                    if (tokens_thread[i].Item1 == TokenType.Type.Semicolon)
                    {
                        state = GrammaticalComponent.State.none;
                    }
                }
                else if (state == GrammaticalComponent.State.wait_condition)
                {
                    if (tokens_thread[i].Item1 == TokenType.Type.BracketClose)
                    {
                        nodes_buffer[nodes_buffer.Count - 1].Children.Add(new TreeNode(GrammaticalComponent.Component.body));
                        state = GrammaticalComponent.State.none;
                    }
                }
            }

            return new SyntaxAnalizer(nodes_buffer[0]);
        }

        public void PrintTree(Dictionary<int, string> identifiers_table)
        {
            TreeNode.TreeCalcCoordinates(root);
            TreeNode.TreePrint(root, identifiers_table);
        }
    }
}
