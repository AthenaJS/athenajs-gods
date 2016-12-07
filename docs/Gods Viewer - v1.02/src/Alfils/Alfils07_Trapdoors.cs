using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;

namespace GodsViewer {
  public class Alfils07_Trapdoors : IAlfil {
    public List<Alfils07_Trapdoor> _trapdoors = new List<Alfils07_Trapdoor> ();
    public int _offset;
    public int _length;

    public Alfils07_Trapdoors (byte [] buffer_alfils_0YX, ref int offset) {
      if (offset != 0x1824)
        throw new Exception ();
      _offset = offset;
      for (int i = 0; i < 20; i++) {
        Alfils07_Trapdoor trapdoor = new Alfils07_Trapdoor (buffer_alfils_0YX, ref offset, i);
        if (trapdoor._pixelXY.X == 0xFFFF) {
          _trapdoors.Add (null);
        }
        else {
          _trapdoors.Add (trapdoor);
        }
      }
      _length = offset - _offset;
    }

    public string Name {
      get {
        return ("Alfils07_Trapdoors");
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
