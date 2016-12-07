using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;

namespace GodsViewer {
  public class Alfils08_MovingBlock {
    public int _index;

    public Point _pixelXY;
    //public int _pixelY;
    public int _mapSpriteIndex_min1;
    public int _speed_min1;

    public Point [] _pixelsXY;

    public int [] _actions;
    public int _nbBlocksX_min1_unk;
    public int _nbBlocksY_min1;

    public int _nbBlocksX_min1;

    public Alfils08_MovingBlock (byte [] buffer, ref int offset, int index) {
      _index = index;

      int pixelX = buffer.ReadWord (offset);
      offset += 2;
      int pixelY = buffer.ReadWord (offset);
      offset += 2;
      _pixelXY = new Point (pixelX, pixelY);
      _mapSpriteIndex_min1 = buffer.ReadByte (offset++);
      _speed_min1 = buffer.ReadByte (offset++);

      _pixelsXY = new Point [4];
      for (int i = 0; i < 4; i++) {
        int x = buffer.ReadWord (offset);
        offset += 2;
        int y = buffer.ReadWord (offset);
        offset += 2;
        _pixelsXY [i] = new Point (x, y);
      }

      _actions = new int [4];
      for (int i = 0; i < 4; i++) {
        _actions [i] = buffer.ReadByte (offset++);
      }

      _nbBlocksX_min1_unk = buffer.ReadByte (offset++);
      _nbBlocksY_min1 = buffer.ReadByte (offset++);

      _nbBlocksX_min1 = _nbBlocksX_min1_unk & 0x07;

      //for (int i = 0; i < 4; i++) {
      //  if ((_actions [i] < 0) || ((_actions [i] > 5) && (_actions [i] != 0xFF)))
      //    Dump ();
      //}
      //if (((_pixelX & 0x8000) != 0) || ((_pixelY & 0x8000) != 0)) {
      //  throw new Exception ();
      //}
    }

    public void Dump () {
      Console.WriteLine ("Moving block #{0} at ({1},{2})", _index, PixelXY.X, PixelXY.Y);
      Console.WriteLine ("Map srpite index: #{0}", _mapSpriteIndex_min1 + 1);
      Console.WriteLine ("Speed: {0} pixels/frame", _speed_min1 + 1);
      for (int i = 0; i < 4; i++) {
        Console.WriteLine ("Pixel XY #{0}: ({1},{2})", i, _pixelsXY [i].X, _pixelsXY [i].Y);
      }
      for (int i = 0; i < 4; i++) {
        int actionTypeIndex = _actions [i];
        string action = null;
        MovingBlockActionTypeEnum actionType = GetActionType (i);
        if (actionType == MovingBlockActionTypeEnum.MoveToCoord0ThenNextCoord)
          action = "Move to coord0 then next coord";
        else if (actionType == MovingBlockActionTypeEnum.MoveToCoord0ThenPrevCoord)
          action = "Move to coord0 then prev coord";
        else if (actionType == MovingBlockActionTypeEnum.MoveToCoordThenStop)
          action = string.Format ("Move to coord{0} ({1},{2}) then stop", actionTypeIndex - 2, _pixelsXY [actionTypeIndex - 2].X, _pixelsXY [actionTypeIndex - 2].Y);
        else if (actionType == MovingBlockActionTypeEnum.Disable)
          action = "Disable";
        else if (actionType == MovingBlockActionTypeEnum.Unk)
          action = "?";
        //throw new Exception ();

        Console.WriteLine ("Function index #{0}: {1} ({2})", i, actionTypeIndex, action);
      }
      Console.WriteLine ("Size: ({0},{1})", _nbBlocksX_min1 + 1, _nbBlocksY_min1 + 1);
      Console.WriteLine ("SizeX unk: {0}", _nbBlocksX_min1_unk);
      Console.WriteLine ();
    }

