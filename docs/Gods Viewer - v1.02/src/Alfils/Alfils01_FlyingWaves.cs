using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;

namespace GodsViewer {
  public class Alfils01_FlyingWaves : IAlfil {
    public List<Alfils01_FlyingWave> _flyingWaves = new List<Alfils01_FlyingWave> ();
    public int _offset;
    public int _length;

    public Alfils01_FlyingWaves (byte [] buffer_alfils_0YX, ref int offset) {
      if (offset != 0x00)
        throw new Exception ();
      _offset = offset;
      for (int i = 0; i < 100; i++) {
        Alfils01_FlyingWave flyingWave = new Alfils01_FlyingWave (buffer_alfils_0YX, ref offset, i);
        _flyingWaves.Add (flyingWave);
      }
      _length = offset - _offset;
    }

    public string Name {
      get {
        return ("Alfils01_FlyingWaves");
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
