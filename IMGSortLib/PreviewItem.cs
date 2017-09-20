using IMGSortLib.Dummies;
using IMGSortLib.Interfaces;
using IMGSortLib.Sort;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMGSortLib
{
    public static class PreviewItem
    {
        public static IEnumerable<IFileData> CreatePreview(IEnumerable<DeltaItem> LogicItems)
        {
            IList<IFileData> retList = new List<IFileData>();

            foreach (DeltaItem item in LogicItems)
            {
                string[] itemPath = item.TargetPath.Split('\\');
                IFileData aktFileData = retList.FirstOrDefault(x => x.Path == itemPath[0]);
                if (aktFileData == null)
                {
                    aktFileData = new FileDataDummy(itemPath[0], string.Empty);
                    retList.Add(aktFileData);

                    //TODO Recursives Datenobjekt anlegen und das IFileData austauschen
                }
                for (int i = 1; i < itemPath.Length - 1; i++)
                {

                }
            }

            return retList;
        }
    }
}
