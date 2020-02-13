using System;
using System.Collections.Generic;
using System.Text;

namespace Y86Emulator
{
    abstract class ProcessorStage
    {
        private PipelineRegisters Input;
        private PipelineRegisters Output;

        public abstract bool Execute();

        public abstract void Reset();
    }
}
