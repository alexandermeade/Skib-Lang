using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Threading;
using System.Diagnostics.Metrics;
using System.Reflection;
using System.Diagnostics;
namespace SkibLang
{
    internal class Program
    {

        public static string helpContent = "\n-v : prints to the terminal the current version of skib lang you are using\n\n-h : prints out this help message to the terminal\n\ncompiler commands:\n\n--lexer-dump: prints out the token info after the lexing phase of compilation\n\n--parser-dump: prints out the node info after the parsing phase of compilation\n\n-r: run the compiled lua53 program";

        public static string compilerVersion = "2.0.0";
        public static string releaseDate = "7/24/2024";

        static void Main(string[] args) {

            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

           
            /*
            //Pass the file path and file name to the StreamReader constructor
            //Console.WriteLine(args.Length);
            foreach (string arg in args)
            {
                //Console.WriteLine(arg);
            }
            */

            bool lexerDump = false;
            bool parserDump = false;
            bool runAfterCompiling = false;

            string input = "";
            int commandCount = 0;
        
            if (args.Length <= 0) { 
                Console.WriteLine("Run skib.cs -h for help with compilation");
                return;
            }

            for (int i = 0; i < args.Length; i++) {

                switch(args[i]) {
                    case "--lexer-dump":
                        lexerDump = true;
                        commandCount++;
                        continue; 
                    case "--parser-dump":
                        parserDump = true;
                        commandCount++;
                        continue; 

                    case "-v":
                        Console.WriteLine($"     __  skib.cs:\n    |  | Compiler version:  (v{Program.compilerVersion})\n ___|  | release date: {Program.releaseDate}\n(    .'\n )__( \n");
                        commandCount++;
                        continue;
                    case "-h":
                        Console.WriteLine($"{helpContent}");
                        commandCount++;
                        continue;
                    case "-r":
                        runAfterCompiling = true;
                        continue;
                }

            }
            input = args[args.Length-1];
            

            if ((args.Length) - commandCount <= 0)
                return; 
            

            if (!input.Contains(".skib")) {
                Console.WriteLine($"ERROR: {input} isn't of extension .skib");
            }


            //Console.WriteLine(input);
            /* 
            string contents; 

            try {

                contents = new StreamReader(input).ReadToEnd();

            }catch (Exception e) {
                Console.WriteLine($"ERROR: unable to read from file {input}!");
                return;
            }
            */
            string contents;
            using (StreamReader streamReader = new StreamReader(input)) {
                contents = streamReader.ReadToEnd();
            } 

            Lexer lexer = new Lexer(contents + "\0");

            lexer.start();

            List<Token> tokens = lexer.getTokens();
            List<Token> newTokens = new List<Token>();

            for (int j = 0; j < tokens.Count; j++)
            {
                if (tokens[j].getTokenTypeLiteral() != TokenType.Unknown && tokens[j].getTokenTypeLiteral() != TokenType.newLine)
                    newTokens.Add(tokens[j]);

            }
            
            if (lexerDump) {
                Console.WriteLine("=================LEXER DUMP================");
                foreach (Token tok in newTokens) {
                    Console.WriteLine(tok.repr());
                }
            }
            
            //Console.WriteLine("===================PARSER START===================");
            
            Parser parser = new Parser(newTokens);
            parser.start();
            //int i = -1;
            if (parserDump){
                foreach (ASTNode node in parser.getNodes()){

                    if (node == null)
                        continue;

                    //Console.WriteLine($"========{i += 1}========");
                    ASTNode.print(node);
                }
            }

            Compiler compiler = new Compiler(parser.getNodes());
            compiler.start();


            // Set a variable to the Documents path.

            string outputPath = Directory.GetCurrentDirectory() + $"\\{input.Replace(".skib", ".lua")}";

            //Console.WriteLine(outputPath);
            string docPath = "";

            /*if (args.Length >= 0)
                outputPath = args[args.Length-1];*/

            System.IO.File.WriteAllText(outputPath, string.Empty);
            using (StreamWriter outputFile = new StreamWriter(Path.GetFullPath(outputPath), true))
            {

                outputFile.WriteLine(compiler.getOutput());
                outputFile.Close();
            }

            if (runAfterCompiling) {
                Program.runSelf(outputPath);
            }
        }
        
        private static void runSelf(string outputPath) {
            // The path to the executable you want to run
            string executablePath = "lua53";

            ProcessStartInfo startInfo = new ProcessStartInfo{ 
                FileName = executablePath, 
                Arguments = outputPath,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            // Initialize a new Process object
            Process process = new Process{
                StartInfo = startInfo
            };

            try {
                process.Start();

                // Read the standard output of the process
                string output = process.StandardOutput.ReadToEnd();
                // Read the standard error output of the process
                string error = process.StandardError.ReadToEnd();

                // Wait for the process to exit
                process.WaitForExit();

                // Output the results
                Console.WriteLine(output);
                Console.WriteLine(error);
            }
            catch (Exception ex){
                // Handle any exceptions
                Console.WriteLine("ERROR: unable to run through the lua53 Executable in your System Enviorments. \n Notes:\n\t if you do not have the lua53 Executable installed you can do so by visiting this link: https://www.lua.org/download.html");

                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
