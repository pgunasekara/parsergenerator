factor
returns[Expression expr]
locals[Expression left, Expression right]
    :
    atom
     (
         MULT atom
       |  DIV atom
       |  MOD atom
     )*
    ;

relational
returns [Expression expr]
locals[Expression left, Expression right, String op]
    :
    term {$left=$term.expr;} ((EQ {$op="EQ";}|NEQ {$op="NEQ";}|LT {$op="LT";}|GT {$op="GT";}|LEQ {$op="LEQ";}|GEQ {$op="GEQ";}) term{$right=$term.expr;})? 
    {
        if($right != null) {
            switch($op) {
                case "EQ":
                    $expr = new BinaryExpr($left, BinaryOp.EQ, $right);
                    break;
                case "NEQ":
                    $expr = new BinaryExpr($left, BinaryOp.NEQ, $right);
                    break;
                case "LEQ":
                    $expr = new BinaryExpr($left, BinaryOp.LEQ, $right);
                    break;
                case "GEQ":
                    $expr = new BinaryExpr($left, BinaryOp.GEQ, $right);
                    break;
                case "LT":
                    $expr = new BinaryExpr($left, BinaryOp.LT, $right);
                    break;
                case "GT":
                    $expr = new BinaryExpr($left, BinaryOp.GT, $right);
                    break;
                default:
                    break;
            }
        } else {
            $expr = $left;
        }
    }
    ;


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