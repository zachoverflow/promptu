//-----------------------------------------------------------------------
// <copyright file="InlineItlScanner.cs" company="ZachJohnson">
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

    internal class InlineItlScanner : IItlScanner
    {
        private FeedbackCollection feedback;
        private List<ScanToken> results;
        private bool hasJustText = true;
        private bool singleFunction;

        public InlineItlScanner(StringReader input, FeedbackCollection feedback, bool singleFunction)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }

            this.singleFunction = singleFunction;

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
                this.hasJustText = false;

                if (input.Peek() == -1)
                {
                    break;
                }

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
        }
    }
}

