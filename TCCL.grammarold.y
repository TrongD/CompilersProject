
// ==========================================================================
//  GPPG error listing for yacc source file <TCCL.grammar.y - 5/15/2017 11:21:18 AM>
// ==========================================================================
//  Version:  1.5.2
//  Machine:  RJL-SURFACE4
//  DateTime: 5/15/2017 11:23:15 AM
//  UserName: Richard LeBlanc
// ==========================================================================


%namespace ASTBuilder
%partial
%parsertype TCCLParser
%visibility internal
%tokentype Token
%YYSTYPE AbstractNode

/* %union{
    public Token token;
    public AbstractNode abstractNode;
} */

%{
    public string yytext
    {
        get { return ((TCCLScanner)Scanner).yytext; }
    }

%}

%start CompilationUnit

%token AND ASTERISK BANG BOOLEAN CLASS
%token COLON COMMA ELSE EQUALS HAT
%token IDENTIFIER IF INSTANCEOF INT INT_NUMBER
%token LBRACE LBRACKET LITERAL LPAREN MINUSOP
%token NEW NULL OP_EQ OP_GE OP_GT
%token OP_LAND OP_LE OP_LOR OP_LT OP_NE
%token PERCENT PERIOD PIPE PLUSOP PRIVATE
%token PUBLIC QUESTION RBRACE RBRACKET RETURN
%token RPAREN RSLASH SEMICOLON STATIC STRUCT
%token SUPER THIS TILDE VOID WHILE


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
// Warning: NonTerminal symbol "ArithmeticUnaryOperator" is unreachable
// Warning: NonTerminal symbol "PrimaryExpression" is unreachable
// Warning: NonTerminal symbol "NotJustName" is unreachable
// Warning: NonTerminal symbol "ArgumentList" is unreachable
// Warning: NonTerminal symbol "IterationStatement" is unreachable
// Warning: NonTerminal symbol "ReturnStatement" is unreachable
// Warning: NonTerminal symbol "Expression" is unreachable
// Warning: NonTerminal symbol "FieldAccess" is unreachable
// Warning: NonTerminal symbol "MethodCall" is unreachable
// Warning: NonTerminal symbol "MethodReference" is unreachable
// Warning: NonTerminal symbol "Number" is unreachable
// Warning: NonTerminal symbol "SpecialName" is unreachable
// Warning: NonTerminal symbol "ComplexPrimary" is unreachable
// Warning: NonTerminal symbol "ComplexPrimaryNoParenthesis" is unreachable
// Warning: NonTerminal symbol "SelectionStatement" is unreachable
// Error: NonTerminal symbol "LocalVariableDeclarationsAndStatements" has no productions
// Warning: NonTerminal symbol "LocalDeclarations" is unreachable
// Warning: NonTerminal symbol "LocalVariableDeclarationOrStatement" is unreachable
// Warning: NonTerminal symbol "LocalVariableDeclaratorName" is unreachable
// Error: There are 2 non-terminating NonTerminal Symbols
   //  {LocalVariableDeclarationsAndStatements, LocalVariableDeclaration}
// Warning: Terminating LocalVariableDeclarationsAndStatements fixes the following size-1 NonTerminal set
   // {LocalVariableDeclarationsAndStatements}
// Warning: Terminating LocalVariableDeclaration fixes the following size-1 NonTerminal set
   // {LocalVariableDeclaration}
// Warning: NonTerminal symbol "LocalVariableDeclarators" is unreachable
// Warning: NonTerminal symbol "EmptyStatement" is unreachable
// Warning: NonTerminal symbol "ExpressionStatement" is unreachable
// Warning: NonTerminal symbol "LocalVariableDeclarationS" is unreachable
// Warning: NonTerminal symbol "LocalVariableDeclaration" is unreachable
// Error: NonTerminal symbol "LocalVariableDeclaration" has no productions
// Warning: NonTerminal symbol "Statement" is unreachable
// ------------------------------------------------------------------------------------------------------

CompilationUnit     :   ClassDeclaration                    {$$ = new CompilationUnit($1);}
                    ;

ClassDeclaration    :   Modifiers CLASS Identifier ClassBody {$$ = new ClassDeclaration($1, $3, $4);}
                    ;

Modifiers           :   PUBLIC                              { $$ = new Modifiers(ModifierType.PUBLIC);}
                    |   PRIVATE                             { $$ = new Modifiers(ModifierType.PRIVATE);}
                    |   STATIC                              { $$ = new Modifiers(ModifierType.STATIC);}
                    |   Modifiers PUBLIC                    { ((Modifiers)$1).AddModType(ModifierType.PUBLIC); $$ = $1;}
                    |   Modifiers PRIVATE                   { ((Modifiers)$1).AddModType(ModifierType.PRIVATE); $$ = $1;}
                    |   Modifiers STATIC                    { ((Modifiers)$1).AddModType(ModifierType.STATIC); $$ = $1;}
                    ;

