selector
    :
    DOT IDENT | LBRAK expression RBRAK
    ;

factor
    :
    IDENT selector | NUMBER | LPARAN expression RPARAN | NOT factor
    ;

expression
    :

    ;