// Copyright 2022 Zach Johnson
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace ZachJohnson.Promptu.Itl
{
    using System;
    using System.IO;
    using ZachJohnson.Promptu.Itl.AbstractSyntaxTree;

    internal class ItlCompiler
    {
        public ItlCompiler()
        {
        }

        public Expression Compile(ItlType type, string text, out FeedbackCollection feedback)
        {
            return this.Compile(type, text, false, out feedback);
        }

        public Expression Compile(ItlType type, string text, bool allowUnterminatedFunctionCalls, out FeedbackCollection feedback)
        {
            bool isJustText;
            return this.Compile(type, text, allowUnterminatedFunctionCalls, out feedback, out isJustText);
        }

        public Expression Compile(ItlType type, string text, out FeedbackCollection feedback, out bool isJustText)
        {
            return this.Compile(type, text, false, out feedback, out isJustText);
        }

        public Expression Compile(ItlType type, string text, bool allowUnterminatedFunctionCalls, out FeedbackCollection feedback, out bool isJustText)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            feedback = new FeedbackCollection();

            IItlScanner scanner;

            if (type == ItlType.InlineExecution)
            {
                scanner = new InlineItlScanner(new StringReader(text), feedback, type == ItlType.SingleFunction);
            }
            else
            {
                scanner = new StandardItlScanner(new StringReader(text), feedback, type == ItlType.SingleFunction);
            }

            isJustText = scanner.HasJustText;

            ItlParser parser = new ItlParser(scanner.Results, feedback, text.Length, allowUnterminatedFunctionCalls);

            Expression expression = null;

            try
            {
                expression = parser.Parse();
            }
            catch (ParseException ex)
            {
                feedback.AddError(ex.Message, text.Length - 1, 0, true);
            }

            return expression;
        }
    }
}
