using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SkibLang
{

    internal enum TokenType
    {
        EOF,
        Unknown,
        Id,
        Num,
        Float,
        String,
        Block,
        Plus,
        Sub,
        Mult,
        Div,
        Mod,
        LParen,
        RParen,
        LCBrace,
        RCBrace,
        Equal,
        varDecleration,
        constDecleration,
        comment,
        functionDecleration,
        embed,
        import,
        funcCall,
        comma,
        pub,
        newLine,
        ret,
        pipe,
        If,
        Loop,
        Break,
        Continue,
        doubleEquals,
        notEqual,
        lessThan,
        greaterThan,
        lessThanOrEqualTo,
        greaterThanOrEqualTo,
        BWLeft,
        BWRight,
        BWAnd,
        BWOr,
        BWXor,
        BWNot,
        And,
        Or,
        Not,
        LBracket,
        RBracket,
        List,
        Index,
        use,
        concat,
        slot, 
        Namespace,

    }
<<<<<<< HEAD
=======

>>>>>>> master
    internal class Token
    {
        private long valueL = 0;
        private double valueD = 0f;
        private string valueS = "";
        private List<Token> args = new List<Token>();
        private List<List<Token>> param = new List<List<Token>>();
        private TokenType type;

        public Token(TokenType type, string valueS)
        {
            this.type = type;
            switch (this.type)
            {
                case TokenType.Num:
                    this.valueL = long.Parse(valueS);
                    break;
                case TokenType.Float:
                    this.valueD = double.Parse(valueS);
                    break;



            }
            this.valueS = valueS;
        }

        public Token(TokenType type, string listName, List<Token> args)
        {
            this.type = type;
            this.valueS = listName;
            this.args = args;
        }
        public Token(TokenType type, string name, List<List<Token>> param)
        {
            this.valueS = name;
            this.type = type;
            this.param = param;
        }


        public static bool isInstruction(string value)
        {
            switch (value)
            {
                case "rizz":

                case "sigma":

                case "tiktokrizzparty":

                case "skibidi":

                case "ohio":

                case "bussin":

                case "mrworldwide":

                case "lowtaperfade":


                case "caseoh":

                case "cuck":

                case "gyatt":

                case "sus":
                case "kaicenat":
                case "livvy":
                    return true;

            }
            return false;
        }

        public static Token toInstruction(string value)
        {

            switch (value)
            {
                case "rizz":
                    return new Token(TokenType.varDecleration, value);
                case "sigma":
                    return new Token(TokenType.constDecleration, value);
                case "tiktokrizzparty":
                    return new Token(TokenType.comment, value);
                case "skibidi":
                    return new Token(TokenType.functionDecleration, value);
                case "ohio":
                    return new Token(TokenType.embed, value);
                case "bussin":
                    return new Token(TokenType.import, value);
                case "mrworldwide":
                    return new Token(TokenType.pub, value);
                case "lowtaperfade":
                    return new Token(TokenType.ret, value);

                case "caseoh":
                    return new Token(TokenType.Break, value);
                case "cuck":
                    return new Token(TokenType.Continue, value);
                case "gyatt":
                    return new Token(TokenType.Loop, value);
                case "sus":
                    return new Token(TokenType.If, value);
                case "fortnite":
                    return new Token(TokenType.use, value);
                case "kaicenat":
                    return new Token(TokenType.slot, value);
                case "livvy":
                    return new Token(TokenType.Namespace, value);


            }
            return new Token(TokenType.Id, value);

        }

        public static Token toInstructionFunc(Token token)
        {
            /*
            switch (token.getValueS())
            {
                case "sus":
                    token.setType(TokenType.If);
                    break;

            }*/
            return token;
        }


        public static bool isTerm(TokenType type)
        {

            switch (type)
            {
                case TokenType.Mult:
                case TokenType.Div:
                case TokenType.Mod:
                    return true;
            }
            return false;

        }

        public static bool isOp(TokenType type)
        {
            switch (type)
            {
                case TokenType.Plus:
                case TokenType.Sub:
                case TokenType.Equal:
                case TokenType.doubleEquals:
                case TokenType.notEqual:
                case TokenType.lessThan:
                case TokenType.greaterThan:
                case TokenType.lessThanOrEqualTo:
                case TokenType.greaterThanOrEqualTo:
                case TokenType.And:
                case TokenType.Or:
                case TokenType.pipe:
                case TokenType.concat:
                    return true;
            }
            return false;
        }



        public long getValueL() => this.valueL;
        public double getValueD() => this.valueD;
        public string getValueS() => this.valueS;

        public TokenType getTokenTypeLiteral() => this.type;
        public string getTokenType() => this.type.ToString();

        public List<Token> GetArgs() => this.args;

        private string getLiteralValueList(Token listToken)
        {

            string output = "{";
            bool first = true;
            foreach (List<Token> tokens in listToken.getParam())
            {
                foreach (Token token in tokens)
                {

                    output += $"{token.getLiteralValue()}";

                }

                output += $",";
            }
            output = output.Remove(output.Length - 1, 1);
            output += "}";
            return output;
        }
        private string getLiteralValueFCBody(Token funcCall)
        {

            string output = "";
            bool first = true;
            int i = 0;



            foreach (List<Token> tokens in funcCall.getParam())
            {
                bool ran = false;
                foreach (Token token in tokens)
                {
                    ran = true;
                    string buffer = "";
                    switch (token.getTokenTypeLiteral())
                    {
                        case TokenType.funcCall:
                            buffer = $"{token.getLiteralValueFC(token)}";
                            break;
                        case TokenType.List:
                            buffer = $"{token.getLiteralValue()},";
                            break;
                        default:
                            buffer = $"{token.getLiteralValue()}";
                            break;

                    }
                    if (first)
                    {
                        output += buffer;
                        first = false;
                        continue;
                    }
                    output += buffer;

                }
                output += $",";

            }

            Console.WriteLine($"===============================================================${output}");

            if (output.Length - 1 < 0)
                return output;

            return output.Remove(output.Length - 1);
        }

        private string getLiteralValueFC(Token funcCall)
        {

            string output = $"{funcCall.getValueS()}({getLiteralValueFCBody(funcCall)})";
            return output;
        }

        private static string getIndexValues(Token indexToken, int index)
        {
            if (index >= indexToken.GetArgs().Count)
                return "";
            if (index == indexToken.GetArgs().Count - 1)
                return $"{indexToken.args[index].getLiteralValue()}";
            return $"{indexToken.args[index].getLiteralValue()} {getIndexValues(indexToken, index + 1)}";

        }
        private static string getLiteralValueIndex(Token index)
        {

            return $"{index.getValueS()}[{getIndexValues(index, 0)}]";

        }
        public string getLiteralValue()
        {
            switch (this.type)
            {
                case TokenType.String:
                    return $"\"{this.valueS}\"";
                case TokenType.List:
                    return getLiteralValueList(this);
                case TokenType.funcCall:
                    return getLiteralValueFC(this);
                case TokenType.Index:
                    return getLiteralValueIndex(this);

            }
            return this.getValueS();
        }

        private string getValues(int index)
        {
            if (index >= this.args.Count)
                return "None";
            return $"{this.args[index].repr()}, {this.getValues(index + 1)}";
        }

        public string getParamValues(List<Token> tokens, int index)
        {
            if (index >= this.args.Count)
                return "None";
            return $"balls: {tokens[index].getValueS()} {tokens[index].getTokenTypeLiteral()}, {this.getParamValues(tokens, index + 1)}";
        }

        public void printFuncCall()
        {

            if (this.getTokenTypeLiteral() != TokenType.funcCall)
                return;
            int i = 0;
            foreach (List<Token> list in this.getParam())
            {
                Console.WriteLine($"======start {i}======");
                foreach (Token tok in list)
                {
                    Console.WriteLine(tok.repr());
                }
                Console.WriteLine($"======end {i}======");
                i += 1;
            }
        }

        public string repr()
        {
            switch (this.type)
            {
                case TokenType.Block:
                case TokenType.funcCall:
                case TokenType.List:
                    string output = $"{this.getValueS()} [{this.getTokenTypeLiteral()}] [";
                    int i = 0;
                    foreach (List<Token> list in this.getParam())
                    {
                        output += $"\n[";
                        foreach (Token tok in list)
                        {
                            output += "," + tok.repr();
                        }
                        output += $"]\n";
                        i += 1;
                    }
                    output += "\n]\n";
                    return output;
                default:
                    return $"{this.getValueS()} [{this.getTokenType()}]";

            }
        }

        public void setType(TokenType type) => this.type = type;
        public List<List<Token>> getParam() => this.param;
    }

}
