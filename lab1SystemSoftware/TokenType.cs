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
            KeywordFor = 45,
            KeywordWhile = 46,

            Error = 50,
            None = 51
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

        public static bool isComparisionType(Type type)
        {
            if (type == Type.ComparisonEqual) return true;
            if (type == Type.ComparisonNotEqual) return true;
            if (type == Type.ComparisonGreater) return true;
            if (type == Type.ComparisonGreaterEqual) return true;
            if (type == Type.ComparisonLess) return true;
            if (type == Type.ComparisonLessEqual) return true;
            
            return false;
        }

        public static bool isOperandType(Type type)
        {
            if (type == Type.Identifier) return true;
            if (type == Type.Number) return true;

            return false;
        }

    }
}
