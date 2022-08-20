//-----------------------------------------------------------------------
// <copyright file="InvisibleEndQuote.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.Skins
{
    using System;

    internal class InvisibleEndQuote
    {
        private int indexOfOpenQuote;
        private int numberOfSpacesInQuote;

        public InvisibleEndQuote(int indexOfOpenQuote, int numberOfSpacesInQuote)
        {
            this.numberOfSpacesInQuote = numberOfSpacesInQuote;
            this.indexOfOpenQuote = indexOfOpenQuote;
        }

        public int NumberOfSpacesInQuote
        {
            get { return this.numberOfSpacesInQuote; }
            set { this.numberOfSpacesInQuote = value; }
        }

        public int IndexOfOpenQuote
        {
            get { return this.indexOfOpenQuote; }
            set { this.indexOfOpenQuote = value; }
        }

        public int GetIndex(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }
            else if (this.indexOfOpenQuote >= text.Length)
            {
                throw new ArgumentOutOfRangeException("'IndexOfOpenQuote' cannot be greater than or equal to 'text.Length'.");
            }
            
            int terminatingIndex = text.Length;
            int spacesSeen = 0;

            for (int i = this.IndexOfOpenQuote; i < text.Length; i++)
            {
                if (text[i] == ' ')
                {
                    if (spacesSeen >= this.NumberOfSpacesInQuote)
                    {
                        terminatingIndex = i;
                        break;
                    }
                    else
                    {
                        spacesSeen++;
                    }
                }
            }

            return terminatingIndex;
        }
    }
}
