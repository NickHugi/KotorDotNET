using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Exceptions;
using Kotor.NET.Formats.BinarySSF;
using Kotor.NET.Resources.KotorSSF;

namespace Kotor.NET.Formats.BinarySFF.Serialisation;

public class SSFBinaryDeserializer
{
    private SSFBinary _binary { get; }

    public SSFBinaryDeserializer(SSFBinary binary)
    {
        _binary = binary;
    }

    public SSF Deserialize()
    {
        try
        {
            var ssf = new SSF();

            ssf.Battlecry1 = _binary.SoundList.Sounds.ElementAt(SSFBinarySoundIndex.Battlecry1);
            ssf.Battlecry2 = _binary.SoundList.Sounds.ElementAt(SSFBinarySoundIndex.Battlecry2);
            ssf.Battlecry3 = _binary.SoundList.Sounds.ElementAt(SSFBinarySoundIndex.Battlecry3);
            ssf.Battlecry4 = _binary.SoundList.Sounds.ElementAt(SSFBinarySoundIndex.Battlecry4);
            ssf.Battlecry5 = _binary.SoundList.Sounds.ElementAt(SSFBinarySoundIndex.Battlecry5);
            ssf.Select1 = _binary.SoundList.Sounds.ElementAt(SSFBinarySoundIndex.Select1);
            ssf.Select2 = _binary.SoundList.Sounds.ElementAt(SSFBinarySoundIndex.Select2);
            ssf.Select3 = _binary.SoundList.Sounds.ElementAt(SSFBinarySoundIndex.Select3);
            ssf.AttackGrunt1 = _binary.SoundList.Sounds.ElementAt(SSFBinarySoundIndex.AttackGrunt1);
            ssf.AttackGrunt2 = _binary.SoundList.Sounds.ElementAt(SSFBinarySoundIndex.AttackGrunt2);
            ssf.AttackGrunt3 = _binary.SoundList.Sounds.ElementAt(SSFBinarySoundIndex.AttackGrunt3);
            ssf.PainGrunt1 = _binary.SoundList.Sounds.ElementAt(SSFBinarySoundIndex.PainGrunt1);
            ssf.PainGrunt2 = _binary.SoundList.Sounds.ElementAt(SSFBinarySoundIndex.PainGrunt2);
            ssf.LowHealth = _binary.SoundList.Sounds.ElementAt(SSFBinarySoundIndex.LowHealth);
            ssf.Dead = _binary.SoundList.Sounds.ElementAt(SSFBinarySoundIndex.Dead);
            ssf.CriticalHit = _binary.SoundList.Sounds.ElementAt(SSFBinarySoundIndex.CriticalHit);
            ssf.TargetImmune = _binary.SoundList.Sounds.ElementAt(SSFBinarySoundIndex.TargetImmune);
            ssf.LayMine = _binary.SoundList.Sounds.ElementAt(SSFBinarySoundIndex.LayMine);
            ssf.DisarmMine = _binary.SoundList.Sounds.ElementAt(SSFBinarySoundIndex.DisarmMine);
            ssf.BeginStealth = _binary.SoundList.Sounds.ElementAt(SSFBinarySoundIndex.BeginStealth);
            ssf.BeginSearch = _binary.SoundList.Sounds.ElementAt(SSFBinarySoundIndex.BeginSearch);
            ssf.BeginUnlock = _binary.SoundList.Sounds.ElementAt(SSFBinarySoundIndex.BeginUnlock);
            ssf.UnlockFailed = _binary.SoundList.Sounds.ElementAt(SSFBinarySoundIndex.UnlockFailed);
            ssf.UnlockSuccess = _binary.SoundList.Sounds.ElementAt(SSFBinarySoundIndex.UnlockSuccess);
            ssf.PartySeparated = _binary.SoundList.Sounds.ElementAt(SSFBinarySoundIndex.PartySeparated);
            ssf.PartyRejoined = _binary.SoundList.Sounds.ElementAt(SSFBinarySoundIndex.PartyRejoined);
            ssf.Poisoned = _binary.SoundList.Sounds.ElementAt(SSFBinarySoundIndex.Poisoned);

            return ssf;
        }
        catch (Exception e)
        {
            throw new DeserializationException("Failed to deserialize the SSF data.", e);
        }
    }
}
