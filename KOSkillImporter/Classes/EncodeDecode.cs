/**
 * ______________________________________________________
 * This file is part of ko-skill-table-importer project.
 * 
 * @author       Mustafa Kemal Gılor <mustafagilor@gmail.com> (2016)
 * .
 * SPDX-License-Identifier:	MIT
 * ______________________________________________________
 */

using System.IO;

namespace KOSkillImporter.Classes
{
    internal class EncryptionKOStandard 
    {
        internal void Decode(ref byte[] data)
        {
            uint num = 0x816;
            for (int i = 0; i < data.Length; i++)
            {
                byte num3 = data[i];
                uint num4 = num;
                byte num5 = 0;
                num4 &= 0xff00;
                num4 = num4 >> 8;
                num5 = (byte)(num4 ^ num3);
                num4 = num3;
                num4 += num;
                num4 &= 0xffff;
                num4 *= 0x6081;
                num4 &= 0xffff;
                num4 += 0x1608;
                num4 &= 0xffff;
                num = num4;
                data[i] = num5;
            }
        }

        void Encode(FileStream stream)
        {
            int num = stream.ReadByte();
            uint num2 = 0x816;
            while (num != -1)
            {
                stream.Seek(-1L, SeekOrigin.Current);
                byte num3 = (byte)(num & 0xff);
                byte num4 = 0;
                uint num5 = num2;
                num5 &= 0xff00;
                num5 = num5 >> 8;
                num4 = (byte)(num5 ^ num3);
                num5 = num4;
                num5 += num2;
                num5 &= 0xffff;
                num5 *= 0x6081;
                num5 &= 0xffff;
                num5 += 0x1608;
                num5 &= 0xffff;
                num2 = num5;
                stream.WriteByte(num4);
                num = stream.ReadByte();
            }
        }

    }
}
