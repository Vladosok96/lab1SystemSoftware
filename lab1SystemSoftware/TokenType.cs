namespace lab1SystemSoftware
{
    class TokenType
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
    }
}
