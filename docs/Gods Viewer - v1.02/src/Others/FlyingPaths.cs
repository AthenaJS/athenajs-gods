using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;

namespace GodsViewer {
  public class FlyingPaths {
    public List<FlyingPath> _flyingPath = new List<FlyingPath> ();

    public FlyingPaths (string filename) {
      byte [] buffer = ExtensionByte.CreateFromFile (filename);
      int offset = 0;
      int offset2 = 0x190;
      for (; ; ) {
        long temp = buffer.ReadLongSigned (offset + 0);
        int size = (int) buffer.ReadLong (offset + 4);
        offset += 8;
        if (temp < 0) {
          break;
        }
        FlyingPath flyingPath = new FlyingPath (buffer, offset2, size);
        _flyingPath.Add (flyingPath);
        offset2 += size;
      }
    }
  }
}
