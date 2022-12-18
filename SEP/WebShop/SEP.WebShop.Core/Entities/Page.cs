namespace SEP.WebShop.Core.Entities
{
    public class Page<T>
    {
        public IEnumerable<T> Items { get; }
        public int Count { get; }
        
        public Page(IEnumerable<T> objects, int count)
        {
            Items = objects;
            Count = count;
        }
    }
}
