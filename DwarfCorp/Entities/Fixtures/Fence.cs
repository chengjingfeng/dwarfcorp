using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DwarfCorp
{
    public class Fence : Fixture
    {
        public float FenceRotation { get; set; }

        public Fence()
        {
            
        }

        public Fence(ComponentManager componentManager, Vector3 position, float orientation, string asset) :
            base(componentManager, position, new SpriteSheet(asset, 32, 32), new Point(0, 0))
        {
            this.Name = "Fence";
            this.LocalBoundingBoxOffset = new Vector3(0, -0.25f, 0);
            this.BoundingBoxSize = new Vector3(1.0f, 0.5f, 0.1f);
            this.SetFlag(Flag.RotateBoundingBox, true);

            FenceRotation = orientation;

            if (GetComponent<SimpleSprite>().HasValue(out var sprite))
                sprite.OrientationType = SimpleSprite.OrientMode.Fixed;

            LocalTransform = Matrix.CreateRotationY(FenceRotation) * Matrix.CreateTranslation(position);

            PropogateTransforms();
        }

        public override void CreateCosmeticChildren(ComponentManager manager)
        {
            base.CreateCosmeticChildren(manager);

            if (GetComponent<SimpleSprite>().HasValue(out var sprite))
                sprite.OrientationType = SimpleSprite.OrientMode.Fixed;
        }

        private struct FenceSegmentInfo
        {
            public GlobalVoxelOffset VoxelOffset;
            public Vector3 VisibleOffset;
            public float Angle;
        }

        private static FenceSegmentInfo[] FenceSegments = new FenceSegmentInfo[]
        {
            new FenceSegmentInfo
            {
                VoxelOffset = new GlobalVoxelOffset(0,0,1),
                VisibleOffset = new Microsoft.Xna.Framework.Vector3(0,0,0.45f),
                Angle = (float)Math.Atan2(0,1)
            },

            new FenceSegmentInfo
            {
                VoxelOffset = new GlobalVoxelOffset(0,0,-1),
                VisibleOffset = new Microsoft.Xna.Framework.Vector3(0,0,-0.45f),
                Angle = (float)Math.Atan2(0,-1)
            },

            new FenceSegmentInfo
            {
                VoxelOffset = new GlobalVoxelOffset(1,0,0),
                VisibleOffset = new Microsoft.Xna.Framework.Vector3(0.45f,0,0),
                Angle = (float)Math.Atan2(1,0)
            },

            new FenceSegmentInfo
            {
                VoxelOffset = new GlobalVoxelOffset(-1,0,0),
                VisibleOffset = new Microsoft.Xna.Framework.Vector3(-0.45f,0,0),
                Angle = (float)Math.Atan2(-1,0)
            },
        };

        public static IEnumerable<GameComponent> CreateFences(
            ComponentManager components,
            string asset, 
            IEnumerable<VoxelHandle> Voxels, 
            bool createWorkPiles)
        {
            Vector3 off = (Vector3.One * 0.5f) + Vector3.Up;
            foreach (var voxel in Voxels)
            {
                foreach (var segment in FenceSegments)
                {
                    var neighbor = VoxelHelpers.GetNeighbor(voxel, segment.VoxelOffset);
                    if (neighbor.IsValid && !Voxels.Any(v => v == neighbor))
                        yield return new Fence(components,
                            voxel.WorldPosition + off + segment.VisibleOffset,
                            segment.Angle, asset);
                }

                if (createWorkPiles && MathFunctions.RandEvent(0.1f))
                {
                    yield return new WorkPile(components, voxel.WorldPosition + off);
                }
            }
        }
    }
}
