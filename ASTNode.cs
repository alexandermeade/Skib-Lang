using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;


namespace SkibLang
{
    internal class ASTNode
    {
        private string offLoadedString = "";
        private Token token;
        private ASTNode left;
        private ASTNode right;
        public ASTNode(Token token)
        {
            this.token = token;
        }
        public ASTNode(Token token, ASTNode left, ASTNode right)
        {
            this.token = token;
            this.left = left;
            this.right = right;
        }

        public static void print(ASTNode node)
        {
            if (node == null)
                return;
            //Console.Writeline($"\nroot:{node.token.repr()} \n");
            /* if (node.token.getTokenTypeLiteral() == TokenType.funcCall) {
                 string output = "";
                 for (int i = 0; i < node.getToken().getParam().Count; i++)
                 {
                     output += $"\n\n~~~~{i}~~~~\n";
                     output += node.getToken().getParamValues(node.getToken().getParam()[i], 0);
                 }
             }*/
            if (node.left != null)
                //Console.Writeline($"\n\tleft:{node.left.token.repr()}");
            if (node.right != null)
                //Console.Writeline($"\n\tright:{node.right.token.repr()}");

            print(node.left);
            print(node.right);

        }

        public Token getToken() => this.token;

        public ASTNode getLeft() => this.left;
        public ASTNode getRight() => this.right;

        public void setOffLoadedString(string content) => this.offLoadedString = content;
        public string getOffLoadedString() => this.offLoadedString;
        public string repr() => $"\n\n\n\nroot:{token.getValueS()} \n\tleft:\t [{(this.left != null ? this.left.repr() : "none")}]" +
            $"\n\tright: \t[{(this.right != null ? this.right.repr() : "none")}]";
    }
}
