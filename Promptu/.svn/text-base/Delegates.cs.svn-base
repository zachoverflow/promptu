using System;
using System.Collections.Generic;
using System.Text;
//using ZachJohnson.Promptu.DynamicEntryModel;
using System.Text.RegularExpressions;
using ZachJohnson.Promptu.UIModel;
//using ZachJohnson.Promptu.DynamicEntryModel.Parsing;

namespace ZachJohnson.Promptu
{
    internal delegate string NameGetter<TObject>(TObject objectToOperateOn);

    //[Obsolete]
    //internal delegate DynamicEntryComponent MatchParser(ComplexMatch complexMatch, string text, out string textAfter);

    public delegate void ParameterlessVoid();

    public delegate T Getter<T>();

    public delegate void Setter<T>(T value);

    internal delegate bool LoopAction<T>(T item);

    internal delegate bool Question();

    public delegate ValueValidationResult Validator<T>(T value);
}
