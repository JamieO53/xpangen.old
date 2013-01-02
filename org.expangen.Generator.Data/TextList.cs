using System.Collections.Generic;

namespace org.xpangen.Generator.Data
{
    public class TextList : List<string>
    {
        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the first occurrence within the entire text list.
        /// </summary>
        /// 
        /// <returns>
        /// The zero-based index of the first occurrence of <paramref name="item"/> within the entire text list, if found; otherwise, –1.
        /// </returns>
        /// <param name="item">The object to locate in the text list. The value can be null.</param>
        /// <returns>The index of the added item.</returns>
        public new int Add(string item)
        {
            // todo: Optimization to reuse values as in Delphi implementation
            //var i = IndexOf(item);
            //if (i == -1)
            //{
                var i = Count;
                base.Add(item);
            //}
            return i;
        }
    }
}
