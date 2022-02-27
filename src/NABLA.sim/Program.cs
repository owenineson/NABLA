using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace NABLA.sim
{
    class Program
    {
        static Circuit LoadElements(string[][] LineArray)
        {
            Circuit c = new Circuit();
            for (int LineNumber = 0; LineNumber < LineArray.Length; LineNumber++)
            {
                //each component get loaded in diffrently depending on its type
                switch (LineArray[LineNumber][0].Substring(0, 1))
                {
                    case "R" or "L" or "C":
                        c.Add(
                            // The name of the element
                            new Resistor(LineArray[LineNumber][0], 
                            // The value of the element
                            Convert.ToDouble(LineArray[LineNumber][3]), 
                            // A list of the nodes for the element
                            new List<int>()
                            {
                                Int32.Parse(LineArray[LineNumber][1]), 
                                Int32.Parse(LineArray[LineNumber][2]) 
                            }));
                        break;

                    case "V":
                        c.Add(
                            // The name of the element
                            new Resistor(LineArray[LineNumber][0],
                            // The value of the element
                            Convert.ToDouble(LineArray[LineNumber][3]),
                            // A list of the nodes for the element
                            new List<int>()
                            {
                                Int32.Parse(LineArray[LineNumber][1]),
                                Int32.Parse(LineArray[LineNumber][2])
                            }));
                        break;

                    case "I":
                        return null;
                        break;
                }
            }
            return c;
        }

        static void Main(string[] args)
        {
            //harcoding the path for the netlist being used
            string FilePath = @"C:\\Users\\owen\\Documents\\NABLA\\src\\NABLA.sim\\ExampleNetlist.txt";
            
            //instantiate a new netlist with the path
            Netlist simpleNetlist = new Netlist(FilePath);
            
            //pulling this out just for convenience
            string[][] LineArray = simpleNetlist.NetlistLineArray;
            
            //whack the array out onto the console
            for (int y = 0; y < LineArray.Length; y++)
            {
                for (int x = 0; x < LineArray[y].Length; x++)
                {
                    Console.Write(LineArray[y][x] + " ");
                }
                Console.WriteLine();
            }

            Circuit c = new Circuit();

            foreach (string[] item in LineArray)
            {
                switch (item[0].Substring(0, 1))
                {
                    case "R":
                        c.Add(
                            new Resistor(item[0], 
                            Convert.ToDouble(item[3]), 
                            new List<int>() 
                            { 
                                Int32.Parse(item[1]), 
                                Int32.Parse(item[2]) 
                            }));
                        break;
                   //TODO: feels a bit janky to be casting to int right here     
                    case "V":
                        c.Add(
                            new IndependentSource(item[0], 
                            Convert.ToDouble(item[3]), 
                            new List<int>() 
                            { 
                                Int32.Parse(item[1]), 
                                Int32.Parse(item[2]) 
                            }));
                        break;

                    default:
                        break;
                }
            }

            
            ModfiedNodalAnalysis mna = new ModfiedNodalAnalysis(c);
            
            Console.WriteLine(mna.Solve().ToString());

            Console.ReadLine();

        }
    }
}