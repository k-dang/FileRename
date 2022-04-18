using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileRename.Util
{
    public class ControlUtil
    {
        public static List<int> GetSelectedIndexes(IList<string> selectedItems, Collection<string> allItems)
        {
            var selectedIndexes = new List<int>();
            foreach (var selectedFile in selectedItems)
            {
                selectedIndexes.Add(allItems.IndexOf(selectedFile));
            }
            return selectedIndexes;
        }
    }
}
