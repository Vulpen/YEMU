# YEMU
A pet project assembler and emulator for the Y86 architecture.
This project isn't configured to run as a standalone executable or on other machines.

## YLib
A core library shared functionality for all the assembler/emulator projects.
Contains sample assembly and binary functions.

## YAS
Currently an assembler for the Y86 Architecture that produces binary files.
This project converts `.yas` assembly files to `.yin` machine code, which can be read in an emulator.

## Y86SEQEmulator
A version of the sequential Y86 emulator.

## SEQRenderer
Uses the sequential emulator in a `MonoGame` project to render pixels.
