%{
	#include <stdio.h>
	#include <stdlib.h>
	#include <string.h>
	#define YYDEBUG 1
%}
%token LeftBracket RightBracket LeftParen RightParen
%token Comma Assign Equal Less LessEqual Greater
%token GreaterEqual Not NotEqual Plus Minus Multiply
%token Exponent Divide Modulus And Or If Elif Else
%token While Colon Indent NextLine Definition Return Delete
%token Type Id Integer Boolean Float String Time Note
%%
Program
	: Functions Main
	{
		fprintf(yyout, "Program: \n%s\n%s", $1, $2);
		tab++;
	}
	;
Functions
	: Function
	{
		strcpy($$, $1);
	}
	| Functions Function
	{
		strcpy($$, $1);
		strcat($$, "\n\n");
		strcat($$, $2);
	}
	;
Function
	: Definition Id LeftParen RightParen Colon NextLine Indents
	{
		fprintf(yyout, "\tFunction: %s\n", $2);
		tab++;
	}
	| Definition Id LeftParen Id RightParen Colon NextLine Indents
	{
		fprintf(yyout, "\tFunction: %s\n", $2);
		fprintf(yyout, "\t\tParameter: %s", $4);
		tab++;
	}
	| Definition Id LeftParen Id Parameter RightParen Colon NextLine Indents
	{
		fprintf(yyout, "\tFunction: %s\n", $2);
		fprintf(yyout, "\t\tParameter: %s", $4);
		tab++;
	}
	| Definition Id LeftParen RightParen Colon NextLine Indents Return Id
	{
		fprintf(yyout, "\tFunction: %s\n", $2);
		tab++;
		strcpy(return_id, $9);
	}
	| Definition Id LeftParen Id RightParen Colon NextLine Indents Return Id
	{
		fprintf(yyout, "\tFunction: %s\n", $2);
		fprintf(yyout, "\t\tParameter: %s", $4);
		tab++;
		strcpy(return_id, $10);
	}
	| Definition Id LeftParen Id Parameter RightParen Colon NextLine Indents Return Id
	{
		fprintf(yyout, "\tFunction: %s\n", $2);
		fprintf(yyout, "\t\tParameter: %s", $4);
		tab++;
		strcpy(return_id, $11);
	}
	;
Parameter
	: Comma Id
	{
		fprintf(yyout, ", %s", $2);
	}
	| Parameter Comma Id
	{
		fprintf(yyout, ", %s", $3);
	}
	;
Return
	: { 
		fprintf(yyout, "Return: %s", return_id);
		tab--;
	}
Main
	: Statements
	{
		fprintf(yyout, "\tMain:\n");
	}
	;
Statements
	: Statement NextLine
	| Statements Statement NextLine
	;
Statement
	: Assignment
	{
		for(int i = 0; i < tab; i++)
			fprintf(yyout, "\t");
		fprintf("Statement:\n");
		tab++;
	}
	| IfStatement
	{
		for(int i = 0; i < tab; i++)
			fprintf(yyout, "\t");
		fprintf("Statement:\n");
		tab++;
	}
	| WhileStatement
	{
		for(int i = 0; i < tab; i++)
			fprintf(yyout, "\t");
		fprintf("Statement:\n");
		tab++;
	}
	| FunctionCall
	{
		for(int i = 0; i < tab; i++)
			fprintf(yyout, "\t");
		fprintf("Statement:\n");
		tab++;
	}
	| DeleteStatement
	{
		for(int i = 0; i < tab; i++)
			fprintf(yyout, "\t");
		fprintf("Statement:\n");
		tab++;
	}
	;
Indents
	: Indent Statement
	{
		fprintf(yyout, "\n");
		for(int i = 0; i < tab; i++)
			fprintf(yyout, "\t");

	}
	| Indents Indent Statement
	;
