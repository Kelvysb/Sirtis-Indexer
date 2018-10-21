using SirtisIndexer.basic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SirtisIndexer.layers.backend
{
    public class SirtisIndexerBO
    {

        #region Declarations
        private static SirtisIndexerBO instance;
        private List<IIndexer> objIndexers;
        private string strWorkDirectory;
        #endregion

        #region Constructor
        private SirtisIndexerBO()
        {
            
            try
            {
                objIndexers = new List<IIndexer>();
                objIndexers.Add(new JsonIndexer());
                objIndexers.Add(new YamlIndexer());

                strWorkDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\SirtisIndexer\\";
                if (Directory.Exists(strWorkDirectory) == false)
                {
                    Directory.CreateDirectory(strWorkDirectory);
                }

            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Events

        #endregion

        #region Functions
        private static SirtisIndexerBO getInstance()
        {
            try
            {
                if(instance == null)
                {
                    instance = new SirtisIndexerBO();
                }

                return instance;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IndexedFolder> ProcessFolder(string p_strPath)
        {

            IndexedFolder objReturn;
            string[] strFiles;
            IIndexer objAuxIndexer;

            try
            {

                objReturn = new IndexedFolder();
                objReturn.Tiemstamp = DateTime.Now;
                objReturn.Path = p_strPath;
                objReturn.Files = new List<IndexedFile>();
                objReturn.Itens = new List<IndexedItem>();

                strFiles = Directory.GetFiles(p_strPath);

                foreach (string file in strFiles)
                {
                    objAuxIndexer = objIndexers.Find((indexer) => indexer.Extensions.Contains(Path.GetExtension(file)));
                    if (objAuxIndexer != null)
                    {
                        objReturn.Files.Add(await objAuxIndexer.ProcessFileAsync(file));
                        objReturn.Itens.AddRange(objReturn.Files.Last().Items);
                    }
                }

                return objReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> SaveModifications(List<IndexedItem> p_objModifications)
        {

            List<IndexedFile> objGroupFiles;
            IIndexer objAuxIndexer;
            IndexedFile objAuxReturn;

            try
            {

                objGroupFiles = (from IndexedItem item in p_objModifications
                                 group item by item.ParentFile into g
                                 select g.Key).ToList();

                foreach (IndexedFile file in objGroupFiles)
                {


                    objAuxIndexer = objIndexers.Find((indexer) => indexer.Extensions.Contains(file.Extension));
                    if (objAuxIndexer != null)
                    {
                        objAuxReturn = await objAuxIndexer.ApplyModificationsAsync(file);
                        file.Raw = objAuxReturn.Raw;
                    }

                }
          
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Properties
        public static SirtisIndexerBO Instance { get => getInstance(); }
        public string WorkDirectory { get => strWorkDirectory; }
        #endregion

    }
}
