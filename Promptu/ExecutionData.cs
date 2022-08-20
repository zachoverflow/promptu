using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.Collections;
using ZachJohnson.Promptu.UserModel;
using ZachJohnson.Promptu.UserModel.Collections;

namespace ZachJohnson.Promptu
{
    internal class ExecutionData
    {
        private string[] arguments;
        private List listCommandBelongsTo;
        private FunctionCollectionComposite prioritizedCompositeFunctions;

        public ExecutionData(string[] arguments, List listCommandBelongsTo, ListCollection allLists)
        {
            this.arguments = arguments;
            this.listCommandBelongsTo = listCommandBelongsTo;
            this.prioritizedCompositeFunctions = new FunctionCollectionComposite(allLists, listCommandBelongsTo);
        }

        public string[] Arguments
        {
            get { return this.arguments; }
        }

        public List ListCommandBelongsTo
        {
            get { return this.listCommandBelongsTo; }
        }

        public FunctionCollectionComposite PrioritizedCompositeFunctions
        {
            get { return this.prioritizedCompositeFunctions; }
        }
    }
}
