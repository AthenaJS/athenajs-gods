using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;

namespace GodsViewer {
  public class Alfils07_Trapdoor {
    public int _index;

    public Point _pixelXY;
    public bool _isOpened;

    public Alfils07_Trapdoor (byte [] buffer, ref int offset, int index) {
      _index = index;

      int pixelX = buffer.ReadWord (offset);
      offset += 2;
      int pixelY = buffer.ReadWord (offset);
      offset += 2;
      _pixelXY = new Point (pixelX, pixelY);
      int isOpened = buffer.ReadWord (offset);
      offset += 2;
      if (_pixelXY.X == 0xFFFF)
        return;

      if (((pixelX % 32) != 0) || ((pixelY % 16) != 0)) {
        throw new Exception ();
      }
      if ((isOpened != 0) && (isOpened != 1)) {
        throw new Exception ();
      }
      if (((pixelX & 0x8000) != 0) || ((pixelY & 0x8000) != 0)) {
        throw new Exception ();
      }
      _isOpened = (isOpened == 1);
    }

    public void DrawOnMap (Graphics g, World world, Point fromXY) {
      g.DrawRectangle (Pens.Yellow, _pixelXY.X, _pixelXY.Y, 32 - 1, 16 - 1);
      Point centerXY = new Point (_pixelXY.X + 16, _pixelXY.Y + 8);
      if (!fromXY.IsEmpty)
        g.DrawArrowOutlinedSolid (Pens.Yellow, fromXY.X, fromXY.Y, centerXY.X, centerXY.Y);
    }

    public void DrawOnMap_Reverse (Graphics g, Point cellXY, World world, Point toXY, bool state) {
      g.DrawRectangle (Pens.Yellow, _pixelXY.X, _pixelXY.Y, 32 - 1, 16 - 1);
      Point centerXY = new Point (_pixelXY.X + 16, _pixelXY.Y + 8);
      if (!toXY.IsEmpty)
        g.DrawArrowOutlinedState (Pens.Yellow, centerXY.X, centerXY.Y, toXY.X, toXY.Y, state);

      // OK: trapdoor used in puzzle effect (open)
      foreach (Alfils02_Event event_ in world.GetEventsWithPuzzleEffect (PuzzleEffectTypeEnum.OpenTrapdoor_07, -1)) {
        MapPuzzle puzzle = event_.GetPuzzle (world);
        if ((puzzle.ItemPixelXY.X != _pixelXY.X) || (puzzle.ItemPixelXY.Y != _pixelXY.Y))
          continue;
        event_.DrawOnMap_Reverse (g, cellXY, world, centerXY, false);
      }

      // OK: trapdoor used in puzzle effect (close)
      foreach (Alfils02_Event event_ in world.GetEventsWithPuzzleEffect (PuzzleEffectTypeEnum.CloseTrapdoor_08, -1)) {
        MapPuzzle puzzle = event_.GetPuzzle (world);
        if ((puzzle.ItemPixelXY.X != _pixelXY.X) || (puzzle.ItemPixelXY.Y != _pixelXY.Y))
          continue;
        event_.DrawOnMap_Reverse (g, cellXY, world, centerXY, false);
      }
    }

    public void DrawBox (Graphics g, World world) {
      BoxHelper.Center ();
      BoxHelper.AddStringLine (g, string.Format ("TRAPDOOR #{0}", _index));
      BoxHelper.AddSeparation ();
      BoxHelper.AddStringLine (g, string.Format ("(x,y): ({0},{1})", _pixelXY.X, _pixelXY.Y));
      BoxHelper.AddStringLine (g, string.Format ("State: {0}", _isOpened ? "open" : "close"));
      BoxHelper.DrawBox (g);
    }
  }
}
