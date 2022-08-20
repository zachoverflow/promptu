//-----------------------------------------------------------------------
// <copyright file="StandardItlScanner.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.Itl
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.IO.Extensions;
    using System.Text;

    internal class StandardItlScanner : IItlScanner
    {
        private FeedbackCollection feedback;
        private List<ScanToken> results;
        private bool hasJustText = true;
        private bool singleFunction;

        public StandardItlScanner(StringReader input, FeedbackCollection feedback, bool singleFunction)
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

            this.singleFunction = singleFunction;

            this.Scan(input);
        }

        public List<ScanToken> Results
        {
            get { return this.results; }
        }

        public bool HasJustText
        {
            get { return this.hasJustText; }
        }

        public FeedbackCollection Feedback
        {
            get { return this.feedback; }
        }

        private void Scan(StringReader input)
        {
            int level = 0;
            while (input.Peek() != -1)
            {
                int initialPosition = input.GetPosition();
                char character = (char)input.Peek();
                if (character == '<' && (character = (char)input.Read()) == '<' && (input.Peek() != '<' || this.singleFunction))
                {
                    this.hasJustText = false;
                    this.results.Add(new ScanToken(ScanTokenLiteral.OpenAngleBracket, initialPosition));

                    if (input.Peek() == -1)
                    {
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

                            while (char.IsLetterOrDigit(character) || character == '_' || character == '.')
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
                            int position = input.GetPosition();
                            StringBuilder accumulation = new StringBuilder();
                            input.Read();

                            if (input.Peek() == -1)
                            {
                                this.feedback.AddError(
                                    Localization.ItlMessages.EndQuoteExpected, 
                                    position + 1, 
                                    0, 
                                    true);
                            }

                            while ((character = (char)input.Read()) != '"')
                            {
                                if (character == '\\')
                                {
                                    int next = input.Read();
                                    if (next == -1)
                                    {
                                        this.feedback.AddError(
                                            Localization.ItlMessages.CharacterExpected, 
                                            input.GetPosition(), 
                                            0, 
                                            true);

                                        break;
                                    }

                                    character = (char)next;

                                    switch (character)
                                    {
                                        case '"':
                                        case '\\':
                                            break;
                                        default:
                                            this.feedback.AddError(
                                                Localization.ItlMessages.UnrecognizedEscapeSequence, 
                                                input.GetPosition() - 1, 
                                                2, 
                                                true);

                                            continue;
                                    }
                                }

                                accumulation.Append(character);

                                if (input.Peek() == -1)
                                {
                                    this.feedback.AddError(
                                        Localization.ItlMessages.EndQuoteExpected, 
                                        position, 
                                        accumulation.Length, 
                                        true);

                                    break;
                                }
                            }

                            if (this.singleFunction && level == 0)
                            {
                                this.feedback.AddError(
                                    Localization.ItlMessages.StringLiteralNotExpected, 
                                    position, 
                                    input.GetPosition() - position, 
                                    true);
                            }
                            else
                            {
                                this.results.Add(new ScanToken(accumulation, position, input.GetPosition()));
                            }
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

                            int? value = null;

                            try
                            {
                                value = Int32.Parse(accumulation.ToString(), CultureInfo.InvariantCulture);
                            }
                            catch (OverflowException)
                            {
                                this.feedback.AddError(
                                    Localization.ItlMessages.Int32Overflow, 
                                    position, 
                                    accumulation.Length, 
                                    true);
                            }

                            if (value != null)
                            {
                                if (this.singleFunction && level == 0)
                                {
                                    this.feedback.AddError(
                                        String.Format(CultureInfo.CurrentCulture, Localization.ItlMessages.GeneralNotExpectedFormat, accumulation), 
                                        position, 
                                        input.GetPosition() - position, 
                                        true);
                                }
                                else
                                {
                                    this.results.Add(new ScanToken(value, position, input.GetPosition()));
                                }
                            }
                        }
                        else
                        {
                            input.Read();
                            int position = input.GetPosition() - 1;
                            switch (character)
                            {
                                case '!':
                                    if (this.singleFunction && level == 0)
                                    {
                                        this.feedback.AddError(
                                            String.Format(CultureInfo.CurrentCulture, Localization.ItlMessages.GeneralNotExpectedFormat, character), 
                                            position, 
                                            1, 
                                            true);
                                    }
                                    else
                                    {
                                        this.results.Add(new ScanToken(ScanTokenLiteral.ExclamationPoint, position));
                                    }

                                    break;
                                case '?':
                                    if (this.singleFunction && level == 0)
                                    {
                                        this.feedback.AddError(
                                            String.Format(CultureInfo.CurrentCulture, Localization.ItlMessages.GeneralNotExpectedFormat, character), 
                                            position, 
                                            1, 
                                            true);
                                    }
                                    else
                                    {
                                        this.results.Add(new ScanToken(ScanTokenLiteral.QuestionMark, position));
                                    }

                                    break;
                                case '-':
                                    if (this.singleFunction && level == 0)
                                    {
                                        this.feedback.AddError(
                                            String.Format(CultureInfo.CurrentCulture, Localization.ItlMessages.GeneralNotExpectedFormat, character), 
                                            position, 
                                            1, 
                                            true);
                                    }
                                    else
                                    {
                                        this.results.Add(new ScanToken(ScanTokenLiteral.Hyphen, position));
                                    }

                                    break;
                                case ':':
                                    if (this.singleFunction && level == 0)
                                    {
                                        this.feedback.AddError(
                                            String.Format(CultureInfo.CurrentCulture, Localization.ItlMessages.GeneralNotExpectedFormat, character), 
                                            position, 
                                            1, 
                                            true);
                                    }
                                    else
                                    {
                                        this.results.Add(new ScanToken(ScanTokenLiteral.Colon, position));
                                    }

                                    break;
                                case '(':
                                    this.results.Add(new ScanToken(ScanTokenLiteral.OpenParantheses, position));
                                    level++;
                                    break;
                                case ')':
                                    this.results.Add(new ScanToken(ScanTokenLiteral.CloseParantheses, position));
                                    level--;
                                    break;
                                case ',':
                                    if (this.singleFunction && level == 0)
                                    {
                                        this.feedback.AddError(
                                            String.Format(CultureInfo.CurrentCulture, Localization.ItlMessages.GeneralNotExpectedFormat, character), 
                                            position, 
                                            1,
                                            true);
                                    }
                                    else
                                    {
                                        this.results.Add(new ScanToken(ScanTokenLiteral.Comma, position));
                                    }

                                    break;
                                default:
                                    this.feedback.AddError(
                                        String.Format(CultureInfo.CurrentCulture, Localization.ItlMessages.NotUnderstoodFormat, character),
                                        position, 
                                        1, 
                                        true);
                                    break;
                            }
                        }

                        if (input.Peek() == -1)
                        {
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

                    if (this.singleFunction && level == 0)
                    {
                        this.feedback.AddError(
                            String.Format(CultureInfo.CurrentCulture, Localization.ItlMessages.GeneralNotExpectedFormat, accumulation),
                            position, 
                            input.GetPosition() - position, 
                            true);
                    }
                    else
                    {
                        this.results.Add(new ScanToken(accumulation, position, input.GetPosition()));
                    }
                }
            }
        }
    }
}