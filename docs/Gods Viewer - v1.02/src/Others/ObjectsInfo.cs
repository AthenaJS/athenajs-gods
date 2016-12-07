using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;

namespace GodsViewer {
  public class ObjectsInfo {
    public List<ObjectInfo> _objectsInfo = new List<ObjectInfo> ();

    public ObjectsInfo (string filename) {
      byte [] buffer = ExtensionByte.CreateFromFile (filename);
      int offset = 0;
      int effectIndex = 0;
      for (int i = 0; i < 132; i++) {
        ObjectInfo objectInfo = new ObjectInfo (buffer, ref offset, ref effectIndex, i);
        _objectsInfo.Add (objectInfo);
        //Console.WriteLine (objectInfo.String);
      }
    }
  }
}
