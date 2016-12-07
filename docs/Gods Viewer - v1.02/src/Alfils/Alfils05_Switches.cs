using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;

namespace GodsViewer {
  public class Alfils05_Switches : IAlfil {
    public List<Alfils05_Switch> _switches = new List<Alfils05_Switch> ();
    public int _offset;
    public int _length;

    public Alfils05_Switches (byte [] buffer_alfils_0YX, ref int offset) {
      if (offset != 0x15F0)
        throw new Exception ();
      _offset = offset;
      for (int i = 0; i < 64; i++) {
        Alfils05_Switch switch_ = new Alfils05_Switch (buffer_alfils_0YX, ref offset, i);
        if (((switch_._pixelX == 0xFFFF) && (switch_._pixelX == 0xFFFF) && (switch_._pixelX == 0xFFFF))  ||
            ((switch_._pixelX == 0x0000) && (switch_._pixelX == 0x0000) && (switch_._pixelX == 0x0000))) {
          _switches.Add (null);
        }
        else {
          _switches.Add (switch_);
        }
      }
      _length = offset - _offset;
    }

    public string Name {
      get {
        return ("Alfils05_Switches");
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
