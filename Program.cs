using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Threading;
using System.Diagnostics.Metrics;

namespace SkibLang
{
    internal class Program
    {
        static void Main(string[] args)
        {
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

            string input = "";
            if (args.Length >= 1){
                for (int i = 0; i < args.Length-1; i++) {

                    switch(args[i]) {
                        case "--lexer-dump":
                            lexerDump = true;
                            continue;
                        case "--parser-dump":
                            parserDump = true;
                            continue;
                        case "-v":
                            Console.WriteLine("     __\n    |  |\n ___|  |\n(    .'\n )  ( \n");
                            continue;
                    }

                }
                input = args[args.Length-1];
            }
            else {
                throw new Exception("no arguments and or file");
            }

            if (!input.Contains(".skib"))
                throw new Exception("file is not of .skib");

            //Console.WriteLine(input);



            string contents = new StreamReader(input).ReadToEnd();
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
            foreach (ASTNode node in parser.getNodes())
            {

                if (node == null)
                    continue;

                //Console.WriteLine($"========{i += 1}========");
                ASTNode.print(node);
            }

            Compiler compiler = new Compiler(parser.getNodes());
            compiler.start();


            // Set a variable to the Documents path.

            string outputPath = Directory.GetCurrentDirectory() + $"\\{input.Replace(".skib", ".lua")}";
            //Console.WriteLine(outputPath);
            string docPath = "";
            if (args.Length >= 2)
                outputPath = args[1];

            System.IO.File.WriteAllText(outputPath, string.Empty);
            using (StreamWriter outputFile = new StreamWriter(Path.GetFullPath(outputPath), true))
            {
                outputFile.WriteLine(compiler.getOutput());
            }
        }
    }
}
