using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace lab1SystemSoftware
{
    internal class TreeConverter
    {
        public TreeConverter() { }

        private static string unwrap_tree(TreeNode root, Dictionary<int, string> identifiers_table, List<List<Tuple<int, VariableType.Type>>> variable_layers)
        {
            string result = "";
            List<TreeNode> forks = new List<TreeNode>();

            forks.Add(root);

            while (forks.Count > 0)
            {
                if (forks.Last().Children.Count == 1)
                {
                    // TODO: Print operator
                    switch (forks.Last().sub_type)
                    {
                        case TokenType.Type.OperatorAddition:
                            result += " + ";
                            break;
                        case TokenType.Type.OperatorSubtraction:
                            result += " - ";
                            break;
                        case TokenType.Type.OperatorMultiplication:
                            result += " * ";
                            break;
                        case TokenType.Type.OperatorDivision:
                            result += " / ";
                            break;
                        case TokenType.Type.ComparisonLess:
                            result += " < ";
                            break;
                        case TokenType.Type.ComparisonGreater:
                            result += " > ";
                            break;
                        case TokenType.Type.ComparisonLessEqual:
                            result += " <= ";
                            break;
                        case TokenType.Type.ComparisonGreaterEqual:
                            result += " >= ";
                            break;
                    }
                }
                if (forks.Last().Children.Count > 0)
                {
                    forks.Add(forks.Last().Children.Last());
                    forks[forks.Count - 2].Children.RemoveAt(forks[forks.Count - 2].Children.Count - 1);
                }
                else
                {
                    if (forks.Last().sub_type == TokenType.Type.Number)
                    {
                        result += forks.Last().value;
                    }
                    else if (forks.Last().sub_type == TokenType.Type.Identifier)
                    {
                        bool check_variable = false;
                        for (int i = 0; i < variable_layers.Count; i++)
                        {
                            for (int j = 0; j < variable_layers[i].Count; j++)
                            {
                                if (variable_layers[i][j].Item1 == forks.Last().value)
                                {
                                    check_variable = true;
                                }
                            }
                        }
                        if (!check_variable)
                        {
                            throw new Exception("using undefined variable");
                        }
                        result += identifiers_table[forks.Last().value];
                    }
                    forks.RemoveAt(forks.Count - 1);
                }
            }

            return result;
        }

        public static void Process(SyntaxAnalizer syntaxAnalizer, LexicalAnalizer lexicalAnalizer)
        {
            TreeNode tree = syntaxAnalizer.GetTree();
            Dictionary<int, string> identifier_table = lexicalAnalizer.GetIdentifiresTable();
            List<List<Tuple<int, VariableType.Type>>> variable_layers = new List<List<Tuple<int, VariableType.Type>>>();
            List<List<TreeNode>> nodes = new List<List<TreeNode>>();
            string output = "";

            nodes.Add(tree.Children);
            variable_layers.Add(new List<Tuple<int, VariableType.Type>>());

            while (nodes.Count > 0)
            {
                if (nodes[nodes.Count - 1][0].sub_type == TokenType.Type.OperatorAssign)
                {
                    bool check_variable = false;
                    for (int i = 0; i < variable_layers.Count; i++)
                    {
                        for (int j = 0; j < variable_layers[i].Count; j++)
                        {
                            if (variable_layers[i][j].Item1 == nodes.Last()[0].Children[0].value)
                            {
                                check_variable = true; break;
                            }
                        }
                    }
                    if (nodes.Last()[0].is_initial)
                    {
                        if (check_variable)
                        {
                            output += "\nERROR: variable already defined";
                            break;
                        }
                        else
                        {
                            variable_layers[variable_layers.Count - 1].Add(new Tuple<int, VariableType.Type>(nodes.Last()[0].Children[0].value, nodes.Last()[0].Children[0].variable_type));
                            for (int i = 0; i < nodes.Count - 1; i++)
                            {
                                output += "    ";
                            }
                            output += identifier_table[nodes.Last()[0].Children[0].value];
                            output += " = ";

                            try
                            {
                                output += unwrap_tree(nodes.Last()[0].Children[1], identifier_table, variable_layers);
                            }
                            catch
                            {
                                output += "\nERROR: using undefined variable";
                                break;
                            }

                            output += "\n";
                        }
                    }
                    else
                    {
                        if (check_variable)
                        {
                            for (int i = 0; i < nodes.Count - 1; i++)
                            {
                                output += "    ";
                            }
                            output += identifier_table[nodes.Last()[0].Children[0].value];
                            output += " = ";

                            try
                            {
                                output += unwrap_tree(nodes.Last()[0].Children[1], identifier_table, variable_layers);
                            }
                            catch
                            {
                                output += "\nERROR: using undefined variable";
                                break;
                            }

                            output += "\n";
                        }
                        else
                        {
                            output += "\nERROR: using undefined variable";
                            break;
                        }
                    }
                    nodes[nodes.Count - 1].RemoveAt(0);
                }
                else if (nodes[nodes.Count - 1][0].sub_type == TokenType.Type.KeywordIf)
                {
                    for (int i = 0; i < nodes.Count - 1; i++)
                    {
                        output += "    ";
                    }
                    output += "if ";

                    try {
                        output += unwrap_tree(nodes.Last().First().Children[0], identifier_table, variable_layers);
                    }
                    catch
                    {
                        output += "\nERROR: using undefined variable";
                        break;
                    }

                    output += ":\n";

                    List<TreeNode> tmp_node_list = new List<TreeNode>(nodes.Last().First().Children[1].Children);
                    nodes[nodes.Count - 1].RemoveAt(0);
                    nodes.Add(tmp_node_list);
                    variable_layers.Add(new List<Tuple<int, VariableType.Type>>());
                }
                while (nodes.Last().Count == 0)
                {
                    variable_layers.RemoveAt(variable_layers.Count - 1);
                    nodes.RemoveAt(nodes.Count - 1);
                    if (nodes.Count == 0)
                    {
                        break;
                    }
                }
            }
            Console.WriteLine("");
            Console.WriteLine(output);
        }
    }
}
