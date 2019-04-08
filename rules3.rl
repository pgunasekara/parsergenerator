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
    IDENT rTypeIds COLON type
    ;

declarations
    :
    VAR typeIds SEMICOLON
    ;

program
    :
    PROGRAM IDENT SEMICOLON declarations compoundStatement
    ;