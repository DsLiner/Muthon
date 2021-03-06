﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LexerParser
{
    public class Lexer {

        private bool isEof = false;
        private char ch = ' '; 
        private StreamReader input;
        private String line = "";
        private int lineno = 0;
        private int col = 1;
        private String letters = "abcdefghijklmnopqrstuvwxyz"
            + "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private String digits = "0123456789";
        private char eofCh = '\0';
    

        public Lexer (String fileName) { // source filename
            try {
                input = new StreamReader(fileName);
            }
            catch (FileNotFoundException e) {
                Console.WriteLine("File not found: " + fileName);
                Environment.Exit(1);
            }
        }

        private char nextChar() { // Return next char
            if (ch == eofCh)
                error("Attempt to read past end of file");
            col++;
            if (col >= line.Length) {
                try {
                    line = input.ReadLine();
                } catch (IOException e) {
                    Console.Error.WriteLine(e);
                    Environment.Exit(1);
                } // try
                if (line == null) // at end of file
                    line = "" + eofCh;
                else {
                    // Console.WriteLine(lineno + ":\t" + line);
                    lineno++;
                    line += "\n";
                } // if line
                col = 0;
            } // if col
            return line[col];
        }
            

        public Token next() { // Return next token
            do {
                if (isLetter(ch)) { // ident or keyword
                    String spelling = concat(letters + digits);
                    return Token.keyword(spelling);
                } else if (isDigit(ch)) { // int or float literal
                    String number = concat(digits);
                    if (ch != '.')  // int Literal
                        return Token.mkIntLiteral(number);
                    number += concat(digits);
                    return Token.mkFloatLiteral(number);
                } else switch (ch) {
                case ' ': case '\r': 
                    ch = nextChar();
                    break;
            
                case '/':  // divide or comment
                    ch = nextChar();
                    if (ch != '/')  return Token.divideTok;
                    // comment
                    do {
                        ch = nextChar();
                    } while (ch != '\n');
                    ch = nextChar();
                    break;
            
                case '\'':  // char literal
                    String str = "\'";
                    ch = nextChar();
                    while(ch != '\'')
                    {
                        str = str + ch;
                        ch = nextChar();
                    }
                    ch = nextChar();
                    return Token.mkStrLiteral(str + "\'");
                
                case '\0': return Token.eofTok;
            
                case '+': ch = nextChar();
                    return Token.plusTok;

                // - * ( ) { } ; ,  student exercise
                case '-' : ch = nextChar();
            	    return Token.minusTok;
                case '*' : ch = nextChar();
                    if (ch == '*')
                    {
                        ch = nextChar();
                        return Token.powerTok;
                    }   
            	    return Token.multiplyTok;
                case '%': ch = nextChar();
                    return Token.modulusTok;
                case '(' : ch = nextChar();
        		    return Token.leftParenTok;
                case ')' : ch = nextChar();
        		    return Token.rightParenTok;
                case '\t' : ch = nextChar();
        		    return Token.tabTok;
                case '\n' : ch = nextChar();
        		    return Token.enterTok;
                case ':' : ch = nextChar();
        		    return Token.colonTok;
                case ',' : ch = nextChar();
        		    return Token.commaTok;
                
                case '&': check('&'); return Token.andTok;
                case '|': check('|'); return Token.orTok;

                case '=':
                    return chkOpt('=', Token.assignTok,
                                       Token.eqeqTok);
                    // < > !  student exercise
                case '<':
                    return chkOpt('=', Token.ltTok,
                                       Token.lteqTok);
                case '>':
                    return chkOpt('=', Token.gtTok,
                                       Token.gteqTok);
                case '!':
                    return chkOpt('=', Token.notTok,
                                       Token.noteqTok);
                default:
                    error("Illegal character " + ch);
                    break;
                } // switch
            } while (true);
        } // next


        private bool isLetter(char c) {
            return (c>='a' && c<='z' || c>='A' && c<='Z');
        }
  
        private bool isDigit(char c) {
    	    return (c>='0' && c<='9');
        }

        private void check(char c) {
            ch = nextChar();
            if (ch != c) 
                error("Illegal character, expecting " + c);
            ch = nextChar();
        }

        private Token chkOpt(char c, Token one, Token two) {
    	    // student exercise
    	    ch = nextChar();
            if (ch != c) 
                return one;
            ch = nextChar();
            return two;
        }

        private String concat(String set) {
            String r = "";
            do {
                r += ch;
                ch = nextChar();
            } while (set.IndexOf(ch) >= 0);
            return r;
        }

        public void error (String msg) {
            Console.Error.Write(line);
            Console.Error.WriteLine("Error: column " + col + " " + msg);
            Environment.Exit(1);
        }
        /*
        static public void main ( String[] argv ) {
            Lexer lexer = new Lexer(argv[0]);
            Token tok = lexer.next( );
            while (tok != Token.eofTok) {
                Console.WriteLine(tok.ToString());
                tok = lexer.next( );
            }
        } // main */

    }
}
