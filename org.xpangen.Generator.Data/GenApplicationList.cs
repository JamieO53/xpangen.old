// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace org.xpangen.Generator.Data
{
    public interface IGenApplicationList : IGenList
    {
    }

    public interface IGenApplicationList<in T> : IGenApplicationList where T : GenApplicationBase, new()
    {
        /// <summary>
        /// Adds an object to the end of the <see cref="T:IGenApplicationList`1"/>.
        /// </summary>
        /// <param name="item">The object to be added to the end of the <see cref="T:IGenApplicationList`1"/>.</param>
        void Add(T item);

        /// <summary>
        /// Removes the specified item from the  <see cref="T:IGenApplicationList`1"/>.
        /// </summary>
        /// <param name="item">The object to be removed from the <see cref="T:IGenApplicationList`1"/>.</param>
        new void Remove(T item);
    }

    public class GenApplicationList<T> : GenList<T>, IGenApplicationList<T> where T : GenApplicationBase, new()
    {
        /// <summary>
        /// Adds an object to the end of the <see cref="T:GenApplicationList`1"/>.
        /// </summary>
        /// <param name="item">The object to be added to the end of the <see cref="T:GenApplicationList`1"/>.</param>
        protected new void Add(T item)
        {
            base.Add(item);
            item.List = this;
        }

        /// <summary>
        /// Removes the specified item from the  <see cref="T:IGenApplicationList`1"/>.
        /// </summary>
        /// <param name="item">The object to be removed from the <see cref="T:IGenApplicationList`1"/>.</param>
        public new void Remove(T item)
        {
            base.Remove(item);
            var genObject = (GenObject) item.GenObject;
            genObject.ParentSubClass.Remove(genObject);
        }

        public override bool Move(ListMove move, int itemIndex)
        {
            var item = this[itemIndex] as GenApplicationBase;
            if (item == null) return base.Move(move, itemIndex);
            var genObject = item.GenObject as GenObject;
            if (genObject == null) return base.Move(move, itemIndex);
            var genList = (GenSubClass) ((GenObject)item.GenObject).ParentSubClass;
            genList.Move(move, itemIndex);

            return base.Move(move, itemIndex);
        }
    }
}
