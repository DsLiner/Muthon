using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexerParser
{
    class Dummy
    {
        static void Main(String[] args)
        {
            Parser parser = new Parser(new Lexer("example.txt"));
            Program prog = parser.program_start();
            prog.display();
        }
    }
}
