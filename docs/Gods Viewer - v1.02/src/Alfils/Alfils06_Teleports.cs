using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;

namespace GodsViewer {
  public class Alfils06_Teleports : IAlfil {
    public List<Alfils06_Teleport> _teleports = new List<Alfils06_Teleport> ();
    public int _offset;
    public int _length;

    public Alfils06_Teleports (byte [] buffer_alfils_0YX, ref int offset) {
      if (offset != 0x1770)
        throw new Exception ();
      _offset = offset;
      for (int i = 0; i < 30; i++) {
        Alfils06_Teleport teleport = new Alfils06_Teleport (buffer_alfils_0YX, ref offset, i);
        if (teleport._srcPixelX == 0) {
          _teleports.Add (null);
        }
        else {
          _teleports.Add (teleport);
        }
      }
      _length = offset - _offset;
    }

    public string Name {
      get {
        return ("Alfils06_Teleports");
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
