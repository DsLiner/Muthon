using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexerParser
{
    public class Parser {
        // Recursive descent parser that inputs a C++Lite program and 
        // generates its abstract syntax.  Each method corresponds to
        // a concrete syntax grammar rule, which appears as a comment
        // at the beginning of the method.
  
        Token token;          // current token from the input stream
        Lexer lexer;
  
        public Parser(Lexer ts) { // Open the C++Lite source program
            lexer = ts;                          // as a token stream, and
            token = lexer.next();            // retrieve its first Token
        }
  
        private String match (TokenType t) { // * return the string of a token if it matches with t *
            String value = token.get_value();
            if (token.get_type() == t)
                token = lexer.next();
            else
                error(t);
            return value;
        }
  
        private void error(TokenType tok) {
            Console.Error.WriteLine("Syntax error: expecting: " + tok 
                               + "; saw: " + token);
            Environment.Exit(1);
        }
  
        private void error(String tok) {
            Console.Error.WriteLine("Syntax error: expecting: " + tok 
                               + "; saw: " + token);
            Environment.Exit(1);
        }
  
        public Program program_start() {
            // Program --> void main ( ) '{' Declarations Statements '}'
            TokenType[ ] header = {TokenType.Int, TokenType.Main,
                              TokenType.LeftParen, TokenType.RightParen};
            for (int i=0; i<header.Length; i++)   // bypass "int main ( )"
                match(header[i]);
            match(TokenType.LeftBrace);
            // student exercise
            Declarations decpart = declarations();
            Block body = statements();
            match(TokenType.RightBrace);
            return new Program(decpart, body);  // student exercise
        }
  
        private Declarations declarations () {
            // Declarations --> { Declaration }
            // student exercise
    	    Declarations ds = new Declarations();
    	    while (isType())
    	    {
    		    declaration(ds);
    	    }
    	    return ds;
        }
  
        private void declaration (Declarations ds) {
            // Declaration  --> Type Identifier { , Identifier } ;
            // student exercise
    	    Type t = type();
    	    while (!(token.get_type() == TokenType.Semicolon))
    	    {
    		    Variable id = new Variable(match(TokenType.Identifier));
    		    ds.Add(new Declaration(id, t));
    		    if(token.get_type() == TokenType.Comma)
    			    match(TokenType.Comma);
    	    }
    	    match(TokenType.Semicolon);
        }
  
        private Type type () {
            // Type  -->  int | bool | float | char 
            Type t = null;
            // student exercise
            if (token.get_type() == TokenType.Int)
        	    t = Type.INT;
            else if (token.get_type() == TokenType.Bool)
        	    t = Type.BOOL;
            else if (token.get_type() == TokenType.Float)
        	    t = Type.FLOAT;
            else if (token.get_type() == TokenType.Char)
        	    t = Type.CHAR;
            else error("int | bool | float | char");
            token = lexer.next();
            return t;          
        }
  
        private Statement statement() {
            // Statement --> ; | Block | Assignment | IfStatement | WhileStatement
            Statement s = new Skip();
            // student exercise
            if (token.get_type() == TokenType.Semicolon)
        	    token = lexer.next();
            else if (token.get_type() == TokenType.LeftBrace)
            {
        	    token = lexer.next();
        	    s = statements();
        	    match(TokenType.RightBrace);
            }
            else if (token.get_type() == TokenType.If)
        	    s = ifStatement();
            else if (token.get_type() == TokenType.While)
        	    s = whileStatement();
            else if (token.get_type() == TokenType.Identifier)
        	    s = assignment();
            else
        	    error("Illegal statement");
            return s;
        }
  
        private Block statements () {
            // Block --> '{' Statements '}'
            Block b = new Block();
            // student exercise
            while (!(token.get_type() == TokenType.RightBrace))
            {
        	    b.members.Add(statement());
            }
            return b;
        }
  
        private Assignment assignment () {
            // Assignment --> Identifier = Expression ;
            // student exercise
    	    Variable target = new Variable(match(TokenType.Identifier));
    	    match(TokenType.Assign);
    	    Expression source = expression();
    	    match(TokenType.Semicolon);
    	    return new Assignment(target, source);
        }
  
        private Conditional ifStatement () {
            // IfStatement --> if ( Expression ) Statement [ else Statement ]
            // student exercise
    	    match(TokenType.If);
    	    Expression test = expression();
    	    Statement thenbranch = statement();
    	    Statement elsebranch = new Skip();
    	    if (token.get_type() == TokenType.Else)
    	    {
    	       token = lexer.next();
    	       elsebranch = statement();
    	    }
    	    return new Conditional(test, thenbranch, elsebranch);
        }
  
        private Loop whileStatement () {
            // WhileStatement --> while ( Expression ) Statement
            // student exercise
    	    match(TokenType.While);
            match(TokenType.LeftParen);
            Expression test = expression();
            match(TokenType.RightParen);
            Statement body = statement();
            return new Loop(test, body);
        }

        private Expression expression () {
            // Expression --> Conjunction { || Conjunction }
            // student exercise
    	    Expression e = conjunction();
            while (token.get_type() == TokenType.Or)
            {
                Operator op = new Operator(token.get_value());
                token = lexer.next();
                Expression term2 = conjunction();
                e = new Binary(op, e, term2);
            }
            return e; 
        }
  
        private Expression conjunction () {
            // Conjunction --> Equality { && Equality }
            // student exercise
    	    Expression e = equality();
            while (token.get_type() == TokenType.And)
            {
                Operator op = new Operator(token.get_value());
                token = lexer.next();
                Expression term2 = equality();
                e = new Binary(op, e, term2);
            }
            return e;
        }
  
        private Expression equality () {
            // Equality --> Relation [ EquOp Relation ]
            // student exercise
    	    Expression e = relation();
            while (isEqualityOp())
            {
                Operator op = new Operator(token.get_value());
                token = lexer.next();
                Expression term2 = relation();
                e = new Binary(op, e, term2);
            }
            return e;
        }

        private Expression relation (){
            // Relation --> Addition [RelOp Addition] 
            // student exercise
    	    Expression e = addition();
            while (isRelationalOp())
            {
                Operator op = new Operator(token.get_value());
                token = lexer.next();
                Expression term2 = addition();
                e = new Binary(op, e, term2);
            }
            return e;
        }
  
        private Expression addition () {
            // Addition --> Term { AddOp Term }
            Expression e = term();
            while (isAddOp()) {
                Operator op = new Operator(match(token.get_type()));
                Expression term2 = term();
                e = new Binary(op, e, term2);
            }
            return e;
        }
  
        private Expression term () {
            // Term --> Factor { MultiplyOp Factor }
            Expression e = factor();
            while (isMultiplyOp()) {
                Operator op = new Operator(match(token.get_type()));
                Expression term2 = factor();
                e = new Binary(op, e, term2);
            }
            return e;
        }
  
        private Expression factor() {
            // Factor --> [ UnaryOp ] Primary 
            if (isUnaryOp()) {
                Operator op = new Operator(match(token.get_type()));
                Expression term = primary();
                return new Unary(op, term);
            }
            else return primary();
        }
  
        private Expression primary () {
            // Primary --> Identifier | Literal | ( Expression )
            //             | Type ( Expression )
            Expression e = null;
            if (token.get_type() == TokenType.Identifier) {
                e = new Variable(match(TokenType.Identifier));
            } else if (isLiteral()) {
                e = literal();
            } else if (token.get_type() == TokenType.LeftParen) {
                token = lexer.next();
                e = expression();       
                match(TokenType.RightParen);
            } else if (isType( )) {
                Operator op = new Operator(match(token.get_type()));
                match(TokenType.LeftParen);
                Expression term = expression();
                match(TokenType.RightParen);
                e = new Unary(op, term);
            } else error("Identifier | Literal | ( | Type");
            return e;
        }

        private Value literal( ) {
            // student exercise
    	    String s = null;
            switch (token.get_type())
            {
            case TokenType.IntLiteral:
                s = match(TokenType.IntLiteral);
                return new IntValue(int.Parse(s));
            case TokenType.CharLiteral:
                s = match(TokenType.CharLiteral);
                return new CharValue(s[0]);
            case TokenType.True:
                s = match(TokenType.True);
                return new BoolValue(true);
            case TokenType.False:
                s = match(TokenType.False);
                return new BoolValue(false);
            case TokenType.FloatLiteral:
                s = match(TokenType.FloatLiteral);
                return new FloatValue(float.Parse(s));
            }
            throw new ArgumentException( "should not reach here");
        }
  

        private bool isAddOp( ) {
            return token.get_type() == TokenType.Plus ||
                   token.get_type() == TokenType.Minus;
        }
    
        private bool isMultiplyOp( ) {
            return token.get_type() == TokenType.Multiply ||
                   token.get_type() == TokenType.Divide ||
                   token.get_type() == TokenType.Modulus;
        }
    
        private bool isUnaryOp( ) {
            return token.get_type() == TokenType.Not ||
                   token.get_type() == TokenType.Minus;
        }
    
        private bool isEqualityOp( ) {
            return token.get_type() == TokenType.Equals ||
                token.get_type() == TokenType.NotEqual;
        }
    
        private bool isRelationalOp( ) {
            return token.get_type() == TokenType.Less ||
                   token.get_type() == TokenType.LessEqual || 
                   token.get_type() == TokenType.Greater ||
                   token.get_type() == TokenType.GreaterEqual;
        }
    
        private bool isType( ) {
            return token.get_type() == TokenType.Int
                || token.get_type() == TokenType.Bool
                || token.get_type() == TokenType.Float
                || token.get_type() == TokenType.Char;
        }
    
        private bool isLiteral( ) {
            return token.get_type() == TokenType.IntLiteral ||
                isboolLiteral() ||
                token.get_type() == TokenType.FloatLiteral ||
                token.get_type() == TokenType.CharLiteral;
        }
    
        private bool isboolLiteral( ) {
            return token.get_type() == TokenType.True ||
                token.get_type() == TokenType.False;
        }
        /*    
        public static void main(String[] args) {
            Parser parser  = new Parser(new Lexer(args[0]));
            Program prog = parser.program();
            prog.display();           // display abstract syntax tree
        } //main*/

    } // Parser
}
