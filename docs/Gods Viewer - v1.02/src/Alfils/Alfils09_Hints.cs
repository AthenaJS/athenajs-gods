using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;

namespace GodsViewer {
  public class Alfils09_Hints : IAlfil {
    public List<Alfils09_Hint> _hints = new List<Alfils09_Hint> ();
    public int _offset;
    public int _length;

    public Alfils09_Hints (byte [] buffer_alfils_0YX, ref int offset) {
      if (offset != 0x1B58)
        throw new Exception ();
      _offset = offset;
      for (int i = 0; i < 40; i++) {
        Alfils09_Hint hint = new Alfils09_Hint (buffer_alfils_0YX, offset, i);
        if (hint._pixelX == 0xFFFF) {
          _hints.Add (null);
        }
        else {
          _hints.Add (hint);
        }
      }
      offset = _offset + 80 + 40 * 40;
      _length = offset - _offset;
    }

    public string Name {
      get {
        return ("Alfils09_Hints");
      }
    }

    public int Offset {
      get {
        return (_offset);
      }
    }

    public int Length {
      get {
        return (_length);
      }
    }
  }
}
