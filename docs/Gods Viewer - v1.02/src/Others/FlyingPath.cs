using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;

namespace GodsViewer {
  public class FlyingPath {
    public int _screenPixelX;
    public int _screenPixelY;
    public int _pixelX;
    public int _pixelY;
    public int [] _deltaX;
    public int [] _deltaY;

    public TypeEnum _type;
    public enum TypeEnum {
      Relative,
      Absolute
    }

    public FlyingPath (byte [] buffer, int offset, int size) {
      int nbDeltaXY = size;
      int magicNumber = buffer.ReadWord (offset);
      if (magicNumber == 0x2345) { // relative to map
        _type = TypeEnum.Absolute;
        nbDeltaXY -= 6;
        offset += 2;

        _pixelX = buffer.ReadWord (offset);
        offset += 2;
        _pixelY = buffer.ReadWord (offset);
        offset += 2;
      }
      else { // relative to screen
        _type = TypeEnum.Relative;
        nbDeltaXY -= 4;

        _screenPixelX = buffer.ReadWordSigned (offset);
        offset += 2;
        _screenPixelY = buffer.ReadWordSigned (offset);
        offset += 2;
      }

      _deltaX = new int [nbDeltaXY];
      _deltaY = new int [nbDeltaXY];
      for (int i = 0; i < nbDeltaXY; i++) {
        int deltaXY = buffer.ReadByte (offset++);
        _deltaX [i] = ((sbyte) (deltaXY << 0)) >> 4;
        _deltaY [i] = ((sbyte) (deltaXY << 4)) >> 4;
      }
    }

    public void Draw (Graphics g, int x, int y) {
      //g.DrawRectangle (Pens.White, xo + x, yo + y, 320 - 1, 192 - 1);
      Pen penDot;
      Pen penLine;
      int x1, y1;
      if (_type == TypeEnum.Absolute) {
        //throw new Exception ("absolute flying wave!");
        x1 = _pixelX;
        y1 = _pixelY;
        penDot = Pens.Yellow;
        penLine = Pens.Orange;
      }
      else {
        x1 = World._xo + x + _screenPixelX;
        y1 = World._yo + y + _screenPixelY;
        penDot = Pens.Cyan;
        penLine = Pens.DarkCyan;
      }
      //g.FillRectangle (Brushes.White, x1 - 1, y1 - 1, 3, 3);
      for (int i = 0; i < _deltaX.Length; i++) {
        int x2 = x1 + _deltaX [i];
        int y2 = y1 + _deltaY [i];
        //g.FillRectangle (Brushes.Cyan, x2 - 1, y2 - 1, 3, 3);
        g.DrawLine (penLine, x1, y1, x2, y2);
        //g.FillRectangle (Brushes.Cyan, x1 - 1, y1 - 1, 3, 3);
        g.DrawRectangle (penDot, x1 - 1, y1 - 1, 3 - 1, 3 - 1);
        x1 = x2;
        y1 = y2;
      }
      //g.FillRectangle (Brushes.Cyan, x1 - 1, y1 - 1, 3, 3);
      g.DrawRectangle (penDot, x1 - 1, y1 - 1, 3 - 1, 3 - 1);
    }

    public Point GetFirstPixelXY (Point centerXY) {
      int x;
      if (_type == TypeEnum.Absolute) {
        x = _pixelX;
      }
      else {
        x = World._xo + centerXY.X + _screenPixelX;
      }
      int y;
      if (_type == TypeEnum.Absolute) {
        y = _pixelY;
      }
      else {
        y = World._yo + centerXY.Y + _screenPixelY;
      }
      return (new Point (x, y));
    }
  }
}
