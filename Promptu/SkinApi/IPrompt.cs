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
