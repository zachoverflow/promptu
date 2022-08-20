//-----------------------------------------------------------------------
// <copyright file="FailedToSaveFileCollection.cs" company="ZachJohnson">
//     Copyright (c) Zach Johnson. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ZachJohnson.Promptu.Collections
{
    using System.Collections.Generic;

    internal class FailedToSaveFileCollection : List<FailedToSaveFile>
    {
        public FailedToSaveFileCollection()
        {
        }

        public void Remove(string id, string fileId)
        {
            List<FailedToSaveFile> itemsToRemove = new List<FailedToSaveFile>();
            foreach (FailedToSaveFile item in this)
            {
                if (item.Id == id && item.FileId == fileId)
                {
                    itemsToRemove.Add(item);
                }
            }

            foreach (FailedToSaveFile item in itemsToRemove)
            {
                this.Remove(item);
            }
        }
    }
}