    public MovingBlockActionTypeEnum GetActionType (int actionIndex) {
      int actionTypeIndex = _actions [actionIndex];
      if (actionTypeIndex == 0)
        return (MovingBlockActionTypeEnum.MoveToCoord0ThenNextCoord);
      else if (actionTypeIndex == 1)
        return (MovingBlockActionTypeEnum.MoveToCoord0ThenPrevCoord);
      else if ((actionTypeIndex >= 2) && (actionTypeIndex <= 5))
        return (MovingBlockActionTypeEnum.MoveToCoordThenStop);
      else if (actionTypeIndex == 0xFF)
        return (MovingBlockActionTypeEnum.Disable);
      else if (actionTypeIndex == 0xFE)
        return (MovingBlockActionTypeEnum.NeverUsed);
      else
        return (MovingBlockActionTypeEnum.Unk);
    }

    public void Draw (Graphics g, Point pixelXY, World world) {
      Draw (g, pixelXY, world, 1);
    }

    public void Draw (Graphics g, Point pixelXY, World world, float alpha) {
      Bitmap bitmap = world.GetBitmapMap (_mapSpriteIndex_min1 + 1);
      if (bitmap == null)
        return;
      for (int y = 0; y < _nbBlocksY_min1 + 1; y++) {
        for (int x = 0; x < _nbBlocksX_min1 + 1; x++) {
          g.DrawImage (bitmap, pixelXY.X + x * 32, pixelXY.Y + y * 16, alpha);
        }
      }
    }

    public void DrawProjection () {
    }

    //*****************************************************************************************************************
    public void DrawOnMap (Graphics g, int actionIndex, World world, Point fromXY) {
      g.DrawRectangle (Pens.White, PixelXY.X, PixelXY.Y, Size.Width - 1, Size.Height - 1);
      Point centerXY = new Rectangle (PixelXY, Size).Center ();
      if (!fromXY.IsEmpty)
        g.DrawArrowOutlinedSolid (Pens.White, fromXY.X, fromXY.Y, centerXY.X, centerXY.Y);

      if (actionIndex == -1) {
        for (int i = 0; i < 4; i++)
          DrawOnMapAction (g, i, world, fromXY);
      }
      else {
        DrawOnMapAction (g, actionIndex, world, fromXY);
      }

      //Alfils02_Event event_ = world.GetEventAtCell (new Point (fromXY.X / 32, fromXY.Y / 16));
      //if (event_ == null)
      //  throw new Exception ();

      //int actionIndex;
      //if (event_.Type == EventTypeEnum.ActivateMovingBlockAction0_06)
      //  actionIndex = 0;
      //else if (event_.Type == EventTypeEnum.ActivateMovingBlockAction1_07)
      //  actionIndex = 1;
      //else if (event_.Type == EventTypeEnum.ActivateMovingBlockAction2_08)
      //  actionIndex = 2;
      //else if (event_.Type == EventTypeEnum.ActivateMovingBlockAction3_09)
      //  actionIndex = 3;
      //else
      //  throw new Exception ();

      if (!fromXY.IsEmpty)
        g.DrawArrowOutlinedSolid (Pens.White, fromXY.X, fromXY.Y, centerXY.X, centerXY.Y);
    }

