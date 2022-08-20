using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu.UserModel
{
    internal class SuggestionConfig
    {
        private KeySuggestionConfig spacebar;

        public SuggestionConfig(KeySuggestionConfig spacebar)
        {
            if (spacebar == null)
            {
                throw new ArgumentNullException("spacebar");
            }

            this.spacebar = spacebar;
        }

        public KeySuggestionConfig Spacebar
        {
            get { return this.spacebar; }
        }
    }
}
