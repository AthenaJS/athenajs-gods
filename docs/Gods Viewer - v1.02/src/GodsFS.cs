using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GodsViewer {
  public class GodsFS {
    private static int _directoryOffset = 0x2C00;

    private byte [] _buffer;
    private string _folder = null;
    private string _diskLabel = null;
    private List<int> _sectorsStart = new List<int> ();
    private List<int> _sectorsNb = new List<int> ();
    private List<string> _filenames = new List<string> ();

    public GodsFS (byte [] buffer, string folder) {
      _buffer = buffer;
      _folder = folder;

      ReadDiskLabel ();
      if (!Directory.Exists (string.Format ("{0}/{1}", _folder, _diskLabel))) {
        Directory.CreateDirectory (string.Format ("{0}/{1}", _folder, _diskLabel));
      }
      ReadDirectory ();
    }

    private void ReadDiskLabel () {
      _diskLabel = _buffer.ReadString (_directoryOffset);
      if (!_diskLabel.StartsWith ("godsd")) {
        throw new Exception ();
      }
    }

    private void ReadDirectory () {
      int offset = _directoryOffset + 18;
      for (; ; ) {
        int sectorStart = _buffer.ReadWord (offset);
        offset += 2;
        int sectorEnd = _buffer.ReadWord (offset);
        offset += 2;
        if ((sectorStart == 0) && (sectorEnd == 0)) {
          break;
        }
        _sectorsStart.Add (sectorStart);
        _sectorsNb.Add (sectorEnd);
      }

      offset = _directoryOffset + _buffer.ReadWord (_directoryOffset + 16);
      for (; ; ) {
        int length = _buffer.ReadByte (offset++);
        if ((length == 0x00) || (length == 0xFF)) {
          break;
        }
        string filename = _buffer.ReadString (offset);
        offset += length;
        if (filename.Length != (length - 1)) {
          throw new Exception ();
        }
        _filenames.Add (filename);
      }
    }

    public void GetFileOffset (string filename, out int offset, out int length) {
      offset = -1;
      length = -1;
      for (int i = 0; i < _filenames.Count; i++) {
        if (_filenames [i] == filename) {
          offset = _sectorsStart [i] * 512;
          length = _sectorsNb [i] * 512;
          return;
        }
      }
    }

    public byte [] ReadFile (string filename) {
      int offset, length;
      GetFileOffset (filename, out offset, out length);
      return (_buffer.Extract (offset, length));
    }

    public byte [] UnpackFileC (string filename) {
      if (filename [0] != 'c') {
        throw new Exception ();
      }
      int offset, length;
      GetFileOffset (filename, out offset, out length);
      return (UnpackerC.Unpack (_buffer, offset));
    }

    public void ExtractFileC (string filename) {
      byte [] buffer = UnpackFileC (filename);
      buffer.WriteToFile (string.Format ("{0}/{1}/{2}", _folder, _diskLabel, filename.Substring (1)));
    }

    public void ExtractFile (string filename) {
      byte [] buffer = ReadFile (filename);
      int size = -1;
      if (filename == "always1.dat") {
        size = 0x188;
      }
      else if (filename == "always2.dat") {
        size = 0x2D2;
      }
      else if (filename == "always3.dat") {
        size = 0xAC;
      }
      else if (filename == "godfont.dat") {
        size = 0x1A6;
      }
      else if (filename == "guard11.dat") {
        size = 0xCA;
      }
      else if (filename == "guard12.dat") {
        size = 0xD4;
      }
      else if (filename == "guard21.dat") {
        size = 0x232;
      }
      else if (filename == "guard22.dat") {
        size = 0x2B4;
      }
      else if (filename == "guard31.dat") {
        size = 0xFC;
      }
      else if (filename == "guard32.dat") {
        size = 0xCA;
      }
      else if (filename == "guard41.dat") {
        size = 0x138;
      }
      else if (filename == "guard42.dat") {
        size = 0xFC;
      }
      else if (filename == "level1a.dat") {
        size = 0x25A;
      }
      else if (filename == "level1b.dat") {
        size = 0x124;
      }
      else if (filename == "level2a.dat") {
        size = 0x21E;
      }
      else if (filename == "level2b.dat") {
        size = 0x174;
      }
      else if (filename == "level3a.dat") {
        size = 0x16A;
      }
      else if (filename == "level3b.dat") {
        size = 0x21E;
      }
      else if (filename == "level4a.dat") {
        size = 0x21E;
      }
      else if (filename == "level4b.dat") {
        size = 0x20A;
      }
      else if (filename == "obj1.dat") {
        size = 0x64C;
      }
      else if (filename == "obj2.dat") {
        size = 0x494;
      }
      else if (filename == "shopfnt.dat") {
        size = 0x516;
      }
      else if (filename == "shopspr.dat") {
        size = 0x1B0;
      }
      else if (filename == "smlfnt.dat") {
        size = 0x516;
      }
      else if (filename == "bezier.dat") {
        size = 0x640;
      }
      else if (filename == "hiscores.bin") {
        size = 0xD8;
      }
      if (size == -1) {
        //buffer.WriteToFile (filename + '_');
        buffer.WriteToFile (string.Format ("{0}/{1}/{2}", _folder, _diskLabel, filename));
      }
      else {
        buffer.Extract (0, size).WriteToFile (string.Format ("{0}/{1}/{2}", _folder, _diskLabel, filename));
      }
    }

    public void ExtractAllFileC () {
      foreach (string filename in _filenames) {
        if (filename [0] != 'c') {
          continue;
        }
        ExtractFileC (filename);
      }
    }

    public void ExtractAllFile () {
      foreach (string filename in _filenames) {
        if (filename [0] == 'c') {
          continue;
        }
        ExtractFile (filename);
      }
    }
  }
}