Assignment
	: Id Assign Expression
	{
		fprintf(yyout, "Assignment:\n");
		tab++;
		for(int i = 0; i < tab; i++)
			fprintf(yyout, "\t");
		fprintf(yyout, "Id: %s\n", $1);
	}
	| Id Assign List
	{
		fprintf(yyout, "Assignment:\n");
		tab++;
		for(int i = 0; i < tab; i++)
			fprintf(yyout, "\t");
		fprintf(yyout, "Id: %s\n", $1);
		listflag++;
	}
	| Id Assign FunctionCall
	{
		fprintf(yyout, "Assignment:\n");
		tab++;
		for(int i = 0; i < tab; i++)
			fprintf(yyout, "\t");
		fprintf(yyout, "Id: %s\n", $1);
	}
	| Id ArrayExpression Assign Expression
	{
		fprintf(yyout, "Assignment:\n");
		tab++;
		for(int i = 0; i < tab; i++)
			fprintf(yyout, "\t");
		fprintf(yyout, "Id: %s", $1);
	}
	| Id ArrayExpression Assign List
	{
		fprintf(yyout, "Assignment:\n");
		tab++;
		for(int i = 0; i < tab; i++)
			fprintf(yyout, "\t");
		fprintf(yyout, "Id: %s", $1);
		listflag++;
	}
	| Id ArrayExpression Assign FunctionCall
	{
		fprintf(yyout, "Assignment:\n");
		tab++;
		for(int i = 0; i < tab; i++)
			fprintf(yyout, "\t");
		fprintf(yyout, "Id: %s", $1);
	}
	;
ArrayExpression
	: LeftBracket Expression RightBracket
	{
		fprintf(yyout, "[%s]\n", $2);
	}
	| ArrayExpression LeftBracket Expression RightBracket
	{
		fprintf(yyout, "[%s]", $2);
	}
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
DeleteStatement
	: Delete Id
List
	: LeftBracket Expression RightBracket
	{
		if(listflag > 0)
		{
			listflag--;
			for(int i = 0; i < tab; i++)
				fprintf(yyout, "\t");
		}
		fprintf("[%s]\n", $2);
	}
	| LeftBracket List RightBracket
	{
		if(listflag > 0)
		{
			listflag--;
			for(int i = 0; i < tab; i++)
				fprintf(yyout, "\t");
		}
		fprintf("[%s]\n", $2);
	}
	| LeftBracket Expression ListAddition RightBracket
	{
		if(listflag > 0)
		{
			listflag--;
			for(int i = 0; i < tab; i++)
				fprintf(yyout, "\t");
		}
		fprintf("[%s", $2);
	}
	| LeftBracket List ListAddition RightBracket
	{
		if(listflag > 0)
		{
			listflag--;
			for(int i = 0; i < tab; i++)
				fprintf(yyout, "\t");
		}
		fprintf("[%s", $2);
	}
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
		// $$ = $1 == $3;
	}
	| Relation NotEqual Relation
	{
		// $$ = $1 != $3;
	}
	;
Relation
	: Addition
	| Addition Less Addition
	{
		// $$ = $1 < $3;
	}
	| Addition LessEqual Addition
	{
		// $$ = $1 <= $3;
	}
	| Addition Greater Addition
	{
		// $$ = $1 > $3;
	}
	| Addition GreaterEqual Addition
	{
		// $$ = $1 >= $3;
	}
	;
Addition
	: Term
	| Addition Plus Term
	{
		// $$ = $1 + $3;
	}
	| Addition Minus Term
	{
		// $$ = $1 - $3;
	}
	;
Term
	: Factor
	| Term Multiply Factor
	{
		// $$ = $1 * $2;
	}
	| Term Divide Factor
	{
		// $$ = $1 / $2;
	}
	| Term Modulus Factor
	{
		// $$ = $1 % $2;
	}
	;
Factor
	: Power
	| Minus Power
	{
		// $$ = -$2;
	}
	| Not Power
	{
		// $$ = !$2;
	}
	;
Power
	: Primary
	| Power Exponent Primary
	{
		// $$ = pow($1, $3);
	}
	;
Primary
	: Id
	| Id ArrayExpression
	| Literal
	| LeftParen Expression RightParen
	| Type LeftParen Expression RightParen
	;
Literal
	: Integer
	| Boolean
	| Float
	| String
	| Time
	;
%%
int tab = 0;
char[] = return_id[100];
int listflag = 0;
int rbFlag = 0;	// RightBracketFlag

int yyerror(char const *str)
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
