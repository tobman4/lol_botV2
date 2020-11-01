﻿namespace LeagueBot.Patterns.Actions {
    public class DefinePatternAction : PatternAction {
        private Pattern Pattern {
            get;
            set;
        }
        public DefinePatternAction(Pattern pattern, string description, double duration = 0) : base(description, duration) {
            this.Pattern = pattern;
        }

        public override void Apply(Bot bot, Pattern pattern) {
            bot.ApplyPattern(Pattern);
        }
    }
}
