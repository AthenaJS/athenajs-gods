using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace GodsViewer {
  public class Pi1 : IDisposable {
    public int _resolution;
    public Palette _palette;
    public Bitmap _bitmap;
    public static Palette.PaletteLookup PaletteLookup = Palette.PaletteLookup.ST;

    public Pi1 (string filename, Sprite.MasksEnum maskEnum) {
      Load (filename, null, maskEnum);
    }

    public Pi1 (string filename, Palette palette, Sprite.MasksEnum maskEnum) {
      Load (filename, palette, maskEnum);
    }

    public void Load (string filename, Palette palette, Sprite.MasksEnum maskEnum) {
      byte [] buffer = ExtensionByte.CreateFromFile (filename);

      _resolution = buffer.ReadWord (0);
      if (_resolution != 0) {
        throw new Exception ();
      }
      if (palette != null) {
        _palette = palette;
      }
      else {
        _palette = new Palette (buffer, 2, 16, PaletteLookup);
      }
      _bitmap = Sprite.CreateBitmap (buffer, 34, 320, 200, 4, Sprite.ModesEnum.Interleaved, 2, _palette, 0, maskEnum);

      //string pngName = string.Format ("{0}.png", Path.GetFileNameWithoutExtension (filename));
      //_bitmap.Save (pngName, ImageFormat.Png);
    }

    public void Dispose () {
      if (_bitmap != null) {
        _bitmap.Dispose ();
      }
    }
  }
}
