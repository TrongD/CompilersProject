%namespace ASTBuilder
%partial
%parsertype TCCLParser
%visibility internal
%tokentype Token
%YYSTYPE AbstractNode


%start CompilationUnit

%token STATIC, STRUCT, QUESTION, RSLASH, MINUSOP, NULL, INT, OP_EQ, OP_LT, COLON, OP_LOR
%token ELSE, PERCENT, THIS, CLASS, PIPE, PUBLIC, PERIOD, HAT, COMMA, VOID, TILDE
%token LPAREN, RPAREN, OP_GE, SEMICOLON, IF, NEW, WHILE, PRIVATE, BANG, OP_LE, AND 
%token LBRACE, RBRACE, LBRACKET, RBRACKET, BOOLEAN, INSTANCEOF, ASTERISK, EQUALS, PLUSOP
%token RETURN, OP_GT, OP_NE, OP_LAND, INT_NUMBER, IDENTIFIER, LITERAL, SUPER

%right EQUALS
%left  OP_LOR
%left  OP_LAND
%left  PIPE
%left  HAT
%left  AND
%left  OP_EQ, OP_NE
%left  OP_GT, OP_LT, OP_LE, OP_GE
%left  PLUSOP, MINUSOP
%left  ASTERISK, RSLASH, PERCENT
%left  UNARY 

%%

CompilationUnit		:	ClassDeclaration						{ $$ = new CompilationUnit($1);  }	
					|	MethodDeclarations						{ $$ = new CompilationUnit($1);  }	
					;

ClassDeclaration	:	Modifiers CLASS Identifier ClassBody	{ $$ = MakeClassDeclaration($1, $3, $4); }	
					;

MethodDeclarations	:	MethodDeclaration						{$$ = $1; }
					|	MethodDeclarations	MethodDeclaration	{$1.makeSibling($2); }	
					;

Modifiers			:	PUBLIC									{ $$ = MakeList((int)Token.PUBLIC); }	
					|	PRIVATE									{ $$ = MakeList((int)Token.PRIVATE); }	
					|	STATIC									{ $$ = MakeList((int)Token.STATIC); }	
					|	Modifiers PUBLIC						{ ((ModifierList)$1).Add((int)Token.PUBLIC); }	
					|	Modifiers PRIVATE						{ ((ModifierList)$1).Add((int)Token.PRIVATE); }
					|	Modifiers STATIC						{ ((ModifierList)$1).Add((int)Token.STATIC); }
					;
ClassBody			:	LBRACE FieldDeclarations RBRACE			{ $$ = $2; }
					|	LBRACE RBRACE							
					;

FieldDeclarations	:	FieldDeclaration						{ $$ = MakeFieldDeclarations($1); }
					|	FieldDeclarations FieldDeclaration		{ $1.adoptChildren($2); }
					;

FieldDeclaration	:	FieldVariableDeclaration SEMICOLON		{ $$ = $1; }
					|	MethodDeclaration						{ $$ = $1; }
					|	ConstructorDeclaration					{ $$ = $1; }				
					|	StaticInitializer						{ $$ = $1; }					
					|	StructDeclaration						{ $$ = $1; }					
					;

StructDeclaration	:	Modifiers STRUCT Identifier ClassBody	{ $$ = MakeStructDeclaration($1, $3, $4); }
					;



/*
 * This isn't structured so nicely for a bottom up parse.  Recall
 * the example I did in class for Digits, where the "type" of the digits
 * (i.e., the base) is sitting off to the side.  You'll have to do something
 * here to get the information where you want it, so that the declarations can
 * be suitably annotated with their type and modifier information.
 */
FieldVariableDeclaration	:	Modifiers TypeSpecifier FieldVariableDeclarators			{ $$ = MakeFieldVariableDeclaration($1,$2,$3); }
							;

TypeSpecifier				:	TypeName													{ $$ = $1; }
							| 	ArraySpecifier										
							;

TypeName					:	PrimitiveType												{ $$ = $1; }
							|   QualifiedName												{ $$ = $1; }
							;

ArraySpecifier				: 	TypeName LBRACKET RBRACKET									{  }
							;
							
PrimitiveType				:	BOOLEAN														{ $$ = MakePrimitiveType((int)Token.BOOLEAN); }	
							|	INT															{ $$ = MakePrimitiveType((int)Token.INT); }		
							|	VOID 														{ $$ = MakePrimitiveType((int)Token.VOID); }		
							;

