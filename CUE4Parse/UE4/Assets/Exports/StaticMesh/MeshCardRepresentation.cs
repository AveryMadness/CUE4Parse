using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.Assets.Exports.StaticMesh
{
    public class FLumenCardBuildData
    {
        public FLumenCardOBB OBB;
        public byte LODLevel;
        public byte AxisAlignedDirectionIndex;

        public FLumenCardBuildData(FArchive Ar)
        {
            OBB = Ar.Read<FLumenCardOBB>();
            LODLevel = Ar.Game < GAME_UE5_1 ? Ar.Read<byte>() : (byte) 0x00;
            AxisAlignedDirectionIndex = Ar.Read<byte>();
        }
    }

    public struct FLumenCardOBB
    {
        public FVector AxisX, AxisY, AxisZ, Origin, Extent;
    }

    public class FCardRepresentationData
    {
        public FBox Bounds;
        public int MaxLodLevel;
        public bool bMostlyTwoSided;
        public FLumenCardBuildData[] CardBuildData;

        public FCardRepresentationData(FArchive Ar)
        {
            Bounds = Ar.Game != GAME_Highguard ? new FBox(Ar) : Ar.Read<FBox>();
            MaxLodLevel = Ar.Game < GAME_UE5_1 || Ar.Game== GAME_WorldofJadeDynasty ? Ar.Read<int>() : 0;
            // Added to the Lumen card build data between 5.1 and 5.2; titles cooked off a main-branch
            // engine between the two (Fortnite 23.x) carry it under a 5.1 package version, so it is
            // a version option rather than a game check
            bMostlyTwoSided = Ar.Versions["StaticMesh.HasCardMostlyTwoSided"] && Ar.ReadBoolean();
            CardBuildData = Ar.ReadArray(() => new FLumenCardBuildData(Ar));
        }
    }
}
