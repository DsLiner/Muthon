﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexerParser
{
    class Token
    {
        private static int KEYWORDS = (int)TokenType.Eof;

        private static String[] reserved = new String[KEYWORDS];
        private static Token[] token = new Token[KEYWORDS];

        public static Token eofTok = new Token(TokenType.Eof, "<<EOF>>");
        public static Token boolTok = new Token(TokenType.Bool, "bool");
        public static Token charTok = new Token(TokenType.Char, "char");
        public static Token elseTok = new Token(TokenType.Else, "else");
        public static Token falseTok = new Token(TokenType.False, "false");
        public static Token floatTok = new Token(TokenType.Float, "float");
        public static Token ifTok = new Token(TokenType.If, "if");
        public static Token intTok = new Token(TokenType.Int, "int");
        public static Token mainTok = new Token(TokenType.Main, "main");
        public static Token trueTok = new Token(TokenType.True, "true");
        public static Token whileTok = new Token(TokenType.While, "while");
        public static Token leftBraceTok = new Token(TokenType.LeftBrace, "{");
        public static Token rightBraceTok = new Token(TokenType.RightBrace, "}");
        public static Token leftBracketTok = new Token(TokenType.LeftBracket, "[");
        public static Token rightBracketTok = new Token(TokenType.RightBracket, "]");
        public static Token leftParenTok = new Token(TokenType.LeftParen, "(");
        public static Token rightParenTok = new Token(TokenType.RightParen, ")");
        public static Token semicolonTok = new Token(TokenType.Semicolon, ";");
        public static Token commaTok = new Token(TokenType.Comma, ",");
        public static Token assignTok = new Token(TokenType.Assign, "=");
        public static Token eqeqTok = new Token(TokenType.Equals, "==");
        public static Token ltTok = new Token(TokenType.Less, "<");
        public static Token lteqTok = new Token(TokenType.LessEqual, "<=");
        public static Token gtTok = new Token(TokenType.Greater, ">");
        public static Token gteqTok = new Token(TokenType.GreaterEqual, ">=");
        public static Token notTok = new Token(TokenType.Not, "!");
        public static Token noteqTok = new Token(TokenType.NotEqual, "!=");
        public static Token plusTok = new Token(TokenType.Plus, "+");
        public static Token minusTok = new Token(TokenType.Minus, "-");
        public static Token multiplyTok = new Token(TokenType.Multiply, "*");
        public static Token divideTok = new Token(TokenType.Divide, "/");
        public static Token modulusTok = new Token(TokenType.Modulus, "%");
        public static Token andTok = new Token(TokenType.And, "&&");
        public static Token orTok = new Token(TokenType.Or, "||");

        private TokenType type;
        private String value = "";

        private Token (TokenType t, String v) {
            type = t;
            value = v;
            if ((int)t < (int)TokenType.Eof) {
                int ti = (int)t;
                reserved[ti] = v;
                token[ti] = this;
            }
        }

        public TokenType get_type() { return type; }

        public String get_value() { return value; }

        public static Token keyword  ( String name ) {
            char ch = name[0];
            if (ch >= 'A' && ch <= 'Z') return mkIdentTok(name);
            for (int i = 0; i < KEYWORDS; i++)
               if (name.CompareTo(reserved[i]) == 0)  return token[i];
            return mkIdentTok(name);
        } // keyword

        public static Token mkIdentTok (String name) {
            return new Token(TokenType.Identifier, name);
        }

        public static Token mkIntLiteral (String name) {
            return new Token(TokenType.IntLiteral, name);
        }

        public static Token mkFloatLiteral (String name) {
            return new Token(TokenType.FloatLiteral, name);
        }

        public static Token mkCharLiteral (String name) {
            return new Token(TokenType.CharLiteral, name);
        }

        public override String ToString() {
            if ((int)type < (int)TokenType.Identifier) return value;
            return type + "\t" + value;
        } // toString
        /*
        static void Main (String[] args) {
            Console.WriteLine(eofTok);
            Console.WriteLine(whileTok);
        }*/
    }

    public enum TokenType {
        Bool, Char, Else, False, Float,
        If, Int, Main, True, While,
        Eof, LeftBrace, RightBrace, LeftBracket, RightBracket,
        LeftParen, RightParen, Semicolon, Comma, Assign,
        Equals, Less, LessEqual, Greater, GreaterEqual,
        Not, NotEqual, Plus, Minus, Multiply,
        Divide, Modulus, And, Or, Identifier, IntLiteral,
        FloatLiteral, CharLiteral
    }
}
