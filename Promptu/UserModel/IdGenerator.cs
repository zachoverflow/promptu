using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu.UserModel
{
    internal class IdGenerator
    {
        private int nextId;

        public IdGenerator()
            : this(Int32.MinValue)
        {
        }

        public IdGenerator(int nextId)
        {
            this.nextId = nextId;
        }

        public IdGenerator(string s)
        {
            this.nextId = new Id(s).Value;
        }

        public Id Peek()
        {
            return new Id(this.nextId);
        }

        public Id GenerateId()
        {
            return new Id(this.nextId++);
        }

        public void SetNextIdFromCollectionAndSetMissingIds<T>(IEnumerable<T> itemsWithId)
            where T : IHasId
        {
            List<T> missingIds = new List<T>();
            foreach (T hasId in itemsWithId)
            {
                if (hasId.Id == null)
                {
                    missingIds.Add(hasId);
                }
                else if (hasId.Id.Value >= this.nextId)
                {
                    this.nextId = hasId.Id.Value + 1;
                }
            }

            foreach (T missingId in missingIds)
            {
                missingId.Id = this.GenerateId();
            }
        }

        public IdGenerator Clone()
        {
            return new IdGenerator(this.nextId);
        }

        public void EnsureNextIdWillNotConflictWith(Id id)
        {
            if (id.Value >= this.nextId)
            {
                this.nextId = id.Value + 1;
            }
        }

        public void Align(IdGenerator otherIdGenerator)
        {
            if (this.nextId > otherIdGenerator.nextId)
            {
                otherIdGenerator.nextId = this.nextId;
            }
        }
    }
}
