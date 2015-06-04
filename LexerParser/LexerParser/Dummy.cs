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
            String filename = "testfile.txt";
            // String filename = args[0];
            Parser parser = new Parser(new Lexer(filename), filename);
            Program prog = parser.program_start();
            prog.display();
        }
    }
}
