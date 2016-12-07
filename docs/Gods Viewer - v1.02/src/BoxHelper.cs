using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace GodsViewer {
  public class BoxHelper {
    public static List<Opcode> _opcodes = new List<Opcode> ();

    public static Point _pixelXY = new Point (0, 0);
    public static Size _lineSize;
    public static List<Size> _linesSize = new List<Size> ();
    public static int _lineIndex;
    public static Size _boxSize;
    public static Point _deltaXY;
    public static bool _centered;
    public static Brush _brushFg;
    public static bool _isActive = false;

    public static Brush _popupBrush = new SolidBrush (Color.FromArgb (128, 0, 0, 0));
    public static Pen _contourActive = new Pen (Color.FromArgb (0, 255, 0));
    public static Pen _contourInactive = new Pen (Color.FromArgb (96, 96, 96));

    public static int _spaceBetweenBoxY = 1;
    public static int _margin = 2;

    public static void Reset (int deltaX, int deltaY) {
      ResetBox ();
      _deltaXY = new Point (deltaX, deltaY);
    }

    public static void ResetBox () {
      _lineSize = Size.Empty;
      _linesSize.Clear ();
      _lineIndex = 0;
      _boxSize = Size.Empty;
      _opcodes.Clear ();
      _centered = false;
      _brushFg = null;
      Brush (Brushes.Yellow);
    }

    private static void Rewind () {
      _lineSize = Size.Empty;
      _lineIndex = 0;
      _boxSize.Height = 0;
      _centered = false;
      _brushFg = null;
    }

    public static void DrawBox (Graphics g) {
      if (!_lineSize.IsEmpty) {
        NextLine ();
      }

      _boxSize.Width--;
      _boxSize.Height--;
      DrawBox_ (g);

      Rewind ();

      foreach (Opcode opcode in _opcodes) {
        ExecuteOpcode (opcode, g);
      }

      _boxSize.Width--;
      _boxSize.Height--;
      _deltaXY.Y += OuterBoxSize.Height + _spaceBetweenBoxY;
      ResetBox ();
    }

    public static Pen ContourPen {
      get {
        if (_isActive)
          return (_contourActive);
        else
          return (_contourInactive);
      }
    }

    public static void ExecuteOpcode (Opcode opcode, Graphics g) {
      if (opcode._opcodeType == OpcodeTypeEnum.Center) {
        if (!_lineSize.IsEmpty)
          throw new Exception ();
        _lineSize.Width += (_boxSize.Width - _linesSize [_lineIndex].Width) / 2;
      }
      else if (opcode._opcodeType == OpcodeTypeEnum.AddString) {
        string text = (string) (opcode._param1);
        Brush brush = (Brush) (opcode._param2);
        Size size = g.MeasureString2 (text, World._fontBox).Size.ToSizeCeiling ();
        //g.DrawRectangle (Pens.Magenta, LineXY.X, LineXY.Y, size.Width - 1, size.Height - 1);
        if (_brushFg != null)
          brush = _brushFg;
        g.DrawStringOutlined (text, World._fontBox, brush, Brushes.Black, LineXY.X, LineXY.Y);
        AddToLine (size);
      }
      else if (opcode._opcodeType == OpcodeTypeEnum.AddBitmap) {
        Bitmap bitmap = (Bitmap) (opcode._param1);
        g.DrawImage (bitmap, LineXY.X, LineXY.Y);
        AddToLine (bitmap.Size);
      }
      else if (opcode._opcodeType == OpcodeTypeEnum.NextLine) {
        NextLine_ ();
      }
      else if (opcode._opcodeType == OpcodeTypeEnum.AddSeparation) {
        AddSeparation_ ();
        g.DrawLine (ContourPen, _pixelXY.X + _deltaXY.X + 1, LineXY.Y - _margin - 1, _pixelXY.X + _deltaXY.X + 1 + OuterBoxSize.Width - 2 - 1 - 1, LineXY.Y - _margin - 1);
      }
      else if (opcode._opcodeType == OpcodeTypeEnum.Color) {
        _brushFg = (Brush) opcode._param1;
      }
    }

    private static Point LineXY {
      get {
        return (new Point (_pixelXY.X + _deltaXY.X + 2 + _margin + _lineSize.Width, _pixelXY.Y + _deltaXY.Y + 2 + _margin + _boxSize.Height));
      }
    }

    public static Size OuterBoxSize {
      get {
        return (new Size ((2 + _margin) * 2 + _boxSize.Width, (2 + _margin) * 2 + _boxSize.Height));
      }
    }

    private static void DrawBox_ (Graphics g) {
      if (_boxSize.IsEmpty)
        throw new Exception ();
      Size outerBoxSize = OuterBoxSize;
      g.FillRectangle (_popupBrush, _pixelXY.X + _deltaXY.X, _pixelXY.Y + _deltaXY.Y, outerBoxSize.Width, outerBoxSize.Height);
      g.DrawRectangle (Pens.Black, _pixelXY.X + _deltaXY.X, _pixelXY.Y + _deltaXY.Y, outerBoxSize.Width - 1, outerBoxSize.Height - 1);
      g.DrawRectangle (ContourPen, _pixelXY.X + _deltaXY.X + 1, _pixelXY.Y + _deltaXY.Y + 1, outerBoxSize.Width - 2 - 1, outerBoxSize.Height - 2 - 1);
    }

    public static void Center () {
      if (_centered)
        return;

      if (!_lineSize.IsEmpty)
        throw new Exception ();

      _opcodes.Add (new Opcode (OpcodeTypeEnum.Center, null, null, null));
      _centered = true;
    }

    public static void Brush (Brush brush) {
      _opcodes.Add (new Opcode (OpcodeTypeEnum.Color, brush, null, null));
      _brushFg = brush;
    }

    public static void AddSeparation () {
      _opcodes.Add (new Opcode (OpcodeTypeEnum.AddSeparation, null, null, null));

      AddSeparation_ ();
    }

    private static void AddSeparation_ () {
      if (!_lineSize.IsEmpty)
        NextLine_ ();
      _boxSize.Height += _margin * 2 + 1;
    }

    public static void NextLine () {
      if (_lineSize.IsEmpty)
        throw new Exception ();

      _opcodes.Add (new Opcode (OpcodeTypeEnum.NextLine, null, null, null));

      NextLine_ ();
    }

    private static void NextLine_ () {
      if (_boxSize.Width < _lineSize.Width) {
        _boxSize.Width = _lineSize.Width;
      }
      _boxSize.Height += _lineSize.Height;
      if (_lineIndex == _linesSize.Count)
        _linesSize.Add (_lineSize);
      _lineSize = Size.Empty;
      _lineIndex++;
      _centered = false;
      _brushFg = null;
    }

    public static void AddString (Graphics g, string text) {
      AddString (g, text, Brushes.White);
    }

    public static void AddStringLine (Graphics g, string text) {
      AddString (g, text, Brushes.White);
      NextLine ();
    }

    public static void AddStringLine (Graphics g, string text, Brush brush) {
      AddString (g, text, brush);
      NextLine ();
    }

    public static void AddString (Graphics g, string text, Brush brush) {
      _opcodes.Add (new Opcode (OpcodeTypeEnum.AddString, text, brush, null));

      Size size = g.MeasureString2 (text, World._fontBox).Size.ToSizeCeiling ();
      AddToLine (size);
    }

    public static void AddBitmap (Graphics g, Bitmap bitmap) {
      _opcodes.Add (new Opcode (OpcodeTypeEnum.AddBitmap, bitmap, null, null));

      AddToLine (bitmap.Size);
    }

    private static void AddToLine (Size size) {
      _lineSize.Width += size.Width;
      if (_lineSize.Height < size.Height) {
        _lineSize.Height = size.Height;
      }
    }
  }

  public class Opcode {
    public OpcodeTypeEnum _opcodeType;
    public object _param1;
    public object _param2;
    public object _param3;

    public Opcode (OpcodeTypeEnum opcodeType, object param1, object param2, object param3) {
      _opcodeType = opcodeType;
      _param1 = param1;
      _param2 = param2;
      _param3 = param3;
    }
  }

  public enum OpcodeTypeEnum {
    Center,
    AddString,
    AddBitmap,
    AddSeparation,
    NextLine,
    Color
  }
}
