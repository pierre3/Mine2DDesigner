using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MinecraftBlockBuilder.Services
{
    public class ServiceCollection : ICollection<IService>
    {
        private IList<IService> items;
        public ServiceCollection()
        {
            items = new List<IService>();
        }

        public T? Get<T>() where T : IService
        {
            return items.OfType<T>().FirstOrDefault();
        }

        public int Count => items.Count;

        public bool IsReadOnly => items.IsReadOnly;

        public void Add(IService item)
        {
            items.Add(item);
        }

        public void Clear()
        {
            items.Clear();
        }

        public bool Contains(IService item)
        {
            return items.Contains(item);
        }

        public void CopyTo(IService[] array, int arrayIndex)
        {
            items.CopyTo(array, arrayIndex);
        }

        public IEnumerator<IService> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        public bool Remove(IService item)
        {
            return items.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)items).GetEnumerator();
        }
    }
}
