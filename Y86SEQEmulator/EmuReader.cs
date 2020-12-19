using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using YLib;

namespace Y86SEQEmulator
{
    //Reads ROMS (.yin files) for the emulator
    class EmuReader : IDisposable
    {
        BinaryReader reader;
        byte[] buffer;

        public EmuReader()
        {
            buffer = new byte[8];
        }

        public void Dispose()
        {
            reader?.Dispose();
        }

        public bool OpenFile(string filePath)
        {
            if (reader != null) { reader.Dispose(); }
            reader = new BinaryReader(File.OpenRead(filePath));
            return true;
        }

        public byte ReadByteAtIndex(uint index)
        {
            if(reader == null)
            {
                throw new ROMException("Attempt to read unopened ROM.");
            }
            reader.BaseStream.Position = index;
            reader.Read(buffer, 0, 1);

            return buffer[0];
        }

        public UInt32 ReadLongAtIndex(uint index)
        {
            if (reader == null)
            {
                throw new ROMException("Attempt to read unopened ROM.");
            }

            reader.BaseStream.Position = index;
            reader.Read(buffer, 0, 4);

            return BitConverter.ToUInt32(buffer, 0);
        }

    }
}
