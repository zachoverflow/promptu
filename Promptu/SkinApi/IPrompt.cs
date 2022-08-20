//-----------------------------------------------------------------------
// <copyright file="IPrompt.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.SkinApi
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    using ZachJohnson.Promptu.UIModel;

    public interface IPrompt : IUIElement, IHasOptionsAndProperties
    {
        event EventHandler<KeyPressedEventArgs> KeyPressed;

        event MouseEventHandler MouseWheel;

        event MouseEventHandler MouseDown;

        event MouseEventHandler MouseUp;

        event EventHandler LocationChanged;
        
        Point Location { get; set; }

        Size Size { get; set; }

        string Text { get; set; }

        int SelectionStart { get; set; }

        int SelectionLength { get; set; }

        UIContextMenu PromptContextMenu { get; }

        bool ContainsFocus { get; }

        bool IsCreated { get; }

        void Show();

        void Hide();

        void EnsureCreated();

        void Activate();

        void FocusOnTextInput();
    }
}
