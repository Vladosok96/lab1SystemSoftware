using System;
using System.Collections.Generic;
using System.IO;

namespace lab1SystemSoftware
{
    class Program
    {
        static void Main(string[] args)
        {
            String PATH_TO_PROGRAM = "script.js";

            String JS_code_string = "";
            String input_string;

            try
            {
                StreamReader sr = new StreamReader(PATH_TO_PROGRAM);
                input_string = sr.ReadLine();
                JS_code_string += input_string;
                while (input_string != null)
                {
                    input_string = sr.ReadLine();
                    JS_code_string += " " + input_string;
                }
                sr.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }

            var lexical_analize = LexicalAnalizer.GetTokens(JS_code_string);
            var identifiers_table = lexical_analize.GetIdentifiresTable();
            var tokens_table = lexical_analize.GetTokensThread();

            Console.WriteLine("Identifiers Table:");
            foreach (var identifier in identifiers_table)
            {
                Console.WriteLine("{0}: {1};", identifier.Key, identifier.Value);
            }
            Console.WriteLine("\nTokens thread:");
            foreach (var token in tokens_table)
            {
                if (token.Item1 == TokenType.Type.Identifier)
                {
                    Console.WriteLine("Type: identifier, id: {0}, name: {1};", token.Item2, identifiers_table[token.Item2]);
                }
                else if (token.Item1 == TokenType.Type.Number)
                {
                    Console.WriteLine("Type: number, value: {0};", token.Item2);
                }
                else if (token.Item1 == TokenType.Type.Error)
                {
                    if (token.Item2 == 0) Console.WriteLine("\nError! unknown character;");
                }
                else
                {
                    Console.WriteLine("Type: {0};", token.Item1.ToString());
                }
            }

            Console.ReadLine();

            //заполняем дерево тестовыми данными
            TreeNode root = new TreeNode("Животные");

            TreeNode node = new TreeNode("Млекопитающие");
            TreeNode node2 = new TreeNode("Хищные");
            node2.Children.Add(new TreeNode("Волк"));
            node2.Children.Add(new TreeNode("Лиса"));
            node.Children.Add(node2);
            node2 = new TreeNode("Зайцеобразные");
            node2.Children.Add(new TreeNode("Заяц"));
            node2.Children.Add(new TreeNode("Кролик"));
            node.Children.Add(node2);
            root.Children.Add(node);

            node = new TreeNode("Птицы");
            node.Children.Add(new TreeNode("Пингвин"));
            node.Children.Add(new TreeNode("Попугай"));
            node.Children.Add(new TreeNode("Ворона"));
            root.Children.Add(node);

            node = new TreeNode("Пресмыкающиеся");
            node.Children.Add(new TreeNode("Черепаха"));
            node.Children.Add(new TreeNode("Крокодил"));
            root.Children.Add(node);

            //вычисляем координаты
            TreeNode.TreeCalcCoordinates(root);

            //выводим дерево в консоль
            TreeNode.TreePrint(root);

            Console.ReadKey();

        }
    }
}
