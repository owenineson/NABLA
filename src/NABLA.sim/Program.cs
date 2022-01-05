using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace NABLA.sim
{
    class Program
    {
        static string FirstCharacter(string Input)
        {
            return Input.Substring(0, 1);
        }

        static string[] ReadToArray(string FilePath)
        {
            try
            {
                string[] FileArray = File.ReadAllLines(FilePath);
                return FileArray;
            }
            catch (FileNotFoundException)
            {
                //throw a general argument exception to handle invalid path
                throw new ArgumentException("Could not find specfied file path");
            }

        }

        //TODO: add validation for data types and form
        static string[][] PreprocessFile(string[] FileArray)
        {
            //Establish a regex to filter comments (starts with * or ;) and surplus spice commands (starts with .)
            Regex CommentFilterRegex = new Regex(@"[\*\;\.]");
            //Put all of the filtered and accepted lines into a list
            List<string> FilteredList = new List<string>();

            for (int FileArrayLine = 0; FileArrayLine < FileArray.Length; FileArrayLine++)
            {
                //if there is a succsesful match the line is then skipped over
                if (CommentFilterRegex.IsMatch(FirstCharacter(FileArray[FileArrayLine])))
                {
                    continue;
                }
                //otherwise pop it onto the list
                else
                {
                    FilteredList.Add(FileArray[FileArrayLine]);
                }
            }

            //split each branch by spaces
            List<string[]> SplitList = new List<string[]>();

            for (int ListLine = 0; ListLine < FilteredList.Count; ListLine++)
            {
                SplitList.Add(FilteredList[ListLine].Split(new char[] { ' ' }));
            }

            //define each count variable
            int BranchCount = 0;
            int RLCCount = 0;
            int VoltageSourceCount = 0;
            int CurrentSourceCount = 0;

            //for some reason inductors are also counted independently...
            int InductorCount = 0;


            for (int ListLineNumber = 0; ListLineNumber < FilteredList.Count; ListLineNumber++)
            {
                //switch on the first charcter of the first item in each branch as well as start counting components and branches
                switch (FirstCharacter(SplitList[ListLineNumber][0]))
                {
                    case "R" or "L" or "C":
                        //quick error check for the write number of parameters in a branch
                        if (SplitList[ListLineNumber].Length != 4)
                        {
                            Console.WriteLine(String.Format("Invalid number of parameters on branch {0}. Should be 4", ListLineNumber));
                            break;
                        }
                        //if a valid componenet, increments its respective counter
                        BranchCount++;
                        RLCCount++;
                        //special case to increment inductors
                        if (FirstCharacter(SplitList[ListLineNumber][0]) == "L")
                        {
                            InductorCount++;
                        }
                        break;

                    case "V":
                        if (SplitList[ListLineNumber].Length != 4)
                        {
                            Console.WriteLine(String.Format("Invalid number of parameters on branch {0}. Should be 4", ListLineNumber));
                            break;
                        }
                        BranchCount++;
                        VoltageSourceCount++;
                        break;

                    case "I":
                        if (SplitList[ListLineNumber].Length != 4)
                        {
                            Console.WriteLine(String.Format("Invalid number of parameters on branch {0}. Should be 4", ListLineNumber));
                            break;
                        }
                        BranchCount++;
                        CurrentSourceCount++;
                        break;
                }
            }

            //return as a string array
            return SplitList.ToArray();

        }

        /// <summary>
        /// Builds a new RLC object using the data from a component line
        /// </summary>
        /// <param name="ElementText">A split line of an RLC component</param>
        /// <returns>An RLC object with all fields filled with respective passed data</returns>
        /// <exception cref="ArgumentException"></exception>
        static RLC GenerateRLC(String[] ElementText)
        {
            //build a new RLC object with the passed data from the line
            RLC newRLC = new RLC(ElementText[0]);

            //Now to add the two nodes, these need to be flipped into ints so a bit of validation is in order
            try
            {
                newRLC.PNode = Int32.Parse(ElementText[1]);
                newRLC.NNode = Int32.Parse(ElementText[2]);
            }
            catch (Exception)
            {
                throw new ArgumentException("Must be an integer");
            }
            
            //and finally the value to a float
            try
            {
                newRLC.Value = float.Parse(ElementText[3]);
            }
            catch (Exception)
            {

                throw new ArgumentException("Invalid input, must be float");
            }

            return newRLC;
        }

        static IndependentSource GenerateIndependentSource(String[] ElementText)
        {
            IndependentSource newIndependentSource = new IndependentSource(ElementText[0]);

            //Now to add the two nodes, these need to be flipped into ints so a bit of validation is in order
            try
            {
                newIndependentSource.PNode = int.Parse(ElementText[1]);
                newIndependentSource.NNode = int.Parse(ElementText[2]);
            }
            catch (Exception)
            {
                throw new ArgumentException("Must be an integer");
            }
            
            //and finally the value to a float
            try
            {
                newIndependentSource.Value = float.Parse(ElementText[3]);
            }
            catch (Exception)
            {

                throw new Exception("Invalid input");
            }

            return newIndependentSource;
        }

        static Connector[] LoadElements(string[][] LineArray)
        {
            Connector[] ConnectorArray = new Connector[LineArray[0].Length + 1];
            for (int LineNumber = 0; LineNumber < LineArray.Length; LineNumber++)
            {
                //each component get loaded in diffrently depending on its type
                switch (LineArray[LineNumber][0].Substring(0, 1))
                {
                    case "R" or "L" or "C":
                        ConnectorArray[LineNumber] = GenerateRLC(LineArray[LineNumber]);
                        break;

                    case "V":
                        ConnectorArray[LineNumber] = GenerateIndependentSource(LineArray[LineNumber]);
                        break;

                    case "I":

                        break;
                }
            }
            return ConnectorArray;
        }

        static void Main(string[] args)
        {
            String[] FileArray;
            string FilePath = @"C:\\Users\\owen\\Documents\\NABLA\\src\\NABLA.sim\\ExampleNetlist.txt";
            FileArray = ReadToArray(FilePath);
            string[][] SplitFileArray = PreprocessFile(FileArray);
            for (int i = 0; i < SplitFileArray.Length; i++)
            {
                for (int q = 0; q < SplitFileArray[i].Length; q++)
                {
                    Console.Write(SplitFileArray[i][q] + " ");
                }
                Console.WriteLine("");
            }
            Connector[] Thing = LoadElements(SplitFileArray);
            Console.WriteLine(Thing[1].PNode);
        }
    }
}
