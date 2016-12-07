using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;

namespace GodsViewer {
  public class Alfils04b_IntelWalkingWaves : IAlfil {
    public List<Alfils04b_IntelWalkingWave> _intelWalkingWaves = new List<Alfils04b_IntelWalkingWave> ();
    public int _offset;
    public int _length;

    public Alfils04b_IntelWalkingWaves (byte [] buffer_alfils_0YX, ref int offset) {
      if (offset != (0xD58 + 1100))
        throw new Exception ();
      _offset = offset;
      for (int i = 0; i < 50; i++) {
        Alfils04b_IntelWalkingWave intelWalkingWave = new Alfils04b_IntelWalkingWave (buffer_alfils_0YX, ref offset, i);
        if (intelWalkingWave._nbEnemies == 0)
          _intelWalkingWaves.Add (null);
        else
          _intelWalkingWaves.Add (intelWalkingWave);
      }
      _length = offset - _offset;
    }

    public string Name {
      get {
        return ("Alfils04b_IntelWalkingWaves");
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
