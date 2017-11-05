# CompilersProject
Educational compiler project for simple code generation in C#

Note:
- Uses GPPG to generate general parser code in C# and GPLex to generate scanner code in C#
- Tokenization, Grammar, and lexicon definitions are written by me 
- Parse tree, symbol table, and abstract tree generation are written by me
- Code generation is written by me into CIL language and saved in an .il file
- .il file is converted to a .exe file using Ilasm 
  (to use ilasm command, use the Developer Command Prompt in VS)
