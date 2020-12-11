﻿using System;
using System.IO;
using System.Data;
using System.Drawing;
using System.Diagnostics;


using LeagueBot.AI;
using LeagueBot.Event;
using LeagueBot.DEBUG;
using LeagueBot.Patterns;
using LeagueBot.Constants;

using InputManager;

namespace LeagueBot {
    public class Bot {


        public event EventHandler<EndGameData> GameEndEvent;

        private Pattern Pattern;

        public AvailableGameType GameType;
        public const Int32 buy_delay = 500;
        public String status; //TODO: REMOVE
        public bool working;
        public bool isEvent = false;
        public Pattern nextPattern = null;

        public Bot() {
            GameEndEvent += on_game_end;
            GameEndEvent += DBG.on_game_end;
        }


        public string getVersion() {
            Version v = System.Reflection.Assembly.GetEntryAssembly().GetName().Version;
            return v.ToString();
        }

        #region Controll
        public void Start(AvailableGameType gameType = AvailableGameType.TFT) {
            DBG.log("Starting bot");

            switch (gameType) {
                case AvailableGameType.TFT:
                    ApplyPattern(new StartTFTPattern(this));
                    break;
                case AvailableGameType.TEST:
                    ApplyPattern(new memTestPattern(this));
                    break;
                default:
                    throw new NotImplementedException("NO default game mode set");
                    //break;
            }
        }

        public void ThreadProc() {
            this.Start();
            return;
        }

        public void ApplyPattern(Pattern p, int i = 0) {
            Pattern = p;
            do {
                Pattern.Execute();
                Pattern.Dispose();
                Pattern = nextPattern;
                nextPattern = null;

            } while(Pattern != null);
            DBG.log("All patterns done!");
        }
        
        public void Abort(String stop_reson = "unknown reson") {
            if (Pattern != null) Pattern.Dispose();
            working = false;
            DBG.log("BOT STOPED: " + stop_reson, DateTime.Now, "BOT");
        }
        #endregion

        #region Mouse
        public void RightClick(Point point) {
            Mouse.Move(point.X, point.Y);
            Mouse.PressButton(Mouse.MouseKeys.Right, 150);
        }

        public void LeftClick(Point point, bool NoPress = false) {
            Mouse.Move(point.X, point.Y);
            if (!NoPress) {
                Mouse.PressButton(Mouse.MouseKeys.Left, 150);
            }
        }
        #endregion

        #region EVENTS
        public void invoke_tft_game_end(object s, EndGameData data) {
            GameEndEvent?.Invoke(s, data);
        }

        private void on_game_end(object sender, EndGameData data) {
            string[] s = DBG.get_usage();
            foreach (string l in s) {
                DBG.log(l, DateTime.Now, "BOT");
            }
        }
        #endregion
    }
}
