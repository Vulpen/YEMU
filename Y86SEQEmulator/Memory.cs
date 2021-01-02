using System;
using System.Collections.Generic;
using System.Text;
using YLib;

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
            ROMReader.OpenFile(@"D:\[KEEP]ProgrammingProjects\C#\Y86Emulator\YAS\Examples\drawPixel.yin");
        }

        public Memory(string RomFilePath)
        {
            memory = new byte[256];
            start_addr = 0x00001000;
            end_addr = start_addr + 255;
            ROMReader = new EmuReader();
            ROMReader.OpenFile(RomFilePath);
        }

        public byte ReadByte(UInt32 address)
        {
            if( address > end_addr)
            {
                throw new MemoryAccessException("Attempt to read memory outside of expected bounds.");
            }else if(address < start_addr)
            {
                return ROMReader.ReadByteAtIndex(address);
            }

            return memory[address - start_addr];
        }
        //public UInt16 ReadLord(UInt32 address);
        public UInt32 ReadLong(UInt32 address)
        {
            if(address + 4 > end_addr)
            {
                throw new MemoryAccessException("Attmpt to read memory outside of expected bounds.");
            }else if(address + 4 < start_addr)
            {
                return ROMReader.ReadLongAtIndex(address);
            }

            return memory[address - start_addr];
        }
        //public UInt64 ReadQuad(UInt32 address);


        public void WriteByte(UInt32 address, byte value)
        {
            if(address > end_addr || address < start_addr)
            {
                throw new MemberAccessException("Attempt to write memory outside of expected bounds.");
            }

            memory[address - start_addr] = value;
        }

        public void WriteLong(UInt32 address, UInt32 value)
        {
            if(address > end_addr || address < start_addr)
            {
                throw new MemberAccessException("Attempt to write memory outside of expected bounds.");
            }
            byte[] encoded = BitConverter.GetBytes(value);

            for(int i = 0; i < 4; i++)
            {
                memory[address + i] = encoded[i];
            }
        }
    }
}