    public void DrawOnMapAction (Graphics g, int actionIndex, World world, Point fromXY) {
      Point centerXY = new Rectangle (PixelXY, Size).Center ();
      MovingBlockActionTypeEnum actionType = GetActionType (actionIndex);
      if (actionType == MovingBlockActionTypeEnum.Disable)
        g.DrawRectangleCrossed (Pens.White, PixelXY.X, PixelXY.Y, Size.Width - 1, Size.Height - 1);
      else if (actionType == MovingBlockActionTypeEnum.MoveToCoordThenStop) {
        Point dstPixelXY = _pixelsXY [_actions [actionIndex] - 2];
        Draw (g, dstPixelXY, world);

        g.DrawRectangle (Pens.White, dstPixelXY.X, dstPixelXY.Y, Size.Width - 1, Size.Height - 1);
        g.DrawArrowOutlinedSolid (Pens.White, centerXY.X, centerXY.Y, dstPixelXY.X + Size.Width / 2, dstPixelXY.Y + Size.Height / 2);
      }
      else if (actionType == MovingBlockActionTypeEnum.MoveToCoord0ThenNextCoord) {
        Point lastDstPixelXY = _pixelsXY [0];
        if (lastDstPixelXY.IsEmpty)
          throw new Exception ();

        Draw (g, lastDstPixelXY, world);
        for (int i = 1; i < 4; i++) {
          Point dstPixelXY = _pixelsXY [i];
          if (dstPixelXY.IsEmpty)
            break;
          Draw (g, dstPixelXY, world);
          lastDstPixelXY = dstPixelXY;
        }

        lastDstPixelXY = _pixelsXY [0];
        Point lastDstCenterPixelXY = new Rectangle (lastDstPixelXY, Size).Center ();
        if (lastDstPixelXY.IsEmpty)
          throw new Exception ();

        g.DrawRectangle (Pens.White, lastDstPixelXY.X, lastDstPixelXY.Y, Size.Width - 1, Size.Height - 1);
        for (int i = 1; i < 4; i++) {
          Point dstPixelXY = _pixelsXY [i];
          if (dstPixelXY.IsEmpty)
            break;
          Point dstCenterPixelXY = new Rectangle (dstPixelXY, Size).Center ();
          g.DrawRectangle (Pens.White, dstPixelXY.X, dstPixelXY.Y, Size.Width - 1, Size.Height - 1);
          g.DrawArrowOutlinedSolid (Pens.White, lastDstCenterPixelXY.X, lastDstCenterPixelXY.Y, dstCenterPixelXY.X, dstCenterPixelXY.Y);
          lastDstPixelXY = dstPixelXY;
          lastDstCenterPixelXY = dstCenterPixelXY;
        }
        g.DrawArrowOutlinedSolid (Pens.White, lastDstCenterPixelXY.X, lastDstCenterPixelXY.Y, _pixelsXY [0].X + Size.Width / 2, _pixelsXY [0].Y + Size.Height / 2);
      }
      else if (actionType == MovingBlockActionTypeEnum.MoveToCoord0ThenPrevCoord) {
        Point lastDstPixelXY = _pixelsXY [0];
        if (lastDstPixelXY.IsEmpty)
          throw new Exception ();

        Draw (g, lastDstPixelXY, world);
        for (int i = 3; i > 0; i--) {
          Point dstPixelXY = _pixelsXY [i];
          if (dstPixelXY.IsEmpty)
            break;
          Draw (g, dstPixelXY, world);
          lastDstPixelXY = dstPixelXY;
        }

        lastDstPixelXY = _pixelsXY [0];
        Point lastDstCenterPixelXY = new Rectangle (lastDstPixelXY, Size).Center ();
        if (lastDstPixelXY.IsEmpty)
          throw new Exception ();

        g.DrawRectangle (Pens.White, lastDstPixelXY.X, lastDstPixelXY.Y, Size.Width - 1, Size.Height - 1);
        for (int i = 3; i > 0; i--) {
          Point dstPixelXY = _pixelsXY [i];
          if (dstPixelXY.IsEmpty)
            break;
          Point dstCenterPixelXY = new Rectangle (dstPixelXY, Size).Center ();
          g.DrawRectangle (Pens.White, dstPixelXY.X, dstPixelXY.Y, Size.Width - 1, Size.Height - 1);
          g.DrawArrowOutlinedSolid (Pens.White, lastDstCenterPixelXY.X, lastDstCenterPixelXY.Y, dstCenterPixelXY.X, dstCenterPixelXY.Y);
          lastDstPixelXY = dstPixelXY;
          lastDstCenterPixelXY = dstCenterPixelXY;
        }
        g.DrawArrowOutlinedSolid (Pens.White, lastDstCenterPixelXY.X, lastDstCenterPixelXY.Y, _pixelsXY [0].X + Size.Width / 2, _pixelsXY [0].Y + Size.Height / 2);
      }
      //else if (actionType == MovingBlockActionTypeEnum.Unk)
      //  throw new Exception ();
    }

