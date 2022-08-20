using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ZachJohnson.Promptu.Itl
{
    internal class ItlScanner
    {
        private FeedbackCollection feedback;
        private List<ScanToken> results;
        //private bool useGeekMessages;

        public ItlScanner(StringReader input, FeedbackCollection feedback)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }

            this.results = new List<ScanToken>();

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

        public List<ScanToken> Results
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
                if (character == '<' && (character = (char)input.Read()) == '<' && input.Peek() != '<')
                {
                    this.results.Add(new ScanToken(ScanTokenLiteral.OpenAngleBracket, initialPosition));

                    if (input.Peek() == -1)
                    {
                        //this.feedback.AddError("'>' expected");
                        break;
                    }

                    while ((character = (char)input.Peek()) != '>')
                    {
                        if (char.IsWhiteSpace(character))
                        {
                            input.Read();
                        }
                        else if (char.IsLetter(character) || character == '_')
                        {
                            int position = input.GetPosition();
                            StringBuilder accumulation = new StringBuilder();

                            while (char.IsLetterOrDigit(character) || character == '_')
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

                            this.results.Add(new ScanToken(accumulation.ToString(), position, input.GetPosition()));
                        }
                        else if (character == '"')
                        {
                            int postion = input.GetPosition();
                            StringBuilder accumulation = new StringBuilder();
                            input.Read();

                            if (input.Peek() == -1)
                            {
                                feedback.AddError("'\"' expected", postion + 1, 0, true);
                            }

                            while ((character = (char)input.Read()) != '"')
                            {
                                if (character == '\\')
                                {
                                    int next = input.Read();
                                    if (next == -1)
                                    {
                                        feedback.AddError("Character expected", input.GetPosition(), 0, true);
                                        break;
                                    }

                                    character = (char)next;

                                    switch (character)
                                    {
                                        case '"':
                                        case '\\':
                                            break;
                                        default:
                                            feedback.AddError("Unrecognized escape sequence", input.GetPosition() - 1, 2, true);
                                            continue;
                                    }
                                }

                                accumulation.Append(character);

                                if (input.Peek() == -1)
                                {
                                    feedback.AddError("'\"' expected", postion, accumulation.Length, true);
                                    break;
                                }
                            }

                            this.results.Add(new ScanToken(accumulation, postion, input.GetPosition()));
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

                            this.results.Add(new ScanToken(Int32.Parse(accumulation.ToString()), position, input.GetPosition()));
                        }
                        else
                        {
                            input.Read();
                            switch (character)
                            {
                                case '!':
                                    this.results.Add(new ScanToken(ScanTokenLiteral.ExclamationPoint, input.GetPosition()));
                                    break;
                                case '?':
                                    this.results.Add(new ScanToken(ScanTokenLiteral.QuestionMark, input.GetPosition()));
                                    break;
                                case '-':
                                    this.results.Add(new ScanToken(ScanTokenLiteral.Hyphen, input.GetPosition()));
                                    break;
                                case ':':
                                    this.results.Add(new ScanToken(ScanTokenLiteral.Colon, input.GetPosition()));
                                    break;
                                case '(':
                                    this.results.Add(new ScanToken(ScanTokenLiteral.OpenParantheses, input.GetPosition()));
                                    break;
                                case ')':
                                    this.results.Add(new ScanToken(ScanTokenLiteral.CloseParantheses, input.GetPosition()));
                                    break;
                                case ',':
                                    this.results.Add(new ScanToken(ScanTokenLiteral.Comma, input.GetPosition()));
                                    break;
                                default:
                                    this.feedback.AddError(String.Format("'{0}' not understood", character), input.GetPosition() - 1, 1, true);
                                    break;
                            }
                        }

                        if (input.Peek() == -1)
                        {
                            //this.feedback.AddError("'>' expected");
                            break;
                        }
                    }

                    if (character == '>')
                    {
                        this.results.Add(new ScanToken(ScanTokenLiteral.CloseAngleBracket, input.GetPosition()));
                        input.Read();
                    }
                }
                else
                {
                    int position = input.GetPosition();
                    StringBuilder accumulation = new StringBuilder();
                    accumulation.Append(character);
                    input.Read();

                    int nextChar = input.Peek();
                    if (nextChar != -1)
                    {
                        character = (char)nextChar;
                        while (character != '<')
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
                    }

                    this.results.Add(new ScanToken(accumulation, position, input.GetPosition()));
                }
            }
        }
    }
}

//do
//{
//    int next = input.Read();

//    if (next == -1)
//    {
//        //if (this.useGeekMessages)
//        //{
//        //    this.feedback.AddError("Unterminated imperative substitution.  Missing '!'");
//        //}

//        this.feedback.AddError("Missing '!'");
//        break;
//    }
//    else
//    {
//        character = (char)next;
//        if (character == '!')
//        {
//            this.results.Add(new ScanToken(ScanTokenLiteral.ExclamationPoint, input.GetPosition()));
//        }
//        else if (char.IsDigit(character))
//        {
//            StringBuilder builder 
//        }
//    }
//}
//while (character != '!');
