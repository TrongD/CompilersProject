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

CompilationUnit     :   ClassDeclaration                    {$$ = $1;}
					;

ClassDeclaration    :   Modifiers CLASS Identifier ClassBody {$$ = new ClassDeclaration($1, $3, $4); Console.WriteLine($4.ClassName());}
                    ;

Modifiers           :   PUBLIC                              { $$ = new Modifiers(ModifierType.PUBLIC);}
                    |   PRIVATE                             { $$ = new Modifiers(ModifierType.PRIVATE);}
                    |   STATIC                              { $$ = new Modifiers(ModifierType.STATIC);}
                    |   Modifiers PUBLIC                    { ((Modifiers)$1).AddModType(ModifierType.PUBLIC); $$ = $1;}
                    |   Modifiers PRIVATE                   { ((Modifiers)$1).AddModType(ModifierType.PRIVATE); $$ = $1;}
                    |   Modifiers STATIC                    { ((Modifiers)$1).AddModType(ModifierType.STATIC); $$ = $1;}
                    ;

ClassBody           :   LBRACE FieldDeclarations RBRACE     { $$ = new ClassBody($2); Console.WriteLine($$.ClassName());}
                    |   LBRACE RBRACE                       { $$ = new ClassBody();}
                    ;

FieldDeclarations   :   FieldDeclaration                    { $$ = new FieldDeclarations($1); }
                    |   FieldDeclarations FieldDeclaration  { $1.adoptChildren($2); $$ = $1;}
					;
//FieldDeclarations   :   FieldDeclaration                    { $$ = new Identifier("Not Implemented: Field Declaration List");}
//                    |   FieldDeclarations FieldDeclaration  { $$ = new Identifier("Not Implemented: Field Declaration List");}                     ;


FieldDeclaration	:	FieldVariableDeclaration SEMICOLON	{ $$ = new Identifier("Not Implemented: Field Declaration");}
					|	MethodDeclaration					{ $$ = $1;} 		
					|	ConstructorDeclaration				{ $$ = new Identifier("Not Implemented: Constructor Declaration");}
					|	StaticInitializer					{ $$ = new Identifier("Not Implemented: Field Declaration");}
					|	StructDeclaration					{ $$ = new Identifier("Not Implemented: Field Declaration");}
					;

StructDeclaration	:	Modifiers STRUCT Identifier ClassBody
					;



/*
 * This isn't structured so nicely for a bottom up parse.  Recall
 * the example I did in class for Digits, where the "type" of the digits
 * (i.e., the base) is sitting off to the side.  You'll have to do something
 * here to get the information where you want it, so that the declarations can
 * be suitably annotated with their type and modifier information.
 */
FieldVariableDeclaration	:	Modifiers TypeSpecifier FieldVariableDeclarators
							;

TypeSpecifier				:	TypeName
							| 	ArraySpecifier
							;

TypeName					:	PrimitiveType
							|   QualifiedName
							;

ArraySpecifier				: 	TypeName LBRACKET RBRACKET
							;
							
PrimitiveType				:	BOOLEAN
							|	INT
							|	VOID 
							;

FieldVariableDeclarators	:	FieldVariableDeclaratorName
							|   FieldVariableDeclarators COMMA FieldVariableDeclaratorName
							;


MethodDeclaration           :   Modifiers TypeSpecifier MethodDeclarator MethodBody         {$$ = new MethodDeclaration($1, $2, $3, $4); }
                            ;

MethodDeclarator            :   MethodDeclaratorName LPAREN ParameterList RPAREN            { $$ = new MethodDeclarator($1, $3); }
                            |   MethodDeclaratorName LPAREN RPAREN                          { $$ = new MethodDeclarator($1); }
                            ;

ParameterList               :   Parameter                                                   { $$ = new ParameterList($1); }
                            |   ParameterList COMMA Parameter                               { $1.adoptChildren($2); $$ = $1;}  
                            ;

Parameter                   :   TypeSpecifier DeclaratorName                                { $$ = new Parameter($1, $2); }
                            ;

QualifiedName				:	Identifier
							|	QualifiedName PERIOD Identifier
							;

