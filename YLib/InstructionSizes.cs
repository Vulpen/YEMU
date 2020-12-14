using System;
using System.Collections.Generic;
using System.Text;

namespace YLib
{
    public class InstructionSizes
    {
        private Dictionary<EnumInstructions, int> Sizes;

        public static Dictionary<EnumInstructions, int> AllSizes = new Dictionary<EnumInstructions, int>
        {
            {EnumInstructions.halt,  1},
            {EnumInstructions.nop,   1},
            {EnumInstructions.rrmov, 2},
            {EnumInstructions.irmov, 10},
            {EnumInstructions.rmmov, 10},
            {EnumInstructions.mrmov, 10},
            {EnumInstructions.ret,   1},
            {EnumInstructions.push,  2},
            {EnumInstructions.pop,   2},
            {EnumInstructions.add,   2},
            {EnumInstructions.sub,   2},
            {EnumInstructions.imul,  2},
            {EnumInstructions.xor,   2},
            {EnumInstructions.jmp,   9},
            {EnumInstructions.je,    9},
            {EnumInstructions.jle,   9},
            {EnumInstructions.jge,   9},
            {EnumInstructions.jg,    9},
            {EnumInstructions.jl,    9},
        };

        public InstructionSizes()
        {
            Sizes = new Dictionary<EnumInstructions, int>();
            Sizes.Add(EnumInstructions.halt, 1);
            Sizes.Add(EnumInstructions.nop, 1);
            Sizes.Add(EnumInstructions.rrmov, 2);
            Sizes.Add(EnumInstructions.irmov, 6);
            Sizes.Add(EnumInstructions.rmmov, 6);
            Sizes.Add(EnumInstructions.mrmov, 6);
            Sizes.Add(EnumInstructions.ret, 1);
            Sizes.Add(EnumInstructions.push, 2);
            Sizes.Add(EnumInstructions.pop, 2);
            Sizes.Add(EnumInstructions.add, 2);
            Sizes.Add(EnumInstructions.sub, 2);
            Sizes.Add(EnumInstructions.imul, 2);
            Sizes.Add(EnumInstructions.xor, 2);
            Sizes.Add(EnumInstructions.jmp, 5);
            Sizes.Add(EnumInstructions.je, 5);
            Sizes.Add(EnumInstructions.jle, 5);
            Sizes.Add(EnumInstructions.jge, 5);
            Sizes.Add(EnumInstructions.jg, 5);
            Sizes.Add(EnumInstructions.jl, 5);
        }

        public int GetInstructionSize(EnumInstructions inst)
        {
            if (Sizes.ContainsKey(inst))
            {
                return Sizes[inst];
            }
            throw new Exception("Tried to get an unsupported instruction size.");
        }
    }
}
