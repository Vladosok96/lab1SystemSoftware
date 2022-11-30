namespace lab1SystemSoftware
{
    public class TokenType
    {
        public enum Type
        {
            Identifier = 1,
            Number = 2,
            Semicolon = 3,

            OperatorAssign = 11,
            OperatorAddition = 12,
            OperatorSubtraction = 13,
            OperatorMultiplication = 14,
            OperatorDivision = 15,
            OperatorIncrement = 16,
            OperatorDecrement = 17,
            OperatorAdditionAssign = 121,
            OperatorSubtractionAssign = 131,
            OperatorMultiplicationAssign = 141,
            OperatorDivisionAssign = 151,

            BracketOpen = 21,
            BracketClose = 22,

            BraceOpen = 23,
            BraceClose = 24,

            ComparisonEqual = 31,
            ComparisonNotEqual = 32,
            ComparisonLess = 33,
            ComparisonLessEqual = 34,
            ComparisonGreater = 35,
            ComparisonGreaterEqual = 36,

            KeywordLet = 41,
            KeywordVar = 42,
            KeywordIf = 43,
            KeywordFor = 43,
            KeywordWhile = 45,

            Error = 50
        }

        public static bool isAssignType(Type type)
        {
            if (type == Type.OperatorAssign) return true;
            if (type == Type.OperatorAdditionAssign) return true;
            if (type == Type.OperatorSubtractionAssign) return true;
            if (type == Type.OperatorMultiplicationAssign) return true;
            if (type == Type.OperatorDivisionAssign) return true;
            return false;
        }

        public static bool isAryphmeticType(Type type)
        {
            if (type == Type.OperatorAddition) return true;
            if (type == Type.OperatorSubtraction) return true;
            if (type == Type.OperatorMultiplication) return true;
            if (type == Type.OperatorDivision) return true;
            return false;
        }

        //public const int Identifier = 1;
        //public const int Number = 2;
        //public const int Semicolon = 3;

        //public const int OperatorAssign = 11;
        //public const int OperatorAddition = 12;
        //public const int OperatorSubtraction = 13;
        //public const int OperatorMultiplication = 14;
        //public const int OperatorDivision = 15;
        //public const int OperatorIncrement = 16;
        //public const int OperatorDecrement = 17;
        //public const int OperatorAdditionAssign = 121;
        //public const int OperatorSubtractionAssign = 131;
        //public const int OperatorMultiplicationAssign = 141;
        //public const int OperatorDivisionAssign = 151;

        //public const int BracketOpen = 21;
        //public const int BracketClose = 22;

        //public const int BraceOpen = 23;
        //public const int BraceClose = 24;

        //public const int ComparisonEqual = 31;
        //public const int ComparisonNotEqual = 32;
        //public const int ComparisonLess = 33;
        //public const int ComparisonLessEqual = 34;
        //public const int ComparisonGreater = 35;
        //public const int ComparisonGreaterEqual = 36;

        //public const int KeywordLet = 41;
        //public const int KeywordVar = 42;
        //public const int KeywordIf = 43;
        //public const int KeywordFor = 43;
        //public const int KeywordWhile = 45;

        //public enum Operator
        //{
        //    Assign,
        //    Addition,
        //    Subtraction,
        //    Multiplication,
        //    Division
        //}

        //public enum Bracket
        //{
        //    OpenBracket,
        //    CloseBracket
        //}

        //public enum Brace
        //{
        //    OpenBrace,
        //    CloseBrace
        //}

        //public enum Comparison
        //{
        //    Equal,
        //    NotEqual,
        //    Less,
        //    LessEqual,
        //    Greater,
        //    GreaterEqual
        //}

        //public enum Keyword
        //{
        //    Let,
        //    Var,
        //    If,
        //    For,
        //    While
        //}
    }
}
