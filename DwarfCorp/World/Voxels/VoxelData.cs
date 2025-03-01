using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DwarfCorp
{
    /// <summary>
    /// Stores actual data for voxels in SOA format.
    /// </summary>
    public class VoxelData
    {
        // C# Needs a "Friend" mechanism similar to C++. Only the classes ChunkFile and VoxelHandle
        //  should ever access this data.
        public byte[] Types;                // Storage per-voxel
        public byte[] Grass;
        public byte[] _Water;
        public byte[] RampsSunlightExploredPlayerBuilt;

        public int[] LiquidPresent;         // Storage per-slice
        public int[] VoxelsPresentInSlice;
        public RawPrimitive[] SliceCache;
        
        public static VoxelData Allocate()
        {
            return new VoxelData()
            {
                Types = new byte[VoxelConstants.ChunkVoxelCount],
                Grass = new byte[VoxelConstants.ChunkVoxelCount],
                //Decals = new byte[VoxelConstants.ChunkVoxelCount],
                _Water = new byte[VoxelConstants.ChunkVoxelCount],
                RampsSunlightExploredPlayerBuilt = new byte[VoxelConstants.ChunkVoxelCount],

                LiquidPresent = new int[VoxelConstants.ChunkSizeY],
                VoxelsPresentInSlice = new int[VoxelConstants.ChunkSizeY],
                SliceCache = new RawPrimitive[VoxelConstants.ChunkSizeY]
            };
        }
    }
}
