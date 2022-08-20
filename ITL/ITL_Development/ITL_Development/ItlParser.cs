using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.Itl.AbstractSyntaxTree;

namespace ZachJohnson.Promptu.Itl
{
    internal class ItlParser
    {
        private static Predicate<ScanToken> CloseAngleBracketPredicate = new Predicate<ScanToken>(IsCloseAngleBracket);
        private static Predicate<ScanToken> OpenParenthesesPredicate = new Predicate<ScanToken>(IsOpenParentheses);
        private static Predicate<ScanToken> CloseParenthesesPredicate = new Predicate<ScanToken>(IsCloseParentheses);
        private static Predicate<ScanToken> CommaPredicate = new Predicate<ScanToken>(IsComma);
        private static Predicate<ScanToken> QuestionMarkPredicate = new Predicate<ScanToken>(IsQuestionMark);
        private List<ScanToken> tokens;
        private FeedbackCollection feedback;
        private int index;
        private int lengthOfInput;

        public ItlParser(List<ScanToken> tokens, FeedbackCollection feedback, int lengthOfInput)
        {
            if (tokens == null)
            {
                throw new ArgumentNullException("tokens");
            }

            if (feedback != null)
            {
                this.feedback = feedback;
            }
            else
            {
                this.feedback = new FeedbackCollection();
            }

            this.tokens = tokens;
            this.lengthOfInput = lengthOfInput;
        }

        public FeedbackCollection Feedback
        {
            get { return this.feedback; }
        }

        public Expression Parse()
        {
            return this.ParseExpressionGroup(this.tokens.Count);
        }

        private ExpressionGroup ParseExpressionGroup(int stopBeforeIndex)
        {
            ExpressionGroup group = new ExpressionGroup(new List<Expression>());
            while (this.index < stopBeforeIndex)
            {
                Expression expression = this.ParseExpression();
                if (expression != null)
                {
                    group.Expressions.Add(expression);
                }
            }

            return group;
        }

