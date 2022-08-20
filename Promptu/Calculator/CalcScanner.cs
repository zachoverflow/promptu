using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.IO.Extensions;

namespace ZachJohnson.Promptu.Calculator
{
    internal class CalcScanner
    {
        private FeedbackCollection feedback;
        private List<CalcScanToken> results;

        public CalcScanner(StringReader input, FeedbackCollection feedback)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }

            this.results = new List<CalcScanToken>();

            if (feedback != null)
            {
                this.feedback = feedback;
            }
            else
            {
                this.feedback = new FeedbackCollection();
            }

            this.Scan(input);
        }

        public List<CalcScanToken> Results
        {
            get { return this.results; }
        }

        public FeedbackCollection Feedback
        {
            get { return this.feedback; }
        }

        private void Scan(StringReader input)
        {
            while (input.Peek() != -1)
            {
                int initialPosition = input.GetPosition();
                char character = (char)input.Peek();

                if (char.IsWhiteSpace(character))
                {
                    input.Read();
                }
                else if (char.IsLetter(character))
                {
                    int position = input.GetPosition();
                    StringBuilder accumulation = new StringBuilder();

                    while (char.IsLetter(character))
                    {
                        accumulation.Append(character);
                        input.Read();

                        int next = input.Peek();
                        if (next == -1)
                        {
                            break;
                        }
                        else
                        {
                            character = (char)next;
                        }
                    }

                    this.results.Add(new CalcScanToken(accumulation.ToString(), position, input.GetPosition()));
                }
                else if (char.IsDigit(character))
                {
                    int position = input.GetPosition();
                    StringBuilder accumulation = new StringBuilder();

                    while (char.IsDigit(character))
                    {
                        accumulation.Append(character);
                        input.Read();

                        int next = input.Peek();
                        if (next == -1)
                        {
                            break;
                        }
                        else
                        {
                            character = (char)next;
                        }
                    }

                    double? value = null;

                    //try
                    //{
                    value = Double.Parse(accumulation.ToString());
                    //}
                    //catch (OverflowException)
                    //{
                    //    feedback.AddError("Value is too big for a double", position, accumulation.Length, true);
                    //}

                    if (value != null)
                    {
                        this.results.Add(new CalcScanToken(value.Value, position, input.GetPosition()));
                    }
                }
                else
                {
                    input.Read();
                    int position = input.GetPosition() - 1;
                    switch (character)
                    {
                        case '(':
                            this.results.Add(new CalcScanToken(CalcScanTokenLiteral.OpenParantheses, position));
                            break;
                        case ')':
                            this.results.Add(new CalcScanToken(CalcScanTokenLiteral.CloseParantheses, position));
                            break;
                        default:
                            if (AbstractSyntaxTree.Operation.IsValidOperator(character))
                            {
                            }
                            else
                            {
                                this.feedback.AddError(
                                    String.Format("'{0}' not understood", character),
                                    position,
                                    1,
                                    true);
                            }

                            break;
                    }
                }

                if (input.Peek() == -1)
                {
                    //this.feedback.AddError("'>' expected");
                    break;
                }
            }
        }
    }
}
