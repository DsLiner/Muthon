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
"["													return LeftBracket;
"]													return RightBracket;
"("													return LeftParen;
")"													return RightParen;
","													return Comma;
"="													return Assign;
"=="												return Equal;
"<"													return Less;
"<="												return LessEqual;
">"													return Greater;
">="												return GreaterEqual;
"!"													return Not;
"!="												return NotEqual;
"+"													return Plus;
"-"													return Minus;
"*"													return Multiply;
"**"												return Exponent;
"/"													return Divide;
"%"													return Modulus;
"&&"												return And;
"||"												return Or;
"if"												return If;
"elif"												return Elif;
"else"												return Else;
"while"												return While;
":"													return Colon;
"\t"												return Indent;
"\n"												return NextLine;
"def"												return Definition;
"return"											return Return;
"del"												return Delete;

"int" | "bool" | "float" | "str" | "Time" | "Note"	return Type
[a-zA-Z][a-zA-Z0-9]*								return Id;
[0-9]+												return Integer;
"True" | "False"									return Boolean;
([1-9][0-9]* | 0)\.[0-9]* 							return Float;
\'[a-zA-Z]*\'										return String;

[0-9]+"h"[0-9]+"m"[0-9]+"s" | [0-9]+"h"[0-9]+"m" |[0-9]+"h"[0-9]+"s" | [0-9]+"m"[0-9]+"s" | [0-9]+"m" | [0-9]+"s"																										return Time;
%%
