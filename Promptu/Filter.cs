using System;
using System.Collections.Generic;
using System.Text;

namespace ZachJohnson.Promptu
{
    internal delegate bool EntryValidator<TKey, TValue>(TKey key, TValue value);
    internal delegate TKey EntryTranslator<TKey, TValue>(TKey key, TValue value);

    internal class Filter<TKey, TValue>
    {
        private EntryValidator<TKey, TValue> validator;
        private EntryTranslator<TKey, TValue> translator;

        public Filter(EntryValidator<TKey, TValue> validator, EntryTranslator<TKey, TValue> translator)
        {
            this.validator = validator;
            this.translator = translator;
        }

        public bool IsValid(TKey key, TValue value)
        {
            if (this.validator != null)
            {
                return this.validator.Invoke(key, value);
            }

            return true;
        }

        public TKey TranslateKey(TKey key, TValue value)
        {
            if (this.translator != null)
            {
                return this.translator.Invoke(key, value);
            }

            return key;
        }
    }
}
