using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data;
using Kotor.NET.Exceptions;
using Kotor.NET.Resources.KotorTXI;

namespace Kotor.NET.Formats.AsciiTXI.Serialisation;

public class TXIAsciiSerializer
{
    private TXI _txi { get; }

    public TXIAsciiSerializer(TXI txi)
    {
        _txi = txi;
    }

    public TXIAscii Serialize()
    {
        try
        {
            var ascii = new TXIAscii();

            SerializeMaterial(ascii, _txi.Material);
            SerializeTexture(ascii, _txi.Texture);
            SerializeBumpmap(ascii, _txi.Bumpmap);
            SerializeProcedure(ascii, _txi.Procedure);
            SerializeFont(ascii, _txi.Font);

            return ascii;
        }
        catch (Exception e)
        {
            throw new SerializationException("Failed to serialize the TXI data.", e);
        }
    }

    public void SerializeMaterial(TXIAscii ascii, TXIMaterial material)
    {
        if (material.BumpyShinyTexture is not null)
            ascii.Fields.Add(new(TXIAsciiInstructions.BumpShinyTexture, [material.BumpyShinyTexture]));

        if (material.BumpmapTexture is not null)
            ascii.Fields.Add(new(TXIAsciiInstructions.BumpmapTexture, [material.BumpmapTexture]));

        if (material.EnvironmentMapTexture is not null)
            ascii.Fields.Add(new(TXIAsciiInstructions.EnvMapTexture, [material.EnvironmentMapTexture]));

        if (material.Blending is not null)
        {
            var blending = material.Blending switch
            {
                TXIBlending.Default => "default",
                TXIBlending.Additive => "additive",
                TXIBlending.Punchthrough => "punchthrough",
                _ => throw new NotImplementedException($"Unknown blending enum value {material.Blending}")
            };
            ascii.Fields.Add(new(TXIAsciiInstructions.Blending, [blending]));
        }

        if (material.WaterAlpha is not null)
            ascii.Fields.Add(new(TXIAsciiInstructions.WaterAlpha, ["1"]));

        if (material.AlphaMean is not null)
            ascii.Fields.Add(new(TXIAsciiInstructions.AlphaMean, ["1"]));
    }

    public void SerializeTexture(TXIAscii ascii, TXITexture texture)
    {
        if (texture.IsCubemap == true)
            ascii.Fields.Add(new(TXIAsciiInstructions.Cube, ["1"]));

        if (texture.IsLightmap == true)
            ascii.Fields.Add(new(TXIAsciiInstructions.IsLightmap, ["1"]));

        if (texture.IsDecal == true)
            ascii.Fields.Add(new(TXIAsciiInstructions.Decal, ["1"]));

        if (texture.DownsampleMin is not null)
            ascii.Fields.Add(new(TXIAsciiInstructions.DownsampleMin, [texture.DownsampleMin.ToString()]));

        if (texture.DownsampleMax is not null)
            ascii.Fields.Add(new(TXIAsciiInstructions.DownsampleMax, [texture.DownsampleMax.ToString()]));

        if (texture.EnableMipmaps == true)
            ascii.Fields.Add(new(TXIAsciiInstructions.Mipmap, ["1"]));

        if (texture.UseLinearFiltering == true)
            ascii.Fields.Add(new(TXIAsciiInstructions.Filter, ["1"]));

        if (texture.Clamp is not null)
        {
            var clamp = texture.Clamp switch
            {
                TXIClamping.XAxis => "1",
                TXIClamping.YAxis => "2",
                TXIClamping.BothAxes => "3",
                _ => throw new NotImplementedException($"Unknown clamping enum value {texture.Clamp}")
            };
            ascii.Fields.Add(new(TXIAsciiInstructions.Clamp, [clamp]));
        }

        if (texture.CompressTexture == true)
            ascii.Fields.Add(new(TXIAsciiInstructions.CompressTexture, ["1"]));
    }

    public void SerializeBumpmap(TXIAscii ascii, TXIBumpmap bumpmap)
    {
        if (bumpmap.Enabled == true)
            ascii.Fields.Add(new(TXIAsciiInstructions.IsBumpmap, ["1"]));

        if (bumpmap.IsDiffuseBumpMap == true)
            ascii.Fields.Add(new(TXIAsciiInstructions.IsDiffuseBumpmap, ["1"]));

        if (bumpmap.IsSpecularBumpMap == true)
            ascii.Fields.Add(new(TXIAsciiInstructions.IsSpecularBumpmap, ["1"]));

        if (bumpmap.Scale is not null)
            ascii.Fields.Add(new(TXIAsciiInstructions.BumpmapScale, [bumpmap.Scale.ToString()]));

        if (bumpmap.Scaling is not null)
            ascii.Fields.Add(new(TXIAsciiInstructions.BumpmapScaling, [bumpmap.Scaling.ToString()]));
    }

