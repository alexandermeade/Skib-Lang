using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkibLang
{
    internal class Lexer
    {

        private string source;
        private int index;
        private List<Token> tokens;
        private char currChar;

        public Lexer(string source)
        {
            this.source = source;
            this.index = 0;
            this.tokens = new List<Token>();
            this.currChar = this.source[this.index];
        }

        private void next()
        {
            if (this.index + 1 >= this.source.Length)
                return;
            this.index++;
            this.currChar = this.source[this.index];
        }

        private char getNext()
        {
            return this.source[this.index + 1];
        }
        private void back()
        {
            if (this.index - 1 < 0)
                return;
            this.index--;
            this.currChar = this.source[this.index];
        }
        private Token parseSymbol()
        {
            string result = "";
            while (char.IsLetter(this.currChar) || char.IsDigit(this.currChar) || this.currChar == '_' || this.currChar == '.' || this.currChar == ':')
            {
                result += this.currChar;
                this.next();
            }
            this.back();
            return new Token(TokenType.Id, result);
        }

        private Token parseString()
        {
            string result = "";
            while (this.currChar != '"')
            {

                if (this.currChar == '\\')
                {
                    this.next();
                    result += this.currChar;
                    this.next();
                    continue;
                }

                if (this.currChar == '\0')
                    throw new Exception("COULDN'T FIND CLOSING \"");

                result += this.currChar;
                this.next();
            }

            return new Token(TokenType.String, result);
        }

        private Token parseNum()
        {
            string result = "";
            while (char.IsDigit(this.currChar))
            {
                result += this.currChar;
                this.next();
            }
            this.back();

            //Console.Writeline($"curr char at parseNum: {this.currChar}");

            return new Token(TokenType.Num, result);
        }

        private void skipWhitespace()
        {
            while (this.currChar == ' ' || this.currChar == '\t' || this.currChar == '\n')
                this.next();
        }

        private Token lex()
        {

            //Console.Writeline($"curr char {this.currChar}");

            if (char.IsLetter(this.currChar) || this.currChar == '_')
            {
                Token temp = this.parseSymbol();
                Token tok = Token.toInstruction(temp.getValueS());

              
                this.skipWhitespace();

                if (this.getNext() == '[')
                {
                    //Console.Writeline("==== parse []");
                    this.next();
                    this.next();
                    List<Token> tokens = new List<Token>();

                    while (true)
                    {


                        Token token = this.lex();
                        this.next();
                        //Console.Writeline(token.repr());
                        if (token.getTokenTypeLiteral() == TokenType.EOF)
                            throw new Exception("SToP");
                        if (token.getTokenTypeLiteral() == TokenType.RBracket)
                        {
                            break;
                        }

                        tokens.Add(token);

                    }
                    this.back();

                    Token tk = new Token(TokenType.Index, tok.getValueS(), tokens);
                    return tk;
                }
                this.skipWhitespace();
                if (this.getNext() == '(')
                {
                    //Console.Writeline("==== parse funcall");
                    this.next();
                    List<List<Token>> param = new List<List<Token>>();
                    List<Token> tokens = new List<Token>();
                    this.next();
                    int nest = 0;
                    while (true)
                    {
                        Token token = this.lex();

                        if (token.getTokenTypeLiteral() == TokenType.comma)
                        {
                            param.Add(tokens);
                            tokens = new List<Token>();
                            this.next();
                            continue;
                        }
                        if (token.getTokenTypeLiteral() == TokenType.RParen && nest == 0)
                        {
                            param.Add(tokens);
                            tokens = new List<Token>();
                            this.next();
                            break;
                        }


                        tokens.Add(token);
                        this.next();
                    }
                    this.back();
                    //Console.Writeline($"\n======= param length : {param.Count}");
                    Token tk = new Token(TokenType.funcCall, tok.getValueS(), param);
                    return Token.toInstructionFunc(tk);

                }
                if (tok.getTokenTypeLiteral() == TokenType.comment)
                {
                    while (this.currChar != '\n' && this.currChar != '\0')
                        this.next();
                    return this.lex();
                }
                return Token.toInstructionFunc(tok);
            }

            if (char.IsDigit(this.currChar))
            {
                return this.parseNum();
            }

            //rizz a = 3
            //sigma is the constant keyword

            switch (this.currChar)
            {
                case '\n':

                    return new Token(TokenType.newLine, "\\n");
                case ' ':
                case '\t':
                    this.next();
                    return this.lex();


                case ',':
                    return new Token(TokenType.comma, ",");
                case '\0':
                    return new Token(TokenType.EOF, "\\0");
                case '=':
                    /*
                     *         doubleEquals,
        notEqual,
        lessThan,
        greaterThan,
        lessThanOrEqualTo,
        greaterThanOrEqualto,*/
                    if (this.getNext() == '=')
                    {
                        this.next();
                        return new Token(TokenType.doubleEquals, "==");
                    }
                    return new Token(TokenType.Equal, char.ToString(this.currChar));
                case '!':
                    if (this.getNext() == '=')
                    {
                        this.next();
                        return new Token(TokenType.notEqual, "!=");
                    }
                    return new Token(TokenType.Not, char.ToString(this.currChar));
                case '<':
                    if (this.getNext() == '=')
                    {
                        this.next();
                        return new Token(TokenType.lessThanOrEqualTo, "<=");
                    }
                    if (this.getNext() == '<')
                    {
                        this.next();
                        return new Token(TokenType.BWLeft, "<<");
                    }
                    return new Token(TokenType.greaterThan, char.ToString(this.currChar));
                case '>':
                    if (this.getNext() == '=')
                    {
                        this.next();
                        return new Token(TokenType.greaterThanOrEqualTo, ">=");
                    }
                    if (this.getNext() == '>')
                    {
                        this.next();
                        return new Token(TokenType.BWRight, ">>");
                    }
                    return new Token(TokenType.lessThan, char.ToString(this.currChar));
                case '&':
                    if (this.getNext() == '&')
                    {
                        this.next();
                        return new Token(TokenType.And, "&&");
                    }
                    return new Token(TokenType.BWAnd, char.ToString(this.currChar));
                case '|':
                    if (this.getNext() == '|')
                    {
                        this.next();
                        return new Token(TokenType.Or, "||");
                    }
                    return new Token(TokenType.BWOr, char.ToString(this.currChar));
                case '^':
                    return new Token(TokenType.BWXor, char.ToString(this.currChar));
                case '~':
                    return new Token(TokenType.BWNot, char.ToString(this.currChar));
                case '+':
                    if (this.getNext() == '+')
                    {
                        this.next();
                        return new Token(TokenType.concat, "++");
                    }
                    return new Token(TokenType.Plus, char.ToString(this.currChar));
                case '-':
                    return new Token(TokenType.Sub, char.ToString(this.currChar));
                case '/':
                    return new Token(TokenType.Div, char.ToString(this.currChar));
                case '*':
                    return new Token(TokenType.Mult, char.ToString(this.currChar));
                case '%':
                    return new Token(TokenType.Mod, char.ToString(this.currChar));

                case ':':
                    if (this.getNext() == '3')
                    {
                        this.next();
                        return new Token(TokenType.pipe, ":3");
                    }
                    return new Token(TokenType.Unknown, char.ToString(this.currChar));

                case '{':
                    List<List<Token>> param = new List<List<Token>>();
                    List<Token> tokens = new List<Token>();
                    this.next();

                    while (true)
                    {
                        Token token = this.lex();
                        if (token.getTokenTypeLiteral() == TokenType.EOF)
                        {
                            throw new Exception("failed to find closing {");
                        }
                        if (token.getTokenTypeLiteral() == TokenType.newLine)
                        {
                            param.Add(tokens);
                            tokens = new List<Token>();
                            this.next();
                            continue;
                        }
                        if (token.getTokenTypeLiteral() == TokenType.RCBrace || token.getTokenTypeLiteral() == TokenType.EOF)
                        {
                            param.Add(tokens);
                            tokens = new List<Token>();
                            this.next();
                            break;
                        }
                        if (token.getTokenTypeLiteral() != TokenType.Unknown && token.getTokenTypeLiteral() != TokenType.newLine)
                            tokens.Add(token);
                        this.next();
                    }
                    this.back();
                    return new Token(TokenType.Block, "BLOCK", param);
                case '[':
                    param = new List<List<Token>>();
                    tokens = new List<Token>();
                    this.next();

                    while (true)
                    {
                        Token token = this.lex();
                        if (token.getTokenTypeLiteral() == TokenType.EOF)
                        {
                            throw new Exception("failed to find closing [");
                        }
                        if (token.getTokenTypeLiteral() == TokenType.comma)
                        {
                            param.Add(tokens);
                            tokens = new List<Token>();
                            this.next();
                            continue;
                        }
                        if (token.getTokenTypeLiteral() == TokenType.RBracket || token.getTokenTypeLiteral() == TokenType.EOF)
                        {
                            param.Add(tokens);
                            tokens = new List<Token>();
                            this.next();
                            break;
                        }
                        if (token.getTokenTypeLiteral() != TokenType.Unknown && token.getTokenTypeLiteral() != TokenType.newLine)
                            tokens.Add(token);
                        this.next();
                    }
                    this.back();
                    return new Token(TokenType.List, "List", param);
                case ']':
                    return new Token(TokenType.RBracket, "]");

                case '}':
                    return new Token(TokenType.RCBrace, "}");
                case '"':
                    this.next();
                    return parseString();
                case '(':
                    return new Token(TokenType.LParen, char.ToString(this.currChar));
                case ')':
                    return new Token(TokenType.RParen, char.ToString(this.currChar));


            }
            return new Token(TokenType.Unknown, char.ToString(this.currChar));
        }

        public void start()
        {
            while (this.currChar != '\0')
            {
                //Console.Writeline(this.currChar);
                Token tok = this.lex();
                this.next();
                if (tok.getTokenTypeLiteral() == TokenType.Unknown)
                    continue;
                this.tokens.Add(tok);

            }
            this.tokens.Add(new Token(TokenType.EOF, "\\0"));
        }

        public List<Token> getTokens() => this.tokens;
    }
}
