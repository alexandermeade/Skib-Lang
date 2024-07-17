using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkibLang
{
    internal class Parser
    {

        private List<Token> tokens = new List<Token>();
        private List<ASTNode> nodes = new List<ASTNode>();
        private Token currToken = null;
        private bool inExpr = false;
        private int currIndex = 0;
        public Parser(List<Token> tokens)
        {

            this.tokens = tokens;
            this.currIndex = 0;
            if (tokens.Count <= 0)
            {
                this.currToken = null;
                return;
            }
            this.currToken = this.tokens[this.currIndex];
        }

        private void next(TokenType type)
        {




            if (this.currIndex + 1 >= this.tokens.Count)
                return;

            if (this.currToken.getTokenTypeLiteral() == type)
            {

                Token prev = this.currToken;
                this.currIndex += 1;

                this.currToken = this.tokens[this.currIndex];
                ////Console.Writeline($"\nprev token: {prev.repr()}, new Token{this.currToken.repr()}");
            }

            ////Console.Writeline($"ERR AT NEXT: {this.currToken.getTokenType()} vs {type}");
        }



        private ASTNode factor()
        {
            ////Console.Writeline($"{this.currToken.repr()} at factor");
            Token tok = this.currToken;
            ASTNode node;

            switch (tok.getTokenTypeLiteral())
            {

                case TokenType.Id:
                case TokenType.Num:
                case TokenType.String:
                case TokenType.Float:
                case TokenType.Block:
                case TokenType.funcCall:
                case TokenType.Break:
                case TokenType.Continue:
                case TokenType.EOF:
                case TokenType.List:
                case TokenType.Index:
                    this.next(tok.getTokenTypeLiteral());
                    return new ASTNode(tok);
                case TokenType.LParen:
                    this.next(TokenType.LParen);
                    node = this.expr();
                    this.next(TokenType.RParen);

                    return node;

                case TokenType.Loop:
                    this.next(tok.getTokenTypeLiteral());
                    return new ASTNode(tok, null, this.expr());
                //unary

                case TokenType.varDecleration:
                case TokenType.constDecleration:
                case TokenType.embed:
                case TokenType.pub:
                    this.next(tok.getTokenTypeLiteral());
                    return new ASTNode(tok, null, this.factor());

                case TokenType.ret:
                    this.next(tok.getTokenTypeLiteral());
                    return new ASTNode(tok, null, this.expr());


                case TokenType.import:
                    this.next(tok.getTokenTypeLiteral());
                    return new ASTNode(tok, this.factor(), this.factor());
                case TokenType.slot:
                case TokenType.Namespace:
                    this.next(tok.getTokenTypeLiteral());
                    return new ASTNode(tok, null, this.factor());
                case TokenType.functionDecleration:
                case TokenType.If:
                    this.next(tok.getTokenTypeLiteral());
                    return new ASTNode(tok, this.factor(), this.expr());

                case TokenType.BWNot:
                case TokenType.Not:
<<<<<<< HEAD
=======
                case TokenType.Sub:
>>>>>>> master
                    this.next(tok.getTokenTypeLiteral());
                    return new ASTNode(tok, null, this.factor());



            }



            throw new Exception($"ERROR AT PARSER.FACTOR {this.currToken.repr()}");
        }

        private ASTNode term()
        {
            ASTNode node = this.factor();

            //////Console.Writeline($"{this.currToken.repr()} at term");
            while (Token.isTerm(this.currToken.getTokenTypeLiteral()))
            {
                Token tok = this.currToken;
                switch (tok.getTokenTypeLiteral())
                {
                    case TokenType.Mult:
                    case TokenType.Div:
                    case TokenType.Mod:
                        /*case TT_BW_LEFT:
                        case TT_BW_RIGHT:
                        case TT_BW_AND:
                        case TT_BW_OR:
                        case TT_BW_XOR:*/
                        this.next(tok.getTokenTypeLiteral());
                        break;
                }

                node = new ASTNode(tok, node, this.factor());

            }
            //////Console.Writeline($"{node.repr()} at term");
            return node;
        }
        private ASTNode expr()
        {

            ASTNode node = this.term();

            ////Console.Writeline($"curr token in expr : {this.currToken.repr()}");

            ////Console.Writeline($"{this.currToken.repr()} at expr");
            while (Token.isOp(this.currToken.getTokenTypeLiteral()))
            {
                Token tok = this.currToken;


                switch (tok.getTokenTypeLiteral())
                {
                    case TokenType.Plus:
                    case TokenType.concat:
                        this.next(tok.getTokenTypeLiteral());
                        break;
                    case TokenType.pipe:
                        this.next(tok.getTokenTypeLiteral());
                        break;

                    case TokenType.Equal:
                        this.next(tok.getTokenTypeLiteral());

                        node = new ASTNode(tok, node, this.expr());
                        continue;
                    case TokenType.Sub:
                        this.next(tok.getTokenTypeLiteral());
                        break;

                    case TokenType.doubleEquals:
                    case TokenType.notEqual:
                    case TokenType.lessThan:
                    case TokenType.greaterThan:
                    case TokenType.lessThanOrEqualTo:
                    case TokenType.greaterThanOrEqualTo:

                        this.next(tok.getTokenTypeLiteral());
                        this.inExpr = true;
                        node = new ASTNode(tok, node, this.expr());
                        continue;
                    case TokenType.Or:
                        ////Console.Writeline($"\n==========curr and : {this.currToken.repr()}");
                        if (this.inExpr)
                        {
                            this.inExpr = false;
                            return node;
                        }


                        this.next(tok.getTokenTypeLiteral());
                        ////Console.Writeline($"curr and after next : {this.currToken.repr()}");
                        node = new ASTNode(tok, node, this.expr());
                        continue;

                    case TokenType.And:
                        ////Console.Writeline($"\n==========curr and : {this.currToken.repr()}");
                        if (this.inExpr)
                        {
                            this.inExpr = false;
                            return node;
                        }


                        this.next(tok.getTokenTypeLiteral());
                        ////Console.Writeline($"curr and after next : {this.currToken.repr()}");
                        node = new ASTNode(tok, node, this.expr());
                        continue;





                }
                node = new ASTNode(tok, node, this.term());
            }

            return node;
        }
        public void start()
        {
            this.nodes = new List<ASTNode>();
            if (this.currToken == null)
                return;
            while (this.currToken.getTokenTypeLiteral() != TokenType.EOF)
            {

                ////Console.Writeline("------------------------------------------");
                this.nodes.Add(this.expr());
            }

        }


        public List<ASTNode> getNodes() => this.nodes;

    }
}
