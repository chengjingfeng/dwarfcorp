﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DwarfCorp.GameStates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;

namespace DwarfCorp
{
    public class Anvil : CraftedBody
    {
        [EntityFactory("Anvil")]
        private static GameComponent __factory00(ComponentManager Manager, Vector3 Position, Blackboard Data)
        {
            return new Anvil(Manager, Position, Data.GetData<List<ResourceAmount>>("Resources", null));
        }

        private static RawPrimitive SharedMesh = null;

        public Anvil()
        {

        }

        public Anvil(ComponentManager manager, Vector3 position, List<ResourceAmount> resources) :
            base(manager, "Anvil", Matrix.CreateTranslation(position),
                new Vector3(0.9f, 0.9f, 0.9f),
                new Vector3(0.0f, 0.5f, 0.0f),
                new DwarfCorp.CraftDetails(manager, "Anvil", resources))
        {
            Tags.Add("Anvil");
            CollisionType = CollisionType.Static;
            SetFlag(Flag.RotateBoundingBox, true);

            CreateCosmeticChildren(manager);
        }

        public override void RenderSelectionBuffer(DwarfTime gameTime, ChunkManager chunks, Camera camera, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice, Shader effect)
        {
            effect.SelectionBufferColor = this.GetGlobalIDColor().ToVector4();
            if (GetComponent<MeshComponent>().HasValue(out var prim))
                prim.Render(gameTime, chunks, camera, spriteBatch, graphicsDevice, effect, false);
        }

        override public void Render(DwarfTime gameTime, ChunkManager chunks, Camera camera, SpriteBatch spriteBatch,
            GraphicsDevice graphicsDevice, Shader effect, bool renderingForWater)
        {
            base.Render(gameTime, chunks, camera, spriteBatch, graphicsDevice, effect, renderingForWater);
        }

        public override void CreateCosmeticChildren(ComponentManager Manager)
        {
            base.CreateCosmeticChildren(Manager);

            if (SharedMesh == null)
                SharedMesh = AssetManager.GetContentMesh("Entities/Furniture/sm_anvil");

            AddChild(new MeshComponent(Manager,
                Matrix.CreateRotationY(0.25f),
                new Vector3(1.0f, 1.0f, 1.0f),
                new Vector3(0.0f, 0.0f, 0.0f),
                SharedMesh,
                "Entities/Furniture/tx_anvil"))
                .SetFlag(Flag.ShouldSerialize, false)
                .SetFlag(Flag.RotateBoundingBox, true);
        }
    }    
}
