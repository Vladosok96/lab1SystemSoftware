using System;
using System.Collections.Generic;
using System.Linq;

namespace lab1SystemSoftware
{
    class LexicalAnalizer
    {
        enum State
        {
            Word,
            Number,
            Operation,
            None
        }

        private static Boolean IsOperator(char c)
        {
            if ("=<>+-*/!".Contains(c))
            {
                return true;
            }
            return false;
        }

        public static Tuple<List<Tuple<TokenType.Type, int>>, Dictionary<int, string>> GetTokens(String code)
        {
            List<Tuple<TokenType.Type, int>> tokens_thread = new List<Tuple<TokenType.Type, int>>();
            Dictionary<string, int> identidiers_table = new Dictionary<string, int>();

            String buffer = "";
            State curent_state = State.None;
            int identifier_increment = 1;
            bool check_error = false;

            for (int i = 0; i < code.Length && !check_error; i++)
            {
                char character = code[i];

                if (curent_state == State.Number)
                {
                    if (Char.IsDigit(character))
                    {
                        buffer += character;
                    }
                    else
                    {
                        tokens_thread.Add(new Tuple<TokenType.Type, int>(TokenType.Type.Number, Convert.ToInt32(buffer)));
                        buffer = "";
                        curent_state = State.None;
                        i--;
                    }
                }

                else if (curent_state == State.Word)
                {
                    if (Char.IsLetterOrDigit(character))
                    {
                        buffer += character;
                    }
                    else
                    {
                        switch(buffer)
                        {
                            case "let":
                                tokens_thread.Add(new Tuple<TokenType.Type, int>(TokenType.Type.KeywordLet, 0));
                                break;
                            case "var":
                                tokens_thread.Add(new Tuple<TokenType.Type, int>(TokenType.Type.KeywordVar, 0));
                                break;
                            case "if":
                                tokens_thread.Add(new Tuple<TokenType.Type, int>(TokenType.Type.KeywordIf, 0));
                                break;
                            case "for":
                                tokens_thread.Add(new Tuple<TokenType.Type, int>(TokenType.Type.KeywordFor, 0));
                                break;
                            case "while":
                                tokens_thread.Add(new Tuple<TokenType.Type, int>(TokenType.Type.KeywordWhile, 0));
                                break;
                            default:
                                int tmp_identidier = 0;
                                identidiers_table.TryGetValue(buffer, out tmp_identidier);

                                if (tmp_identidier == 0)
                                {
                                    identidiers_table.Add(buffer, identifier_increment);
                                    tmp_identidier = identifier_increment;
                                    identifier_increment++;
                                }

                                tokens_thread.Add(new Tuple<TokenType.Type, int>(TokenType.Type.Identifier, tmp_identidier));
                                break;
                        }
                        buffer = "";
                        curent_state = State.None;
                        i--;
                    }
                }

                else if (curent_state == State.Operation)
                {
                    if (IsOperator(character))
                    {
                        buffer += character;
                    }

                    switch (buffer)
                    {
                        case "=":
                            tokens_thread.Add(new Tuple<TokenType.Type, int>(TokenType.Type.OperatorAssign, 0));
                            break;
                        case "+":
                            tokens_thread.Add(new Tuple<TokenType.Type, int>(TokenType.Type.OperatorAddition, 0));
                            break;
                        case "-":
                            tokens_thread.Add(new Tuple<TokenType.Type, int>(TokenType.Type.OperatorSubtraction, 0));
                            break;
                        case "*":
                            tokens_thread.Add(new Tuple<TokenType.Type, int>(TokenType.Type.OperatorMultiplication, 0));
                            break;
                        case "/":
                            tokens_thread.Add(new Tuple<TokenType.Type, int>(TokenType.Type.OperatorDivision, 0));
                            break;
                        case "++":
                            tokens_thread.Add(new Tuple<TokenType.Type, int>(TokenType.Type.OperatorIncrement, 0));
                            break;
                        case "--":
                            tokens_thread.Add(new Tuple<TokenType.Type, int>(TokenType.Type.OperatorDecrement, 0));
                            break;
                        case "+=":
                            tokens_thread.Add(new Tuple<TokenType.Type, int>(TokenType.Type.OperatorAdditionAssign, 0));
                            break;
                        case "-=":
                            tokens_thread.Add(new Tuple<TokenType.Type, int>(TokenType.Type.OperatorSubtractionAssign, 0));
                            break;
                        case "*=":
                            tokens_thread.Add(new Tuple<TokenType.Type, int>(TokenType.Type.OperatorMultiplicationAssign, 0));
                            break;
                        case "/=":
                            tokens_thread.Add(new Tuple<TokenType.Type, int>(TokenType.Type.OperatorDivisionAssign, 0));
                            break;
                        case "==":
                            tokens_thread.Add(new Tuple<TokenType.Type, int>(TokenType.Type.ComparisonEqual, 0));
                            break;
                        case "!=":
                            tokens_thread.Add(new Tuple<TokenType.Type, int>(TokenType.Type.ComparisonNotEqual, 0));
                            break;
                        case "<":
                            tokens_thread.Add(new Tuple<TokenType.Type, int>(TokenType.Type.ComparisonLess, 0));
                            break;
                        case "<=":
                            tokens_thread.Add(new Tuple<TokenType.Type, int>(TokenType.Type.ComparisonLessEqual, 0));
                            break;
                        case ">":
                            tokens_thread.Add(new Tuple<TokenType.Type, int>(TokenType.Type.ComparisonGreater, 0));
                            break;
                        case ">=":
                            tokens_thread.Add(new Tuple<TokenType.Type, int>(TokenType.Type.ComparisonGreaterEqual, 0));
                            break;
                    }
                    buffer = "";
                    curent_state = State.None;
                    i--;
                }

                else if (curent_state == State.None)
                {
                    if (Char.IsDigit(character))
                    {
                        curent_state = State.Number;
                        buffer += character;
                    }
                    else if (Char.IsLetter(character))
                    {
                        curent_state = State.Word;
                        buffer += character;
                    }
                    else if (IsOperator(character))
                    {
                        curent_state = State.Operation;
                        buffer += character;
                    }

                    else if (character == ';')
                    {
                        tokens_thread.Add(new Tuple<TokenType.Type, int>(TokenType.Type.Semicolon, 0));
                        buffer = "";
                    }
                    else if (character == '(')
                    {
                        tokens_thread.Add(new Tuple<TokenType.Type, int>(TokenType.Type.BracketOpen, 0));
                        buffer = "";
                    }
                    else if (character == ')')
                    {
                        tokens_thread.Add(new Tuple<TokenType.Type, int>(TokenType.Type.BracketClose, 0));
                        buffer = "";
                    }
                    else if (character == '{')
                    {
                        tokens_thread.Add(new Tuple<TokenType.Type, int>(TokenType.Type.BraceOpen, 0));
                        buffer = "";
                    }
                    else if (character == '}')
                    {
                        tokens_thread.Add(new Tuple<TokenType.Type, int>(TokenType.Type.BraceClose, 0));
                        buffer = "";
                    }

                    else if (Char.IsSeparator(character))
                    {
                    }
                    else if (character == '\t')
                    {
                    }

                    else
                    {
                        tokens_thread.Add(new Tuple<TokenType.Type, int>(TokenType.Type.Error, 0));
                        check_error = true;
                    }
                }
            }

            var reversed_identidiers_table = identidiers_table.ToDictionary(x => x.Value, x => x.Key);
            return new Tuple<List<Tuple<TokenType.Type, int>>, Dictionary<int, string>>(tokens_thread, reversed_identidiers_table);
        }
    }
}
