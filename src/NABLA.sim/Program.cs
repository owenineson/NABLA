using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using CommandLine;

namespace NABLA.sim
{
    class Program
    {

        class Options
        {
            [Option('p', "pipe", Required = false, HelpText = "Open NABLA with a pipe for use with a UI client")]
            public bool WithPipe { get; set; }
            
            [Option('f', "inputPath", Required = true, HelpText = "Path for the file to process")]
            public string ImportPath { get; set; }
            
            [Option('e', "exportPath", Required = false, HelpText = "Path to export formatted results too")]
            public string ExportPath { get; set; }

        }

        static void Main(string[] args)
        {
            //parse the input args and launch into the command handling method
            var result = Parser.Default
                .ParseArguments<Options>(args)
                .WithParsed((Options options) => RunCommand(options));
        }

        static void RunCommand(Options options)
        {
            if (options.WithPipe == true)
            {
                //open up a pipe and start listenting for file
            }
            //check that the supplied paths are blank, if not begin simulation
            else if (options.ImportPath != "")
            {
                RunConsoleMNA(options.ImportPath);
            }
        }

        static bool RunConsoleMNA(string path)
        {
            Netlist netlist = new Netlist();
            if (netlist.LoadNetlist(path) == false)
            {
                Console.WriteLine("Error - Program terminated");
                return false;
            }

            Circuit circuit = new Circuit();

            circuit.LoadFromNetlist(netlist);

            ModfiedNodalAnalysis mna = new ModfiedNodalAnalysis(circuit);

            Console.WriteLine(mna.Solve().ToString());

            return true;

            
        }

        //    while (true)
    //        {
    //            //harcoding the path for the netlist being used
    //            string FilePath = @"C:\\Users\\owen\\Documents\\NABLA\\src\\NABLA.sim\\ExampleNetlist.txt";

    //    //instantiate a new netlist with the path
    //    Netlist simpleNetlist = new Netlist();
    //            if (simpleNetlist.LoadNetlist(FilePath) == false)
    //            {
    //                Console.WriteLine("Program Terminated");
    //                break;
    //            }

    //Console.WriteLine(simpleNetlist);

    //            Circuit c = new Circuit();

    //c.LoadFromNetlist(simpleNetlist);

    //            ModfiedNodalAnalysis mna = new ModfiedNodalAnalysis(c);

    //Console.WriteLine(mna.Solve().ToString());

    //            Console.ReadLine();
    //        }

    }
}