using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Windows.Forms;

namespace GodsViewer {
  public class Alfils02_Event {
    public int _index;

    public int _functionIndex_min1;
    public int _param;

    //public bool _isTriggered = true;

    public static string [] _names = new string [] { "FW", "WW", "P", "I2", "I1", "CP", "MB0", "MB1", "MB2", "MB3", "G" };
    public Alfils02_Event (byte [] buffer, ref int offset, int index) {
      _index = index;

      _functionIndex_min1 = buffer.ReadWordSigned (offset);
      offset += 2;
      _param = buffer.ReadWord (offset);
      offset += 2;

      if (_functionIndex_min1 < -1) {
        throw new Exception ();
      }
    }

    public string Acronym {
      get {
        return (_names [_functionIndex_min1 - 1]);
      }
    }

    public EventTypeEnum Type {
      get {
        if (_functionIndex_min1 == -1) {
          return (EventTypeEnum.Inactive_11);
        }
        return ((EventTypeEnum) _functionIndex_min1 - 1);
      }
    }

    public void Dump (World world) {
      Console.WriteLine (string.Format ("Event: {0}", Enum.GetName (typeof (EventTypeEnum), Type)));
      Console.WriteLine (string.Format ("Param: {0}", _param));

      if (Type == EventTypeEnum.CheckPuzzle_02) {
        MapPuzzle puzzle = world.GetPuzzle (_param);
        puzzle.Dump (world);
      }
    }


    public MapPuzzle GetPuzzle (World world) {
      if (Type != EventTypeEnum.CheckPuzzle_02)
        throw new Exception ();
      return (world.GetPuzzle (_param));
    }

    public List<Point> GetMapCells (World world) {
      List<Point> cellsXY = new List<Point> ();
      // add events triggered from the map
      for (int y = 0; y < 64; y++) {
        for (int x = 0; x < 128; x++) {
          Point cellXY = new Point (x, y);
          int eventIndex = world.GetMapEventIndexAt (cellXY);
          if (eventIndex == -1)
            continue;
          if (eventIndex != _index)
            continue;
          cellsXY.Add (cellXY);
        }
      }
      return (cellsXY);
    }

    public List<Point> GetCells (World world) {
      List<Point> cellsXY = GetMapCells (world);
      // add events triggered from puzzle effect
      foreach (Alfils02_Event event_ in world.GetEventsWithPuzzleEffect (PuzzleEffectTypeEnum.TriggerEvent_05, _index + 1)) {
        cellsXY.AddRange (event_.GetCells (world));
      }
      return (cellsXY);
    }

    //public void DrawOnMapOnBox (Graphics g, Point eventXY, Point pixelXY, World world) {
    //  DrawOnMap (g, eventXY, pixelXY, world);
    //  DrawOnBox (g, eventXY, pixelXY, world);
    //}

    public static double Distance (Point p1, Point p2) {
      int xDist = p1.X - p2.X;
      int yDist = p1.Y - p2.Y;
      return (Math.Sqrt (xDist * xDist + yDist * yDist));
    }

    public static Point ClosestPoint (Point p, List<Point> points) {
      Point closestPoint = Point.Empty;
      double shortestDistance = int.MaxValue;
      foreach (Point point in points) {
        double distance = Distance (p, point);
        if (distance < shortestDistance) {
          shortestDistance = distance;
          closestPoint = point;
        }
      }
      return (closestPoint);
    }

    public void DrawOnMap (Graphics g, Point cellXY, World world, Point fromXY, bool state) {
      List<Point> fromsXY = new List<Point> ();
      fromsXY.Add (fromXY);
      DrawOnMap (g, cellXY, world, fromsXY, state);
    }

