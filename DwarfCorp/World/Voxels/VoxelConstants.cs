﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DwarfCorp
{
    public class VoxelConstants
    {
        public const Int32 ChunkSizeX = 16;
        public const Int32 ChunkSizeY = 16;
        public const Int32 ChunkSizeZ = 16;
        public const Int32 ChunkVoxelCount = ChunkSizeX * ChunkSizeY * ChunkSizeZ;

        public const Int32 XDivShift = 4;
        public const Int32 YDivShift = 4;
        public const Int32 ZDivShift = 4;

        public const Int32 XModMask = 0x0000000F;
        public const Int32 YModMask = 0x0000000F;
        public const Int32 ZModMask = 0x0000000F;

        public static Int32 DataIndexOf(LocalVoxelCoordinate C)
        {
            return (C.Y * ChunkSizeX * ChunkSizeZ) + (C.Z * ChunkSizeX) + C.X;
        }

        public const Int32 MaximumVoxelTypes = 256;

        //  Todo: Use inverse mask naming pattern here.
        // Byte - [1111 0000] Grass Type
        //        [0000 1111] Grass Decay
        public const Int32 MaximumGrassTypes = 16;
        public const Int32 GrassTypeShift = 4;
        public const Int32 GrassTypeMask = 0xF0;
        public const Int32 MaximumGrassDecay = 16;
        public const Int32 GrassDecayMask = 0x0F;

        // Byte - [0001 1111] Ramp Type
        //        [0010 0000] Sunlight
        //        [0100 0000] Explored
        //        [1000 0000] Player built voxel
        public const Int32 RampTypeShift = 0x0;
        public const Int32 RampTypeMask = 0x1F;
        public const Int32 InverseRampTypeMask = 0xE0;
        public const Int32 SunlightShift = 5;
        public const Int32 SunlightMask = 0x20;
        public const Int32 InverseSunlightMask = 0xDF;
        public const Int32 ExploredShift = 6;
        public const Int32 ExploredMask = 0x40;
        public const Int32 InverseExploredMask = 0xBF;
        public const Int32 PlayerBuiltVoxelShift = 7;
        public const Int32 PlayerBuiltVoxelMask = 0x80;
        public const Int32 InversePlayerBuiltVoxelMask = 0x7F;

        public const Int32 LiquidTypeShift = 6;
        public const Int32 LiquidTypeMask = 0xC0;
        public const Int32 InverseLiquidTypeMask = 0x3F;
        public const Int32 LiquidLevelMask = 0x3F;
        public const Int32 InverseLiquidLevelMask = 0xC0;

        public const UInt32 SelectionIDBit = 0x80000000;
        public const UInt32 SelectionIDYMask = 0x7F;
        public const Int32 SelectionIDYShift = 24;
        public const UInt32 SelectionIDXMask = 0xFFF;
        public const Int32 SelectionIDXShift = 12;
        public const UInt32 SelectionIDZMask = 0xFFF;
        public const Int32 SelectionIDZShift = 0;

    }
}
