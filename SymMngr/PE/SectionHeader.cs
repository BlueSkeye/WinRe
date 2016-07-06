using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SymMngr.PE
{
    internal class SectionHeader
    {
        internal SectionHeader(IntPtr at, ref int offset)
        {
        }

      //  BYTE Name[IMAGE_SIZEOF_SHORT_NAME];
      //  union {
      //  DWORD PhysicalAddress;
      //      DWORD VirtualSize;
      //  } Misc;
      //  DWORD VirtualAddress;
      //  DWORD SizeOfRawData;
      //  DWORD PointerToRawData;
      //  DWORD PointerToRelocations;
      //  DWORD PointerToLinenumbers;
      //  WORD NumberOfRelocations;
      //  WORD NumberOfLinenumbers;
      //  DWORD Characteristics;
    }
}
