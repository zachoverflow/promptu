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

namespace ZachJohnson.Promptu.Collections
{
    using System.Collections.Generic;
    using ZachJohnson.Promptu.UserModel;
    using ZachJohnson.Promptu.UserModel.Collections;

    internal abstract class ItemWithIdChangeNotifiedList<T> : ChangeNotifiedList<T>, IItemsWithIdList<T> where T : class, IHasId
    {
        public ItemWithIdChangeNotifiedList()
        {
        }

        public T TryGet(Id id)
        {
            int? index;
            return this.TryGet(id, out index);
        }

        public T TryGet(Id id, out int? index)
        {
            if (id != null)
            {
                for (int i = 0; i < this.Count; i++)
                {
                    T item = this[i];
                    if (item.Id == id)
                    {
                        index = i;
                        return item;
                    }
                }
            }

            index = null;
            return null;
        }

        public List<T> GetConflictsWith(T item)
        {
            return GetConflictsWithCore(item);
        }

        public bool Remove(Id id)
        {
            if (id != null)
            {
                for (int i = 0; i < this.Count; i++)
                {
                    T item = this[i];
                    if (item.Id == id)
                    {
                        return this.Remove(item);
                    }
                }
            }

            return false;
        }

        protected abstract List<T> GetConflictsWithCore(T item);
    }
}
