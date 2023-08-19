﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KotorDotNET.Common;
using KotorDotNET.FileFormats.KotorTLK;
using static KotorDotNET.FileFormats.KotorSSF.SSFBinaryStructure;

namespace KotorDotNET.FileFormats.KotorSSF
{
    public class SSFBinaryWriter : IWriter<SSF>
    {
        public BinaryWriter? _writer;
        public SSF? _ssf;

        public SSFBinaryWriter(string filepath)
        {
            _writer = new BinaryWriter(new FileStream(filepath, FileMode.OpenOrCreate));
        }
        public SSFBinaryWriter(Stream stream)
        {
            _writer = new BinaryWriter(stream);
        }

        public void Write(SSF ssf)
        {
            _ssf = ssf;

            var root = new FileRoot();

            foreach (var (sound, stringref) in ssf._sounds)
            {
                root.SoundTable.SoundStringRefs[(int)sound] = stringref;
            }
            
            root.Write(_writer);
        }
    }
}