    public void DrawOnMap (Graphics g, Point cellXY, World world, List<Point> fromsXY, bool state) {
      List<Point> cellsXY = GetCells (world);
      if (cellXY.IsEmpty)
        cellXY = ClosestPoint (Main._cellXY, cellsXY);
      else
        if (!cellsXY.Contains (cellXY))
          cellXY = ClosestPoint (cellXY, cellsXY);

      foreach (Point cellXY2 in cellsXY) {
        Point pixelXY2 = new Point (cellXY2.X * 32, cellXY2.Y * 16);
        Point centerXY2 = new Point (pixelXY2.X + 16, pixelXY2.Y + 8);

        g.DrawRectangle (Pens.Magenta, pixelXY2.X, pixelXY2.Y, 32 - 1, 16 - 1);
      }
      Point pixelXY = new Point (cellXY.X * 32, cellXY.Y * 16);
      Point centerXY = new Point (pixelXY.X + 16, pixelXY.Y + 8);
      if (!cellXY.IsEmpty)
        if (fromsXY != null)
          foreach (Point fromXY in fromsXY)
            g.DrawArrowOutlinedState (Pens.Magenta, fromXY.X, fromXY.Y, centerXY.X, centerXY.Y, state);

      //List<Point> centersXY = new List<Point> ();
      //centersXY.Add (centerXY);

      if ((!World._recurseOnEvent) && (fromsXY != null))
        return;
      if (World._eventsVisited.ContainsKey (this))
        return;
      World._eventsVisited.Add (this, null);

      if (Type == EventTypeEnum.SpawnFlyingWave_00) {
        Alfils01_FlyingWave wave = world.GetFlyingWave (_param);
        wave.DrawOnMap (g, cellXY, world, centerXY);
      }
      else if (Type == EventTypeEnum.SpawnWalkingWave_01) {
        Alfils03_WalkingWave wave = world.GetWalkingWave (_param);
        wave.DrawOnMap (g, cellXY, world, centerXY);
      }
      else if (Type == EventTypeEnum.CheckPuzzle_02) {
        MapPuzzle puzzle = GetPuzzle (world);
        puzzle.DrawOnMap (g, cellXY, world, centerXY);
      }
      else if (Type == EventTypeEnum.SpawnIntelWalkingWave_03) {
        Alfils04b_IntelWalkingWave wave = world.GetIntelWalkingWave (_param);
        wave.DrawOnMap (g, cellXY, world, centerXY);
      }
      else if (Type == EventTypeEnum.SpawnIntelFlyingWave_04) {
        Alfils04a_IntelFlyingWave wave = world.GetIntelFlyingWave (_param);
        wave.DrawOnMap (g, cellXY, world, centerXY);
      }
      else if (Type == EventTypeEnum.Checkpoint_05) {
      }
      else if ((Type == EventTypeEnum.ActivateMovingBlockAction0_06) ||
               (Type == EventTypeEnum.ActivateMovingBlockAction1_07) ||
               (Type == EventTypeEnum.ActivateMovingBlockAction2_08) ||
               (Type == EventTypeEnum.ActivateMovingBlockAction3_09)) {
        int actionIndex = ((int) Type) - ((int) EventTypeEnum.ActivateMovingBlockAction0_06);
        Alfils08_MovingBlock movingBlock = world.GetMovingBlock (_param);
        movingBlock.DrawOnMap (g, actionIndex, world, centerXY);
      }
      else if (Type == EventTypeEnum.LoadGuardian_10) {
      }
      else if (Type == EventTypeEnum.Inactive_11) {
      }
      else {
        throw new Exception ();
      }

      // event used in a puzzle condition (triggered)
      foreach (Alfils02_Event event_ in world.GetEventsWithPuzzleCondition (ConditionTypeEnum.EventTriggered_05, _index + 1)) {
        event_.DrawOnMap (g, cellXY, world, centerXY, false);
      }
      foreach (Alfils02_Event event_ in world.GetEventsWithPuzzleCondition (ConditionTypeEnum.EventNotTriggered_06, _index + 1)) {
        event_.DrawOnMap (g, cellXY, world, centerXY, true);
      }
      World._eventsVisited.Remove (this);
    }

    //public void DrawOnMap_Reverse (Graphics g, Point cellXY, World world, Point toXY, bool state) {
    //  List<Point> tosXY = new List<Point> ();
    //  tosXY.Add (toXY);
    //  DrawOnMap_Reverse (g, cellXY, world, tosXY, state);
    //}

    public void DrawOnMap_Reverse (Graphics g, Point cellXY, World world, Point toXY, bool state) {
      List<Point> cellsXY = GetCells (world);
      if (cellXY.IsEmpty)
        cellXY = ClosestPoint (Main._cellXY, cellsXY);
      else
        if (!cellsXY.Contains (cellXY))
          cellXY = ClosestPoint (cellXY, cellsXY);

      foreach (Point cellXY2 in cellsXY) {
        Point pixelXY2 = new Point (cellXY2.X * 32, cellXY2.Y * 16);
        Point centerXY2 = new Point (pixelXY2.X + 16, pixelXY2.Y + 8);

        g.DrawRectangle (Pens.Magenta, pixelXY2.X, pixelXY2.Y, 32 - 1, 16 - 1);
      }
      Point pixelXY = new Point (cellXY.X * 32, cellXY.Y * 16);
      Point centerXY = new Point (pixelXY.X + 16, pixelXY.Y + 8);
      if (!cellXY.IsEmpty)
        if (!toXY.IsEmpty)
          g.DrawArrowOutlinedState (Pens.Magenta, centerXY.X, centerXY.Y, toXY.X, toXY.Y, state);

      //List<Point> centersXY = new List<Point> ();
      //centersXY.Add (centerXY);

      if ((!World._recurseOnEvent) && (!toXY.IsEmpty))
        return;
      if (World._eventsVisited.ContainsKey (this))
        return;
      World._eventsVisited.Add (this, null);

      if (Type == EventTypeEnum.SpawnFlyingWave_00) {
        //Alfils01_FlyingWave wave = world.GetFlyingWave (_param);
        //wave.DrawOnMap (g, centerXY, world, centerXY);
      }
      else if (Type == EventTypeEnum.SpawnWalkingWave_01) {
        //Alfils03_WalkingWave wave = world.GetWalkingWave (_param);
        //wave.DrawOnMap (g, world, centerXY);
      }
      else if (Type == EventTypeEnum.CheckPuzzle_02) {
        MapPuzzle puzzle = GetPuzzle (world);
        puzzle.DrawOnMap_Reverse_FromEvent (g, cellXY, world, centerXY, false);
      }
      else if (Type == EventTypeEnum.SpawnIntelWalkingWave_03) {
      }
      else if (Type == EventTypeEnum.SpawnIntelFlyingWave_04) {
      }
      else if (Type == EventTypeEnum.Checkpoint_05) {
      }
      else if ((Type == EventTypeEnum.ActivateMovingBlockAction0_06) ||
               (Type == EventTypeEnum.ActivateMovingBlockAction1_07) ||
               (Type == EventTypeEnum.ActivateMovingBlockAction2_08) ||
               (Type == EventTypeEnum.ActivateMovingBlockAction3_09)) {
      }
      else if (Type == EventTypeEnum.LoadGuardian_10) {
      }
      else if (Type == EventTypeEnum.Inactive_11) {
      }
      else {
        throw new Exception ();
      }

      // event triggered by a puzzle effect (trigger event)
      foreach (Alfils02_Event event_ in world.GetEventsWithPuzzleEffect (PuzzleEffectTypeEnum.TriggerEvent_05, _index + 1)) {
        event_.DrawOnMap_Reverse (g, cellXY, world, centerXY, false);
      }
      World._eventsVisited.Remove (this);
    }

