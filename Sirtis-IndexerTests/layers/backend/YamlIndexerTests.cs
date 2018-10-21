using Microsoft.VisualStudio.TestTools.UnitTesting;
using SirtisIndexer.basic;
using SirtisIndexer.layers.backend;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SirtisIndexer.layers.backend.Tests
{
    [TestClass()]
    public class YamlIndexerTests
    {
        [TestMethod()]
        public void ProcessFileTest()
        {
            YamlIndexer objIndexer;
            IndexedFile objReturn;
            StreamWriter objFile;

            try
            {

                if (!File.Exists("testYaml.yml"))
                {
                    objFile = new StreamWriter("testYaml.yml");
                    objFile.WriteLine(" item1: ");
                    objFile.WriteLine("  subItem1: Hey1");
                    objFile.WriteLine("  subItem2: Hey2");
                    objFile.WriteLine("  subItem3: Hey3");
                    objFile.WriteLine("  subItem4: Hey4");
                    objFile.WriteLine(" item2: ");
                    objFile.WriteLine("  subItem1: ");
                    objFile.WriteLine("   subSubItem1: Oi1");
                    objFile.WriteLine("   subSubItem2: Oi2");
                    objFile.WriteLine("   subSubItem3: Oi3");
                    objFile.WriteLine("   subSubItem4: Oi4");
                    objFile.WriteLine("  subItem2: ");
                    objFile.WriteLine("   - ");
                    objFile.WriteLine("    1");
                    objFile.WriteLine("   - ");
                    objFile.WriteLine("    2");
                    objFile.WriteLine("   - ");
                    objFile.WriteLine("    3");
                    objFile.WriteLine("   - ");
                    objFile.WriteLine("    4");
                    objFile.WriteLine("   - ");
                    objFile.WriteLine("    5");
                    objFile.WriteLine("   - ");
                    objFile.WriteLine("    6");
                    objFile.WriteLine("  subItem3: ");
                    objFile.WriteLine("   - ");
                    objFile.WriteLine("    subSubItem1: Oi1");
                    objFile.WriteLine("   - ");
                    objFile.WriteLine("    subSubItem2: Oi2");
                    objFile.WriteLine("   - ");
                    objFile.WriteLine("    subSubItem3: Oi3");
                    objFile.WriteLine("   - ");
                    objFile.WriteLine("    subSubItem4: Oi4");
                    objFile.Close();
                    objFile.Dispose();
                }

                objIndexer = new YamlIndexer();
                objReturn = objIndexer.ProcessFile("testYaml.yml");

                Console.Write(objReturn.ToString());

            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

        }
    }
}