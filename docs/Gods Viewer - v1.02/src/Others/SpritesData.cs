using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace GodsViewer {
  public class SpritesData {
    public int _nb;
    public List<SpriteData> _spritesData = new List<SpriteData> ();

    //public SpritesData (string filenameDat, Pi1 pi1) {
    //  byte [] bufferDat = ExtensionByte.CreateFromFile (filenameDat);
    //  Init (bufferDat, pi1);
    //}

    public SpritesData (string filename, Palette palette) {
      byte [] bufferDat = ExtensionByte.CreateFromFile (Path.ChangeExtension (filename, ".dat"));
      using (Pi1 pi1 = new Pi1 (Path.ChangeExtension (filename, ".pi1"), palette, Sprite.MasksEnum.Transparent)) {
        Init (bufferDat, pi1);
      }
    }

    private void Init (byte [] bufferDat, Pi1 pi1) {
      _nb = bufferDat.ReadWord (0);
      int offset = 2;
      for (int i = 0; i < _nb; i++) {
        SpriteData spriteData = new SpriteData (bufferDat, ref offset, pi1._bitmap);
        _spritesData.Add (spriteData);

        //string sprName = string.Format ("{0}_{1}.png", Path.GetFileNameWithoutExtension (filename), (i + 1).ToString ().PadLeft (2, '0'));
        //datSpr._bitmap.Save (sprName, ImageFormat.Png);
      }
    }
  }
}
