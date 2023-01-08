using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Mine2DDesigner.Services
{
    public class DialogServiceCollection : ICollection<IDialogService>
    {
        private IList<IDialogService> items;
        public DialogServiceCollection()
        {
            items = new List<IDialogService>();
        }

        public IDialogService? Get<TVm>()
        {
            return items.FirstOrDefault(s => s.VmType == typeof(TVm));
        }

        public int Count => items.Count;

        public bool IsReadOnly => items.IsReadOnly;

        public void Add(IDialogService item)
        {
            items.Add(item);
        }

        public void Clear()
        {
            items.Clear();
        }

        public bool Contains(IDialogService item)
        {
            return items.Contains(item);
        }

        public void CopyTo(IDialogService[] array, int arrayIndex)
        {
            items.CopyTo(array, arrayIndex);
        }

        public IEnumerator<IDialogService> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        public bool Remove(IDialogService item)
        {
            return items.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)items).GetEnumerator();
        }
    }
}
