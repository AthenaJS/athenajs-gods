using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GodsViewer {
  public class DoubleBufferedPanel : Panel {
    public DoubleBufferedPanel () {
      // Set the value of the double-buffering style bits to true.
      this.SetStyle (ControlStyles.DoubleBuffer |
                     ControlStyles.UserPaint |
                     ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw,
      true);
      //SetStyle (ControlStyles.OptimizedDoubleBuffer, true);

      UpdateStyles ();
    }
  }
}