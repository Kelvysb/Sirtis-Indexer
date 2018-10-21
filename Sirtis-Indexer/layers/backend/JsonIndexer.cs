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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using SirtisIndexer.basic;
using Newtonsoft.Json.Linq;

namespace SirtisIndexer.layers.backend
{
    public class JsonIndexer : IIndexer
    {
        #region Constructor
        public JsonIndexer()
        {
            try
            {
                Extensions = new List<string>(new string[] {".json" , ".jsn"});
                Name = "Json";
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Functions
        public async virtual Task<IndexedFile> ProcessFileAsync(string p_strPath)
        {
            try
            {
                return await Task.Run(() => { return ProcessFile(p_strPath); });
            }
            catch (Exception)
            {
                throw;
            }

        }

        public virtual IndexedFile ProcessFile(string p_strPath)
        {
            IndexedFile objReturn;
            JObject objJson;
            StreamReader objFile;
            string strRawFile;

            try
            {

                if(!Extensions.Contains(Path.GetExtension(p_strPath)))
                {
                    throw new Exception("Invalid file.");
                }

                objReturn = new IndexedFile();
                objReturn.Name = Path.GetFileName(p_strPath);
                objReturn.Extension = Path.GetExtension(p_strPath);
                objReturn.Path = p_strPath;
                
                objFile = new StreamReader(p_strPath);
                strRawFile = objFile.ReadToEnd();
                objFile.Close();
                objFile.Dispose();

                objReturn.Raw = strRawFile;

                objJson = JObject.Parse(strRawFile);

                objReturn.Items = GetItens(objJson, objReturn);
            
                return objReturn;

            }
            catch (Exception)
            {
                throw;
            }
        }

        protected List<IndexedItem> GetItens(JToken p_objJson, IndexedFile p_objParent)
        {
            List<IndexedItem> objReturn;

            try
            {

                objReturn = new List<IndexedItem>();
                

                foreach (JToken item in p_objJson.Children())
                {
                    if(item.Type == JTokenType.Property || item.Type == JTokenType.Object)
                    {
                        objReturn.AddRange(GetItens(item, p_objParent));
                    }
                    else
                    {

                        if(item is JArray)
                        {
                            foreach (JToken subItem in item)
                            {
                                if(subItem is JValue)
                                {
                                    objReturn.Add(new IndexedItem());
                                    objReturn.Last().ParentFile = p_objParent;
                                    objReturn.Last().Flat = item.Path;
                                    objReturn.Last().Index = item.ToList().IndexOf(subItem);
                                    objReturn.Last().Alias = ((JProperty)item.Parent).Name;
                                    objReturn.Last().Tiemstamp = DateTime.Now;
                                    objReturn.Last().Modified = false;
                                    if (((JValue)subItem).Value != null)
                                    {
                                        objReturn.Last().Value = ((JValue)subItem).Value.ToString();
                                    }
                                    else
                                    {
                                        objReturn.Last().Value = "";
                                    }
                                }
                                else
                                {
                                    objReturn.AddRange(GetItens(subItem, p_objParent));
                                }
                            }

                        }
                        else
                        {
                            objReturn.Add(new IndexedItem());
                            objReturn.Last().ParentFile = p_objParent;
                            objReturn.Last().Flat = item.Path;
                            objReturn.Last().Alias = ((JProperty)item.Parent).Name;
                            objReturn.Last().Tiemstamp = DateTime.Now;
                            objReturn.Last().Modified = false;
                            if(((JValue)item).Value != null)
                            {
                                objReturn.Last().Value = ((JValue)item).Value.ToString();
                            }
                            else
                            {
                                objReturn.Last().Value = "";
                            }
                        }

                    }
                   

                }


                return objReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual IndexedFile ApplyModifications(IndexedFile p_objFile)
        {

            IndexedFile objReturn;
            JObject objJson;
            StreamWriter objfile;
            List<IndexedItem> objModified;

            try
            {

                objReturn = p_objFile;

                objJson = JObject.Parse(objReturn.Raw);

                objModified = objReturn.Items.FindAll((item) => { return item.Modified; });

                foreach (IndexedItem item in objModified)
                {
                    ((JValue)objJson.SelectToken(item.Flat)).Value = item.Value;
                }

                objfile = new StreamWriter(objReturn.Path);
                objfile.Write(objJson.ToString());
                objfile.Close();
                objfile.Dispose();
                objfile = null;

                objReturn.Raw = objJson.ToString();

                return objReturn;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual async Task<IndexedFile> ApplyModificationsAsync(IndexedFile p_objFile)
        {
            try
            {
                return await Task.Run(() => { return ApplyModifications(p_objFile); });
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Properties
        public virtual List<string> Extensions { get; set; }
        public virtual string Name { get; set; }
        #endregion

    }

    public static class JsonHelper
    {
        public static object Deserialize(string json)
        {
            return ToObject(JToken.Parse(json));
        }

        private static object ToObject(JToken token)
        {
            switch (token.Type)
            {
                case JTokenType.Object:
                    return token.Children<JProperty>()
                                .ToDictionary(prop => prop.Name,
                                              prop => ToObject(prop.Value));

                case JTokenType.Array:
                    return token.Select(ToObject).ToList();

                default:
                    return ((JValue)token).Value;
            }
        }
    }

}
