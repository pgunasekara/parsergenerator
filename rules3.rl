type
    :
    IDENT
    ;

rTypeIds
    :
    COMMA IDENT
    ;

typeIds
    :
    IDENT COLON type
    ;

compoundStatement
    :
    BEGIN statement
    ;

declarations
    :
    VAR typeIds SEMICOLON
    ;

program
    :
    PROGRAM IDENT SEMICOLON declarations
    ;