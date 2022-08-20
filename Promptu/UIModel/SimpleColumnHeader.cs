using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu.UIModel
{
    internal delegate object CellValueGetter(object context);

    //internal class SimpleColumnHeader
    //{
    //    private string text;
    //    private CellValueGetter cellValueGetter;

    //    public SimpleColumnHeader(string text, CellValueGetter cellValueGetter)
    //    {
    //        if (cellValueGetter == null)
    //        {
    //            throw new ArgumentNullException("cellValueGetter");
    //        }

    //        this.text = text;
    //        this.cellValueGetter = cellValueGetter;
    //    }

    //    public string Text
    //    {
    //        get { return this.text; }
    //    }

    //    public CellValueGetter CellValueGetter
    //    {
    //        get { return this.cellValueGetter; }
    //    }
    //}
}
