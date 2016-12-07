using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;

namespace GodsViewer {
  public class ObjectiveLocation {
    public Point _pixelXY;

    public ObjectiveLocation (byte [] buffer, ref int offset) {
      int cellX = buffer.ReadWord (offset);
      offset += 2;
      int cellY = buffer.ReadWord (offset);
      offset += 2;
      if (cellX == 0xFFFF)
        return;
      _pixelXY = new Point (cellX * 32, (cellY + 1) * 16);
    }

    public Point PixelXY {
      get {
        return (_pixelXY);
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
      g.DrawRectangle (Pens.White, PixelXY.X, PixelXY.Y, Size.Width - 1, Size.Height - 1);
      Point centerXY = new Point (PixelXY.X + Size.Width / 2, PixelXY.Y + Size.Height / 2);
      if (!fromXY.IsEmpty)
        g.DrawArrowOutlinedSolid (Pens.Cyan, fromXY.X, fromXY.Y, centerXY.X, centerXY.Y);
    }

    public void DrawOnMap_Reverse (Graphics g, Point cellXY, World world, Point toXY, bool state) {
      g.DrawRectangle (Pens.White, PixelXY.X, PixelXY.Y, Size.Width - 1, Size.Height - 1);
      Point centerXY = new Point (PixelXY.X + Size.Width / 2, PixelXY.Y + Size.Height / 2);
      if (!toXY.IsEmpty)
        g.DrawArrowOutlinedState (Pens.Cyan, centerXY.X, centerXY.Y, toXY.X, toXY.Y, state);
    }

    public void DrawBox (Graphics g, World world) {
      BoxHelper.Center ();
      BoxHelper.AddString (g, string.Format ("OBJECTIVE LOCATION"));
      //BoxHelper.AddSeparation ();
      //BoxHelper.AddStringLine (g, string.Format ("Teleport to (x,y): ({0},{1})", PixelXY.X, PixelXY.Y));
      BoxHelper.DrawBox (g);
    }
  }
}
