using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Collections;

namespace NABLA.sim
{
    class Program
    {
        static void Main(string[] args)
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
            
            static string[] PreprocessFile(string[] FileArray)
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
                            if(SplitList[ListLineNumber].Length != 4)
                            {
                                Console.WriteLine(String.Format("Invalid number of parameters on branch {0}. Should be 4", ListLineNumber));
                                break;
                            }
                            //if a valid componenet, increments its respective counter
                            BranchCount++;
                            RLCCount++;
                            break;
                        case "V":
                            if(SplitList[ListLineNumber].Length != 4)
                            {
                                Console.WriteLine(String.Format("Invalid number of parameters on branch {0}. Should be 4", ListLineNumber));
                                break;
                            }
                            BranchCount++;
                            VoltageSourceCount++;
                            break;
                        case "I":
                            if(SplitList[ListLineNumber].Length != 4)
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
                return FilteredList.ToArray();
            }

            static Hashtable GenerateRLC(String[] ElementText)
            {
                //gonna each element from its string array into a hashtable
                Hashtable Element = new Hashtable();
                //start with the type of the element
                Element.Add("Element", ElementText[0]);
                //Now to add the two nodes, these need to be flipped into ints so a bit of validation is in order
                try
                {
                    Element.Add("PNode", int.Parse(ElementText[1]));
                    Element.Add("NNode", int.Parse(ElementText[2]));
                }
                catch (Exception)
                {
                    
                }
                //and finally the value to a float
                try
                {
                    Element.Add("Value", float.Parse(ElementText[3]));
                }
                catch (Exception)
                {

                    throw new Exception("Invalid input");
                }

                return Element;
            }

            static void LoadElements()
            {

            }

            

            String[] FileArray;
            string FilePath = @"C:\\Users\\owen\\Documents\\NABLA\\src\\NABLA.sim\\ExampleNetlist.txt";
            FileArray = ReadToArray(FilePath);
            FileArray = PreprocessFile(FileArray);
            for (int i = 0; i < FileArray.Length; i++)
            {
                Console.WriteLine(FileArray[i]);
            }

        }
    }
}