    public void DrawOnMap_Reverse (Graphics g, Point cellXY, World world, Point toXY, bool state) {
      g.DrawRectangle (Pens.White, PixelXY.X, PixelXY.Y, Size.Width - 1, Size.Height - 1);
      Point centerXY = new Rectangle (PixelXY, Size).Center ();
      if (!toXY.IsEmpty)
        g.DrawArrowOutlinedState (Pens.White, centerXY.X, centerXY.Y, toXY.X, toXY.Y, state);

      // OK: moving block used in event effect (move block)
      foreach (Alfils02_Event event_ in world.GetEventsWithEffect (EventTypeEnum.ActivateMovingBlockAction0_06, _index)) {
        event_.DrawOnMap_Reverse (g, cellXY, world, centerXY, false);
      }
      foreach (Alfils02_Event event_ in world.GetEventsWithEffect (EventTypeEnum.ActivateMovingBlockAction1_07, _index)) {
        event_.DrawOnMap_Reverse (g, cellXY, world, centerXY, false);
      }
      foreach (Alfils02_Event event_ in world.GetEventsWithEffect (EventTypeEnum.ActivateMovingBlockAction2_08, _index)) {
        event_.DrawOnMap_Reverse (g, cellXY, world, centerXY, false);
      }
      foreach (Alfils02_Event event_ in world.GetEventsWithEffect (EventTypeEnum.ActivateMovingBlockAction3_09, _index)) {
        event_.DrawOnMap_Reverse (g, cellXY, world, centerXY, false);
      }
    }

    public void DrawBox (Graphics g, World world) {
      BoxHelper.Center ();
      BoxHelper.AddString (g, string.Format ("MOVING BLOCK #{0}", _index));
      BoxHelper.AddSeparation ();
      BoxHelper.AddStringLine (g, string.Format ("(x,y): ({0},{1})", _pixelXY.X, _pixelXY.Y));
      BoxHelper.AddStringLine (g, string.Format ("(width,height): ({0},{1})", _nbBlocksX_min1 + 1, _nbBlocksY_min1 + 1));
      BoxHelper.AddStringLine (g, string.Format ("Speed: {0} pixels/frame", _speed_min1 + 1));

      for (int i = 0; i < 4; i++) {
        string description = GetActionDescription (i);
        if (description != null)
          BoxHelper.AddStringLine (g, string.Format ("Action #{0}: {1}", i, description));
      }
      BoxHelper.DrawBox (g);
    }

    public string GetActionDescription (int actionIndex) {
      int actionTypeIndex = _actions [actionIndex];
      string description = null;
      MovingBlockActionTypeEnum actionType = GetActionType (actionIndex);
      if (actionType == MovingBlockActionTypeEnum.MoveToCoord0ThenNextCoord) {
        string coords = string.Format ("({0},{1})", _pixelsXY [0].X, _pixelsXY [0].Y);
        for (int j = 1; j < 4; j++) {
          if (_pixelsXY [j].IsEmpty)
            break;
          coords += string.Format (", ({0},{1})", _pixelsXY [j].X, _pixelsXY [j].Y);
        }
        description = string.Format ("Loop {0}", coords);
      }
      else if (actionType == MovingBlockActionTypeEnum.MoveToCoord0ThenPrevCoord) {
        string coords = string.Format ("({0},{1})", _pixelsXY [0].X, _pixelsXY [0].Y);
        for (int j = 3; j > 0; j--) {
          if (_pixelsXY [j].IsEmpty)
            break;
          coords += string.Format (", ({0},{1})", _pixelsXY [j].X, _pixelsXY [j].Y);
        }
        description = string.Format ("Loop {0}", coords);
      }
      else if (actionType == MovingBlockActionTypeEnum.MoveToCoordThenStop)
        description = string.Format ("Move block to ({0},{1})", _pixelsXY [actionTypeIndex - 2].X, _pixelsXY [actionTypeIndex - 2].Y);
      else if (actionType == MovingBlockActionTypeEnum.Disable)
        description = "Disable";
      else if (actionType == MovingBlockActionTypeEnum.NeverUsed)
        description = null;
      else if (actionType == MovingBlockActionTypeEnum.Unk)
        description = "?";
      return (description);
    }

    public Point PixelXY {
      get {
        return (_pixelXY);
      }
    }

    public Size Size {
      get {
        return (new Size ((_nbBlocksX_min1 + 1) * 32, (_nbBlocksY_min1 + 1) * 16));
      }
    }

    public Rectangle Rectangle {
      get {
        return (new Rectangle (PixelXY, Size));
      }
    }
  }

  public enum MovingBlockActionTypeEnum {
    MoveToCoord0ThenNextCoord,
    MoveToCoord0ThenPrevCoord,
    MoveToCoordThenStop,
    Disable,
    Unk,
    NeverUsed
  }
}
