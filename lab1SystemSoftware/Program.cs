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
            
            var syntax_analyzer = SyntaxAnalizer.Process(lexical_analize);
            syntax_analyzer.PrintTree(identifiers_table);

            Console.ReadKey();

        }
    }
}
