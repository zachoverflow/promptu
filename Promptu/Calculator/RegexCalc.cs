using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ZachJohnson.Promptu.Calculator
{
    internal static class RegexCalc
    {
        private const string number = "(\\-?(?:\\d+\\.?\\d*|\\d*\\.?\\d+))";
        private const string oneParamFunction = "([A-Za-z]+)";
        private const string constants = "(pi|e)";
        private static Regex powerRegex = new Regex(number + "\\s*(\\^)\\s*" + number);
        private static Regex addSubtractRegex = new Regex(number + "\\s*([-+])\\s*" + number);
        private static Regex multiplyDivideModulusRegex = new Regex(number + "\\s*([*/\\%])\\s*" + number);
        private static Regex oneParamFunctionClosedRegex = new Regex("(?<prev>[^A-Za-z]?)\\s*" + oneParamFunction + "\\(\\s*" + number + "\\s*(?:\\))?\\s*(?<next>(?<=\\)\\s*).?)", RegexOptions.IgnoreCase);
        private static Regex oneParamFunctionOpenRegex = new Regex("(?<prev>[^A-Za-z]?)\\s*" + oneParamFunction + "\\(\\s*" + number + "\\s*", RegexOptions.IgnoreCase);
        private static Regex sign1Regex = new Regex("([-+/*^])\\s*\\+");
        private static Regex sign3Regex = new Regex("\\+\\s*([-])");
        private static Regex sign2Regex = new Regex("\\-\\s*\\-");
        private static Regex parenthesesRegex = new Regex("(?<prev>.?)\\s*(?<![A-Za-z])\\(\\s*([-+]?\\d+.?\\d*)\\d*\\)\\s*(?<next>.?)");
        private static Regex openParenthesesRegex = new Regex("(?<prev>.?)\\s*(?<![A-Za-z])\\(\\s*([-+]?\\d+.?\\d*)\\d*$");
        private static Regex isNumberRegex = new Regex("^\\s*[-+]?\\d+\\.?\\d*\\s*$");
        private static Regex constantRegex = new Regex("(?<prev>[^A-Za-z]?)\\s*(?<![A-Za-z])" + constants + "(?![A-Za-z])\\s*(?<next>[^A-Za-z]?)", RegexOptions.IgnoreCase);

        public static double Evaluate(string expression)
        {
            expression = constantRegex.Replace(expression, new MatchEvaluator(DoConstants));

            while (!isNumberRegex.IsMatch(expression))
            {
                string oldExpression = expression;

                while (powerRegex.IsMatch(expression))
                {
                    expression = powerRegex.Replace(expression, new MatchEvaluator(DoPower));
                }

                while (multiplyDivideModulusRegex.IsMatch(expression))
                {
                    expression = multiplyDivideModulusRegex.Replace(expression, new MatchEvaluator(DoMultiplyDivideModulus));
                }

                while (oneParamFunctionClosedRegex.IsMatch(expression))
                {
                    expression = oneParamFunctionClosedRegex.Replace(expression, new MatchEvaluator(DoOneParamFunction));
                }

                expression = sign1Regex.Replace(expression, "$1");
                expression = sign3Regex.Replace(expression, "$1");
                expression = sign2Regex.Replace(expression, "+");

                while (addSubtractRegex.IsMatch(expression))
                {
                    expression = addSubtractRegex.Replace(expression, new MatchEvaluator(DoAddSubtract));
                    System.Diagnostics.Debug.WriteLine(expression);
                }

                expression = parenthesesRegex.Replace(expression, new MatchEvaluator(DoParentheses));

                if (expression == oldExpression)
                {
                    while (oneParamFunctionOpenRegex.IsMatch(expression))
                    {
                        expression = oneParamFunctionOpenRegex.Replace(expression, new MatchEvaluator(DoOneParamFunction));
                    }

                    expression = openParenthesesRegex.Replace(expression, new MatchEvaluator(DoParentheses));

                    if (expression == oldExpression)
                    {
                        throw new ConversionException("Invalid syntax error");
                    }
                }
            }

            return Double.Parse(expression);
        }

        private static string DoPower(Match m)
        {
            double first = Double.Parse(m.Groups[1].Value);
            double second = Double.Parse(m.Groups[3].Value);

            return Math.Pow(first, second).ToString("F15");
        }

        private static string DoMultiplyDivideModulus(Match m)
        {
            double first = Double.Parse(m.Groups[1].Value);
            double second = Double.Parse(m.Groups[3].Value);

            string op = m.Groups[2].Value;
            switch (op)
            {
                case "/":
                    if (second == 0)
                    {
                        throw new ConversionException("Error: division by zero");
                    }

                    return (first / second).ToString("F15");
                case "*":
                    return (first * second).ToString("F15");
                case "%":
                    return (first % second).ToString("F15");
                default:
                    throw new ConversionException(String.Format("Unknown operator: '{0}'", op));
            }
        }

        private static string DoOneParamFunction(Match m)
        {
            double number = Double.Parse(m.Groups[2].Value);

            string functionName = m.Groups[1].ToString();
            // (log|ln|lg|abs|sqr|sqrt|sin|cos|tan|csc|sec|cot|arcsin|arctan|arccos|arcsec|arccsc|arccot|asin|atan|acos|asec|acsc|acot)
            double result;
            switch (functionName.ToUpperInvariant())
            {
                case "LOG":
                    result = Math.Log10(number);
                    break;
                case "LN":
                    result = Math.Log(number, Math.E);
                    break;
                case "LG":
                    result = Math.Log(number, 2);
                    break;
                case "ABS":
                    result = Math.Abs(number);
                    break;
                case "SQR":
                case "SQRT":
                    result = Math.Sqrt(number);
                    break;
                case "SIN":
                    result = Math.Sin(number);
                    break;
                case "COS":
                    result = Math.Cos(number);
                    break;
                case "TAN":
                    result = Math.Tan(number);
                    break;
                case "CSC":
                    result = 1 / Math.Sin(number);
                    break;
                case "SEC":
                    result = 1 / Math.Cos(number);
                    break;
                case "COT":
                    result = 1 / Math.Tan(number);
                    break;
                case "ASIN":
                case "ARCSIN":
                    if (number < -1 || number > 1)
                    {
                        throw new ConversionException(String.Format("'{0}' can only take a value between -1 and 1", functionName));
                    }

                    result = Math.Asin(number);
                    break;
                case "ACOS":
                case "ARCCOS":
                    if (number < -1 || number > 1)
                    {
                        throw new ConversionException(String.Format("'{0}' can only take a value between -1 and 1", functionName));
                    }

                    result = Math.Acos(number);
                    break;
                case "ATAN":
                case "ARCTAN":
                    result = Math.Atan(number);
                    break;
                case "ACSC":
                case "ARCCSC":
                    if (number == 0)
                    {
                        throw new ConversionException(String.Format("Error: '{0}' cannot take an argument of zero", functionName));
                    }

                    result = Math.Asin(1 / number);
                    break;
                case "ASEC":
                case "ARCSEC":
                    if (number == 0)
                    {
                        throw new ConversionException(String.Format("Error: '{0}' cannot take an argument of zero", functionName));
                    }

                    result = Math.Acos(1 / number);
                    break;
                case "ACOT":
                case "ARCCOT":
                    if (number == 0)
                    {
                        throw new ConversionException(String.Format("Error: '{0}' cannot take an argument of zero", functionName));
                    }

                    result = Math.Atan(1 / number);
                    break;
                default:
                    throw new ConversionException(String.Format("Unknown math function: '{0}'", functionName));
            }

            return AddMultiplyOperatorAsNecessary(result.ToString("F15"), m);
        }

        private static string AddMultiplyOperatorAsNecessary(string returnValue, Match m)
        {
            string prev = m.Groups["prev"].Value;
            if (prev.Length > 0)
            {
                switch (prev[0])
                {
                    case '+':
                    case '-':
                    case '*':
                    case '/':
                    case '(':
                    case '^':
                    case '%':
                        break;
                    default:
                        returnValue = "*" + returnValue;
                        break;
                }

                returnValue = prev + returnValue;
            }

            string next = m.Groups["next"].Value;
            if (next.Length > 0)
            {
                switch (next[0])
                {
                    case '+':
                    case '-':
                    case '*':
                    case '/':
                    case ')':
                    case '^':
                    case '%':
                        break;
                    default:
                        returnValue += "*";
                        break;
                }

                returnValue += next;
            }

            return returnValue;
        }

        private static string DoParentheses(Match m)
        {
            //string returnValue = ;

            return AddMultiplyOperatorAsNecessary(m.Groups[1].Value, m);

            //string prev = m.Groups["prev"].Value;
            //if (prev.Length > 0)
            //{
            //    switch (prev[0])
            //    {
            //        case '+':
            //        case '-':
            //        case '*':
            //        case '/':
            //            break;
            //        default:
            //            returnValue = "*" + returnValue;
            //            break;
            //    }

            //    returnValue = prev + returnValue;
            //}

            //string next = m.Groups["next"].Value;
            //if (next.Length > 0)
            //{
            //    switch (next[0])
            //    {
            //        case '+':
            //        case '-':
            //        case '*':
            //        case '/':
            //            break;
            //        default:
            //            returnValue += "*";
            //            break;
            //    }

            //    returnValue += next;
            //}

            //return returnValue;
        }

        private static string DoAddSubtract(Match m)
        {
            double first = Double.Parse(m.Groups[1].Value);
            double second = Double.Parse(m.Groups[3].Value);

            double result;

            string op = m.Groups[2].Value;
            switch (op)
            {
                case "+":
                    result = (first + second);
                    break;
                case "-":
                    result = (first - second);
                    break;
                default:
                    throw new ConversionException(String.Format("Unknown operator: '{0}'", op));
            }

            if (first < 0)
            {
                return "+" + result.ToString("F15");
            }
            else
            {
                return result.ToString("F15");
            }
        }

        private static string DoConstants(Match m)
        {
            string constantName = m.Groups[1].Value;

            string returnValue;

            switch (constantName.ToUpperInvariant())
            {
                case "PI":
                    returnValue = Math.PI.ToString("F15");
                    break;
                case "E":
                    returnValue = Math.E.ToString("F15");
                    break;
                default:
                    throw new ConversionException(String.Format("Unknown constant: '{0}'", constantName));
            }

            return AddMultiplyOperatorAsNecessary(returnValue, m);
        }
    }
}
