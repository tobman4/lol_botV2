﻿using System;

namespace LCU.Event {

    public delegate void TftGameEndEventHandler(EndGameData data);

    public class EndGameData : EventArgs {
        public int place;
        public string sender;
        public int GameLength;
    }
}