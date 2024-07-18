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
            //Console.Writeline(args.Length);
            foreach (string arg in args)
            {
                //Console.Writeline(arg);
            }
            */
            string input = "";
            if (args.Length >= 1)
            {
                input = args[0];
            }
            else {
                throw new Exception("no input file");
            }

            if (!input.Contains(".skib"))
                throw new Exception("file is not of .skib");

            //Console.Writeline(input);



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

            //Console.Writeline("=================LEXER TEST================");
            /*
            foreach (Token tok in newTokens)
            {
                //Console.Writeline(tok.repr());
            }
            */
            //Console.Writeline("===================PARSER START===================");

            Parser parser = new Parser(newTokens);
            parser.start();
            int i = -1;
            foreach (ASTNode node in parser.getNodes())
            {

                if (node == null)
                    continue;

                //Console.Writeline($"========{i += 1}========");
                ASTNode.print(node);
            }

            Compiler compiler = new Compiler(parser.getNodes());
            compiler.start();


            // Set a variable to the Documents path.

            string outputPath = Directory.GetCurrentDirectory() + $"\\{input.Replace(".skib", ".lua")}";
            //Console.Writeline(outputPath);
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
