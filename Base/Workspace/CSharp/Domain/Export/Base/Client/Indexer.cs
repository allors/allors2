namespace Allors.Workspace.Client
{
    using System.Collections.Generic;

    public class Indexer<T>
    {
        private readonly Dictionary<string, T> dictionary = new Dictionary<string, T>();

        public Indexer(Dictionary<string, T> dictionary)
        {
            this.dictionary = dictionary;
        } 

        public T this[string index] => this.dictionary.TryGetValue(index, out var value) ? value : default(T);
    }
}