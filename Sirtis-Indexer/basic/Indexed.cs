using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SirtisIndexer.basic
{

    public class IndexedFolder
    {

        #region Functions
        public override string ToString()
        {
            string strReturn;

            try
            {
                strReturn = Path + " => " + (Char)13;

                foreach (IndexedFile file in Files)
                {
                    strReturn = strReturn + (Char)9 + file.ToString() + (Char)13;
                }

                return strReturn;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Properties
        public DateTime Tiemstamp { get; set; }
        public string Path { get; set; }
        public List<IndexedFile> Files { get; set; }
        public List<IndexedItem> Itens { get; set; }
        #endregion
    }

    public class IndexedFile
    {

        #region Functions
        public override string ToString()
        {
            string strReturn;

            try
            {
                strReturn = Name  + " => " + (Char)13;

                foreach (IndexedItem item in Items)
                {
                    strReturn = strReturn + (Char)9 + item.ToString() + (Char)13;
                }
                return strReturn;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Properties
        public string Alias { get; set; }
        public string Extension { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public List<IndexedItem> Items { get; set; }
        public string Raw { get; set; }
        #endregion
    }

    public class IndexedItem
    {

        #region Functions
        public override string ToString()
        {
            try
            {
                return Flat + " => " + Value;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Properties
        public DateTime Tiemstamp { get; set; }
        public string Alias { get; set; }
        public string Flat { get; set; }
        public string Value { get; set; }
        public int Index { get; set; }
        public bool Modified { get; set; }
        public IndexedFile ParentFile { get; set; }
        #endregion
    }


}
