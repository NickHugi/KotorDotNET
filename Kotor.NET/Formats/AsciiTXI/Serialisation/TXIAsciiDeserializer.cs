using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Exceptions;
using Kotor.NET.Resources.KotorTXI;

namespace Kotor.NET.Formats.AsciiTXI.Serialisation;

public class TXIAsciiDeserializer
{
    private TXIAscii _ascii { get; }

    public TXIAsciiDeserializer(TXIAscii ascii)
    {
        _ascii = ascii;
    }

    public TXI Deserialize()
    {
        try
        {
            var txi = new TXI();

            foreach (var field in _ascii.Fields)
            {
                Action<TXI, TXIAsciiField> assignFieldAction = field.Instruction switch
                {
                    TXIAsciiInstructions.AlphaMean => AssignAlphaMean,
                    TXIAsciiInstructions.BumpmapTexture => AssignBumpmapTexture,
                    TXIAsciiInstructions.BumpShinyTexture => AssignBumpShinyTexture,
                    TXIAsciiInstructions.EnvMapTexture => AssignEnvMapTexture,
                    TXIAsciiInstructions.Blending => AssignBlending,
                    TXIAsciiInstructions.WaterAlpha => AssignWaterAlpha,
                    TXIAsciiInstructions.Cube => AssignCube,
                    TXIAsciiInstructions.IsLightmap => AssignIsLightmap,
                    TXIAsciiInstructions.Decal => AssignDecal,
                    TXIAsciiInstructions.DownsampleMin => AssignDownsampleMin,
                    TXIAsciiInstructions.DownsampleMax => AssignDownsampleMax,
                    TXIAsciiInstructions.Mipmap => AssignMipmap,
                    TXIAsciiInstructions.CompressTexture => AssignCompress,
                    TXIAsciiInstructions.Clamp => AssignClamp,
                    TXIAsciiInstructions.Filter => AssignFilter,
                    TXIAsciiInstructions.IsBumpmap => AssignIsBumpmap,
                    TXIAsciiInstructions.BumpmapScale => AssignBumpmapScale,
                    TXIAsciiInstructions.BumpmapScaling => AssignBumpmapScaling,
                    TXIAsciiInstructions.IsDiffuseBumpmap => AssignIsDiffuseBumpmap,
                    TXIAsciiInstructions.IsSpecularBumpmap => AssignIsSpecularBumpmap,
                    TXIAsciiInstructions.ProcedureType => AssignProcedureType,
                    TXIAsciiInstructions.DefaultWidth => AssignDefaultWidth,
                    TXIAsciiInstructions.DefaultHeight => AssignDefaultHeight,
                    TXIAsciiInstructions.NumX => AssignNumFramesOnXAxis,
                    TXIAsciiInstructions.NumY => AssignNumFramesOnYAxis,
                    TXIAsciiInstructions.FPS => AssignFPS,
                    TXIAsciiInstructions.Speed => AssignSpeed,
                    TXIAsciiInstructions.Distort => AssignDistort,
                    TXIAsciiInstructions.DistortAmplitude => AssignDistortionAmplitude,
                    TXIAsciiInstructions.BaselineHeight => AssignBaselineHeight,
                    TXIAsciiInstructions.NumChars => AssignNumberOfCharacters,
                    TXIAsciiInstructions.FontHeight => AssignFontHeight,
                    TXIAsciiInstructions.TextureWidth => AssignTextureWidth,
                    TXIAsciiInstructions.SpacingB => AssignSpacingB,
                    TXIAsciiInstructions.SpacingR => AssignSpacingR,
                    TXIAsciiInstructions.CaretIndent => AssignCaretIndent,
                    TXIAsciiInstructions.UpperLeftCoords => AssignUpperLeftCoords,
                    TXIAsciiInstructions.LowerRightCoords => AssignLowerRightCoords,
                    _ => throw new NotImplementedException($"Unknown instruction {field.Instruction}")
                };

                assignFieldAction(txi, field);
            }

            return txi;
        }
        catch (Exception e)
        {
            throw new DeserializationException("Failed to deserialize the TXI data.", e);
        }
    }