        private Expression ParseExpression()
        {
            Expression result;

            if (this.index == this.tokens.Count)
            {
               throw new ParseException("Expression expected");
            }

            ScanToken currentToken = this.tokens[this.index];

            if (currentToken.Value is StringBuilder)
            {
                this.index++;
                result = new StringLiteral(currentToken.Value.ToString());
            }
            else if (currentToken.Value.Equals(ScanTokenLiteral.OpenAngleBracket))
            {
                this.index++;
                int indexOfCloseAngleBracket = this.tokens.FindIndex(this.index, CloseAngleBracketPredicate);

                if (indexOfCloseAngleBracket == -1)
                {
                    this.feedback.AddError("'>' expected", this.lengthOfInput, 0, true);
                    indexOfCloseAngleBracket = this.tokens.Count;
                }

                result = this.ParseExpressionGroup(indexOfCloseAngleBracket);
                this.index++;
            }
            else if (currentToken.Value.Equals(ScanTokenLiteral.ExclamationPoint))
            {
                this.index++;
                ArgumentSubstitution substitution = this.ParseArgumentSubstitution(
                    ScanTokenLiteral.ExclamationPoint,
                    new Predicate<ScanToken>(delegate { return true; }));
                if (substitution != null)
                {
                    //this.index++;
                    return new ImperativeSubstitution(substitution.ArgumentNumber, substitution.LastArgumentNumber, substitution.SingularSubstitution);
                }
                else
                {
                    return null;
                }
            }
            else if (currentToken.Value.Equals(ScanTokenLiteral.QuestionMark))
            {
                this.index++;
                ArgumentSubstitution substitution = this.ParseArgumentSubstitution(
                    ScanTokenLiteral.QuestionMark,
                    new Predicate<ScanToken>(delegate (ScanToken token)
                        {
                            return !token.Value.Equals(ScanTokenLiteral.QuestionMark) && !token.Value.Equals(ScanTokenLiteral.Colon);
                        }));

                Expression defaultValue = null;

                if (this.index != this.tokens.Count)
                {
                    currentToken = this.tokens[this.index];

                    if (currentToken.Value.Equals(ScanTokenLiteral.Colon))
                    {
                        int indexOfColon = currentToken.Position;
                        this.index++;

                        if (this.index == this.tokens.Count)
                        {
                            substitution = null;
                            this.feedback.AddError("'?' expected", this.lengthOfInput, 0, true);
                        }
                        else
                        {
                            currentToken = this.tokens[this.index];
                            this.index++;
                            bool checkToken = true;
                            if (currentToken.Value.Equals(ScanTokenLiteral.OpenParantheses))
                            {
                                int indexOfCloseParentheses = this.FindIndexOfToken(CloseParenthesesPredicate, OpenParenthesesPredicate);

                                if (indexOfCloseParentheses == -1)
                                {
                                    substitution = null;
                                    this.feedback.AddError("')' expected", this.lengthOfInput, 0, true);
                                    indexOfCloseParentheses = this.tokens.Count;
                                }

                                defaultValue = this.ParseExpressionGroup(indexOfCloseParentheses);

                                //this.index++;
                            }
                            else if (currentToken.Value is StringBuilder)
                            {
                                defaultValue = new StringLiteral(currentToken.Value.ToString());
                            }
                            else if (currentToken.Value.Equals(ScanTokenLiteral.QuestionMark))
                            {
                                substitution = null;
                                this.feedback.AddError("Expression expected", indexOfColon + 1, 0, true);
                                checkToken = false;
                            }
                            else
                            {
                                substitution = null;
                                this.feedback.AddError("Expression expected", indexOfColon + 1, 0, true);

                                while (true)
                                {
                                    //this.index++;
                                    if (this.index == this.tokens.Count)
                                    {
                                        this.feedback.AddError("'?' expected", this.lengthOfInput - 1, 0, true);
                                        break;
                                    }

                                    if (currentToken.Value.Equals(ScanTokenLiteral.QuestionMark))
                                    {
                                        checkToken = false;
                                        //this.index--;
                                        break;
                                    }

                                    this.feedback.AddError(
                                        String.Format("'{0}' not understood", currentToken.Value.ToString()), 
                                        currentToken.Position, 
                                        currentToken.EndPosition - currentToken.Position, 
                                        true);

                                    currentToken = this.tokens[this.index];

                                    this.index++;
                                }

                                //this.index--;

                                //int nextQuestionMarkIndex = this.FindFirstIndexOf(QuestionMarkPredicate);
                                //if (nextQuestionMarkIndex != -1)
                                //{
                                //    this.index = nextQuestionMarkIndex;z
                                //}
                                //else
                                //{
                                //    this.index++;
                                //}
                            }

                            this.index++;

                            if (checkToken && (this.index >= this.tokens.Count || !(currentToken = this.tokens[this.index - 1]).Value.Equals(ScanTokenLiteral.QuestionMark)))
                            {
                                substitution = null;
                                this.feedback.AddError("'?' expected", currentToken.Position, currentToken.EndPosition - currentToken.Position, true);
                            }
                        }
                    }

                    this.index++;
                }

                if (substitution != null)
                {
                    return new OptionalSubsitution(
                        substitution.ArgumentNumber, 
                        substitution.LastArgumentNumber, 
                        substitution.SingularSubstitution, 
                        defaultValue);
                }
                else
                {
                    return null;
                }
            }
            else if (currentToken.Value is string)
            {
                this.index++;
                Identifier identifier = new Identifier((string)currentToken.Value);
                List<Expression> parameters = new List<Expression>();

                if (this.index == this.tokens.Count)
                {
                    throw new ParseException("'(' and ')' expected");
                }

                currentToken = this.tokens[index];

                if (!currentToken.Value.Equals(ScanTokenLiteral.OpenParantheses))
                {
                    feedback.AddError("'(' expected", currentToken.Position, 0, true);
                }

                this.index++;

                if (this.index == this.tokens.Count)
                {
                    throw new ParseException("')' expected");
                }

                currentToken = this.tokens[this.index];

                int closeParentheses = this.FindIndexOfToken(CloseParenthesesPredicate, OpenParenthesesPredicate);

                if (currentToken.Value.Equals(ScanTokenLiteral.CloseParantheses))
                {
                    result = new Function(identifier, parameters);
                    this.index++;
                }
                else
                {
                    while (true)
                    {
                        ScanToken nextItemToken = null;
                        int nextComma = this.FindFirstIndexOf(CommaPredicate);

                        int indexOfNextItem;
                        if (closeParentheses != -1 && (closeParentheses < nextComma || nextComma == -1))
                        {
                            indexOfNextItem = closeParentheses;
                            nextItemToken = this.tokens[closeParentheses];
                        }
                        else if (nextComma != -1 && nextComma < closeParentheses)
                        {
                            indexOfNextItem = nextComma;
                            nextItemToken = this.tokens[nextComma];
                        }
                        else
                        {
                            indexOfNextItem = this.tokens.Count;
                        }

                        parameters.Add(this.ParseExpressionGroup(indexOfNextItem));

                        if (nextItemToken != null)
                        {
                            this.index = indexOfNextItem + 1;

                            if (nextItemToken.Value.Equals(ScanTokenLiteral.Comma))
                            {
                                if (this.index == this.tokens.Count)
                                {
                                    throw new ParseException("')' expected");
                                }
                                else
                                {
                                    currentToken = this.tokens[this.index];
                                    
                                    bool isCloseParentheses = currentToken.Value.Equals(ScanTokenLiteral.CloseParantheses);

                                    if (isCloseParentheses || currentToken.Value.Equals(ScanTokenLiteral.Comma))
                                    {
                                        result = null;
                                        this.feedback.AddError("Parameter missing", currentToken.Position - 1, 0, true);
                                        if (isCloseParentheses)
                                        {
                                            this.index++;
                                            break;
                                        }
                                    }
                                }
                            }
                            else if (nextItemToken.Value.Equals(ScanTokenLiteral.CloseParantheses))
                            {
                                result = new Function(identifier, parameters);
                                break;
                            }
                        }
                        else
                        {
                            throw new ParseException("')' expected");
                        }
                    }
                }
            }
            else
            {
                result = null;
                this.feedback.AddError(String.Format("'{0}' not understood", currentToken.Value.ToString()), currentToken.Position, currentToken.EndPosition - currentToken.Position, true);
                this.index++;
            }

            return result;
        }

