﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Chroma.Engine.Utilities
{
    public class Alarm : Component
    {
        private float _interval;
        private float  _elapsed;
        private IScript _script;
        private object[] _scriptArgs;
        private bool _loop;
        private bool _renders;
        private bool _active;

        public Alarm(IScript script, object[] args, bool renders, bool loop, bool startImmediately, float seconds)
        {
            _script = script;
            _loop = loop;
            _interval = seconds;
            _scriptArgs = args;
            _renders = renders;
            _active = startImmediately;
        }

        public override void Update(GameTime gameTime)
        {
            if (!_active) return;
            _elapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_elapsed < _interval) return;
            if (!_renders) _script.Execute(_scriptArgs);
            if (_loop)
            {
                _elapsed = 0;
                return;
            }
            Stop();
        }

        public override void Render(GameTime gameTime)
        {
            if (!_renders) return;
            if (_elapsed < _interval) return;
            _script.Execute(_scriptArgs);
        }

        public void Start()
        {
            _active = true;
        }

        public void Pause()
        {
            _active = false;
        }

        public void Stop()
        {
            _active = false;
            _elapsed = 0;

        }
    }
}
