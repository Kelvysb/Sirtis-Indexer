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
using System.Threading.Tasks;
using System.IO;
using SirtisIndexer.basic;
using YamlDotNet.Serialization;
using Newtonsoft.Json.Linq;

namespace SirtisIndexer.layers.backend
{
    public class YamlIndexer :JsonIndexer, IIndexer
    {

        #region Constructor
        public YamlIndexer()
        {
            try
            {
                Extensions = new List<string>(new string[] { ".yaml", ".yml" });
                Name = "Yaml";
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Functions
        public async override Task<IndexedFile> ProcessFileAsync(string p_strPath)
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

        public override IndexedFile ProcessFile(string p_strPath)
        {
            IndexedFile objReturn;
            object objYaml;
            Deserializer objDeserializer;
            Newtonsoft.Json.JsonSerializer objSerializer;
            StreamReader objFile;
            string strRawFile;
            StringWriter strRawJson;
            JObject objJson;

            try
            {

                if (!Extensions.Contains(Path.GetExtension(p_strPath)))
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

                objDeserializer = new Deserializer();

         
                objYaml = objDeserializer.Deserialize(new StringReader(strRawFile));

                objSerializer = new Newtonsoft.Json.JsonSerializer();

                strRawJson = new StringWriter();
                objSerializer.Serialize(strRawJson, objYaml);

                objJson = JObject.Parse(strRawJson.ToString());

                objReturn.Items = GetItens(objJson, objReturn);

                return objReturn;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override IndexedFile ApplyModifications(IndexedFile p_objFile)
        {

            IndexedFile objReturn;
            JObject objJson;
            Deserializer objDeserializer;
            Serializer objSerializer;
            StringWriter strRawJson;
            StringWriter strRawYaml;
            StreamWriter objfile;
            object objYaml;
            object objJsonRaw;
            Newtonsoft.Json.JsonSerializer objSerializerJson;
            List<IndexedItem> objModified;

            try
            {

                objReturn = p_objFile;

                objDeserializer = new Deserializer();
                objYaml = objDeserializer.Deserialize(new StringReader(objReturn.Raw));

                objSerializerJson = new Newtonsoft.Json.JsonSerializer();
                strRawJson = new StringWriter();
                objSerializerJson.Serialize(strRawJson, objYaml);
                objJson = JObject.Parse(strRawJson.ToString());

                objModified = objReturn.Items.FindAll((item) => { return item.Modified; });

                foreach (IndexedItem item in objModified)
                {
                    ((JValue)objJson.SelectToken(item.Flat)).Value = item.Value;
                }

                objJsonRaw = JsonHelper.Deserialize(objJson.ToString());

                objSerializer = new Serializer();

                strRawYaml = new StringWriter();
                objSerializer.Serialize(strRawYaml, objJsonRaw);

                objfile = new StreamWriter(objReturn.Path);
                objfile.Write(strRawYaml.ToString());
                objfile.Close();
                objfile.Dispose();
                objfile = null;

                objReturn.Raw = strRawYaml.ToString();

                return objReturn;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override async Task<IndexedFile> ApplyModificationsAsync(IndexedFile p_objFile)
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
        public override List<string> Extensions { get; set; }
        public override string Name { get; set; }
        #endregion

    }
  
}


