using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu.UserModel.Differencing
{
    internal static class ValueComparisons
    {
        private static Comparison<string> stringComparison = new Comparison<string>(CompareStrings);
        private static Comparison<bool> booleanComparison = new Comparison<bool>(CompareBooleans);
        private static Comparison<int> int32Comparison = new Comparison<int>(CompareInt32s);
        private static Comparison<FunctionParameter> functionParameterComparison = new Comparison<FunctionParameter>(FunctionParameter.Compare);
        private static Comparison<CommandParameterMetaInfo> commandParameterMetaInfoComparison = new Comparison<CommandParameterMetaInfo>(CommandParameterMetaInfo.Compare);
        private static Comparison<ReturnValue> returnValueComparison = new Comparison<ReturnValue>(CompareReturnValues);
        private static Comparison<ValueListItem> valueListItemUIComparison = new Comparison<ValueListItem>(ValueListItem.CompareFromUI);

        public static Comparison<string> StringComparison
        {
            get { return stringComparison; }
        }

        public static Comparison<bool> BooleanComparison
        {
            get { return booleanComparison; }
        }

        public static Comparison<int> Int32Comparison
        {
            get { return int32Comparison; }
        }

        public static Comparison<ReturnValue> ReturnValueComparison
        {
            get { return returnValueComparison; }
        }

        public static Comparison<FunctionParameter> FunctionParameterComparison
        {
            get { return functionParameterComparison; }
        }

        public static Comparison<CommandParameterMetaInfo> CommandParameterMetaInfoComparison
        {
            get { return commandParameterMetaInfoComparison; }
        }

        public static Comparison<ValueListItem> ValueListItemUIComparison
        {
            get { return valueListItemUIComparison; }
        }

        private static int CompareStrings(string x, string y)
        {
            return String.Compare(x, y);
        }

        private static int CompareBooleans(bool x, bool y)
        {
            return x.CompareTo(y);
        }

        private static int CompareInt32s(int x, int y)
        {
            return x.CompareTo(y);
        }

        private static int CompareReturnValues(ReturnValue x, ReturnValue y)
        {
            return x.CompareTo(y);
        }
    }
}
