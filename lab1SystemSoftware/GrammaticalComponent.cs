using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab1SystemSoftware
{
    public class GrammaticalComponent
    {
        public enum Component
        {
            empty_set = 1,
            stmts = 2,
            identifier = 3,
            value = 4,
            condition = 5,
            alternate = 6
        }

        public enum State
        {
            none = 0,
            stmts = 1,
            expression = 2,
            wait_expression = 3,
            wait_condition = 4
        }
    }
}
