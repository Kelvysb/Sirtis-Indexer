using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SirtisIndexer.basic;
using SirtisIndexer.layers.backend;

namespace Sirtis_Shell
{
    class SirtisShell
    {

        #region Declarations
        private static bool blnActive = true;
        #endregion

        #region Constructor

        #endregion

        #region Main
        static void Main(string[] args)
        {
            string strInput;

            Console.Title = "Sirtis indexer";

            try
            {
                if (args.Count() > 0)
                {
                    Folder(args.ToList());
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            while (blnActive)
            {
                try
                {

                    if (SirtisIndexerBO.Instance.IndexedFolder == null)
                    {
                        Console.WriteLine("Insert folder:");
                        strInput = Console.ReadLine();

                        if (strInput.Trim() != "")
                        {
                            Folder(args.ToList());
                        }
                    }
                    else
                    {
                        Commands();
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }
        #endregion

        #region Functions
        private static void Commands()
        {

            string strInput;

            try
            {

                strInput = Console.ReadLine();

                if (CommandsSet.Help(strInput))
                {
                    Help();
                }
                else if (CommandsSet.All(strInput))
                {
                    Listall(GetParameters(strInput));
                }
                else if (CommandsSet.Find(strInput))
                {
                    Find(GetParameters(strInput));
                }
                else if (CommandsSet.FindIndex(strInput))
                {
                    FindIndex(GetParameters(strInput));
                }
                else if (CommandsSet.Folder(strInput))
                {
                    Folder(GetParameters(strInput));
                }
                else if (CommandsSet.Exit(strInput))
                {
                    blnActive = false;
                }
                else
                {
                    Console.WriteLine("Unknow command, type help for a command list.");
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        private static void Find(List<string> args)
        {

            bool blnDetails;
            string strKey;
            List<IndexedItem> objResult;

            try
            {

                objResult = new List<IndexedItem>();

                if (args.Count() > 0)
                {

                    strKey = args.First();

                    blnDetails = false;
                    if (args.Count() > 1)
                    {
                        if (args.Last().Trim().ToUpper() == "-D")
                        {
                            blnDetails = true;
                        }
                    }

                    //Filter flat
                    objResult.AddRange(SirtisIndexerBO.Instance.IndexedFolder.Itens.FindAll(item => item.Flat.StartsWith(strKey.Trim(), StringComparison.InvariantCultureIgnoreCase)));

                    if (objResult.Count() > 0)
                    {
                        foreach (IndexedItem item in objResult)
                        {
                            FormatKey(item, blnDetails);
                        }
                    }
                    else
                    {
                        Console.WriteLine("No results");
                    }

                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        private static void FindIndex(List<string> args)
        {

            bool blnDetails;
            int intIndex;
            List<IndexedItem> objResult;

            try
            {

                objResult = new List<IndexedItem>();

                if (args.Count() > 0)
                {

                     if(int.TryParse(args.First(), out intIndex))
                    {
                        blnDetails = false;
                        if (args.Count() > 1)
                        {
                            if (args.Last().Trim().ToUpper() == "-D")
                            {
                                blnDetails = true;
                            }
                        }

                        FormatKey(SirtisIndexerBO.Instance.IndexedFolder.Itens[intIndex], blnDetails);

                    }
                    else
                    {
                        Console.WriteLine("Invalid index.");
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        private static void Listall(List<string> args)
        {

            bool blnDetails;

            try
            {
                blnDetails = false;
                if (args.Count() > 0)
                {
                    if (args.First().Trim().ToUpper() == "-D")
                    {
                        blnDetails = true;
                    }
                }

                foreach (IndexedItem item in SirtisIndexerBO.Instance.IndexedFolder.Itens)
                {
                    FormatKey(item, blnDetails);
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        private static void FormatKey(IndexedItem p_objItem, bool p_blnDetail)
        {
            try
            {


                Console.WriteLine(p_objItem.Flat + " = " + p_objItem.Value);

                if (p_blnDetail)
                {

                    Console.WriteLine(" File: " + p_objItem.ParentFile.Name);
                    Console.WriteLine(" File index: " + p_objItem.ParentFile.Items.IndexOf(p_objItem));
                    Console.WriteLine(" Global index: " + SirtisIndexerBO.Instance.IndexedFolder.Itens.IndexOf(p_objItem));

                }

                Console.WriteLine("");

            }
            catch (Exception)
            {
                throw;
            }

        }

        private static void Folder(List<string> args)
        {
            try
            {

                if (args.Count() > 0)
                {
                    SirtisIndexerBO.Instance.ProcessFolderSync(args.First());
                    FolderInfo();
                }
                else
                {
                    Console.WriteLine("Inform a folder path.");
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        private static void Help()
        {
            try
            {

                Console.WriteLine("Load a folder: ");
                Console.WriteLine(" folder \"folder path\"");
                Console.WriteLine("");

                Console.WriteLine("Find a key: ");
                Console.WriteLine(" find \"key\"");
                Console.WriteLine("     -d -> detailed list.");
                Console.WriteLine("");

                Console.WriteLine("List all keys: ");
                Console.WriteLine(" all");
                Console.WriteLine("     -d -> detailed list.");
                Console.WriteLine("");

                Console.WriteLine("Find a value: ");
                Console.WriteLine(" value \"value\"");
                Console.WriteLine("     -d -> detailed list.");
                Console.WriteLine("");

                Console.WriteLine("Find a item by global index: ");
                Console.WriteLine(" index \"index\"");
                Console.WriteLine("     -d -> detailed list.");
                Console.WriteLine("");

                Console.WriteLine("Set a key: ");
                Console.WriteLine(" set \"key\"");
                Console.WriteLine("");

                Console.WriteLine("Set a key by global index: ");
                Console.WriteLine(" setidx \"index\"");
                Console.WriteLine("");

                Console.WriteLine("information of a key: ");
                Console.WriteLine(" info \"key\"");
                Console.WriteLine("");

                Console.WriteLine("information of a key by global index: ");
                Console.WriteLine(" infoidx \"index\"");
                Console.WriteLine("");

                Console.WriteLine("save modifications: ");
                Console.WriteLine(" save");
                Console.WriteLine("");

                Console.WriteLine("help: ");
                Console.WriteLine(" help");
                Console.WriteLine(" h");
                Console.WriteLine(" ?");
                Console.WriteLine("");

                Console.WriteLine("exit: ");
                Console.WriteLine(" exit");
                Console.WriteLine(" quit");
                Console.WriteLine(" q");

            }
            catch (Exception)
            {
                throw;
            }
        }

        private static void FolderInfo()
        {
            try
            {

                if (SirtisIndexerBO.Instance.IndexedFolder != null)
                {
                    Console.WriteLine("Loaded folder: " + SirtisIndexerBO.Instance.IndexedFolder.Path);
                    Console.WriteLine("Files: " + SirtisIndexerBO.Instance.IndexedFolder.Files.Count());
                    Console.WriteLine("Itens: " + SirtisIndexerBO.Instance.IndexedFolder.Itens.Count());
                }
                else
                {
                    Console.WriteLine("No Folder Loaded.");
                }

            }
            catch (Exception)
            {
                throw;
            }
        }


        private static List<string> GetParameters(string p_strCommand)
        {
            List<string> strReturn;

            try
            {
                strReturn = new List<string>();

                strReturn = p_strCommand.Split(' ').ToList();

                strReturn.RemoveAt(0);

                return strReturn;

            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }


    public class CommandsSet
    {

        private static List<string> cmdHelp = new List<string>(new string[] { "HELP", "H", "?" });
        public static Boolean Help(string p_cmdCommand) { return cmdHelp.Contains(p_cmdCommand.Trim().ToUpper().Split(' ').First()); }

        private static List<string> cmdExit = new List<string>(new string[] { "EXIT", "QUIT" });
        public static Boolean Exit(string p_cmdCommand) { return cmdExit.Contains(p_cmdCommand.Trim().ToUpper().Split(' ').First()); }

        private static List<string> cmdFind = new List<string>(new string[] { "FIND" });
        public static Boolean Find(string p_cmdCommand) { return cmdFind.Contains(p_cmdCommand.Trim().ToUpper().Split(' ').First()); }

        private static List<string> cmdFindIndex = new List<string>(new string[] { "FINDIDX" });
        public static Boolean FindIndex(string p_cmdCommand) { return cmdFindIndex.Contains(p_cmdCommand.Trim().ToUpper().Split(' ').First()); }

        private static List<string> cmdall = new List<string>(new string[] { "ALL" });
        public static Boolean All(string p_cmdCommand) { return cmdall.Contains(p_cmdCommand.Trim().ToUpper().Split(' ').First()); }

        private static List<string> cmdfolder = new List<string>(new string[] { "FOLDER" });
        public static Boolean Folder(string p_cmdCommand) { return cmdfolder.Contains(p_cmdCommand.Trim().ToUpper().Split(' ').First()); }



    }

}
