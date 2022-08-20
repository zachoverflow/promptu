//using System;
//using System.Collections.Generic;
//using System.Text;
//using ZachJohnson.Promptu.Calculator.AbstractSyntaxTree;

//namespace ZachJohnson.Promptu.Calculator
//{
//    internal class CalcParser
//    {
//        private List<CalcScanToken> tokens;
//        private FeedbackCollection feedback;
//        private int index;

//        public CalcParser(List<CalcScanToken> tokens, FeedbackCollection feedback)
//        {
//            if (tokens == null)
//            {
//                throw new ArgumentNullException("tokens");
//            }

//            if (feedback != null)
//            {
//                this.feedback = feedback;
//            }
//            else
//            {
//                this.feedback = new FeedbackCollection();
//            }

//            this.tokens = tokens;
//        }

//        public FeedbackCollection Feedback
//        {
//            get { return this.feedback; }
//        }

//        public Expression Parse()
//        {
//            return this.ParseExpression();
//        }

//        private Expression ParseExpression()
//        {
//            Expression result;

//            if (this.index == this.tokens.Count)
//            {
//                throw new ParseException(Localization.ItlMessages.ExpressionExpected);
//            }

//            CalcScanToken currentToken = this.tokens[this.index];

//            if (currentToken.Value is double)
//            {
//                result = new Number((double)currentToken.Value);
//            }
//            else
//            {
//                throw new ParseException(Localization.ItlMessages.ExpressionExpected);
//            }

//            //else if (currentToken.Value is String)
//            //{
//            //    if (this.index + 1 < this.tokens.Count && this.tokens[this.index + 1].Value.Equals(CalcScanTokenLiteral.OpenParantheses))
//            //    {
//            //        this.index++;
//            //        if (MathFunction.IsValidMathFunctionName(currentToken.Value.ToString()))
//            //        {
//            //            result = new MathFunction(currentToken.Value.ToString(), this.ParseExpression());
//            //        }
//            //        else if (Constant.IsValidConstantName(currentToken.Value.ToString()))
//            //        {
//            //            this.index--;
//            //            result = new Operation('*', new Constant(currentToken.Value.ToString()), this.ParseExpression());
//            //        }
//            //    }
//            //    else if (Constant.IsValidConstantName(currentToken.Value.ToString()))
//            //    {
//            //        result = new MathFunction(
//            //    }
//            //}

//            return result;
//        }
//    }
//}
