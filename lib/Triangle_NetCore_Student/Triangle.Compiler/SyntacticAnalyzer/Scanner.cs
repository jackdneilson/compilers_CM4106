using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.RegularExpressions;

namespace Triangle.Compiler.SyntacticAnalyzer
{
    public class Scanner : IEnumerable<Token>
    {
        SourceFile _source;

        StringBuilder _currentSpelling;

        bool _debug;

        public Scanner(SourceFile source)
        {
            _source = source;
            _source.Reset();
            _currentSpelling = new StringBuilder();
        }

        public Scanner EnableDebugging()
        {
            _debug = true;
            return this;
        }
        public IEnumerator<Token> GetEnumerator()
        {
            while (true)
            {
                while (_source.Current == '!' || _source.Current == ' ' || _source.Current == '\t' || _source.Current == '\n')
                {
                    ScanSeparator();
                }

                _currentSpelling.Clear();

                var startLocation = _source.location();
                var kind = ScanToken();
                var endLocation = _source.location();
                var position = new SourcePosition(startLocation, endLocation);

                var token = new Token(kind, _currentSpelling.ToString(), position);
                if (_debug)
                {
                    Console.WriteLine(token);
                }

                yield return token;
                if (token.Kind == TokenKind.EndOfText) { break; }
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

     
        // Appends the current character to the current token, and gets
        //the next character from the source program.

        void TakeIt()
        {
            _currentSpelling.Append((char)_source.Current);
            _source.MoveNext();
        }


        //Skip a single separator.
       
        void ScanSeparator()
        {
            switch (_source.Current) {
                case '!':
                    while (!(_source.Current.Equals('\n'))) {
                        TakeIt();
                    }
                    break;
                case ' ':
                    TakeIt();
                    break;
                case '\t':
                    TakeIt();
                    break;
                case '\n':
                    TakeIt();
                    break;
            }
                
        }

		//Build up tokens.
		TokenKind ScanToken()
        {
            Regex token = new Regex("^[a-zA-Z]+");

            if (token.IsMatch(((char) _source.Current).ToString())) {
                while (token.IsMatch(((char) _source.Current).ToString())) {
                    TakeIt();
                }
                return TokenKind.Identifier;
            }
            
            switch (_source.Current)
            {   
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    TakeIt();
                    while (IsDigit(_source.Current))
                    {
                        TakeIt();
                    }
                    return TokenKind.IntLiteral;
                    
                case '+':
                    TakeIt();
                    return TokenKind.Operator;
                    
                case '~':
                    TakeIt();
                    return TokenKind.Is;
                    
                case ':':
                    TakeIt();
                    return TokenKind.Colon;
                    
                case ';':
                    TakeIt();
                    if (_source.Current == '=') {
                        TakeIt();
                        return TokenKind.Becomes;
                    }
                    else {
                        return TokenKind.Semicolon;
                    }
                    
                case '(':
                    TakeIt();
                    return TokenKind.LeftParen;
                    
                case ')':
                    TakeIt();
                    return TokenKind.RightParen;
                    
                case '\'':
                    TakeIt();
                    _currentSpelling.Remove(_currentSpelling.Length - 1, 1);
                    TakeIt();
                    TakeIt();
                    _currentSpelling.Remove(_currentSpelling.Length - 1, 1);

                    return TokenKind.CharLiteral;
                    
                case '>':
                    return TokenKind.Operator;
                    
                case '<':
                    return TokenKind.Operator;
                    
                case '/':
                    TakeIt();
                    if (_source.Current == '\\') {
                        TakeIt();
                        return TokenKind.Operator;
                    }
                    else {
                        return TokenKind.Error;
                    }
                    
                
                    
                case -1:
                    return TokenKind.EndOfText;
          
                default:
                    TakeIt();
                    return TokenKind.Error;
            }
        }

        bool IsLetter(int ch)
        {
            return ('a' <= ch && ch <= 'z') || ('A' <= ch && ch <= 'Z');
        }

        bool IsDigit(int ch)
        {
            return '0' <= ch && ch <= '9';
        }

        bool IsOperator(int ch)
        {
            switch (ch)
            {
                case '+':
                case '-':
                case '*':
                case '/':
                case '=':
                case '<':
                case '>':
                case '\\':
                case '&':
                case '@':
                case '%':
                case '^':
                case '?':
                    return true;

                default:
                    return false;
            }
        }
    }
}