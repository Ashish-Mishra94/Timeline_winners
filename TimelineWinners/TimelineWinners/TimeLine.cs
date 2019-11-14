using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimelineWinners
{
    public class TimeLine
    {
    }

    public class MatchDetails
    {
        public string series_name;
        public string series_id;
        public string winning_team_name;
        public string winning_team_id;
        public string year;
        public string series_outcome;
        public string series_status;
    }

    public class Gender
    {
        public List<MatchDetails> Men = new List<MatchDetails>();
        public List<MatchDetails> Women = new List<MatchDetails>();
    }

    public class RootObject
    {
        public Gender WorldCup = new Gender();
        //public List<MatchDetails> WomensWorldCupDetails = new List<MatchDetails>();
        public Gender T20WorldCup = new Gender();
        //public List<MatchDetails> WomensT20WorldCupDetails = new List<MatchDetails>();
        public Gender ChampionsTrophy = new Gender();
        //public List<MatchDetails> WomensChampionsTrophyDetails = new List<MatchDetails>();
        public Gender Ashes = new Gender();
        //public List<MatchDetails> WomensAshesDetails = new List<MatchDetails>();
        public Gender T20Leagues = new Gender();
        //public List<MatchDetails> WomensT20LeaguesDetails = new List<MatchDetails>();
        public Gender AsiaCup = new Gender();
        //public List<MatchDetails> WomensAsiaCupDetails = new List<MatchDetails>();

    }
}
