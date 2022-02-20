# Y86 Docs

`YEMU` is a pet project, which is a collection of tools for the academic Y86 language.


`YAS` is the Y86 assembler, which translates `.yas` assembly files and outputs `.yin` binary versions.
`Y86SEQEmulator` is a simple emulator based on the `Sequental` machine from the book.
`SEQRenderer` is a machine which is able to render pixels with MonoGame.

## YLib

Core library for this project. Contains the basic definitions of `Tokens` and general language constructs.


## YAS

### Assembler
Reads in a Y86 file and fully converts to machine language.
`FirstPass()` - Function which performs lexical analysis and parsing
`SecondPass()` - Performs token resolution

### Keywords

Contains definitions of key words within the Y86 language.



No license released yet