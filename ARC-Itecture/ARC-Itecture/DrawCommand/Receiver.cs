/*
 * ARC-Itecture
 * Romain Capocasale, Vincent Moulin and Jonas Freiburghaus
 * He-Arc, INF3dlm-a
 * 2019-2020
 * .NET Course
 */


using System.Collections.Generic;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using ARC_Itecture.Utils;
using ARC_Itecture.Geometry;
using System.Windows.Input;
using System.Linq;
using ARC_Itecture.DrawCommand.Drawers;

namespace ARC_Itecture.DrawCommand
{
    public class Receiver
    {
        private AreaDrawer _areaDrawer;
        private CameraDrawer _cameraDrawer;
        private List<Point> _doorAvailablePoints;
        private DoorDrawer _doorDrawer;
        private WallDrawer _wallDrawer;
        private List<Rect> _windowAvailableWalls;
        private WindowDrawer _windowDrawer;

        public Shape LastShape { get; set; }
        public ViewModel ViewModel { get; }

        public Receiver(ViewModel viewModel)
        {
            this.ViewModel = viewModel;
            this._doorAvailablePoints = new List<Point>();
            this._windowAvailableWalls = new List<Rect>();
            this._areaDrawer = new AreaDrawer(this);
            this._cameraDrawer = new CameraDrawer(this);
            this._doorDrawer = new DoorDrawer(this, ref _doorAvailablePoints);
            this._windowAvailableWalls = new List<Rect>();
            this._wallDrawer = new WallDrawer(this, ref _doorAvailablePoints, ref _windowAvailableWalls);
            this._windowDrawer = new WindowDrawer(this, ref _windowAvailableWalls);
        }

        public void DrawArea(Point p, string areaTypeName)
        {
            _areaDrawer.AreaTypeName = areaTypeName;
            _areaDrawer.Draw(p);
        }

        public void DrawAreaPreview(Point p)
        {
            _areaDrawer.DrawPreview(p);
        }

        public void DrawCamera(Point p)
        {
            _cameraDrawer.Draw(p);
        }

        public void DrawDoor(Point p)
        {
            _doorDrawer.Draw(p);
        }

        public void DrawDoorPreview(Point p)
        {
            _doorDrawer.DrawPreview(p);
        }

        public void DrawWall(Point p)
        {
            _wallDrawer.Draw(p);
        }

        public void DrawWallPreview(Point p)
        {
            _wallDrawer.DrawPreview(p);
        }

        public void DrawWindow(Point p)
        {
            _windowDrawer.Draw(p);
        }

        public void DrawWindowPreview(Point p)
        {
            _windowDrawer.DrawPreview(p);
        }

        public void RemoveLastPreview()
        {
            ViewModel._mainWindow.canvas.Children.Remove(LastShape);
        }

        public void StartNewWall()
        {
            _wallDrawer.StartNewWall();
        }

        public void UpdateAvailableWindowList(Rectangle rect)
        {
            _windowDrawer.UpdateAvailableWindowList(rect);
        }
    }
}
