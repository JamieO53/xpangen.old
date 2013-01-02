using org.xpangen.Generator.Data;

namespace org.xpangen.Generator.Application
{
    public class GenApplicationList<T> : GenList<T>
    {
        public override void Move(ListMove move, int itemIndex)
        {

            var item = this[itemIndex] as GenApplicationBase;
            if (item != null)
            {
                var genList = item.GenObject.ParentSubClass;
                genList.MoveItem(move, itemIndex);
            }

            base.Move(move, itemIndex);
        }
    }
}
