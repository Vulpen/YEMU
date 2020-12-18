

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
        rsp,
        rbp,
        rsi,
        rdi,
        r8,
        r9,
        r10,
        r11,
        r12,
        r13,
        r14
    }

}