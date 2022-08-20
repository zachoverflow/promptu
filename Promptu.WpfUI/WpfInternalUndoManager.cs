using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Windows.Controls;
using System.Windows;

namespace ZachJohnson.Promptu.WpfUI
{
    internal class WpfInternalUndoManager
    {
        private static readonly MethodInfo GetUndoManagerMethod;
        private static readonly MethodInfo PopUndoStackMethod;
        private static readonly PropertyInfo UndoCountProperty;
        private object undoManager;

        public WpfInternalUndoManager(object undoManager)
        {
            this.undoManager = undoManager;
        }

        static WpfInternalUndoManager()
        {
            Assembly asm = Assembly.GetAssembly(typeof(TextBox));
            Type undoManager = asm.GetType("MS.Internal.Documents.UndoManager");
            GetUndoManagerMethod = undoManager.GetMethod("GetUndoManager", BindingFlags.NonPublic | BindingFlags.Static);
            PopUndoStackMethod = undoManager.GetMethod("PopUndoStack", BindingFlags.NonPublic | BindingFlags.Instance);
            UndoCountProperty = undoManager.GetProperty("UndoCount", BindingFlags.NonPublic | BindingFlags.Instance);
        }

        public static WpfInternalUndoManager GetUndoManager(DependencyObject target)
        {
            return new WpfInternalUndoManager(GetUndoManagerMethod.Invoke(null, new object[] { target }));
        }

        public int UndoCount
        {
            get { return (int)UndoCountProperty.GetValue(this.undoManager, null); }
        }

        public void PopUndoStack()
        {
            if (this.UndoCount > 0)
            {
                PopUndoStackMethod.Invoke(this.undoManager, null);
            }
        }
    }
}
