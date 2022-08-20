using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu.UIModel
{
    internal class SkinInfo
    {
        private string name;
        private object image;
        private string creator;
        private string description;

        public SkinInfo(string name, object image, string creator, string description)
        {
            this.name = name;
            this.image = image;
            this.creator = creator;
            this.description = description;
        }

        public string Name
        {
            get { return this.name; }
        }

        public object Image
        {
            get { return this.image; }
        }

        public string Creator
        {
            get { return this.creator; }
        }

        public string Description
        {
            get { return this.description; }
        }
    }
}
