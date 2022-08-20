using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.Itl.AbstractSyntaxTree;

namespace ZachJohnson.Promptu.Itl
{
    class Program
    {
        static void Main(string[] args)
        {
            string test = "hello<\"\\h\">";
            Console.WriteLine(test);
            ItlScanner scanner = new ItlScanner(new System.IO.StringReader(test), new FeedbackCollection());
            ItlParser parser = new ItlParser(scanner.Results, scanner.Feedback, test.Length);
            Console.WriteLine("Results:");

            //foreach (ScanToken token in scanner.Results)
            //{
            //    Console.WriteLine("{0} value: {1}", token.Value.GetType().Name, token.Value.ToString());
            //}
            try
            {
                WriteAST(parser.Parse(), string.Empty);
            }
            catch (ParseException ex)
            {
                scanner.Feedback.AddError(ex.Message, test.Length - 1, 0, true);
            }

            Console.WriteLine();

            foreach (FeedbackMessage message in scanner.Feedback)
            {
                Console.WriteLine("{0}: {1}", message.MessageType, message.Description);
            }

            Console.ReadKey(true);
        }

        static void WriteAST(Expression expression, string tabs)
        {
            if (expression == null)
            {
                Console.WriteLine(tabs + "null");
                return;
            }

            string childTabs = tabs + '\t';
            Console.WriteLine(tabs + expression.GetType().Name);
            if (expression is ExpressionGroup)
            {
                Console.WriteLine(tabs + "Expressions:");
                foreach (Expression innerExpression in ((ExpressionGroup)expression).Expressions)
                {
                    WriteAST(innerExpression, childTabs);
                }
                Console.WriteLine(tabs + "End Expressions");
            }
            else if (expression is Function)
            {
                Function function = expression as Function;
                Console.WriteLine(tabs + "Identifier: {0}", function.Identifier.Name);

                Console.WriteLine(tabs + "Parameters:");
                foreach (Expression parameter in function.Parameters)
                {
                    WriteAST(parameter, childTabs);
                }
                Console.WriteLine(tabs + "End Parameters");
            }
            else if (expression is ArgumentSubstitution)
            {
                ArgumentSubstitution subsitution = expression as ArgumentSubstitution;

                Console.WriteLine(tabs + "Singular: {0}", subsitution.SingularSubstitution);
                Console.WriteLine(tabs + "Arg: {0}", subsitution.ArgumentNumber);
                Console.WriteLine(tabs + "LastArg: {0}", subsitution.LastArgumentNumber);

                OptionalSubsitution optional = subsitution as OptionalSubsitution;

                if (optional != null)
                {
                    Console.WriteLine(tabs + "Default:");
                    WriteAST(optional.DefaultValue, childTabs);
                    Console.WriteLine(tabs + "End Default");
                }
            }
            else if (expression is StringLiteral)
            {
                Console.WriteLine(tabs + "Value: {0}", ((StringLiteral)expression).Value);
            }
        }
    }
}