DeclaratorName				:	Identifier													{ $$ = $1; }
							;

MethodDeclaratorName		:	Identifier													{ $$ = $1; }
							;

FieldVariableDeclaratorName	:	Identifier
							;

LocalVariableDeclaratorName	:	Identifier
							;

MethodBody					:	Block
							;

ConstructorDeclaration		:	Modifiers MethodDeclarator Block
							;

StaticInitializer			:	STATIC Block
							;
		
/*
 * These can't be reorganized, because the order matters.
 * For example:  int i;  i = 5;  int j = i;
 */
Block						:	LBRACE LocalVariableDeclarationsAndStatements RBRACE
							|   LBRACE RBRACE
							;

LocalVariableDeclarationsAndStatements	:	LocalVariableDeclarationOrStatement
										|   LocalVariableDeclarationsAndStatements LocalVariableDeclarationOrStatement
										;

LocalVariableDeclarationOrStatement	:	LocalVariableDeclarationStatement
									|   Statement
									;

LocalVariableDeclarationStatement	:	TypeSpecifier LocalVariableDeclarators SEMICOLON
									|   StructDeclaration                      						
									;

LocalVariableDeclarators	:	LocalVariableDeclaratorName
							|   LocalVariableDeclarators COMMA LocalVariableDeclaratorName
							;

							

Statement					:	EmptyStatement
							|	ExpressionStatement SEMICOLON
							|	SelectionStatement
							|	IterationStatement
							|	ReturnStatement
							|   Block
							;

EmptyStatement				:	SEMICOLON
							;

ExpressionStatement			:	Expression
							;

/*
 *  You will eventually have to address the shift/reduce error that
 *     occurs when the second IF-rule is uncommented.
 *
 */

SelectionStatement			:	IF LPAREN Expression RPAREN Statement ELSE Statement
//							|   IF LPAREN Expression RPAREN Statement
							;


IterationStatement			:	WHILE LPAREN Expression RPAREN Statement
							;

ReturnStatement				:	RETURN Expression SEMICOLON
							|   RETURN            SEMICOLON
							;

ArgumentList				:	Expression
							|   ArgumentList COMMA Expression
							;


Expression					:	QualifiedName EQUALS Expression
							|   Expression OP_LOR Expression   /* short-circuit OR */
							|   Expression OP_LAND Expression   /* short-circuit AND */
							|   Expression PIPE Expression
							|   Expression HAT Expression
							|   Expression AND Expression
							|	Expression OP_EQ Expression
							|   Expression OP_NE Expression
							|	Expression OP_GT Expression
							|	Expression OP_LT Expression
							|	Expression OP_LE Expression
							|	Expression OP_GE Expression
							|   Expression PLUSOP Expression
							|   Expression MINUSOP Expression
							|	Expression ASTERISK Expression
							|	Expression RSLASH Expression
							|   Expression PERCENT Expression	/* remainder */
							|	ArithmeticUnaryOperator Expression  %prec UNARY
							|	PrimaryExpression
							;

ArithmeticUnaryOperator		:	PLUSOP
							|   MINUSOP
							;
							
PrimaryExpression			:	QualifiedName
							|   NotJustName
							;

NotJustName					:	SpecialName
							|   ComplexPrimary
							;

ComplexPrimary				:	LPAREN Expression RPAREN
							|   ComplexPrimaryNoParenthesis
							;

ComplexPrimaryNoParenthesis	:	LITERAL
							|   Number
							|	FieldAccess
							|	MethodCall
							;

FieldAccess					:	NotJustName PERIOD Identifier
							;		

MethodCall					:	MethodReference LPAREN ArgumentList RPAREN
							|   MethodReference LPAREN RPAREN
							;

MethodReference				:	ComplexPrimaryNoParenthesis
							|	QualifiedName
							|   SpecialName
							;

SpecialName					:	THIS
							|	NULL
							;

Identifier					:	IDENTIFIER						{ $$ = $1; }
							;

Number                      :   INT_NUMBER                      { $$ = new Identifier("Not Implemented: Number");}
							;

%%