ClassBody           :   LBRACE FieldDeclarations RBRACE     { $$ = new ClassBody($2);}
                    |   LBRACE RBRACE                       { $$ = new ClassBody();}
                    ;

FieldDeclarations   :   FieldDeclaration                    { $$ = new FieldDeclarations($1); }
                    |   FieldDeclarations FieldDeclaration  { $1.adoptChildren($2); $$ = $1;}
                    ;

FieldDeclaration    :   FieldVariableDeclaration SEMICOLON  { $$ = new Identifier("Not Implemented: FieldVarDecl"); }
                    |   MethodDeclaration                   { $$ = $1;  }
                    |   ConstructorDeclaration              { $$ = new Identifier("Not Implemented: ConstructorDeclaration");          }
                    |   StaticInitializer                   { $$ = new Identifier("Not Implemented: StaticInitializer");   }
                    |   StructDeclaration                   { $$ = new Identifier("Not Implemented: StructDeclaration");        }
                    ;

StructDeclaration   :   Modifiers STRUCT Identifier ClassBody   {}
                    ;



/*
 * This isn't structured so nicely for a bottom up parse.  Recall
 * the example I did in class for Digits, where the "type" of the digits
 * (i.e., the base) is sitting off to the side.  You'll have to do something
 * here to get the information where you want it, so that the declarations can
 * be suitably annotated with their type and modifier information.
 */
FieldVariableDeclaration    :   Modifiers TypeSpecifier FieldVariableDeclarators            {}
                            ;

TypeSpecifier               :   TypeName                                                    { $$ = new TypeSpecifier($1); }
                            |   ArraySpecifier                                              { $$ = new TypeSpecifier($1); }
                            ;

TypeName                    :   PrimitiveType                                               { $$ = new TypeName($1); }
                            |   QualifiedName                                               { $$ = new TypeName($1); }
                            ;

ArraySpecifier              :   TypeName LBRACKET RBRACKET                                  {}
                            ;
                            
PrimitiveType               :   BOOLEAN                                                     { $$ = new PrimitiveType(EnumPrimitiveType.BOOLEAN); }
                            |   INT                                                         { $$ = new PrimitiveType(EnumPrimitiveType.INT); }
                            |   VOID                                                        { $$ = new PrimitiveType(EnumPrimitiveType.VOID); }
                            ;

FieldVariableDeclarators    :   FieldVariableDeclaratorName                                 {}
                            |   FieldVariableDeclarators COMMA FieldVariableDeclaratorName  {}
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

QualifiedName               :   Identifier                                                  { $$ = new QualifiedName($1);}
                            |   QualifiedName PERIOD Identifier                             { $$.adoptChildren($3); $$ = $1;}
                            ;

DeclaratorName              :   Identifier                                                  { $$ = new DeclaratorName($1); }
                            ;

MethodDeclaratorName        :   Identifier                                                  { $$ = new MethodDeclaratorName($1); }
                            ;

FieldVariableDeclaratorName :   Identifier                                                  {}
                            ;

LocalVariableDeclaratorName :   Identifier                                                  { $$ = new LocalVariableDeclaratorName($1); }
                            ;

MethodBody                  :   Block                                                       { $$ = new MethodBody($1); }
                            ;

ConstructorDeclaration      :   Modifiers MethodDeclarator Block                            {}
                            ;

StaticInitializer           :   STATIC Block                                                {}
                            ;
        
/*
 * These can't be reorganized, because the order matters.
 * For example:  int i;  i = 5;  int j = i;
 */

Block                       :   LBRACE LocalVariableDeclarationsAndStatements RBRACE        { $$ = new Block($2); }
                            |   LBRACE RBRACE                                               { $$ = new Block(); }
                            ;

LocalDeclarations			:   LocalVariableDeclarationOrStatement             { $$ = new LocalDeclarations($1);}
                            |   LocalDeclarations LocalVariableDeclarationOrStatement
                                                                                            { $1.adoptChildren($2); $$ = $1; }
                                        ;

LocalVariableDeclarationOrStatement :   LocalVariableDeclaration                   { $$ = $1;}
                                    |   Statement                                           { $$ = $1;}
                                    ;

LocalVariableDeclarationS			:   TypeSpecifier LocalVariableDeclarators SEMICOLON    { $$ = new LocalVariableDeclaration($1, $2);}
                                    |   StructDeclaration                                   { $$ = new Identifier("Not Implemented: StructDeclaration");}
                                    ;

