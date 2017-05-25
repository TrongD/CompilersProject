%namespace ASTBuilder
%scannertype TCCLScanner
%visibility internal
%tokentype Token

%option stack, minimize, parser, verbose, persistbuffer, noembedbuffers 

%{
   StringBuffer string = new StringBuffer(); // for building trings
   
   private Symbol makeSymbol(int type) {
      return makeSymbol(type, new Symbol(type, yyline, yycolumn));
   }
   
   private Symbol makeSymbol(int type, Object val) {
      Listing.get().echo(yytext());
      return new Symbol(type, yyline, yycolumn, val);
   }
%}

LineTerminator  (\r|\n|\r\n)
InputCharacter  ([^\r\n])
WhiteSpace	    ({LineTerminator}|[ \t\f])

/* comments */
//CommentContent     ( [^*] | \*+ [^/*] )*
TraditionalComment   ("/*"[^*]~"*/"|"/*"("*")+"/")
EndOfLineComment    (("//")({InputCharacter})*{LineTerminator})
//DocumentationComment ("/**" {CommentContent} "*"+ "/")
Comment  ({TraditionalComment}|{EndOfLineComment}) // | {DocumentationComment})

Identifier [:jletter:][:jletterdigit:]*

DecIntegerLiteral (0|[1-9][0-9]*)



%s STRING

%%



/* keywords and special characters */
<INITIAL> {
"static"		{ return makeSymbol(sym.STATIC); }
"+=" 		{ return makeSymbol(sym.ASS_ADD); }
"?" 			{ return makeSymbol(sym.QUESTION); }
"/" 			{ return makeSymbol(sym.RSLASH); }
"-" 			{ return makeSymbol(sym.MINUSOP); }
"null" 		{ return makeSymbol(sym.NULL); }
"int" 		{ return makeSymbol(sym.INT); }
"==" 		{ return makeSymbol(sym.OP_EQ); }
"<" 			{ return makeSymbol(sym.OP_LT); }
":" 			{ return makeSymbol(sym.COLON); }
"||" 		{ return makeSymbol(sym.OP_LOR); }
"else" 		{ return makeSymbol(sym.ELSE); }
"%" 			{ return makeSymbol(sym.PERCENT); }
"this" 		{ return makeSymbol(sym.THIS); }
"class" 		{ return makeSymbol(sym.CLASS); }
"|" 			{ return makeSymbol(sym.PIPE); }
"public" 	{ return makeSymbol(sym.PUBLIC); }
[\.] 		{ return makeSymbol(sym.PERIOD); }
"\^" 		{ return makeSymbol(sym.HAT); }
"," 			{ return makeSymbol(sym.COMMA); }
"void" 		{ return makeSymbol(sym.VOID); }
"~" 			{ return makeSymbol(sym.TILDE); }
"(" 			{ return makeSymbol(sym.LPAREN); }
")" 			{ return makeSymbol(sym.RPAREN); }
">=" 		{ return makeSymbol(sym.OP_GE); }
";" 			{ return makeSymbol(sym.SEMICOLON); }
"if" 		{ return makeSymbol(sym.IF); }
"new" 		{ return makeSymbol(sym.NEW); }
"while" 		{ return makeSymbol(sym.WHILE); }
"private" 	{ return makeSymbol(sym.PRIVATE); }
"!" 			{ return makeSymbol(sym.BANG); }
"<=" 		{ return makeSymbol(sym.OP_LE); }
"&" 			{ return makeSymbol(sym.AND); }
[\{] 		{ return makeSymbol(sym.LBRACE); }
[\}] 		{ return makeSymbol(sym.RBRACE); }
[\[] 		{ return makeSymbol(sym.LBRACKET); }
[\]] 		{ return makeSymbol(sym.RBRACKET); }
"boolean" 	{ return makeSymbol(sym.BOOLEAN); }
"instanceof" 	{ return makeSymbol(sym.INSTANCEOF); }
"*" 			{ return makeSymbol(sym.ASTERICK); }
"=" 			{ return makeSymbol(sym.EQUALS); }
"+" 			{ return makeSymbol(sym.PLUSOP); }
"return" 	{ return makeSymbol(sym.RETURN); }
">" 			{ return makeSymbol(sym.OP_GT); }
"!=" 		{ return makeSymbol(sym.OP_NE); }
"&&" 		{ return makeSymbol(sym.OP_LAND); }

}


<INITIAL> {
{Identifier}                  { return makeSymbol(sym.IDENTIFIER, yytext()); }
 
{DecIntegerLiteral}            { return makeSymbol(sym.INTNUMBER, new Integer(Integer.parseInt(yytext()))); }
\"                             { string.setLength(0); yybegin(STRING); }

{Comment}                      { Listing.get().echo(yytext()); /* ignore */ }
  
{LineTerminator}               { Listing.get().newLine(1); }
 
{WhiteSpace}                   { Listing.get().echo(yytext()); /* ignore */ }
}



<STRING> {
  \"                             { yybegin(INITIAL); 
                                   return makeSymbol(sym.LITERAL, string.toString()); }
  [^\n\r\"\\]+                   { string.append( yytext() ); }
  \\t                            { string.append('\t'); }
  \\n                            { string.append('\n'); }

  \\r                            { string.append('\r'); }
  \\\"                           { string.append('\"'); }
  \\                             { string.append('\\'); }
}



/* error fallback */
.|\n                             { throw new Error("Illegal character <"+
                                                    yytext()+">"); }
