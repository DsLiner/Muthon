using System;
using System.Collections.ArrayList;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace LexerParser
{
    class Indenter
    {
	    public int level;
	    public Indenter(int nextlevel) { level = nextlevel; }

	    public void display(String message)
        {
	       // displays a message on the next line at the current level
	       String tab = "";
	       Console.WriteLine();
	       for (int i=0; i<level; i++)
	           tab = tab + "  ";
	       Console.Write(tab + message);
	    }
	} // Indenter

    class Program
    {
        // Program = Declarations decpart ; Block body
        Declarations decpart;
        Block body;

        Program (Declarations d, Block b)
        {
            decpart = d;
            body = b;
        }

	    public void display()
        {
		    // TODO Auto-generated method stub
		    int level = 0;
            Indenter indent = new Indenter(level);
            indent.display("Program (abstract syntax): ");
            decpart.display(level+1);
            body.display(level+1);
            Console.WriteLine();
	    }

    }

    class Declarations : ArrayList<Declaration> {
        // Declarations = Declaration*
        // (a list of declarations d1, d2, ..., dn)
	
	    public void display (int level) {
            Indenter indent = new Indenter(level);
            indent.display(getClass().toString().substring(6) + ": ");
            indent.display("  Declarations = {");
            String sep = "";
            foreach (Declaration dcl in this) {
                Console.WriteLine(sep);
                dcl.display();
                sep = ", ";
            }
            Console.WriteLine("}");
        }
    }

    class Declaration {
    // Declaration = Variable v; Type t
        Variable v;
        Type t;

        Declaration (Variable var, Type type) {
            v = var; t = type;
        } // declaration */

        public void display () {
            Console.Write("<" + v + ", " + t.toString() + ">");
        }
    }

    class Type {
        // Type = int | bool | char | float 
        public static Type INT = new Type("int");
        public static Type BOOL = new Type("bool");
        public static Type CHAR = new Type("char");
        public static Type FLOAT = new Type("float");
        // static Type UNDEFINED = new Type("undef");
    
        private String id;

        private Type (String t) { id = t; }

        public String toString ( ) { return id; }
    }

    abstract class Statement {
        // Statement = Skip | Block | Assignment | Conditional | Loop
	    public void display (int level) {
            Indenter indent = new Indenter(level);
            indent.display(getClass().toString().substring(6) + ": ");
       }
    }

    class Skip : Statement {
    }

    class Block : Statement {
        // Block = Statement*
        //         (a Vector of members)
        public ArrayList<Statement> members = new ArrayList<Statement>();

        public void display(int level) {
            base.display(level);
            foreach (Statement s in members) 
                s.display(level+1);
        }
    }

    class Assignment : Statement {
        // Assignment = Variable target; Expression source
        Variable target;
        Expression source;

        Assignment (Variable t, Expression e) {
            target = t;
            source = e;
        }

        public void display (int level) {
            base.display(level);
            target.display(level+1);
            source.display(level+1);
         }
    }

    class Conditional : Statement {
    // Conditional = Expression test; Statement thenbranch, elsebranch
        Expression test;
        Statement thenbranch, elsebranch;
        // elsebranch == null means "if... then"
    
        Conditional (Expression t, Statement tp) {
            test = t; thenbranch = tp; elsebranch = new Skip( );
        }
    
        Conditional (Expression t, Statement tp, Statement ep) {
            test = t; thenbranch = tp; elsebranch = ep;
        }
    
        public void display (int level) {
            base.display(level);
            test.display(level+1);
            thenbranch.display(level+1);
            Debug.Assert(elsebranch != null, "else branch cannot be null");
            elsebranch.display(level+1);
         }
    }

    class Loop : Statement {
    // Loop = Expression test; Statement body
        Expression test;
        Statement body;

        Loop (Expression t, Statement b) {
            test = t; body = b;
        }
    
        public void display (int level) {
            base.display(level);
            test.display(level+1);
            body.display(level+1);
         }
    }

    abstract class Expression {
        // Expression = Variable | Value | Binary | Unary
	    public void display (int level) {
            Indenter indent = new Indenter(level);
            indent.display(getClass().toString().substring(6) + ": ");
       }
    }

    class Variable : Expression {
        // Variable = String id
        private String id;

        Variable (String s) { id = s; }

        public String toString( ) { return id; }
    
        public bool equals (Object obj) {
            String s = ((Variable) obj).id;
            return id.equals(s); // case-sensitive identifiers
        }
    
        public int hashCode ( ) { return id.GetHashCode(); }

        public void display (int level) {
            base.display(level);
            Console.Write(id);
       }
    }

    abstract class Value : Expression {
        // Value = IntValue | BoolValue |
        //         CharValue | FloatValue
        protected Type type;
        protected bool undef = true;

        int intValue ( ) {
            Debug.Assert(false, "should never reach here");
            return 0;
        } // implementation of this function is unnecessary can can be removed.
    
        bool boolValue ( ) {
            Debug.Assert(false, "should never reach here");
            return false;
        }
    
        char charValue ( ) {
            Debug.Assert(false, "should never reach here");
            return ' ';
        }
    
        float floatValue ( ) {
            Debug.Assert(false, "should never reach here");
            return 0.0f;
        }

        bool isUndef( ) { return undef; }

        Type get_type ( ) { return type; }

        static Value mkValue (Type type) {
            if (type == Type.INT) return new IntValue( );
            if (type == Type.BOOL) return new BoolValue( );
            if (type == Type.CHAR) return new CharValue( );
            if (type == Type.FLOAT) return new FloatValue( );
            throw new IllegalArgumentException("Illegal type in mkValue");
        }
    }

    class IntValue : Value {
        private int value = 0;

        IntValue ( ) { type = Type.INT; }

        IntValue (int v) { this( ); value = v; undef = false; }

        int intValue ( ) {
            Debug.Assert(!undef, "reference to undefined int value");
            return value;
        }

        public String toString( ) {
            if (undef)  return "undef";
            return "" + value;
        }

        public void display (int level) {
            base.display(level);
            Console.Write(value);
        }
    }

    class BoolValue : Value {
        private bool value = false;

        BoolValue ( ) { type = Type.BOOL; }

        BoolValue (bool v) { this( ); value = v; undef = false; }

        bool boolValue ( ) {
            Debug.Assert(!undef, "reference to undefined bool value");
            return value;
        }

        int intValue ( ) {
            Debug.Assert(!undef, "reference to undefined bool value");
            return value ? 1 : 0;
        }

        public String toString( ) {
            if (undef)  return "undef";
            return "" + value;
        }

        public void display (int level) {
            base.display(level);
            Console.Write(value);
        }
    }

    class CharValue : Value {
        private char value = ' ';

        CharValue ( ) { type = Type.CHAR; }

        CharValue (char v) { this( ); value = v; undef = false; }

        char charValue ( ) {
            Debug.Assert(!undef, "reference to undefined char value");
            return value;
        }

        public String toString( ) {
            if (undef)  return "undef";
            return "" + value;
        }

        public void display (int level) {
            base.display(level);
            Console.Write(value);
        }
    }

    class FloatValue : Value {
        private float value = 0;

        FloatValue ( ) { type = Type.FLOAT; }

        FloatValue (float v) { this( ); value = v; undef = false; }

        float floatValue ( ) {
            Debug.Assert(!undef, "reference to undefined float value");
            return value;
        }

        public String toString( ) {
            if (undef)  return "undef";
            return "" + value;
        }

        public void display (int level) {
            base.display(level);
            Console.Write(value);
        }
    }

    class Binary : Expression {
    // Binary = Operator op; Expression term1, term2
        Operator op;
        Expression term1, term2;

        Binary (Operator o, Expression l, Expression r) {
            op = o; term1 = l; term2 = r;
        } // binary

        public void display (int level) {
            base.display(level);
            op.display(level+1);
            term1.display(level+1);
            term2.display(level+1);
         }
    }

    class Unary : Expression {
        // Unary = Operator op; Expression term
        Operator op;
        Expression term;

        Unary (Operator o, Expression e) {
            op = o; term = e;
        } // unary

        public void display (int level) {
            base.display(level);
            op.display(level+1);
            term.display(level+1);
         }
    }

    class Operator {
        // Operator = boolOp | RelationalOp | ArithmeticOp | UnaryOp
        // boolOp = && | ||
        static String AND = "&&";
        static String OR = "||";
        // RelationalOp = < | <= | == | != | >= | >
        static String LT = "<";
        static String LE = "<=";
        static String EQ = "==";
        static String NE = "!=";
        static String GT = ">";
        static String GE = ">=";
        // ArithmeticOp = + | - | * | /
        static String PLUS = "+";
        static String MINUS = "-";
        static String TIMES = "*";
        static String DIV = "/";
        // UnaryOp = !    
        static String NOT = "!";
        static String NEG = "-";
        // CastOp = int | float | char
        static String INT = "int";
        static String FLOAT = "float";
        static String CHAR = "char";
        // Typed Operators
        // RelationalOp = < | <= | == | != | >= | >
        static String INT_LT = "INT<";
        static String INT_LE = "INT<=";
        static String INT_EQ = "INT==";
        static String INT_NE = "INT!=";
        static String INT_GT = "INT>";
        static String INT_GE = "INT>=";
        // ArithmeticOp = + | - | * | / | %
        static String INT_PLUS = "INT+";
        static String INT_MINUS = "INT-";
        static String INT_TIMES = "INT*";
        static String INT_DIV = "INT/";
        static String INT_MOD = "INT%";
        // UnaryOp = !    
        static String INT_NEG = "-";
        // RelationalOp = < | <= | == | != | >= | >
        static String FLOAT_LT = "FLOAT<";
        static String FLOAT_LE = "FLOAT<=";
        static String FLOAT_EQ = "FLOAT==";
        static String FLOAT_NE = "FLOAT!=";
        static String FLOAT_GT = "FLOAT>";
        static String FLOAT_GE = "FLOAT>=";
        // ArithmeticOp = + | - | * | / | %
        static String FLOAT_PLUS = "FLOAT+";
        static String FLOAT_MINUS = "FLOAT-";
        static String FLOAT_TIMES = "FLOAT*";
        static String FLOAT_DIV = "FLOAT/";
        static String FLOAT_MOD = "FLOAT%";
        // UnaryOp = !    
        static String FLOAT_NEG = "-";
        // RelationalOp = < | <= | == | != | >= | >
        static String CHAR_LT = "CHAR<";
        static String CHAR_LE = "CHAR<=";
        static String CHAR_EQ = "CHAR==";
        static String CHAR_NE = "CHAR!=";
        static String CHAR_GT = "CHAR>";
        static String CHAR_GE = "CHAR>=";
        // RelationalOp = < | <= | == | != | >= | >
        static String BOOL_LT = "BOOL<";
        static String BOOL_LE = "BOOL<=";
        static String BOOL_EQ = "BOOL==";
        static String BOOL_NE = "BOOL!=";
        static String BOOL_GT = "BOOL>";
        static String BOOL_GE = "BOOL>=";
        // Type specific cast
        static String I2F = "I2F";
        static String F2I = "F2I";
        static String C2I = "C2I";
        static String I2C = "I2C";
    
        String val;
    
        Operator (String s) { val = s; }

        public String toString( ) { return val; }
        public bool equals(Object obj) { return val == obj; }
    
        bool boolOp ( ) { return val == AND || val == OR; }
        bool RelationalOp ( ) {
            return val == LT || val == LE || val == EQ
                || val == NE || val == GT || val == GE;
        }
        bool ArithmeticOp ( ) {
            return val == PLUS || val == MINUS
                || val == TIMES || val == DIV;
        }
        bool NotOp ( ) { return val == NOT ; }
        bool NegateOp ( ) { return val == NEG ; }
        bool intOp ( ) { return val == INT; }
        bool floatOp ( ) { return val == FLOAT; }
        bool charOp ( ) { return val == CHAR; }

        static String[ , ] intMap = {
            {PLUS, INT_PLUS}, {MINUS, INT_MINUS},
            {TIMES, INT_TIMES}, {DIV, INT_DIV},
            {EQ, INT_EQ}, {NE, INT_NE}, {LT, INT_LT},
            {LE, INT_LE}, {GT, INT_GT}, {GE, INT_GE},
            {NEG, INT_NEG}, {FLOAT, I2F}, {CHAR, I2C}
        };

        static String[ , ] floatMap = {
            {PLUS, FLOAT_PLUS}, {MINUS, FLOAT_MINUS},
            {TIMES, FLOAT_TIMES}, {DIV, FLOAT_DIV},
            {EQ, FLOAT_EQ}, {NE, FLOAT_NE}, {LT, FLOAT_LT},
            {LE, FLOAT_LE}, {GT, FLOAT_GT}, {GE, FLOAT_GE},
            {NEG, FLOAT_NEG}, {INT, F2I}
        };

        static String[ , ] charMap = {
            {EQ, CHAR_EQ}, {NE, CHAR_NE}, {LT, CHAR_LT},
            {LE, CHAR_LE}, {GT, CHAR_GT}, {GE, CHAR_GE},
            {INT, C2I}
        };

        static String[ , ] boolMap = {
            {EQ, BOOL_EQ}, {NE, BOOL_NE}, {LT, BOOL_LT},
            {LE, BOOL_LE}, {GT, BOOL_GT}, {GE, BOOL_GE},
        };

        static private Operator map (String[][] tmap, String op) {
            for (int i = 0; i < tmap.length; i++)
                if (tmap[i][0] == op)
                    return new Operator(tmap[i][1]);
            Debug.Assert(false, "should never reach here");
            return null;
        }

        static public Operator intMap (String op) {
            return map (intMap, op);
        }

        static public Operator floatMap (String op) {
            return map (floatMap, op);
        }

        static public Operator charMap (String op) {
            return map (charMap, op);
        }

        static public Operator boolMap (String op) {
            return map (boolMap, op);
        }

        public void display (int level) {
            Indenter indent = new Indenter(level);
            indent.display(getClass().toString().substring(6) + ": " + val);
       }
    }

}