LocalVariableDeclarators    :   LocalVariableDeclaratorName                                 { $$ = new LocalVariableDeclarators($1); }
                            |   LocalVariableDeclarators COMMA LocalVariableDeclaratorName  { $1.adoptChildren($3); $$ = $1; }
                            ;

                            
Statement                   :   EmptyStatement                                              { $$ = new Statement($1);}
                            |   ExpressionStatement SEMICOLON                               { $$ = new Statement($1);}
                            |   SelectionStatement                                          { $$ = new Identifier("Not Implemented: SelectionStatement");}
                            |   IterationStatement                                          { $$ = new Identifier("Not Implemented: IterationStatement");}
                            |   ReturnStatement                                             { $$ = new Identifier("Not Implemented: IterationStatement");}
                            |   Block                                                       { $$ = new Statement($1);}
                            ;

EmptyStatement              :   SEMICOLON                                                   { $$ = new EmptyStatement();}
                            ;

ExpressionStatement         :   Expression                                                  { $$ = new ExpressionStatement($1); }
                            ;

/*
 *  You will eventually have to address the shift/reduce error that
 *     occurs when the second IF-rule is uncommented.
 *
 */

SelectionStatement          :   IF LPAREN Expression RPAREN Statement ELSE Statement
//                          |   IF LPAREN Expression RPAREN Statement
                            ;


IterationStatement          :   WHILE LPAREN Expression RPAREN Statement
                            ;

ReturnStatement             :   RETURN Expression SEMICOLON
                            |   RETURN            SEMICOLON
                            ;

ArgumentList                :   Expression
                            |   ArgumentList COMMA Expression
                            ;


// TODO
Expression                  :   QualifiedName EQUALS Expression     { $$ = new Expression($1, ExprType.EQUALS, $3); }
   /* short-circuit OR  */  |   Expression OP_LOR Expression   
   /* short-circuit AND */  |   Expression OP_LAND Expression   
                            |   Expression PIPE Expression
                            |   Expression HAT Expression
                            |   Expression AND Expression
                            |   Expression OP_EQ Expression
                            |   Expression OP_NE Expression
                            |   Expression OP_GT Expression
                            |   Expression OP_LT Expression
                            |   Expression OP_LE Expression
                            |   Expression OP_GE Expression
                            |   Expression PLUSOP Expression
                            |   Expression MINUSOP Expression
                            |   Expression ASTERISK Expression
                            |   Expression RSLASH Expression
                            |   Expression PERCENT Expression   /* remainder */
                            |   ArithmeticUnaryOperator Expression  %prec UNARY
                            |   PrimaryExpression               { $$ = $1; }
                            ;

ArithmeticUnaryOperator     :   PLUSOP
                            |   MINUSOP
                            ;
                            
PrimaryExpression           :   QualifiedName                   { $$ = $1;}   
                            |   NotJustName                     { $$ = $1;}
                            ;

NotJustName                 :   SpecialName                     { $$ = $1;}
                            |   ComplexPrimary                  { $$ = $1;}
                            ;

ComplexPrimary              :   LPAREN Expression RPAREN        { $$ = $2;}
                            |   ComplexPrimaryNoParenthesis     { $$ = $1;}
                            ;

ComplexPrimaryNoParenthesis :   LITERAL                         { $$ = $1;}
                            |   Number                          { $$ = $1;}
                            |   FieldAccess                     { $$ = $1;}   
                            |   MethodCall                      { $$ = $1;}    
                            ;

FieldAccess                 :   NotJustName PERIOD Identifier   { $$ = new Identifier("Not Implemented: Number");}   
                            ;       

MethodCall                  :   MethodReference LPAREN ArgumentList RPAREN
                                                                { $$ = new MethodCall($1, $3);}
                            |   MethodReference LPAREN RPAREN   { $$ = new Identifier("Not Implemented: MethodRef()");}
                            ;

MethodReference             :   ComplexPrimaryNoParenthesis     { $$ = new MethodReference($1);}
                            |   QualifiedName                   { $$ = new MethodReference($1);}
                            |   SpecialName                     { $$ = new MethodReference($1);}
                            ;

SpecialName                 :   THIS                            { $$ = new SpecialName(SpecialNameType.THIS);}
                            |   NULL                            { $$ = new SpecialName(SpecialNameType.NULL);}
                            ;

Identifier                  :   IDENTIFIER                      {$$ = new Identifier(yytext);}
                            ;

Number                      :   INT_NUMBER                      { $$ = new Identifier("Not Implemented: Number");}
                            ;

%%

// ==========================================================================

