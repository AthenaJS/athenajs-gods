using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;

namespace GodsViewer {
  public class Alfils03_WalkingWaves : IAlfil {
    public List<Alfils03_WalkingWave> _walkingWaves = new List<Alfils03_WalkingWave> ();
    public int _offset;
    public int _length;

    public Alfils03_WalkingWaves (byte [] buffer_alfils_0YX, ref int offset) {
      if (offset != 0x718)
        throw new Exception ();
      _offset = offset;
      for (int i = 0; i < 100; i++) {
        Alfils03_WalkingWave walkingWave = new Alfils03_WalkingWave (buffer_alfils_0YX, ref offset, i);
        _walkingWaves.Add (walkingWave);
      }
      _length = offset - _offset;
    }

    public string Name {
      get {
        return ("Alfils03_WalkingWaves");
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
