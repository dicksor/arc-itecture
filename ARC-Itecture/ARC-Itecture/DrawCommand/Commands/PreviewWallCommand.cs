﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ARC_Itecture.DrawCommand.Commands
{
    class PreviewWallCommand : IDrawCommand
    {
        private Receiver _receiver;

        public PreviewWallCommand(Receiver receiver)
        {
            this._receiver = receiver;
        }

        public void Execute(Point point)
        {
            this._receiver.DrawWallPreview(point);
        }
    }
}