    public void SerializeProcedure(TXIAscii ascii, TXIProcedure procedure)
    {
        if (procedure.Type is not null)
        {
            var type = procedure.Type switch
            {
                TXIProcedureType.Cycle => "cycle",
                TXIProcedureType.Water => "water",
                TXIProcedureType.Arturo => "arturo",
                TXIProcedureType.Random => "random",
                TXIProcedureType.RingTexDistortion => "ringtexdistortion",
                _ => throw new NotImplementedException($"Unknown procedure type enum value {procedure.Type}")
            };
            ascii.Fields.Add(new(TXIAsciiInstructions.ProcedureType, [type]));
        }

        if (procedure.DefaultWidth is not null)
            ascii.Fields.Add(new(TXIAsciiInstructions.DefaultWidth, [procedure.DefaultWidth.ToString()]));

        if (procedure.DefaultHeight is not null)
            ascii.Fields.Add(new(TXIAsciiInstructions.DefaultHeight, [procedure.DefaultWidth.ToString()]));

        if (procedure.NumFramesOnXAxis is not null)
            ascii.Fields.Add(new(TXIAsciiInstructions.NumX, [procedure.DefaultHeight.ToString()]));

        if (procedure.NumFramesOnYAxis is not null)
            ascii.Fields.Add(new(TXIAsciiInstructions.NumY, [procedure.NumFramesOnXAxis.ToString()]));

        if (procedure.FPS is not null)
            ascii.Fields.Add(new(TXIAsciiInstructions.FPS, [procedure.NumFramesOnYAxis.ToString()]));

        if (procedure.Speed is not null)
            ascii.Fields.Add(new(TXIAsciiInstructions.Speed, [procedure.Speed.ToString()]));

        if (procedure.ArturoWidth is not null)
            ascii.Fields.Add(new(TXIAsciiInstructions.ArtuoWidth, [procedure.ArturoWidth.ToString()]));

        if (procedure.ArturoHeight is not null)
            ascii.Fields.Add(new(TXIAsciiInstructions.ArtuoHeight, [procedure.ArturoHeight.ToString()]));

        if (procedure.Distort is not null)
            ascii.Fields.Add(new(TXIAsciiInstructions.Distort, [procedure.Distort.ToString()]));

        if (procedure.DistortAmplitude is not null)
            ascii.Fields.Add(new(TXIAsciiInstructions.DistortAmplitude, [procedure.DistortAmplitude.ToString()]));

        if (procedure.Channel is not null)
        {
            var scales = procedure.Channel.Select(x => new string[] { x.Scale.ToString() });
            ascii.Fields.Add(new(TXIAsciiInstructions.ChannelScale, [procedure.Channel.Count.ToString()], scales));

            var translate = procedure.Channel.Select(x => new string[] { x.Translate.ToString() });
            ascii.Fields.Add(new(TXIAsciiInstructions.ChannelTranslate, [procedure.Channel.Count.ToString()], translate));
        }
    }

    public void SerializeFont(TXIAscii ascii, TXIFont font)
    {
        if (font.BaselineHeight is not null)
            ascii.Fields.Add(new(TXIAsciiInstructions.BaselineHeight, [font.BaselineHeight.ToString()]));

        if (font.NumberOfCharacters is not null)
            ascii.Fields.Add(new(TXIAsciiInstructions.NumChars, [font.NumberOfCharacters.ToString()]));

        if (font.FontHeight is not null)
            ascii.Fields.Add(new(TXIAsciiInstructions.FontHeight, [font.FontHeight.ToString()]));

        if (font.TextureWidth is not null)
            ascii.Fields.Add(new(TXIAsciiInstructions.TextureWidth, [font.TextureWidth.ToString()]));

        if (font.SpacingBottom is not null)
            ascii.Fields.Add(new(TXIAsciiInstructions.SpacingB, [font.SpacingBottom.ToString()]));

        if (font.SpacingRight is not null)
            ascii.Fields.Add(new(TXIAsciiInstructions.SpacingR, [font.SpacingRight.ToString()]));

        if (font.CaretIndent is not null)
            ascii.Fields.Add(new(TXIAsciiInstructions.CaretIndent, [font.CaretIndent.ToString()]));

        if (font.Coords is not null)
        {
            var upperleft = font.Coords.Select(x => new string[] { x.UpperLeftX.ToString(), x.UpperLeftY.ToString(), x.UpperLeftUnknown.ToString() });
            ascii.Fields.Add(new(TXIAsciiInstructions.UpperLeftCoords, [font.Coords.Count.ToString()], upperleft));

            var lowerright = font.Coords.Select(x => new string[] { x.LowerRightX.ToString(), x.LowerRightY.ToString(), x.UpperLeftUnknown.ToString() });
            ascii.Fields.Add(new(TXIAsciiInstructions.LowerRightCoords, [font.Coords.Count.ToString()], lowerright));
        }
    }

}
