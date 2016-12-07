using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;

namespace GodsViewer {

  public interface IAlfil {
    string Name {
      get;
    }
    int Offset {
      get;
    }
    int Length {
      get;
    }
  }
}
