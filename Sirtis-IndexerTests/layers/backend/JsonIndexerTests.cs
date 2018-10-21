using Microsoft.VisualStudio.TestTools.UnitTesting;
using SirtisIndexer.layers.backend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SirtisIndexer.basic;
using System.Diagnostics;

namespace SirtisIndexer.layers.backend.Tests
{
    [TestClass()]
    public class JsonIndexerTests
    {
        [TestMethod()]
        public void ProcessFileTest()
        {

            JsonIndexer objIndexer;
            IndexedFile objReturn;
            StreamWriter objFile;

            try
            {

                if (!File.Exists("testJson.json"))
                {
                    objFile = new StreamWriter("testJson.json");
                    objFile.Write("{\"item1\":{\"subItem1\":\"Hey1\",\"subItem2\":\"Hey2\",\"subItem3\":\"Hey3\",\"subItem4\":\"Hey4\"},\"item2\":{\"subItem1\":{\"subSubItem1\":\"Oi1\",\"subSubItem2\":\"Oi2\",\"subSubItem3\":\"Oi3\",\"subSubItem4\":\"Oi4\"},\"subItem2\":[1,2,3,4,5,6],\"subItem3\":[{\"subSubItem1\":\"Oi1\"},{\"subSubItem2\":\"Oi2\"},{\"subSubItem3\":\"Oi3\"},{\"subSubItem4\":\"Oi4\"}]}}");
                    objFile.Close();
                    objFile.Dispose();
                }

                objIndexer = new JsonIndexer();
                objReturn = objIndexer.ProcessFile("testJson.json");
                
                Console.Write(objReturn.ToString());

            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);                
            }
        }
    }
}