FieldVariableDeclarators	:	FieldVariableDeclaratorName									{ $$ = MakeFieldVariableDeclarators($1); }
							|   FieldVariableDeclarators COMMA FieldVariableDeclaratorName	{ $1.adoptChildren($3); $$ = $1; }
							;


MethodDeclaration			:	Modifiers TypeSpecifier MethodDeclarator MethodBody			{ $$ = MakeMethodDeclaration($1,$2,$3,$4); }
							;

MethodDeclarator			:	MethodDeclaratorName LPAREN ParameterList RPAREN			{ $$ = MakeMethodDeclarator($1, $3); }
							|   MethodDeclaratorName LPAREN RPAREN							{ $$ = MakeMethodDeclarator($1); }
							;

ParameterList				:	Parameter													{ $$ = MakeParameterList($1); }
							|   ParameterList COMMA Parameter								{ $1.adoptChildren($3); $$ = $1; }
							;

Parameter					:	TypeSpecifier DeclaratorName								{ $$ = MakeParameter($1, $2); }
							;

QualifiedName				:	Identifier													{ $$ = MakeQualifiedName($1); }
							|	QualifiedName PERIOD Identifier								{ $1.adoptChildren($3); $$ = $1; }
							;

DeclaratorName				:	Identifier													{ $$ = $1;  }
							;

MethodDeclaratorName		:	Identifier													{ $$ = $1;  }
							;

FieldVariableDeclaratorName	:	Identifier													{ $$ = $1;  }
							;

LocalVariableDeclaratorName	:	Identifier													{ $$ = $1;  }
							;

MethodBody					:	Block														{ $$ = $1; }
							;

ConstructorDeclaration		:	Modifiers MethodDeclarator Block							{  }
							;

StaticInitializer			:	STATIC Block												{  }
							;
		
/*
 * These can't be reorganized, because the order matters.
 * For example:  int i;  i = 5;  int j = i;
 */
Block						:	LBRACE LocalVariableDeclarationsAndStatements RBRACE									{ $$ = $2; }
							|   LBRACE RBRACE																			{  }
							;

LocalVariableDeclarationsAndStatements	:	LocalVariableDeclarationOrStatement											{ $$ = MakeLocalVariableDeclarationAndStatement($1); }
										|   LocalVariableDeclarationsAndStatements LocalVariableDeclarationOrStatement	{ $1.adoptChildren($2); $$ = $1; }
										;

LocalVariableDeclarationOrStatement	:	LocalVariableDeclarationStatement												{ $$ = $1; }
									|   Statement																		{ $$ = $1; }
									;

LocalVariableDeclarationStatement	:	TypeSpecifier LocalVariableDeclarators SEMICOLON								{ $$ = MakeLocalVariableDeclarationStatement($1,$2); }
									|   StructDeclaration                    											{ $$ = $1; }  						
									;

LocalVariableDeclarators	:	LocalVariableDeclaratorName																{ $$ = MakeLocalVariableDeclarators($1); }
							|   LocalVariableDeclarators COMMA LocalVariableDeclaratorName								{ $1.adoptChildren($3); $$ = $1; }
							;

							

Statement					:	EmptyStatement																			{  }
							|	ExpressionStatement SEMICOLON															{ $$ = $1; }
							|	SelectionStatement																		{ $$ = $1; }
							|	IterationStatement																		{ $$ = $1; }
							|	ReturnStatement																			{ $$ = $1; }
							|   Block																					{ $$ = $1; }	
							;

EmptyStatement				:	SEMICOLON																				{  }								
							;

ExpressionStatement			:	Expression																				{ $$ = $1; }
							;

/*
 *  You will eventually have to address the shift/reduce error that
 *     occurs when the second IF-rule is uncommented.
 *
 */

SelectionStatement			:	IF LPAREN Expression RPAREN Statement ELSE Statement									{ $$ = MakeSelectionStatement($3, $5, $7); }
//							|   IF LPAREN Expression RPAREN Statement													{ $$ = MakeSelectionStatement($3, $5); }
							;


IterationStatement			:	WHILE LPAREN Expression RPAREN Statement												{ $$ = MakeIterationStatement($3, $5); }
							;

