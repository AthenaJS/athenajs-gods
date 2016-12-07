using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;

namespace GodsViewer {
  public class Alfils04a_IntelFlyingWaves : IAlfil {
    public List<Alfils04a_IntelFlyingWave> _intelFlyingWaves = new List<Alfils04a_IntelFlyingWave> ();
    public int _offset;
    public int _length;

    public Alfils04a_IntelFlyingWaves (byte [] buffer_alfils_0YX, ref int offset) {
      if (offset != 0xD58)
        throw new Exception ();
      _offset = offset;
      for (int i = 0; i < 50; i++) {
        Alfils04a_IntelFlyingWave intelFlyingWave = new Alfils04a_IntelFlyingWave (buffer_alfils_0YX, ref offset, i);
        if (intelFlyingWave._nbEnemies == 0)
          _intelFlyingWaves.Add (null);
        else
          _intelFlyingWaves.Add (intelFlyingWave);
      }
      _length = offset - _offset;
    }

    public string Name {
      get {
        return ("Alfils04a_IntelFlyingWaves");
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
