﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LCU;
using LeagueBot.DEBUG;

namespace LeagueBot.Patterns.Actions {
    class StartQueAction : PatternAction {

        int loby_time_out = 120;

        public StartQueAction() : base("Starting que") {
            needWindowHelp = false;
        }
        
        public override void Apply(Bot bot, Pattern pattern) {
            DateTime start = DateTime.Now;

            gameFlowPhase state;
            bool tryedRestart = false;

            do {
                state = clientLCU.GetGamePhase();
                if(DateTime.Now.Subtract(start).TotalSeconds > loby_time_out && !tryedRestart) {
                    clientLCU.StartSearch();
                    tryedRestart = true;
                    start = DateTime.Now;
                    DBG.log("Tryeing to start que again",MessageLevel.Warning, "StartQueAction");
                } else if (DateTime.Now.Subtract(start).TotalSeconds > loby_time_out) {
                    bot.stop("Wait to long for loby to be ready");
                    return;
                }
            } while (state != gameFlowPhase.Lobby && state != gameFlowPhase.InProgress && !isStoped);
            
            if(state == gameFlowPhase.InProgress) {
                DBG.log("Already in game", MessageLevel.Warning);
            } else if(!clientLCU.StartSearch()) {
                bot.stop("Faild to search");
            }
        }
    }
}
