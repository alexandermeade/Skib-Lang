using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SkibLang
{
    internal class Compiler
    {

        private List<ASTNode> nodes = new List<ASTNode>();
        private ASTNode currNode;
        private string output = "";
        private string NameSpace;
        public Compiler(List<ASTNode> nodes)
        {
            this.nodes = nodes;
        }

        private void generateEqualCode(TokenType type, string varName, ASTNode right)
        {

            string varIdent = "";

            switch (type)
            {
                case TokenType.varDecleration:
                    varIdent = "local";
                    break;
                case TokenType.constDecleration:
                    varIdent = "";
                    break;
                case TokenType.Id:
                case TokenType.Index:
                    varIdent = "";
                    break;

                default:
                    throw new Exception("MISSING VAR DECLERATION TYPE");

            }
            this.output += $"\n{varIdent} {varName} = {Compiler.evaluateMath(right)}";

        }

        private void evaluateEqual()
        {
            ASTNode left = this.currNode.getLeft();
            ASTNode right = this.currNode.getRight();
            switch (left.getToken().getTokenTypeLiteral())
            {
                case TokenType.varDecleration:
                case TokenType.constDecleration:
                    generateEqualCode(left.getToken().getTokenTypeLiteral(), left.getRight().getToken().getValueS(), right);
                    break;
                case TokenType.Id:
                case TokenType.Index:
                    generateEqualCode(left.getToken().getTokenTypeLiteral(), left.getToken().getLiteralValue(), right);
                    break;

            }
        }

        private void evaluateEmbededCode()
        {
            ASTNode right = this.currNode.getRight();

            if (right == null)
                throw new Exception("ERROR EXPECTING [STRING] TYPE AFTER [ohio] Keyword");

            this.output += $"\n{right.getToken().getValueS()}";

        }
        private void evaluateImport()
        {
            ASTNode left = this.currNode.getLeft(); //var name
            ASTNode right = this.currNode.getRight(); // path

            if (left == null)
                throw new Exception("ERROR EXPECTING [STRING] TYPE AFTER [ohio] Keyword");

            if (right == null)
                throw new Exception("ERROR EXPECTING [Id] TYPE AFTER [String] ");


            this.output += $"\nlocal {right.getToken().getValueS()} = require \"{left.getToken().getValueS()}\"";

        }

        private static List<string> crackParam(Token funcCall)
        {
            List<string> result = new List<string>();
            foreach (List<Token> tokens in funcCall.getParam())
            {
                string output = "";
                foreach (Token token in tokens)
                {
                    output += token.getLiteralValue() + " ";
                }
                //Console.Writeline($"adding : {output}");
                result.Add(output);
            }
            return result;
        }

        private static string collectArgsString(ASTNode node)
        {
            List<string> tempArgs = Compiler.crackParam(node.getToken());
            bool first = true;
            string args = "";

            foreach (string str in tempArgs)
            {
                if (first)
                {
                    args += str;
                    first = false;
                    continue;
                }
                args += $", {str}";

            }

            return args;
        }

        private void evaluateFunctionDecl()
        {
            ASTNode left = this.currNode.getLeft(); //var name
            ASTNode right = this.currNode.getRight(); // path

            if (left == null)
                throw new Exception("ERROR EXPECTING [ID] or [ACCESS TYPE] AFTER [skibidi] Keyword");

            ASTNode pathNode = right;


            bool isGlobal = false;
            string name = "";
            string args = "";
            bool first = true;

            ASTNode n = null;

            if (right == null)
            {
                pathNode = left;
            }
            string value = "";
            switch (left.getToken().getTokenTypeLiteral())
            {
                case TokenType.pub:
                    isGlobal = true;
                    value = left.getRight().getToken().getValueS();
                    name = value == "_" ? "" : value;
                    n = left.getRight();
                    break;
                case TokenType.funcCall:
                    value = left.getToken().getValueS();
                    name = value == "_" ? "" : value;
                    isGlobal = name == "";
                    n = left;
                    break;


            }
            if (n != null)
                args = Compiler.collectArgsString(n);


            this.output += $"\n{(isGlobal ? "" : "local ")}function {name}({args})\n";

            switch (pathNode.getToken().getTokenTypeLiteral())
            {

                case TokenType.Block:
                    foreach (List<Token> tokens in pathNode.getToken().getParam())
                    {
                        tokens.Add(new Token(TokenType.EOF, "\\0"));
                        Parser parser = new Parser(tokens);
                        parser.start();
                        Compiler compiler = new Compiler(parser.getNodes());
                        compiler.start();
                        output += "\t" + compiler.output;
                    }
                    break;
                default:
                    this.output += Compiler.evaluateMath(this.currNode);
                    break;
            }
            this.output += "\nend\n";
        }

        private void evaluateFunctionCall()
        {
            if (this.currNode.getToken().getTokenTypeLiteral() != TokenType.funcCall)
                return;
            string args = "";

            //Console.Writeline($"\n\n\n\n\n ================PARSNIG{this.currNode.getToken().getValueS()}\n\n\n\n");

            foreach (List<Token> tokens in this.currNode.getToken().getParam())
            {
                tokens.Add(new Token(TokenType.EOF, "\\0"));
                Parser parser = new Parser(tokens);
                parser.start();
                Compiler compiler = new Compiler(parser.getNodes());
                compiler.start();
                args += compiler.getOutput();
                args += ',';
            }

         
            args = args.Remove(args.Length - 1, 1);

            //Console.Writeline(args);

            this.output += $"\n{this.currNode.getToken().getValueS()}({args})\n";
        }
        private static string evaluateFunctionCallStatic(ASTNode node)
        {
            if (node.getToken().getTokenTypeLiteral() != TokenType.funcCall)
                return "";
            string args = "";

            //Console.Writeline($"\n\n\n\n\n ================PARSNIG{this.currNode.getToken().getValueS()}\n\n\n\n");

            foreach (List<Token> tokens in node.getToken().getParam())
            {
                tokens.Add(new Token(TokenType.EOF, "\\0"));
                Parser parser = new Parser(tokens);
                parser.start();
                Compiler compiler = new Compiler(parser.getNodes());
                compiler.start();
                args += compiler.getOutput();
                args += ',';
            }


            args = args.Remove(args.Length - 1, 1);

            //Console.Writeline(args);

            return $"\n{node.getToken().getValueS()}({args})\n";
        }

        private static string evaluateBlock(ASTNode node) {
            string args = "";
            foreach (List<Token> tokens in node.getToken().getParam())
            {
                tokens.Add(new Token(TokenType.EOF, "\\0"));
                Parser parser = new Parser(tokens);
                parser.start();
                Compiler compiler = new Compiler(parser.getNodes());
                compiler.start();
                args += compiler.getOutput();
                args += '\n';
            }

            return args;

        }

        private static string evaluateMath(ASTNode node)
        {


            switch (node.getToken().getTokenTypeLiteral())
            {

                case TokenType.concat:
                    return $"({evaluateMath(node.getLeft())}) .. ({evaluateMath(node.getRight())})";
                case TokenType.Num:
                case TokenType.String:
                case TokenType.Id:
                case TokenType.List:
                case TokenType.Index:
                    return $"{node.getToken().getLiteralValue()}";



                case TokenType.funcCall:
                    return Compiler.evaluateFunctionCallStatic(node);

                case TokenType.Plus:
                    ASTNode left = node.getLeft();
                    ASTNode right = node.getRight();
                    if (left != null && right != null){
                        if (left.getToken().getTokenTypeLiteral() == TokenType.String && right.getToken().getTokenTypeLiteral() == TokenType.String)
                        {
                            return $"({evaluateMath(node.getLeft())}) .. ({evaluateMath(node.getRight())})";
                        }
                    }
                    return $"({evaluateMath(node.getLeft())}) {node.getToken().getValueS()} ({evaluateMath(node.getRight())})";

                case TokenType.Sub:
                case TokenType.Div:
                case TokenType.Mult:
                case TokenType.Mod:

                case TokenType.doubleEquals:
                    return $"({evaluateMath(node.getLeft())}) {node.getToken().getValueS()} ({evaluateMath(node.getRight())})";

                case TokenType.notEqual:
                    return $"({evaluateMath(node.getLeft())}) ~= ({evaluateMath(node.getRight())})";
                case TokenType.lessThan:
                case TokenType.greaterThan:
                case TokenType.lessThanOrEqualTo:
                case TokenType.greaterThanOrEqualTo:
                    return $"({evaluateMath(node.getLeft())}) {node.getToken().getValueS()} ({evaluateMath(node.getRight())})";
                case TokenType.And:

                    return $"({evaluateMath(node.getLeft())}) and ({evaluateMath(node.getRight())})";
                case TokenType.Or:

                    return $"({evaluateMath(node.getLeft())}) or {node.getToken().getValueS()} ({evaluateMath(node.getRight())})";

                case TokenType.Not:
                    return $"not {evaluateMath(node.getRight())}";
                case TokenType.EOF:
                    return "";
            
            }
            List<ASTNode> aSTNodes = new List<ASTNode>();
            aSTNodes.Add(node);
            Compiler compiler = new Compiler(aSTNodes);
            compiler.start();
            //Console.Writeline($"compiler output {compiler.output}");
            return compiler.output;
        }

        private void evaluateRet()
        {
            if (this.currNode.getToken().getTokenTypeLiteral() != TokenType.ret)
                return;

            this.output += $"\nreturn {Compiler.evaluateMath(this.currNode.getRight())}";
        }

        private void evaulateLoop()
        {

            ASTNode right = this.currNode.getRight();


            this.output += $"\nwhile true do\n";

            switch (right.getToken().getTokenTypeLiteral())
            {

                case TokenType.Block:
                    foreach (List<Token> tokens in right.getToken().getParam())
                    {
                        tokens.Add(new Token(TokenType.EOF, "\\0"));
                        Parser parser = new Parser(tokens);
                        parser.start();
                        Compiler compiler = new Compiler(parser.getNodes());
                        compiler.start();
                        output += "\t" + compiler.output;
                    }
                    break;
                default:

                    List<ASTNode> tokenss = new List<ASTNode>();
                    tokenss.Add(right);
                    Compiler compilerr = new Compiler(tokenss);
                    compilerr.start();
                    output += "\t" + compilerr.output;

                    break;
            }
            this.output += "\nend\n";
        }

        private void evaluateIf()
        {

            ASTNode left = this.currNode.getLeft();
            ASTNode right = this.currNode.getRight();

            this.output += $"\nif {Compiler.evaluateMath(left)} then \n";



            switch (right.getToken().getTokenTypeLiteral())
            {

                case TokenType.Block:
                    foreach (List<Token> tokens in right.getToken().getParam())
                    {
                        tokens.Add(new Token(TokenType.EOF, "\\0"));
                        Parser parser = new Parser(tokens);
                        parser.start();
                        Compiler compiler = new Compiler(parser.getNodes());
                        compiler.start();
                        output += "\t" + compiler.output;
                    }
                    break;
                default:
                    List<ASTNode> tokenss = new List<ASTNode>();
                    tokenss.Add(right);
                    Compiler compilerr = new Compiler(tokenss);
                    compilerr.start();
                    output += "\t" + compilerr.output;

                    break;
            }
            this.output += "\nend\n";
        }

        private static string evaluatePipeBody(ASTNode node)
        {

            ASTNode left = node.getLeft();
            ASTNode right = node.getRight();


            switch (node.getToken().getTokenTypeLiteral())
            {
                case TokenType.funcCall:
                case TokenType.String:
                case TokenType.Num:
                case TokenType.Index:
                case TokenType.Id:
                case TokenType.List:
                    return node.getToken().getLiteralValue();

                    // return (new Token(TokenType.funcCall, node.getToken().getValueS(), new List<List<Token>>())).getLiteralValue();

            }

            if (left == null || right == null)
                return "";

            string args = "";
            switch (right.getToken().getTokenTypeLiteral())
            {
                case TokenType.funcCall:

                    foreach (List < Token > tokens in right.getToken().getParam()) {
                        tokens.Add(new Token(TokenType.EOF, "\\0"));
                        Parser parser = new Parser(tokens);
                        parser.start();
                        Compiler compiler = new Compiler(parser.getNodes());
                        compiler.start();
                        args += compiler.getOutput();
                    }
                    break;
                case TokenType.Id:
                    args = "";
                    break;
            }
            return $"{right.getToken().getValueS()}({evaluatePipeBody(node.getLeft())} {(args.Length > 0 ? $", {args}" : "")})";

        }

        private void evaluatePipe()
        {


            this.output += evaluatePipeBody(this.currNode);

        }

        private void evaluateLiteral()
        {
            this.output += this.currNode.getToken().getLiteralValue();
        }
        private void evaluateSlot() { 
            ASTNode right = this.currNode.getRight();
            if (right == null)
                throw new Exception("no path found for slot [kaicenat]");
            
            string filePath = this.NameSpace + right.getToken().getValueS();

            //Console.Writeline(filePath);
            try {
                string content = new StreamReader(filePath).ReadToEnd();
                Lexer lexer = new Lexer(content += '\0');
                lexer.start();

                List<Token> tokens = lexer.getTokens();
                List<Token> newTokens = new List<Token>();
                for (int j = 0; j < tokens.Count; j++)
                {
                    if (tokens[j].getTokenTypeLiteral() != TokenType.Unknown && tokens[j].getTokenTypeLiteral() != TokenType.newLine)
                        newTokens.Add(tokens[j]);

                }

                Parser parser = new Parser(newTokens);
                parser.start();
                Compiler compiler = new Compiler(parser.getNodes());
                compiler.start();
                this.output += compiler.getOutput();
            }
            catch (Exception e) {
                Console.WriteLine($"couldn't open file : {filePath}");
            }

            
        }
        private void evaluateNamespace()
        {
            try
            {
                this.NameSpace = this.currNode.getRight().getToken().getValueS();
            }
            catch (Exception e) { 
                //Console.Writeline($"inncorrect or null value given for namespace [livvy] \n{e}");
            }
        }
        private void compile()
        {
            switch (this.currNode.getToken().getTokenTypeLiteral())
            {
                case TokenType.Equal:
                    this.evaluateEqual();
                    return;
                case TokenType.embed:
                    this.evaluateEmbededCode();
                    return;
                case TokenType.slot:
                    this.evaluateSlot();
                    return;
                case TokenType.import:
                    this.evaluateImport();
                    return;
                case TokenType.functionDecleration:
                    this.evaluateFunctionDecl();
                    return;
                case TokenType.funcCall:
                    this.evaluateFunctionCall();
                    return;
                case TokenType.If:
                    this.evaluateIf();
                    return;
                case TokenType.ret:
                    this.evaluateRet();
                    return;
                case TokenType.Break:
                    this.output += "\nbreak";
                    return;
                case TokenType.Continue:
                    this.output += "\ncontinue";
                    return;
                case TokenType.Loop:
                    this.evaulateLoop();
                    return;
                case TokenType.pipe:
                    output += "\n";
                    this.evaluatePipe();
                    return;
                case TokenType.Namespace:
                    this.evaluateNamespace();
                    return;

            }
            this.output += evaluateMath(this.currNode);
        }

        public void start()
        {
            foreach (ASTNode node in nodes)
            {
                this.currNode = node;
                this.compile();
            }

        }

        public string getOutput() => this.output;
    }
}
