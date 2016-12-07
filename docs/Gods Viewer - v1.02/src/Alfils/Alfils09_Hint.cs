using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;

namespace GodsViewer {
  public class Alfils09_Hint {
    public int _index;

    public int _pixelX;
    public string _string;

    public Alfils09_Hint (byte [] buffer, int offset, int index) {
      _index = index;

      _pixelX = buffer.ReadWord (offset + 2 * index + 0);
      if (_pixelX != 0xFFFF) {
        _string = buffer.ReadString (offset + 80 + 40 * index);
      }
    }

    public void DrawBox (Graphics g, World world) {
      BoxHelper.Center ();
      BoxHelper.AddString (g, string.Format ("HINT #{0}", _index));
      BoxHelper.AddSeparation ();
      BoxHelper.AddStringLine (g, string.Format ("\"{0}\"", _string));
      BoxHelper.DrawBox (g);
    }
  }
}
