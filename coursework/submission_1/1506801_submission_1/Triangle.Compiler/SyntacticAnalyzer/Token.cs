using System;
using System.Collections.Generic;
using System.Linq;

namespace Triangle.Compiler.SyntacticAnalyzer
{
    public class Token
    {
        // Lookup table of reserved words used to screen tokens.
        static readonly IDictionary<string, TokenKind> ReservedWords =
                Enumerable.Range((int)TokenKind.Array, (int)TokenKind.While).Cast<TokenKind>()
                    .ToDictionary(kind => kind.ToString().ToLower(), kind => kind);

        public SourcePosition position { get; private set; }
        
        //The kind of a source token.
        public TokenKind Kind { get; private set; }

        //The spelling of a source token.
        public string Spelling { get; private set; }

        // Creates a token
        public Token(TokenKind kind, string spelling, SourcePosition pos)
        {

            Kind = kind;
            if (kind == TokenKind.Identifier)
            {
                TokenKind match;
                if (ReservedWords.TryGetValue(spelling, out match))
                {
                    Kind = match;
                }
            }

            Spelling = spelling;
            position = pos;
        }

        public int getLine() {
            return position.Start.Line;
        }

        public int getIndex() {
            return position.Start.Column;
        }

        public override string ToString()
        {
            return string.Format("Kind={0}, spelling=\"{1}\"", Kind, Spelling);
        }
    }
}