        private static bool IsCloseAngleBracket(ScanToken token)
        {
            return token.Value.Equals(ScanTokenLiteral.CloseAngleBracket);
        }

        private static bool IsOpenParentheses(ScanToken token)
        {
            return token.Value.Equals(ScanTokenLiteral.OpenParantheses);
        }

        private static bool IsCloseParentheses(ScanToken token)
        {
            return token.Value.Equals(ScanTokenLiteral.CloseParantheses);
        }

        private static bool IsComma(ScanToken token)
        {
            return token.Value.Equals(ScanTokenLiteral.Comma);
        }

        private static bool IsQuestionMark(ScanToken token)
        {
            return token.Value.Equals(ScanTokenLiteral.QuestionMark);
        }

        private int FindFirstIndexOf(params Predicate<ScanToken>[] matches)
        {
            for (int i = this.index; i < this.tokens.Count; i++)
            {
                ScanToken token = this.tokens[i];
                foreach (Predicate<ScanToken> match in matches)
                {
                    if (match(token))
                    {
                        return i;
                    }
                }
            }

            return -1;
        }

        private int FindIndexOfToken(Predicate<ScanToken> match, Predicate<ScanToken> matchForEnclosingPair)
        {
            int enclosingPair = 0;
            bool isEnclosingPair = matchForEnclosingPair != null;

            for (int i = this.index; i < this.tokens.Count; i++)
            {
                ScanToken token = this.tokens[i];
                if (isEnclosingPair && matchForEnclosingPair(token))
                {
                    enclosingPair++;
                }
                else if (match(token))
                {
                    if (enclosingPair == 0)
                    {
                        return i;
                    }
                    else
                    {
                        enclosingPair--;
                    }
                }
            }

            return -1;
        }

