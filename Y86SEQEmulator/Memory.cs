using System;
using System.Collections.Generic;
using System.Text;

namespace Y86SEQEmulator
{
    class Memory
    {
        EmuReader ROMReader;
        byte[] memory;
        UInt32 start_addr;
        UInt32 end_addr;

        public Memory()
        {
            memory = new byte[256];
            start_addr = 0x00001000;
            end_addr = start_addr + 255;
            ROMReader = new EmuReader();
            ROMReader.OpenFile(@"D:\[KEEP]ProgrammingProjects\C#\Y86Emulator\YAS\Examples\ex1.yin");
        }

        public byte ReadByte(UInt32 address)
        {
            if( address > end_addr)
            {
                throw new MemberAccessException("Attempt to read memory outside of expected bounds.");
            }else if(address < start_addr)
            {
                return ROMReader.ReadByteAtIndex(address);
            }

            return memory[address - start_addr];
        }
        //public UInt16 ReadWord(UInt32 address);
        public UInt32 ReadLong(UInt32 address)
        {
            return 0;
        }
        //public UInt64 ReadQuad(UInt32 address);


        public void WriteByte(UInt32 address, byte value)
        {
            if(address > end_addr)
            {
                throw new MemberAccessException("Attempt to read memory outside of expected bounds.");
            }

            memory[address - start_addr] = value;
        }
    }
}
