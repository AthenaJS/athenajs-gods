using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;

namespace GodsViewer {
  public class Alfils06_Teleport {
    public int _index;

    public int _srcPixelX;
    public int _dstPixelX;
    public int _dstPixelY;

    public Alfils06_Teleport (byte [] buffer, ref int offset, int index) {
      _index = index;

      _srcPixelX = buffer.ReadWord (offset);
      offset += 2;
      _dstPixelX = buffer.ReadWord (offset);
      offset += 2;
      _dstPixelY = buffer.ReadWord (offset);
      offset += 2;

      if ((_dstPixelX % 32) != 0)
        _dstPixelX = (_dstPixelX / 32) * 32;
      if ((_dstPixelY % 16) != 0)
        _dstPixelX = (_dstPixelX / 16) * 16;
    }

    public Point PixelXY {
      get {
        //int pixelX = (_effectParam >> 8) * 32 + 16;
        //int pixelY = (_effectParam & 0x00FF) * 16;
        return (new Point (_dstPixelX + 16, _dstPixelY));
      }
    }

    public static Size Size {
      get {
        return (new Size (32, 48));
      }
    }

    public Rectangle Rectangle {
      get {
        return (new Rectangle (PixelXY, Size));
      }
    }

    public void DrawOnMap (Graphics g, Point cellXY, World world, Point fromXY) {
      g.DrawRectangle (Pens.Cyan, PixelXY.X, PixelXY.Y, Size.Width - 1, Size.Height - 1);
      Point centerXY = new Point (PixelXY.X + Size.Width / 2, PixelXY.Y + Size.Height / 2);
      if (!fromXY.IsEmpty)
        g.DrawArrowOutlinedSolid (Pens.Cyan, fromXY.X, fromXY.Y, centerXY.X, centerXY.Y);
    }

    public void DrawOnMap_Reverse (Graphics g, Point cellXY, World world, Point toXY, bool state) {
      g.DrawRectangle (Pens.Cyan, PixelXY.X, PixelXY.Y, Size.Width - 1, Size.Height - 1);
      Point centerXY = new Point (PixelXY.X + Size.Width / 2, PixelXY.Y + Size.Height / 2);
      if (!toXY.IsEmpty)
        g.DrawArrowOutlinedState (Pens.Cyan, centerXY.X, centerXY.Y, toXY.X, toXY.Y, state);

      // OK: teleport used in map object effect (used)
      foreach (MapItem mapItem in world.GetMapItemsWithEffect (ObjectEffectTypeEnum.TeleportStone_17)) {
        if (_srcPixelX == mapItem.ItemPixelXY.X) {
          mapItem.Item_DrawOnMap_Reverse (g, cellXY, world, centerXY, false);
        }
      }
    }

    public void DrawBox (Graphics g, World world) {
      BoxHelper.Center ();
      BoxHelper.AddString (g, string.Format ("TELEPORT #{0}", _index));
      BoxHelper.AddSeparation ();
      //BoxHelper.AddStringLine (g, string.Format ("(x,y): ({0},{1})", PixelXY.X, PixelXY.Y));
      BoxHelper.AddStringLine (g, string.Format ("Teleport to (x,y): ({0},{1})", PixelXY.X, PixelXY.Y));
      BoxHelper.DrawBox (g);
    }
  }
}
