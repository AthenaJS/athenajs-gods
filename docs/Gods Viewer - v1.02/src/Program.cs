using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Globalization;

// 1.01:
// - raster effect added
// - explosion entity added

namespace GodsViewer {
  static class Program {
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main () {
      Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
      Application.EnableVisualStyles ();
      Application.SetCompatibleTextRenderingDefault (false);

      if (!Directory.Exists ("Atari")) {
        Directory.CreateDirectory ("Atari");
        GodsFS godsFS1 = new GodsFS (Resource.Gods_Disk_1_of_2_Track_00_filled_st, "Atari");
        godsFS1.ExtractAllFileC ();
        godsFS1.ExtractAllFile ();

        GodsFS godsFS2 = new GodsFS (Resource.Gods_Disk_2_of_2_st, "Atari");
        godsFS2.ExtractAllFileC ();
        godsFS2.ExtractAllFile ();
      }
      if (!Directory.Exists ("Amiga")) {
        Directory.CreateDirectory ("Amiga");
        GodsFS godsFS1 = new GodsFS (Resource.Gods_1991_Renegade_Disk_1_of_2_adf, "Amiga");
        godsFS1.ExtractAllFileC ();
        godsFS1.ExtractAllFile ();

        GodsFS godsFS2 = new GodsFS (Resource.Gods_1991_Renegade_Disk_2_of_2_adf, "Amiga");
        godsFS2.ExtractAllFileC ();
        godsFS2.ExtractAllFile ();
      }

      //for (int level = 1; level <= 4; level++) {
      //  for (int world = 1; world <= 2; world++) {
      //    new World (level, world, "ST/godsd2", true);
      //    //break;
      //    //return;
      //  }
      //}
      //World world = new World (1, 1, "PC", false);

      //Pi1 pi1 = new Pi1 ("godsd2/obj1.pi1", Sprite.MasksEnum.Transparent);
      //DatPi1 datPi1 = new DatPi1 ("godsd2/obj1.dat", pi1);
      //pi1._bitmap.Save ("level1a.png", ImageFormat.Png);
      //int offset = godFS.GetFileOffset ("cpayoff.pi1");
      //byte [] unpacked = UnpackerC.Unpack (buffer, offset);
      //unpacked.WriteToFile ("cpayoff.pi1");

      Application.Run (new Main ());
    }
  }
}
