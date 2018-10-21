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

            try
            {

                objReturn = p_objFile;

                objJson = JObject.Parse(objReturn.Raw);




                return objReturn;

            }
            catch (Exception)
            {
                throw;
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
}
