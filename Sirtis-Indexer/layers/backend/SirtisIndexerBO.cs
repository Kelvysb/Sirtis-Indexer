/*
Copyright 2018 Kelvys B. Pantaleão

This file is part of Sirtis Indexer

Sirtis Indexer Is free software: you can redistribute it And/Or modify
it under the terms Of the GNU General Public License As published by
the Free Software Foundation, either version 3 Of the License, Or
(at your option) any later version.

This program Is distributed In the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty Of
MERCHANTABILITY Or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License For more details.

You should have received a copy Of the GNU General Public License
along with this program.  If Not, see <http://www.gnu.org/licenses/>.

Este arquivo é parte Do programa Sirtis Indexer

Sirtis Indexer é um software livre; você pode redistribuí-lo e/ou 
modificá-lo dentro dos termos da Licença Pública Geral GNU como 
publicada pela Fundação Do Software Livre (FSF); na versão 3 da 
Licença, ou(a seu critério) qualquer versão posterior.

Este programa é distribuído na esperança de que possa ser  útil, 
mas SEM NENHUMA GARANTIA; sem uma garantia implícita de ADEQUAÇÃO
a qualquer MERCADO ou APLICAÇÃO EM PARTICULAR. Veja a
Licença Pública Geral GNU para maiores detalhes.

Você deve ter recebido uma cópia da Licença Pública Geral GNU junto
com este programa, Se não, veja <http://www.gnu.org/licenses/>.

'GitHub: https://github.com/Kelvysb/Sirtis-Indexer
*/

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
