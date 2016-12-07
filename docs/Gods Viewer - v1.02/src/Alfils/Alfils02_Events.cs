using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;

namespace GodsViewer {
  public class Alfils02_Events : IAlfil {
    public List<Alfils02_Event> _events = new List<Alfils02_Event> ();
    public int _offset;
    public int _length;

    public Alfils02_Events (byte [] buffer_alfils_0YX, ref int offset) {
      if (offset != 0x320)
        throw new Exception ();
      _offset = offset;
      offset += 4;
      for (int i = 0; i < 253; i++) {
        Alfils02_Event event_ = new Alfils02_Event (buffer_alfils_0YX, ref offset, i);
        if (event_._functionIndex_min1 == 0) {
          _events.Add (null);
        }
        else {
          _events.Add (event_);
        }
      }
      _length = offset - _offset;
    }

    public string Name {
      get {
        return ("Alfils02_Events");
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

