

using System;
using System.Collections.Generic;

namespace YLib
{
    public enum EnumVerboseLevels
    {
        None = 0,
        Little,
        All
    }

    public enum EnumTokenTypes
    {
        Instruction = 0,
        Register,
        AddressRegister,
        Immediate,
        Label,
        Unkown,
        AssemblerDirective
    }

    public enum EnumTokenProperties
    {
        TokenType = 0,   //Corresponds to EnumTokenTypes
        LabelId,  //Used for label table
        OpSizeBytes,  //stores 1, 2, 4, 8 if size is added
        ImmediateValue, //If is immediate, stores the value
        RealInstruction, //Corresponds to EnumInstructions
        RegisterNumber,   //Corresponds to EnumRegisters
        AssemblerDirective,      //Corresponds to AssemblerDirectives
        InstructionSize
    }

    public enum EnumAssemblerDirectives
    {
        Position,
        Align,
        Byte,       // 1 Byte
        Word,       // 2 Bytes
        Long,       // 4 Bytes
        Quad        // 8 Bytes
    }

    public enum EnumInstructionSizes
    {
        Byte,
        Word,
        Long,
        Quad
    }

    public enum EnumInstructions
    {
        //Todo: add cmov's
        add = 0,
        sub,
        and,
        xor,
        imul,
        jmp,
        jle,
        jl,
        je,
        jne,
        jge,
        jg,
        irmov,
        rrmov,
        mrmov,
        rmmov,
        halt,
        nop,
        call,
        ret,
        push,
        pop
    }


    /// <summary>
    /// Represents Y86 Registers. These are in order of their Identifying number.
    /// </summary>
    public enum EnumRegisters
    {
        rax = 0,
        rcx,
        rdx,
        rbx,
        rsi,
        rdi,
        rsp,
        rbp,
        r8,
        r9,
        r10,
        r11,
        r12,
        r13,
        r14
    }

    public enum EnumConditionCodes
    {
        /// <summary>
        /// Unsigned overflow
        /// </summary>
        CF,
        /// <summary>
        /// Zero Flag
        /// </summary>
        ZF,
        /// <summary>
        /// Negative Flag
        /// </summary>
        SF,
        /// <summary>
        /// Signed Overflow
        /// </summary>
        OF
    }

    public static class EnumHelper
    {
        public static EnumInstructions GetInstructions(byte icode, byte ifun)
        {
            switch(icode)
            {
                case 0:
                    return EnumInstructions.halt;
                case 1:
                    return EnumInstructions.nop;
                case 2:
                    //Need FN for cmov's
                    return EnumInstructions.rrmov;
                case 3:
                    return EnumInstructions.irmov;
                case 4:
                    return EnumInstructions.rmmov;
                case 5:
                    return EnumInstructions.mrmov;
                case 6:
                    //Need FN
                    switch(ifun)
                    {
                        default:
                        case 0:
                            return EnumInstructions.add;
                        case 1:
                            return EnumInstructions.sub;
                        case 2:
                            return EnumInstructions.and;
                        case 3:
                            return EnumInstructions.xor;
                    }
                case 7:
                    //Need FN
                    switch(ifun)
                    {
                        default:
                        case 0:
                            return EnumInstructions.jmp;
                        case 1:
                            return EnumInstructions.jle;
                        case 2:
                            return EnumInstructions.jl;
                        case 3:
                            return EnumInstructions.je;
                        case 4:
                            return EnumInstructions.jne;
                        case 5:
                            return EnumInstructions.jge;
                        case 6:
                            return EnumInstructions.jg;
                    }
                case 8:
                    return EnumInstructions.call;
                case 9:
                    return EnumInstructions.ret;
                case 10:
                    return EnumInstructions.push;
                case 11:
                    return EnumInstructions.pop;
            }

            return EnumInstructions.nop;
        }
    }

}