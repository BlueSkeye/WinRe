
namespace SymMngr.Api
{
    public enum DebugInformationType
    {
        /// <summary>Unknown value, ignored by all tools.</summary>
        Unknown = 0,
        /// <summary>COFF debugging information (line numbers, symbol table, and string table).
        /// This type of debugging information is also pointed to by fields in the file headers.</summary>
        Coff,
        /// <summary>CodeView debugging information. The format of the data block is described by
        /// the CodeView 4.0 specification.</summary>
        Codeview,
        /// <summary>Frame pointer omission (FPO) information. This information tells the debugger how
        /// to interpret nonstandard stack frames, which use the EBP register for a purpose other than
        /// as a frame pointer.</summary>
        FramePointerOmission,
        /// <summary>Miscellaneous information.</summary>
        Miscellaneous,
        /// <summary>Exception information.</summary>
        Exception,
        /// <summary>Fixup information.</summary>
        Fixup,
        /// <summary>Borland debugging information.</summary>
        Borland,
    }
}