ReturnStatement				:	RETURN Expression SEMICOLON																{ $$ = $2; }
							|   RETURN            SEMICOLON																{  }
							;

ArgumentList				:	Expression																				{ $$ = $1; }
							|   ArgumentList COMMA Expression															{ $1.makeSibling($3); $$ = $1; }
							;


Expression					:	QualifiedName EQUALS Expression															{ $$ = MakeExpression($1,$3, (int)Token.EQUALS); }
							|   Expression OP_LOR Expression   /* short-circuit OR */									{ $$ = MakeExpression($1,$3, (int)Token.OP_LOR);  }
							|   Expression OP_LAND Expression   /* short-circuit AND */									{ $$ = MakeExpression($1,$3, (int)Token.OP_LAND);  }
							|   Expression PIPE Expression																{ $$ = MakeExpression($1,$3, (int)Token.PIPE);  }
							|   Expression HAT Expression																{ $$ = MakeExpression($1,$3, (int)Token.HAT);  }
							|   Expression AND Expression																{ $$ = MakeExpression($1,$3, (int)Token.AND);  }
							|	Expression OP_EQ Expression																{ $$ = MakeExpression($1,$3, (int)Token.OP_EQ);  }
							|   Expression OP_NE Expression																{ $$ = MakeExpression($1,$3, (int)Token.OP_NE);  }
							|	Expression OP_GT Expression																{ $$ = MakeExpression($1,$3, (int)Token.OP_GT);  }
							|	Expression OP_LT Expression																{ $$ = MakeExpression($1,$3, (int)Token.OP_LT);  }
							|	Expression OP_LE Expression																{ $$ = MakeExpression($1,$3, (int)Token.OP_LE);  }
							|	Expression OP_GE Expression																{ $$ = MakeExpression($1,$3, (int)Token.OP_GE);  }
							|   Expression PLUSOP Expression															{ $$ = MakeExpression($1,$3, (int)Token.PLUSOP);  }
							|   Expression MINUSOP Expression															{ $$ = MakeExpression($1,$3, (int)Token.MINUSOP);  }
							|	Expression ASTERISK Expression															{ $$ = MakeExpression($1,$3, (int)Token.ASTERISK);  }
							|	Expression RSLASH Expression															{ $$ = MakeExpression($1,$3, (int)Token.RSLASH);  }
							|   Expression PERCENT Expression	/* remainder */											{ $$ = MakeExpression($1,$3, (int)Token.PERCENT);  }
							|	ArithmeticUnaryOperator Expression  %prec UNARY											{   }
							|	PrimaryExpression																		{ $$ =$1;  }
							;

ArithmeticUnaryOperator		:	PLUSOP																					{  }
							|   MINUSOP																					{  }
							;
							
PrimaryExpression			:	QualifiedName																			{ $$ = $1; }
							|   NotJustName																				{ $$ = $1; }
							;

NotJustName					:	SpecialName																				{ $$ = $1; }
							|   ComplexPrimary																			{ $$ = $1; }
							;

ComplexPrimary				:	LPAREN Expression RPAREN																{ $$ = $2; }
							|   ComplexPrimaryNoParenthesis																{ $$ = $1; }
							;

ComplexPrimaryNoParenthesis	:	LITERAL																					{ $$ = $1; }
							|   Number																					{ $$ = $1; }
							|	FieldAccess																				{ $$ = $1; }
							|	MethodCall																				{ $$ = $1; }
							;

FieldAccess					:	NotJustName PERIOD Identifier															{ $$ = MakeBinary($1, $3); }
							;		

MethodCall					:	MethodReference LPAREN ArgumentList RPAREN												{ $$ = MakeMethodCall($1, $3);  }
							|   MethodReference LPAREN RPAREN															{ $$ = MakeMethodCall($1);  }
							;

MethodReference				:	ComplexPrimaryNoParenthesis																{ $$ = $1; }
							|	QualifiedName																			{ $$ = $1; }
							|   SpecialName																				{ $$ = $1; }
							;

SpecialName					:	THIS																					{ $$ = MakeSpecialName((int)Token.THIS); }												
							|	NULL																					{ $$ = MakeSpecialName((int)Token.NULL); }													
							;

Identifier					:	IDENTIFIER																				{ $$ = $1; }			
							;

Number						:	INT_NUMBER																				{ $$ = $1; }												
							;

%%

