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
    public class Netlist
    {
        
        // ***** Properties *****

        /// <summary>
        /// true if netlist has been processed and is valid
        /// </summary>
        private bool _isNetlistValid;
        /// <summary>
        /// True if the netlist is verfifed to be valid
        /// </summary>
        public bool IsNetlistValid
        {
            get { return _isNetlistValid; }
        }


        /// <summary>
        /// Backing field for NetlistLineArray
        /// </summary>
        private string[][] _elementArray;     
        /// <summary>
        /// A Netlist split into lines and then by spaces
        /// </summary>
        public string[][] ElementArray
        {
            //ensure that the netlist is valid before returning
            get { 
                if(_isNetlistValid == true){
                    return _elementArray;
                }
                else{
                    return null;
                }
             }
            set { _elementArray = value;}
        }


        // ***** Constructors *****

        /// <summary>
        /// Creates a new instance of a netlist
        /// </summary>
        public Netlist()
        {
            _isNetlistValid = false;
        }
        

        // ***** Loaders *****

        /// <summary>
        /// Load a netlist from a text file path
        /// </summary>
        /// <param name="FilePath"></param>
        /// <returns>True if succseful</returns>
        /// <exception cref="FileNotFoundException"></exception>
        public bool LoadNetlist(string FilePath)
        {
            string[] FileArray;
            
            //Establish a regex to filter comments (starts with * or ;) and surplus spice commands (starts with .)
            Regex CommentFilterRegex = new Regex(@"[\*\;\.]");


            try
            {
                FileArray = File.ReadAllLines(FilePath);
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File path could not be found");
                return false;
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Invalid file path");
                return false;
            }

            //Put all of the filtered and accepted lines into a list
            List<string> FilteredNetlist = new List<string>();

            for (int ListLineNumber = 0; ListLineNumber < FileArray.Length - 1; ListLineNumber++)
            {
                //implemented for catching invalid components
                try
                {
                    //skip the line if its a comment
                    if (CommentFilterRegex.IsMatch(FileArray[ListLineNumber].Substring(0, 1)))
                    {
                        continue;
                    }
                    //null case handling, useful when working with piped data
                    else if (FileArray[ListLineNumber] == null)
                    {
                        continue;
                    }

                    //switch on the first charcter of the first item in each branch
                    switch (FileArray[ListLineNumber].Substring(0, 1))
                    {
                        //RLCs all have the same basic footprint therefore can be checked together
                        case "R" or "L" or "C":
                            
                            //quick error check for the write number of parameters in a branch
                            //this is done by splitting the list by spaces then counting the length of the array returned
                            if (FileArray[ListLineNumber].Split(" ").Length != 4)
                            {
                                throw new ArgumentException(String.Format("Invalid number of parameters on branch {0}. Should be 4", ListLineNumber + 1));
                            }
                            else
                            {
                                FilteredNetlist.Add(FileArray[ListLineNumber]);
                            }
                            break;

                        //Independent Voltage source
                        case "V":
                            
                            if (FileArray[ListLineNumber].Split(" ").Length != 4)
                            {
                                throw new ArgumentException(String.Format("Invalid number of parameters on branch {0}. Should be 4", ListLineNumber + 1));
                            }
                            else
                            {
                                FilteredNetlist.Add(FileArray[ListLineNumber]);
                            }
                            break;

                        //independent current source
                        case "I":
                            
                            if (FileArray[ListLineNumber].Split(" ").Length != 4)
                            {
                                throw new ArgumentException(String.Format("Invalid number of parameters on branch {0}. Should be 4", ListLineNumber + 1));
                            }
                            else
                            {
                                FilteredNetlist.Add(FileArray[ListLineNumber]);
                            }
                            break;

                        default:
                            throw new ArgumentException("Invalid element type");


                    }
                }
                //more or less just to catch errors thrown by invalid lines, print with the error message
                catch (ArgumentException e)
                {
                    Console.WriteLine(string.Format("Error on line {0}: {1}", ListLineNumber + 1, e.Message));
                    return false;
                }
            }

            //split the lines by spaces to make processing easier
            String[][] SplitNetlistLines = new string[FilteredNetlist.Count()][];
            for (int i = 0; i < FilteredNetlist.Count; i++)
            {
                SplitNetlistLines[i] = FilteredNetlist[i].Split(" ");
            }

            _elementArray = SplitNetlistLines;
            _isNetlistValid = true;
            return true;
        }

        /// <summary>
        /// Load a netlist from an array of lines, each expressing an element
        /// </summary>
        /// <param name="LineArray">An array of strings</param>
        /// <returns>True if succsesful</returns>
        public bool LoadNetlist(string[] LineArray)
        {

            //Establish a regex to filter comments (starts with * or ;) and surplus spice commands (starts with .)
            Regex CommentFilterRegex = new Regex(@"[\*\;\.]");

            //Put all of the filtered and accepted lines into a list
            List<string> FilteredNetlist = new List<string>();

            for (int ListLineNumber = 0; ListLineNumber < LineArray.Length - 1; ListLineNumber++)
            {
                //implemented for catching invalid components
                try
                {
                    //skip the line if its a comment
                    if (CommentFilterRegex.IsMatch(LineArray[ListLineNumber].Substring(0, 1)))
                    {
                        continue;
                    }
                    
                    //null case handling, useful when working with piped data
                    if (LineArray[ListLineNumber] == null || LineArray[ListLineNumber] == "")
                    {
                        continue;
                    }

                    //switch on the first charcter of the first item in each branch
                    switch (LineArray[ListLineNumber].Substring(0, 1))
                    {
                        //RLCs all have the same basic footprint therefore can be checked together
                        case "R" or "L" or "C":

                            //quick error check for the write number of parameters in a branch
                            //this is done by splitting the list by spaces then counting the length of the array returned
                            if (LineArray[ListLineNumber].Split(" ").Length != 4)
                            {
                                throw new ArgumentException(String.Format("Invalid number of parameters on branch {0}. Should be 4", ListLineNumber + 1));
                            }
                            else
                            {
                                FilteredNetlist.Add(LineArray[ListLineNumber]);
                            }
                            break;

                        //Independent Voltage source
                        case "V":

                            if (LineArray[ListLineNumber].Split(" ").Length != 4)
                            {
                                throw new ArgumentException(String.Format("Invalid number of parameters on branch {0}. Should be 4", ListLineNumber + 1));
                            }
                            else
                            {
                                FilteredNetlist.Add(LineArray[ListLineNumber]);
                            }
                            break;

                        //independent current source
                        case "I":

                            if (LineArray[ListLineNumber].Split(" ").Length != 4)
                            {
                                throw new ArgumentException(String.Format("Invalid number of parameters on branch {0}. Should be 4", ListLineNumber + 1));
                            }
                            else
                            {
                                FilteredNetlist.Add(LineArray[ListLineNumber]);
                            }
                            break;

                        default:
                            throw new ArgumentException("Invalid element type");


                    }
                }
                //more or less just to catch errors thrown by invalid lines, print with the error message
                catch (ArgumentException e)
                {
                    Console.WriteLine(string.Format("Error on line {0}: {1}", ListLineNumber + 1, e.Message));
                    return false;
                }
            }

            //split the lines by spaces to make processing easier
            String[][] SplitNetlistLines = new string[FilteredNetlist.Count()][];
            for (int i = 0; i < FilteredNetlist.Count; i++)
            {
                SplitNetlistLines[i] = FilteredNetlist[i].Split(" ");
            }

            _elementArray = SplitNetlistLines;
            _isNetlistValid = true;
            return true;
        }


        // ***** Utilities *****

        /// <summary>
        /// Return a formatted representation of the netlist
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string formatNetlist = "";

            if (_elementArray != null)
            {
                //iterate over the array and make a formatted string
                for (int y = 0; y < _elementArray.GetLength(0); y++)
                {
                    for (int x = 0; x < _elementArray[y].Length; x++)
                    {
                        formatNetlist += _elementArray[y][x] + " ";
                    }
                    formatNetlist += "\n";
                }
            }

            return formatNetlist;

        }
    }
}
