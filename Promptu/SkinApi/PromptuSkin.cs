//-----------------------------------------------------------------------
// <copyright file="PromptuSkin.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.SkinApi
{
    using System;

    public abstract class PromptuSkin
    {
        private string id;
        private string name;
        private string creator;
        private string creatorContact;
        private string description;
        private string imagePath;
        private ISkinInstanceFactory instanceFactory;

        internal PromptuSkin(
            string name,
            string id,
            string creator,
            string creatorContact,
            string description,
            string imagePath,
            ISkinInstanceFactory instanceFactory)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            this.name = name ?? Localization.UIResources.NoNameSkin;
            this.creator = creator;
            this.creatorContact = PromptuUtilities.SanitizeContactLink(creatorContact);
            this.description = description;
            this.imagePath = imagePath;
            this.id = id;
            this.instanceFactory = instanceFactory;
        }

        public string Id
        {
            get { return this.id; }
        }

        public string Name
        {
            get { return this.name; }
        }

        public string ImagePath
        {
            get { return this.imagePath; }
        }

        public string Creator
        {
            get { return this.creator; }
        }

        public string CreatorContact
        {
            get { return this.creatorContact; }
        }

        public string Description
        {
            get { return this.description; }
        }

        internal ISkinInstanceFactory InstanceFactory
        {
            get { return this.instanceFactory; }
        }
    }
}
