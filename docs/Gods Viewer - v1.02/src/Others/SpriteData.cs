using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;

namespace GodsViewer {
  public class SpriteData : IDisposable {
    public int _width;
    public int _height;
    public int _unk04;
    public int _x;
    public int _y;
    public Bitmap _bitmap;

    public SpriteData (byte [] buffer, ref int offset, Bitmap bitmap) {
      _width = buffer.ReadWord (offset + 0) + 1;
      _height = buffer.ReadWord (offset + 2) + 1;
      _unk04 = buffer.ReadWord (offset + 4);
      _x = buffer.ReadWord (offset + 6);
      _y = buffer.ReadWord (offset + 8);
      offset += 10;

      _bitmap = bitmap.ExtractBitmap (_x, _y, _width, _height);
    }

    public void Dispose () {
      if (_bitmap != null) {
        _bitmap.Dispose ();
      }
    }
  }
}
