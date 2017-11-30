using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace DwarfCorp.Gui.Widgets
{
    public class CheckBox : Widget
    {
        private bool _checkState = false;
        public bool CheckState 
        {
            get { return _checkState; }
            set { _checkState = value; Root.SafeCall(OnCheckStateChange, this); Invalidate(); }
        }

        public void SilentSetCheckState(bool NewState)
        {
            _checkState = NewState;
        }

        public Action<Widget> OnCheckStateChange = null;

        public override void Construct()
        {
            OnClick += (sender, args) => { CheckState = !CheckState; };
            TextVerticalAlign = VerticalAlign.Center;
            ChangeColorOnHover = true;
            HoverTextColor = new Vector4(0.5f, 0, 0, 1.0f);

            if (String.IsNullOrEmpty(Graphics))
                Graphics = "checkbox";
        }

        public override Rectangle GetDrawableInterior()
        {
            var baseDrawArea = base.GetDrawableInterior();
            return baseDrawArea.Interior(baseDrawArea.Height + 2, 0, 0, 0);
        }

        protected override Mesh Redraw()
        {
            var baseMesh = base.Redraw();
            var baseDrawArea = base.GetDrawableInterior();

            var checkMesh = Mesh.Quad()
                .Scale(baseDrawArea.Height, baseDrawArea.Height)
                .Translate(baseDrawArea.X, baseDrawArea.Y)
                .Texture(Root.GetTileSheet(Graphics).TileMatrix(CheckState ? 1 : 0));

            return Mesh.Merge(baseMesh, checkMesh);
        }

        public override Point GetBestSize()
        {
            var size = new Point(0, 0);
            if (!String.IsNullOrEmpty(Text))
            {
                var font = Root.GetTileSheet(Font);
                size = font.MeasureString(Text).Scale(TextSize);
            }

            var gfx = Root.GetTileSheet(Graphics);
            return new Point(gfx.TileWidth + size.X + 2, Math.Max(gfx.TileHeight, size.Y));
        }
    }
}
