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
