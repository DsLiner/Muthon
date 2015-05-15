%{
	#include <stdio.h>
	#include "muthonc.tab.h"

	int
	yywrap(void)
	{
		return 1;
	}
%}
%%
"["									return LeftBracket;
"]									return RightBracket;
"("									return LeftParen;
")"									return RightParen;
","									return Comma;
"="									return Assign;
"=="								return Equal;
"<"									return Less;
"<="								return LessEqual;
">"									return Greater;
">="								return GreaterEqual;
"!"									return Not;
"!="								return NotEqual;
"+"									return Plus;
"-"									return Minus;
"*"									return Multiply;
"**"								return Exponent;
"/"									return Divide;
"%"									return Modulus;
"&&"								return And;
"||"								return Or;
"if"								return If;
"elif"								return Elif;
"else"								return Else;
"while"								return While;
":"									return Colon;
"\t"								return Indent;
"\n"								return NextLine;
"def"								return Definition;

"int" | "bool" | "float" | "str"	return Type
[a-zA-Z][a-zA-Z0-9]*				return Id;
[0-9]+	{
	yylval = atoi(yytext);
	return Integer;
}
"True" | "False"	{
	if(strcmp("True", yytext) == 0)
		yylval = true;
	else
		yylval = false;
	return Boolean;
}
([1-9][0-9]* | 0)\.[0-9]* {
	yylval = atof(yytext);
	return Float;
}
\'[a-zA-Z]*\'						return String;
%%
