
namespace FTCCompiler.Common
{
    enum TokenType
    {
        /// <summary>
        /// 'integer'
        /// </summary>
        IntegerDataType,
        /// <summary>
        /// 'boolean'
        /// </summary>
        BooleanDataType,
        /// <summary>
        /// 'const'
        /// </summary>
        ConstDefinition,
        /// <summary>
        /// ':'
        /// </summary>
        IdTypeSeparator,
        /// <summary>
        /// ','
        /// </summary>
        ListSeparator,
        /// <summary>
        /// ';'
        /// </summary>
        EndOfInstruction,
        /// <summary>
        /// 'var'
        /// </summary>
        VariableDefinition,
        /// <summary>
        /// '['
        /// </summary>
        RightSquareBracket,
        /// <summary>
        /// ']'
        /// </summary>
        LeftSquareBracket,
        /// <summary>
        /// 'procedure'
        /// </summary>
        ProcedureDefinition,
        /// <summary>
        /// '('
        /// </summary>
        RightParenthesis,
        /// <summary>
        /// ')'
        /// </summary>
        LeftParenthesis,
        /// <summary>
        /// 'begin'
        /// </summary>
        ScopeStart,
        /// <summary>
        /// ':='
        /// </summary>
        AssignOperator,
        /// <summary>
        /// '='
        /// </summary>
        EqualOperator,
        /// <summary>
        /// '&gt'
        /// </summary>
        GreaterThanOperator,
        /// <summary>
        /// '&lt'
        /// </summary>
        LessThanOperator,
        /// <summary>
        /// '>='
        /// </summary>
        GreaterThanOrEqualOperator,
        /// <summary>
        /// '&lt='
        /// </summary>
        LessThanOrEqualOperator,
        /// <summary>
        /// '&lt&gt'
        /// </summary>
        DistinctOperator,
        /// <summary>
        /// '*'
        /// </summary>
        MultiplicationOperator,
        /// <summary>
        /// '/'
        /// </summary>
        DivisionOperator,
        /// <summary>
        /// '+'
        /// </summary>
        AdditionOperator,
        /// <summary>
        /// '-'
        /// </summary>
        SubstractionOperator,
        /// <summary>
        /// 'or'
        /// </summary>
        OrOperator,
        /// <summary>
        /// 'and'
        /// </summary>
        AndOperator,
        /// <summary>
        /// 'xor'
        /// </summary>
        XorOperator,
        /// <summary>
        /// Secuencia de dígitos.
        /// </summary>
        LiteralNumber,
        /// <summary>
        /// Secuencia de caracteres entre comillas simples.
        /// </summary>
        LiteralString,
        /// <summary>
        /// 'end-proc'
        /// </summary>
        ProcedureEnd,
        /// <summary>
        /// 'function'
        /// </summary>
        FunctionDefinition,
        /// <summary>
        /// 'end-func'
        /// </summary>
        FunctionEnd,
        /// <summary>
        /// 'if'
        /// </summary>
        ConditionalIf,
        /// <summary>
        /// 'then'
        /// </summary>
        ConditionalThen,
        /// <summary>
        /// 'else'
        /// </summary>
        ConditionalElse,
        /// <summary>
        /// 'end-if'
        /// </summary>
        ConditionalEnd,
        /// <summary>
        /// 'while'
        /// </summary>
        IterationWhile,
        /// <summary>
        /// 'do'
        /// </summary>
        IterationDo,
        /// <summary>
        /// 'end-while'
        /// </summary>
        IterationEnd,
        /// <summary>
        /// 'byval'
        /// </summary>
        ByVal,
        /// <summary>
        /// 'byref'
        /// </summary>
        ByRef,
        /// <summary>
        /// 'read'
        /// </summary>
        Read,
        /// <summary>
        /// 'showln'
        /// </summary>
        WriteLine,
        /// <summary>
        /// 'show'
        /// </summary>
        Write,
        /// <summary>
        /// 'true' o 'false'
        /// </summary>
        LiteralBoolean,      
        /// <summary>
        /// Identificador (debe empezar con al menos una letra y puede seguir con letras y/o números)
        /// </summary>
        Identifier,
        /// <summary>
        /// Token de error
        /// </summary>
        Error,
        /// <summary>
        /// Fin de archivo
        /// </summary>
        EndOfFile
    }

    enum Separators
    {
        Space = 32,
        Tab = 9,
        CR = 13,
        LF = 10,
        CRLF = -2
    }

    enum Symbols
    {
        CommentStart = '{',
        CommentEnd = '}',
        Colon = ':',
        Comma = ',',
        Semicolon = ';',
        LeftSquareBracket = '[',
        RightSquareBracket = ']',
        LeftParenthesis = '(',
        RightParenthesis = ')',
        Equal = '=',
        GreaterThan = '>',
        LessThan = '<',
        Asterisk = '*',
        Slash = '/',
        Plus = '+',
        Minus = '-',
        Quote = '\''
    }

    public enum DataType
    {
        Integer,
        Boolean
    }

    public enum ParameterType
    {
        ByVal,
        ByRef
    }
}
