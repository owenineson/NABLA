using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NABLA.sim
{
    /// <summary>
    /// A netlist describing a circuit
    /// </summary>
    internal class Netlist
    {
        /// <summary>
        /// The path at which the netlist is stored
        /// </summary>
        private string _filePath;

        /// <summary>
        /// Backing field for NetlistLineArray
        /// </summary>
        private string[][] _netlistLineArray;
        /// <summary>
        /// A Netlist split into lines and then by spaces
        /// </summary>
        public string[][] NetlistLineArray
        {
            get { return _netlistLineArray; }
            set { _netlistLineArray = value;}
        }

        //Establish a regex to filter comments (starts with * or ;) and surplus spice commands (starts with .)
        //TODO: i dont think this should be stored here
        Regex CommentFilterRegex = new Regex(@"[\*\;\.]");

        //TODO: this is super messy and needs a load more validation, also this just defo shouldnt all be in the constructor
        //TODO: could probably split validation and filtering out into another function just for the sake of modularity as well
        //TODO: build in refrencing to the TypeDescriptions document
        /// <summary>
        /// Creates a new instance of a netlist
        /// </summary>
        /// <param name="FilePath">The path of a text netlist</param>
        /// <exception cref="FileNotFoundException"></exception>
        public Netlist(string FilePath)
        {
            _filePath = FilePath;
            string[] FileArray;

            try
            {
                FileArray = File.ReadAllLines(FilePath);
            }
            catch (FileNotFoundException)
            {
                //throw a general argument exception to handle invalid path
                throw new FileNotFoundException("Could not find specfied file path");
            }


            //Put all of the filtered and accepted lines into a list
            List<string> FilteredNetlist = new List<string>();

            for (int NetlistArrayLine = 0; NetlistArrayLine < FileArray.Length; NetlistArrayLine++)
            {
                //if there is not a succsesful match then the line is added to the FilteredNetlist
                if (!CommentFilterRegex.IsMatch(FileArray[NetlistArrayLine].Substring(0, 1)))
                {
                    FilteredNetlist.Add(FileArray[NetlistArrayLine]);
                }
            }

            for (int ListLineNumber = 0; ListLineNumber < FilteredNetlist.Count; ListLineNumber++)
            {
                //switch on the first charcter of the first item in each branch
                switch (FilteredNetlist[ListLineNumber].Substring(0, 1))
                {
                    //RLCs all have the same basic footprint therefore can be checked together
                    case "R" or "L" or "C":
                        //quick error check for the write number of parameters in a branch
                        //this is done by splitting the list by spaces then counting the length of the array returned
                        if (FilteredNetlist[ListLineNumber].Split(" ").Length != 4)
                        {
                            Console.WriteLine(String.Format("Invalid number of parameters on branch {0}. Should be 4", ListLineNumber));
                            break;
                        }
                        break;

                    //Independent Voltage source
                    case "V":
                        if (FilteredNetlist[ListLineNumber].Split(" ").Length != 4)
                        {
                            Console.WriteLine(String.Format("Invalid number of parameters on branch {0}. Should be 4", ListLineNumber));
                            break;
                        }
                        break;
                    
                    //independent current source
                    case "I":
                        if (FilteredNetlist[ListLineNumber].Split(" ").Length != 4)
                        {
                            Console.WriteLine(String.Format("Invalid number of parameters on branch {0}. Should be 4", ListLineNumber));
                            break;
                        }
                        break;
                }
            }

            String[][] SplitNetlistLines = new string[FilteredNetlist.Count() - 1][];
            for (int i = 0; i < (FilteredNetlist.Count - 1); i++)
            {
                SplitNetlistLines[i] = FilteredNetlist[i].Split(" ");
            }

            NetlistLineArray = SplitNetlistLines;
        }

    }
}
