using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Exceptions;
using Kotor.NET.Formats.BinarySSF;
using Kotor.NET.Resources.KotorSSF;

namespace Kotor.NET.Formats.BinarySFF.Serialisation;

public class SSFBinarySerializer
{
    private SSF _ssf { get; }

    public SSFBinarySerializer(SSF ssf)
    {
        _ssf = ssf;
    }

    public SSFBinary Serialize()
    {
        try
        {
            var binary = new SSFBinary();

            binary.FileHeader.FileType = SSFBinaryFileHeader.FILE_TYPES[0];
            binary.FileHeader.FileVersion = SSFBinaryFileHeader.FILE_VERSION;

            binary.SoundList = new();
            binary.SoundList.Sounds[SSFBinarySoundIndex.Battlecry1] = _ssf.Battlecry1;
            binary.SoundList.Sounds[SSFBinarySoundIndex.Battlecry2] = _ssf.Battlecry2;
            binary.SoundList.Sounds[SSFBinarySoundIndex.Battlecry3] = _ssf.Battlecry3;
            binary.SoundList.Sounds[SSFBinarySoundIndex.Battlecry4] = _ssf.Battlecry4;
            binary.SoundList.Sounds[SSFBinarySoundIndex.Battlecry5] = _ssf.Battlecry5;
            binary.SoundList.Sounds[SSFBinarySoundIndex.Select1] = _ssf.Select1;
            binary.SoundList.Sounds[SSFBinarySoundIndex.Select2] = _ssf.Select2;
            binary.SoundList.Sounds[SSFBinarySoundIndex.Select3] = _ssf.Select3;
            binary.SoundList.Sounds[SSFBinarySoundIndex.AttackGrunt1] = _ssf.AttackGrunt1;
            binary.SoundList.Sounds[SSFBinarySoundIndex.AttackGrunt2] = _ssf.AttackGrunt2;
            binary.SoundList.Sounds[SSFBinarySoundIndex.AttackGrunt3] = _ssf.AttackGrunt3;
            binary.SoundList.Sounds[SSFBinarySoundIndex.PainGrunt1] = _ssf.PainGrunt1;
            binary.SoundList.Sounds[SSFBinarySoundIndex.PainGrunt2] = _ssf.PainGrunt2;
            binary.SoundList.Sounds[SSFBinarySoundIndex.LowHealth] = _ssf.LowHealth;
            binary.SoundList.Sounds[SSFBinarySoundIndex.Dead] = _ssf.Dead;
            binary.SoundList.Sounds[SSFBinarySoundIndex.CriticalHit] = _ssf.CriticalHit;
            binary.SoundList.Sounds[SSFBinarySoundIndex.TargetImmune] = _ssf.TargetImmune;
            binary.SoundList.Sounds[SSFBinarySoundIndex.LayMine] = _ssf.LayMine;
            binary.SoundList.Sounds[SSFBinarySoundIndex.DisarmMine] = _ssf.DisarmMine;
            binary.SoundList.Sounds[SSFBinarySoundIndex.BeginStealth] = _ssf.BeginStealth;
            binary.SoundList.Sounds[SSFBinarySoundIndex.BeginSearch] = _ssf.BeginSearch;
            binary.SoundList.Sounds[SSFBinarySoundIndex.BeginUnlock] = _ssf.BeginUnlock;
            binary.SoundList.Sounds[SSFBinarySoundIndex.UnlockFailed] = _ssf.UnlockFailed;
            binary.SoundList.Sounds[SSFBinarySoundIndex.UnlockSuccess] = _ssf.UnlockSuccess;
            binary.SoundList.Sounds[SSFBinarySoundIndex.PartySeparated] = _ssf.PartySeparated;
            binary.SoundList.Sounds[SSFBinarySoundIndex.PartyRejoined] = _ssf.PartyRejoined;
            binary.SoundList.Sounds[SSFBinarySoundIndex.Poisoned] = _ssf.Poisoned;

            binary.Recalculate();

            return binary;
        }
        catch (Exception e)
        {
            throw new SerializationException("Failed to serialize the SSF data.", e);
        }
    }
}
