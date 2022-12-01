using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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



        public static SyntaxAnalizer Process(LexicalAnalizer lexical_analizer)
        {
            List<Tuple<TokenType.Type, int>> tokens_thread = lexical_analizer.GetTokensThread();
            Dictionary<int, string> identifiers_table = lexical_analizer.GetIdentifiresTable();
            
            TreeNode _root = new TreeNode(GrammaticalComponent.Component.empty_set);

            List<TreeNode> nodes_buffer = new List<TreeNode>();

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
                            
                        List<TreeNode> expr_nodes_buffer = new List<TreeNode>();
                        expr_nodes_buffer.Add(new TreeNode(GrammaticalComponent.Component.value, tokens_thread[i + 2].Item1, VariableType.Type._int, tokens_thread[i + 2].Item2));

                        TreeNode tmpTreeNode;
                        TokenType.Type lastOperator = TokenType.Type.None;

                        for (int j = i + 3; tokens_thread[j].Item1 != TokenType.Type.BracketClose;)
                        {
                            if (tokens_thread[j].Item1 == TokenType.Type.OperatorMultiplication || tokens_thread[j].Item1 == TokenType.Type.OperatorDivision)
                            {
                                expr_nodes_buffer.Add(new TreeNode(GrammaticalComponent.Component.value, tokens_thread[j].Item1));
                                expr_nodes_buffer[expr_nodes_buffer.Count - 1].Children.Add(new TreeNode(GrammaticalComponent.Component.value, tokens_thread[j + 1].Item1, VariableType.Type._int, tokens_thread[j + 1].Item2));
                                expr_nodes_buffer[expr_nodes_buffer.Count - 1].Children.Add(expr_nodes_buffer[expr_nodes_buffer.Count - 2]);
                                expr_nodes_buffer.RemoveAt(expr_nodes_buffer.Count - 2);
                                j += 2;
                            }
                            else if (tokens_thread[j].Item1 == TokenType.Type.OperatorAddition || tokens_thread[j].Item1 == TokenType.Type.OperatorSubtraction)
                            {
                                if (expr_nodes_buffer.Count > 1)
                                {
                                    tmpTreeNode = new TreeNode(GrammaticalComponent.Component.value, tokens_thread[j].Item1);
                                    tmpTreeNode.Children.Add(expr_nodes_buffer[expr_nodes_buffer.Count - 1]);
                                    tmpTreeNode.Children.Add(expr_nodes_buffer[expr_nodes_buffer.Count - 2]);
                                    expr_nodes_buffer[expr_nodes_buffer.Count - 2] = tmpTreeNode;
                                    expr_nodes_buffer.RemoveAt(expr_nodes_buffer.Count - 1);
                                }
                                lastOperator = tokens_thread[j].Item1;
                                expr_nodes_buffer.Add(new TreeNode(GrammaticalComponent.Component.value, tokens_thread[j + 1].Item1, VariableType.Type._int, tokens_thread[j + 1].Item2));
                                j += 2;
                            }
                            else if (TokenType.isComparisionType(tokens_thread[j].Item1))
                            {
                                if (expr_nodes_buffer.Count > 1)
                                {
                                    tmpTreeNode = new TreeNode(GrammaticalComponent.Component.value, tokens_thread[j].Item1);
                                    tmpTreeNode.Children.Add(expr_nodes_buffer[expr_nodes_buffer.Count - 1]);
                                    tmpTreeNode.Children.Add(expr_nodes_buffer[expr_nodes_buffer.Count - 2]);
                                    expr_nodes_buffer[expr_nodes_buffer.Count - 2] = tmpTreeNode;
                                    expr_nodes_buffer.RemoveAt(expr_nodes_buffer.Count - 1);
                                }
                                lastOperator = tokens_thread[j].Item1;
                                expr_nodes_buffer.Add(new TreeNode(GrammaticalComponent.Component.value, tokens_thread[j + 1].Item1, VariableType.Type._int, tokens_thread[j + 1].Item2));
                                j += 2;
                            }
                        }

                        if (expr_nodes_buffer.Count > 1)
                        {
                            tmpTreeNode = new TreeNode(GrammaticalComponent.Component.value, lastOperator);
                            tmpTreeNode.Children.Add(expr_nodes_buffer[expr_nodes_buffer.Count - 1]);
                            tmpTreeNode.Children.Add(expr_nodes_buffer[expr_nodes_buffer.Count - 2]);
                            expr_nodes_buffer[expr_nodes_buffer.Count - 2] = tmpTreeNode;
                            expr_nodes_buffer.RemoveAt(expr_nodes_buffer.Count - 1);
                        }

                        nodes_buffer[nodes_buffer.Count - 1].Children[0].Children.Add(expr_nodes_buffer[0]);
                        state = GrammaticalComponent.State.wait_condition;
                    }
                    else if (tokens_thread[i].Item1 == TokenType.Type.Identifier)
                    {
                        nodes_buffer.Add(new TreeNode(GrammaticalComponent.Component.stmts, tokens_thread[i + 1].Item1));
                        nodes_buffer[nodes_buffer.Count - 1].Children.Add(new TreeNode(GrammaticalComponent.Component.identifier, TokenType.Type.Identifier, VariableType.Type._int, tokens_thread[i].Item2));
                        
                        TreeNode tmpTreeNode;
                        TokenType.Type lastOperator = TokenType.Type.None;

                        List<TreeNode> expr_nodes_buffer = new List<TreeNode>();
                        expr_nodes_buffer.Add(new TreeNode(GrammaticalComponent.Component.value, tokens_thread[i + 2].Item1, VariableType.Type._int, tokens_thread[i + 2].Item2));

                        for (int j = i + 3; tokens_thread[j].Item1 != TokenType.Type.Semicolon;)
                        {
                            if (tokens_thread[j].Item1 == TokenType.Type.OperatorMultiplication || tokens_thread[j].Item1 == TokenType.Type.OperatorDivision)
                            {
                                expr_nodes_buffer.Add(new TreeNode(GrammaticalComponent.Component.value, tokens_thread[j].Item1));
                                expr_nodes_buffer[expr_nodes_buffer.Count - 1].Children.Add(new TreeNode(GrammaticalComponent.Component.value, tokens_thread[j + 1].Item1, VariableType.Type._int, tokens_thread[j + 1].Item2));
                                expr_nodes_buffer[expr_nodes_buffer.Count - 1].Children.Add(expr_nodes_buffer[expr_nodes_buffer.Count - 2]);
                                expr_nodes_buffer.RemoveAt(expr_nodes_buffer.Count - 2);
                                j += 2;
                            }
                            else if (tokens_thread[j].Item1 == TokenType.Type.OperatorAddition || tokens_thread[j].Item1 == TokenType.Type.OperatorSubtraction)
                            {
                                if (expr_nodes_buffer.Count > 1)
                                {
                                    tmpTreeNode = new TreeNode(GrammaticalComponent.Component.value, tokens_thread[j].Item1);
                                    tmpTreeNode.Children.Add(expr_nodes_buffer[expr_nodes_buffer.Count - 1]);
                                    tmpTreeNode.Children.Add(expr_nodes_buffer[expr_nodes_buffer.Count - 2]);
                                    expr_nodes_buffer[expr_nodes_buffer.Count - 2] = tmpTreeNode;
                                    expr_nodes_buffer.RemoveAt(expr_nodes_buffer.Count - 1);
                                }
                                lastOperator = tokens_thread[j].Item1;
                                expr_nodes_buffer.Add(new TreeNode(GrammaticalComponent.Component.value, tokens_thread[j + 1].Item1, VariableType.Type._int, tokens_thread[j + 1].Item2));
                                j += 2;
                            }
                        }
                        
                        if (expr_nodes_buffer.Count > 1)
                        {
                            tmpTreeNode = new TreeNode(GrammaticalComponent.Component.value, lastOperator);
                            tmpTreeNode.Children.Add(expr_nodes_buffer[expr_nodes_buffer.Count - 1]);
                            tmpTreeNode.Children.Add(expr_nodes_buffer[expr_nodes_buffer.Count - 2]);
                            expr_nodes_buffer[expr_nodes_buffer.Count - 2] = tmpTreeNode;
                            expr_nodes_buffer.RemoveAt(expr_nodes_buffer.Count - 1);
                        }

                        nodes_buffer[nodes_buffer.Count - 1].Children.Add(expr_nodes_buffer[0]);
                        state = GrammaticalComponent.State.wait_expression;
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
                        _root.Children.Add(nodes_buffer[nodes_buffer.Count - 1]);
                        nodes_buffer.RemoveAt(nodes_buffer.Count - 1);
                        state = GrammaticalComponent.State.none;
                    }
                }
                else if (state == GrammaticalComponent.State.wait_condition)
                {
                    if (tokens_thread[i].Item1 == TokenType.Type.BracketClose)
                    {
                        _root.Children.Add(nodes_buffer[nodes_buffer.Count - 1]);
                        nodes_buffer.RemoveAt(nodes_buffer.Count - 1);
                        state = GrammaticalComponent.State.none;
                    }
                }
            }

            return new SyntaxAnalizer(_root);
        }

        public void PrintTree(Dictionary<int, string> identifiers_table)
        {
            TreeNode.TreeCalcCoordinates(root);
            TreeNode.TreePrint(root, identifiers_table);
        }
    }
}
