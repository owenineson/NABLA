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
            [Option('p', "pipe", Required = false, HelpText = "Open NABLA with a named pipe as specified")]
            public string PipeName { get; set; }
            
            [Option('f', "inputPath", Required = false, HelpText = "Path for the file to process")]
            public string ImportPath { get; set; }
            
            [Option('e', "exportPath", Required = false, HelpText = "Path to export formatted results too")]
            public string ExportPath { get; set; }

            [Option('v', "invisible", Required = false, HelpText = "Hide the console on opening")]
            public bool ConsoleInvisible { get; set; }

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
            if (options.ConsoleInvisible == true)
            {
                //hide the console
                
            }
            if (options.PipeName != null)
            {
                while (true)
                {

                    //open up a pipe and start listenting for file
                    PipeClient pc = new PipeClient(options.PipeName);
                    Console.WriteLine("\nOpening pipe...");
                    string netlist = pc.ReadFromPipe();

                    //clean up the netlist, remove whitespace, handle double escape chars and then split to an array
                    string[] netlistArray = netlist.Trim().Replace("\r\n\r\n", "\r\n").Split("\r\n");

                    Netlist n = new Netlist();

                    if (n.LoadNetlist(netlistArray) == false)
                    {
                        Console.WriteLine("Netlist load failed");
                    }
                    Console.WriteLine(n.ToString());

                    Circuit c = new Circuit();
                    c.LoadFromNetlist(n);

                    if (n.IsNetlistValid)
                    {
                        ModfiedNodalAnalysis mna = new ModfiedNodalAnalysis(c);
                        Console.WriteLine(mna.Solve());
                        pc.WriteToPipe(mna.Result);
                    }



                }
                

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

            Console.WriteLine(mna.Result);

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