        private ArgumentSubstitution ParseArgumentSubstitution(object endOfSubstitution, Predicate<ScanToken> substitutionBoundDelineator)
        {
            int? argumentIndex = null;
            int? lastArgumentIndex = null;
            bool singularSubstitution = true;
            bool valid = true;
            int indexOfBeginning = this.index - 1;

            if (this.index == this.tokens.Count)
            {
                throw new ParseException("Argument number or 'n' expected");
            }
            else
            {
                ScanToken currentToken = this.tokens[this.index];
                while (substitutionBoundDelineator(currentToken))
                {
                    int relativeIndex = this.index - indexOfBeginning;
                    switch (relativeIndex)
                    {
                        case 3:
                        case 1:
                            bool mustBeNumber = relativeIndex == 1
                                    && this.index + 1 < this.tokens.Count
                                    && this.tokens[this.index + 1].Value.Equals(ScanTokenLiteral.Hyphen);

                            string tokenValue = currentToken.Value as string;
                            if (tokenValue != null)
                            {
                                if (tokenValue.ToUpperInvariant() != "N")
                                {
                                    valid = false;

                                    if (mustBeNumber)
                                    {
                                        this.feedback.AddError("Argument number expected", currentToken.Position, currentToken.EndPosition - currentToken.Position, true);
                                    }
                                    else
                                    {
                                        this.feedback.AddError("Argument number or 'n' expected", currentToken.Position, currentToken.EndPosition - currentToken.Position, true);
                                    }
                                }
                                else
                                {
                                    if (mustBeNumber)
                                    {
                                        valid = false;
                                        this.feedback.AddError("The value before the hyphen cannot be 'n'", currentToken.Position, currentToken.EndPosition - currentToken.Position, true);
                                    }
                                    //if (relativeIndex == 3)
                                    //{
                                    //    if (valid && argumentIndex == null)
                                    //    {
                                    //        valid = false;
                                    //        this.feedback.AddError("The value before the hyphen cannot be 'n'");
                                    //    }
                                    //}
                                }

                            }
                            else if (currentToken.Value is int)
                            {
                                int intValue = (int)currentToken.Value;
                                if (intValue == 0)
                                {
                                    valid = false;
                                    this.feedback.AddError("Argument number cannot be zero", currentToken.Position, currentToken.EndPosition - currentToken.Position, true);
                                }
                                else
                                {
                                    if (relativeIndex == 1)
                                    {
                                        argumentIndex = intValue;
                                    }
                                    else
                                    {
                                        //if (valid && argumentIndex == null)
                                        //{
                                        //    valid = false;
                                        //    this.feedback.AddError("The value before the hyphen cannot be 'n'");
                                        //}

                                        lastArgumentIndex = intValue;
                                    }
                                }
                            }
                            else
                            {
                                valid = false;
                                if (mustBeNumber)
                                {
                                    this.feedback.AddError("Argument number expected", currentToken.Position, currentToken.EndPosition - currentToken.Position, true);
                                }
                                else
                                {
                                    this.feedback.AddError("Argument number or 'n' expected", currentToken.Position, currentToken.EndPosition - currentToken.Position, true);
                                }
                            }

                            break;
                        case 2:
                            if (currentToken.Value.Equals(ScanTokenLiteral.Hyphen))
                            {
                                singularSubstitution = false;
                            }
                            else if (!currentToken.Value.Equals(endOfSubstitution))
                            {
                                valid = false;
                                this.feedback.AddError("'-' expected", currentToken.Position, currentToken.EndPosition - currentToken.Position, true);
                            }

                            break;
                    }

                    if (currentToken.Value.Equals(endOfSubstitution))
                    {
                        break;
                    }
                    else if (relativeIndex == 4)
                    {
                        valid = false;
                        //if (onReachedMaxAndNotEndChar == null)
                        //{
                        //    this.feedback.AddError(String.Format("'{0}' expected", endOfSubstitution));
                        //}
                        //else
                        //{
                        //    onReachedMaxAndNotEndChar(currentToken);
                        //}

                        while (true)
                        {
                            if (this.index == this.tokens.Count)
                            {
                                throw new ParseException(String.Format("'{0}' expected", endOfSubstitution));
                            }

                            currentToken = this.tokens[this.index];

                            this.index++;

                            if (currentToken.Value.Equals(endOfSubstitution))
                            {
                                break;
                            }

                            this.feedback.AddError(String.Format("'{0}' not understood", currentToken.Value.ToString()), currentToken.Position, currentToken.EndPosition - currentToken.Position, true);
                        }

                        return null;
                    }
                    else
                    {
                        this.index++;
                    }

                    if (this.index == this.tokens.Count)
                    {
                        throw new ParseException(String.Format("'{0}' expected", endOfSubstitution));
                    }
                    else
                    {
                        currentToken = this.tokens[this.index];
                    }
                }
            }

            if (valid)
            {
                return new ArgumentSubstitution(argumentIndex, lastArgumentIndex, singularSubstitution);
            }
            else
            {
                return null;
            }
        }
    }
}
