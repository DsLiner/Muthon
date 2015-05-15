%{
	#include <stdio.h>
	#include <stdlib.h>
	#include <string.h>
	#include <math.h>
	#define YYDEBUG 1
%}
%union {
	int		int_value;
	double		double_value;
}
%token ArrayLeft RightBracket LeftParen RightParen
%token Comma Assign Equal Less LessEqual Greater
%token GreaterEqual Not NotEqual Plus Minus Multiply
%token Exponent Divide Modulus And Or If Elif Else
%token While Colon Indent NextLine Definition
%token Type Id Integer Boolean Float String
%%
Program
	: Functions Main
	;
Functions
	: Function
	| Functions Function
	;
Function
	: Definition Id LeftParen RightParen Colon NextLine Indents
	| Definition Id LeftParen Id RightParen Colon NextLine Indents
	| Definition Id LeftParen Id Parameter RightParen Colon NextLine Indents
	;
Parameter
	: Comma Id
	| Parameter Comma Id
	;
Main
	: Statements
	;
Statements
	: Statement NextLine
	| Statements Statement NextLine
	;
Statement
	: Assignment
	| IfStatement
	| WhileStatement
	| FunctionCall
	;
Indents
	: Indent Statement
	| Indents Indent Statement
	;
Assignment
	: Id Assign Expression
	| Id Assign List
	| Id Assign FunctionCall
	| Id ArrayExpression Assign Expression
	| Id ArrayExpression Assign List
	| Id ArrayExpression Assign FunctionCall
	;
ArrayExpression
	: LeftBracket Expression RightBracket
	| ArrayExpression LeftBracket Expression RightBracket
	;
IfStatement
	: If LeftParen Condition RightParen Colon NextLine Indents
	| If LeftParen Condition RightParen Colon NextLine Indents ElifStatement
	| If LeftParen Condition RightParen Colon NextLine Indents ElseStatement
	| If LeftParen Condition RightParen Colon NextLine Indents ElifStatement ElseStatement
	;
ElifStatement
	: Elif LeftParen Condition RightParen Colon NextLine Indents
	| ElifStatement Elif LeftParen Condition RightParen Colon NextLine Indents
	;
ElseStatement
	: Else Colon NextLine Indents
	;
WhileStatement
	: While LeftParen Condition RightParen Colon NextLine Indents
	;
Condition
	: Id
	| Id LeftBracket Expression RightBracket
	| Expression
	;
FunctionCall
	: Id LeftParen RightParen
	| Id LeftParen Id RightParen
	| Id LeftParen Id Parameter RightParen
	;
List
	: LeftBracket Expression RightBracket
	| LeftBracket List RightBracket
	| LeftBracket Expression ListAddition RightBracket
	| LeftBracket List ListAddition RightBracket
	;
ListAddition
	: Comma Expression
	| Comma List
	;
Expression
	: Conjunction
	| Expression Or Conjunction
	;
Conjunction
	: Equality
	| Conjunction And Equality
	;
Equality
	: Relation
	| Relation Equal Relation
	{
		$$ = $1 == $3;
	}
	| Relation NotEqual Relation
	{
		$$ = $1 != $3;
	}
	;
Relation
	: Addition
	| Addition Less Addition
	{
		$$ = $1 < $3;
	}
	| Addition LessEqual Addition
	{
		$$ = $1 <= $3;
	}
	| Addition Greater Addition
	{
		$$ = $1 > $3;
	}
	| Addition GreaterEqual Addition
	{
		$$ = $1 >= $3;
	}
	;
Addition
	: Term
	| Addition Plus Term
	{
		$$ = $1 + $3;
	}
	| Addition Minus Term
	{
		$$ = $1 - $3;
	}
	;
Term
	: Factor
	| Term Multiply Factor
	{
		$$ = $1 * $2;
	}
	| Term Divide Factor
	{
		$$ = $1 / $2;
	}
	| Term Modulus Factor
	{
		$$ = $1 % $2;
	}
	;
Factor
	: Power
	| Minus Power
	{
		$$ = -$2;
	}
	| Not Power
	{
		$$ = !$2;
	}
	;
Power
	: Primary
	| Power Exponent Primary
	{
		$$ = pow($1, $3);
	}
	;
Primary
	: Id
	| Id ArrayExpression
	| Literal
	| LeftParen Expression RightParen
	| Type LeftParen Expression RightParen
	;
%%
int
yyerror(char const *str)
{
	extern char *yytext;
	fprintf(stderr, "parser error near %s\n", yytext);
	return 0;
}

int main(int argc, char **argv)
{
	extern int yyparse(void);
	extern FILE *yyin;
	extern FILE *yyout;
	char *target;

	if(argc != 2) {
		fprintf(stderr, "Number of parameter is not correct!!!\n");
		exit(1);
	}

	if(strcmp(strstr(argv[1], ".mu"), ".mu") != 0){
		fprintf(stderr, "File format Error!\n");
		exit(1);
	}

	yyin = fopen(argv[1], "r");

	strncpy(target, argv[1], strlen(argv[1]) - strlen(".mu"));
	strcat(target, ".prt");

	yyout = fopen(target, "w");

	yyparse();

	fclose(yyin);
	fclose(yyout);
	return 0;
}