    public void DrawBox (Graphics g, World world) {
      if (World._eventsVisited.ContainsKey (this))
        return;
      World._eventsVisited.Add (this, null);

      BoxHelper.Center ();
      BoxHelper.AddString (g, string.Format ("EVENT #{0} - ", _index));
      //BoxHelper.AddSeparation ();

      if (Type == EventTypeEnum.SpawnFlyingWave_00) {
        Alfils01_FlyingWave wave = world.GetFlyingWave (_param);
        wave.DrawBox (g, world);
      }
      else if (Type == EventTypeEnum.SpawnWalkingWave_01) {
        Alfils03_WalkingWave wave = world.GetWalkingWave (_param);
        wave.DrawBox (g, world);
      }
      else if (Type == EventTypeEnum.CheckPuzzle_02) {
        MapPuzzle puzzle = GetPuzzle (world);
        puzzle.DrawBox (g, world);
      }
      else if (Type == EventTypeEnum.SpawnIntelWalkingWave_03) {
        Alfils04b_IntelWalkingWave wave = world.GetIntelWalkingWave (_param);
        wave.DrawBox (g, world);
      }
      else if (Type == EventTypeEnum.SpawnIntelFlyingWave_04) {
        Alfils04a_IntelFlyingWave wave = world.GetIntelFlyingWave (_param);
        wave.DrawBox (g, world);
      }
      else if ((Type == EventTypeEnum.ActivateMovingBlockAction0_06) ||
               (Type == EventTypeEnum.ActivateMovingBlockAction1_07) ||
               (Type == EventTypeEnum.ActivateMovingBlockAction2_08) ||
               (Type == EventTypeEnum.ActivateMovingBlockAction3_09)) {
        int movingBlockIndex = _param;
        BoxHelper.Center ();
        BoxHelper.AddString (g, string.Format ("ACTIVATE MOVING BLOCK #{0}", movingBlockIndex));
        BoxHelper.AddSeparation ();
        int actionIndex = ((int) Type) - ((int) EventTypeEnum.ActivateMovingBlockAction0_06);
        BoxHelper.AddStringLine (g, string.Format ("Execute action #{0}:", actionIndex));
        Alfils08_MovingBlock movingBlock = world.GetMovingBlock (movingBlockIndex);
        BoxHelper.AddStringLine (g, movingBlock.GetActionDescription (actionIndex));
        BoxHelper.DrawBox (g);

        movingBlock.DrawBox (g, world);
      }
      else {
        BoxHelper.Center ();
        BoxHelper.AddString (g, string.Format ("TODO"));
        BoxHelper.AddSeparation ();
        BoxHelper.AddStringLine (g, Enum.GetName (typeof (EventTypeEnum), Type));
        BoxHelper.DrawBox (g);
      }
    }
  }

  public enum EventTypeEnum {
    SpawnFlyingWave_00,
    SpawnWalkingWave_01,
    CheckPuzzle_02,
    SpawnIntelWalkingWave_03,
    SpawnIntelFlyingWave_04,
    Checkpoint_05,
    ActivateMovingBlockAction0_06,
    ActivateMovingBlockAction1_07,
    ActivateMovingBlockAction2_08,
    ActivateMovingBlockAction3_09,
    LoadGuardian_10,
    Inactive_11
  }
}