    private void AssignAlphaMean(TXI txi, TXIAsciiField field) => txi.Material.AlphaMean = decimal.Parse(field.Values[0]);
    private void AssignBumpmapTexture(TXI txi, TXIAsciiField field) => txi.Material.BumpmapTexture = field.Values[0];
    private void AssignBumpShinyTexture(TXI txi, TXIAsciiField field) => txi.Material.BumpyShinyTexture = field.Values[0];
    private void AssignEnvMapTexture(TXI txi, TXIAsciiField field) => txi.Material.EnvironmentMapTexture = field.Values[0];
    private void AssignBlending(TXI txi, TXIAsciiField field)
    {
        var blending = field.Values.First();
        txi.Material.Blending = blending switch
        {
            "default" => TXIBlending.Default,
            "additive" => TXIBlending.Additive,
            "punchthrough" => TXIBlending.Punchthrough,
            _ => throw new NotImplementedException($"Unknown blending type {blending}")
        };
    }
    private void AssignWaterAlpha(TXI txi, TXIAsciiField field) => txi.Material.WaterAlpha = decimal.Parse(field.Values[0]);
    private void AssignCube(TXI txi, TXIAsciiField field) => txi.Texture.IsCubemap = field.Values[0] == "1";
    private void AssignIsLightmap(TXI txi, TXIAsciiField field) => txi.Texture.IsLightmap = field.Values[0] == "1";
    private void AssignDecal(TXI txi, TXIAsciiField field) => txi.Texture.IsDecal = field.Values[0] == "1";
    private void AssignDownsampleMin(TXI txi, TXIAsciiField field) => txi.Texture.DownsampleMin = int.Parse(field.Values[0]);
    private void AssignDownsampleMax(TXI txi, TXIAsciiField field) => txi.Texture.DownsampleMax = int.Parse(field.Values[0]);
    private void AssignMipmap(TXI txi, TXIAsciiField field) => txi.Texture.EnableMipmaps = field.Values[0] == "1";
    private void AssignFilter(TXI txi, TXIAsciiField field) => txi.Texture.UseLinearFiltering = field.Values[0] == "1";
    private void AssignCompress(TXI txi, TXIAsciiField field) => txi.Texture.CompressTexture = field.Values[0] == "1";
    private void AssignClamp(TXI txi, TXIAsciiField field) => txi.Texture.Clamp = (TXIClamping)int.Parse(field.Values[0]);
    private void AssignIsBumpmap(TXI txi, TXIAsciiField field) => txi.Bumpmap.Enabled = field.Values[0] == "1";
    private void AssignBumpmapScale(TXI txi, TXIAsciiField field) => txi.Bumpmap.Scale = decimal.Parse(field.Values[0]);
    private void AssignBumpmapScaling(TXI txi, TXIAsciiField field) => txi.Bumpmap.Scaling = decimal.Parse(field.Values[0]);
    private void AssignIsDiffuseBumpmap(TXI txi, TXIAsciiField field) => txi.Bumpmap.IsDiffuseBumpMap = field.Values[0] == "1";
    private void AssignIsSpecularBumpmap(TXI txi, TXIAsciiField field) => txi.Bumpmap.IsSpecularBumpMap = field.Values[0] == "1";
    private void AssignProcedureType(TXI txi, TXIAsciiField field) => txi.Procedure.Type = field.Values[0] switch
    {
        "cycle" => TXIProcedureType.Cycle,
        "water" => TXIProcedureType.Water,
        "arturo" => TXIProcedureType.Arturo,
        "random" => TXIProcedureType.Random,
        "ringtexdistortion" => TXIProcedureType.RingTexDistortion,
        _ => throw new Exception() // TODO
    };
    private void AssignDefaultWidth(TXI txi, TXIAsciiField field) => txi.Procedure.DefaultWidth = int.Parse(field.Values[0]);
    private void AssignDefaultHeight(TXI txi, TXIAsciiField field) => txi.Procedure.DefaultHeight = int.Parse(field.Values[0]);
    private void AssignNumFramesOnXAxis(TXI txi, TXIAsciiField field) => txi.Procedure.NumFramesOnXAxis = int.Parse(field.Values[0]);
    private void AssignNumFramesOnYAxis(TXI txi, TXIAsciiField field) => txi.Procedure.NumFramesOnYAxis = int.Parse(field.Values[0]);
    private void AssignFPS(TXI txi, TXIAsciiField field) => txi.Procedure.FPS = int.Parse(field.Values[0]);
    private void AssignSpeed(TXI txi, TXIAsciiField field) => txi.Procedure.Speed = decimal.Parse(field.Values[0]);
    private void AssignDistort(TXI txi, TXIAsciiField field) => txi.Procedure.Distort = decimal.Parse(field.Values[0]);
    private void AssignDistortionAmplitude(TXI txi, TXIAsciiField field) => txi.Procedure.DistortAmplitude = decimal.Parse(field.Values[0]);
    private void AssignBaselineHeight(TXI txi, TXIAsciiField field) => txi.Font.BaselineHeight = decimal.Parse(field.Values[0]);
    private void AssignNumberOfCharacters(TXI txi, TXIAsciiField field) => txi.Font.NumberOfCharacters = int.Parse(field.Values[0]);
    private void AssignFontHeight(TXI txi, TXIAsciiField field) => txi.Font.FontHeight = decimal.Parse(field.Values[0]);
    private void AssignTextureWidth(TXI txi, TXIAsciiField field) => txi.Font.TextureWidth = decimal.Parse(field.Values[0]);
    private void AssignSpacingB(TXI txi, TXIAsciiField field) => txi.Font.SpacingBottom = decimal.Parse(field.Values[0]);
    private void AssignSpacingR(TXI txi, TXIAsciiField field) => txi.Font.SpacingRight = decimal.Parse(field.Values[0]);
    private void AssignCaretIndent(TXI txi, TXIAsciiField field) => txi.Font.CaretIndent = decimal.Parse(field.Values[0]);
    private void AssignUpperLeftCoords(TXI txi, TXIAsciiField field)
    {
        txi.Font.Coords ??= new();
        var count = int.Parse(field.Values[0]);

        if (txi.Font.Coords.Count < count)
            txi.Font.Coords.AddRange(Enumerable.Range(0, count - txi.Font.Coords.Count).Select(x => new TXIFontCoords()));

        for (int i = 0; i < count; i++)
        {
            var asciiCoords = field.SubValues.ElementAt(i);
            txi.Font.Coords[i].UpperLeftX = decimal.Parse(asciiCoords.ElementAt(0));
            txi.Font.Coords[i].UpperLeftY = decimal.Parse(asciiCoords.ElementAt(1));
            txi.Font.Coords[i].UpperLeftUnknown = int.Parse(asciiCoords.ElementAt(2));
        }
    }
    private void AssignLowerRightCoords(TXI txi, TXIAsciiField field)
    {
        txi.Font.Coords ??= new();
        var count = int.Parse(field.Values[0]);

        if (txi.Font.Coords.Count < count)
            txi.Font.Coords.AddRange(Enumerable.Range(0, count - txi.Font.Coords.Count).Select(x => new TXIFontCoords()));

        for (int i = 0; i < count; i++)
        {
            var asciiCoords = field.SubValues.ElementAt(i);
            txi.Font.Coords[i].LowerRightX = decimal.Parse(asciiCoords.ElementAt(0));
            txi.Font.Coords[i].LowerRightY = decimal.Parse(asciiCoords.ElementAt(1));
            txi.Font.Coords[i].LowerRightUnknown = int.Parse(asciiCoords.ElementAt(2));
        }
    }
}
