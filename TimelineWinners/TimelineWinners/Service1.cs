using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;
using Newtonsoft.Json;

namespace TimelineWinners
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
            LogFileName = ConfigurationManager.AppSettings["savePathLogsServer"] + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            JsonFilePath = ConfigurationManager.AppSettings["savePathJsonServer"];
        }

        string LogFileName;
        string JsonFilePath;
        public void OnStart()
        {
            OnStart(null);
        }

        public DataSet FetchDataDB()
        {
            DataSet dt = null;
            try
            {
                using (SqlConnection sql_conn = new SqlConnection("data source=192.168.101.1;database=sportz_cricket;uid=scripts;pwd=sportzCT;"))
                {
                    sql_conn.Open();
                    SqlCommand sql_comm = new SqlCommand("cricket_get_series_winner", sql_conn);
                    sql_comm.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter a = new SqlDataAdapter(sql_comm);
                    dt = new DataSet();
                    a.Fill(dt);
                }
                return dt;
            }
            catch (Exception ex)
            {
                string exception = DateTime.Now.ToString() + Environment.NewLine +
                                    "Error in FetchDataDB: " + Environment.NewLine + ex.ToString() + Environment.NewLine;
                File.AppendAllText(LogFileName, exception);
                return dt;
            }
        }

        protected override void OnStart(string[] args)
        {
            DataSet ds = new DataSet();
            ds = FetchDataDB();

            RootObject rootObject = new RootObject();


            rootObject.WorldCup = CreateWorldCupFiles(ds.Tables[0]);
            rootObject.T20WorldCup = CreateWorldCupFiles(ds.Tables[3]);
            rootObject.ChampionsTrophy = CreateWorldCupFiles(ds.Tables[2]);
            rootObject.Ashes = CreateWorldCupFiles(ds.Tables[1]);
            //rootObject.T20Leagues = CreateWorldCupFiles(ds.Tables[0]);
            rootObject.AsiaCup = CreateWorldCupFiles(ds.Tables[4]);

            //CreateT20WorldCupFiles(ds.Tables[3]);
            //CreateChampionsTrophyFiles(ds.Tables[2]);
            //CreateAshesFiles(ds.Tables[1]);
            ////CreateT20LeaguesFiles(ds.Tables[4]);
            //CreateAsiaCupFiles(ds.Tables[4]);

            string JsonStr = JsonConvert.SerializeObject(rootObject);
            string FileName = JsonFilePath + "Timeline.json";
            File.WriteAllText(FileName, JsonStr);//@"D:\BallbyBall\Data\" + 

            string processComplete = Environment.NewLine + "==================================" + Environment.NewLine +
                                        DateTime.Now.ToString() + Environment.NewLine + "Process Completed";
            File.AppendAllText(LogFileName, processComplete);
        }

        public Gender CreateWorldCupFiles(DataTable dt)
        {
            Gender gender = new Gender();
            try
            {
                //RootObject rootObject = new RootObject();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["gender"].ToString().ToLower() == "m")
                    {
                        MatchDetails matchDetails = new MatchDetails();
                        matchDetails.series_name = dt.Rows[i]["series_name"].ToString();
                        matchDetails.series_id = dt.Rows[i]["series_id"].ToString();
                        matchDetails.winning_team_name = dt.Rows[i]["winner_name"].ToString();
                        matchDetails.winning_team_id = dt.Rows[i]["winner_id"].ToString();
                        matchDetails.year = dt.Rows[i]["year"].ToString();
                        matchDetails.series_outcome = dt.Rows[i]["series_result"].ToString() == "0" ? "Draw" : "Won";
                        matchDetails.series_status = dt.Rows[i]["series_status"].ToString();

                        gender.Men.Add(matchDetails);
                    }
                    else
                    {
                        MatchDetails matchDetails = new MatchDetails();
                        matchDetails.series_name = dt.Rows[i]["series_name"].ToString();
                        matchDetails.series_id = dt.Rows[i]["series_id"].ToString();
                        matchDetails.winning_team_name = dt.Rows[i]["winner_name"].ToString();
                        matchDetails.winning_team_id = dt.Rows[i]["winner_id"].ToString();
                        matchDetails.year = dt.Rows[i]["year"].ToString();
                        matchDetails.series_outcome = dt.Rows[i]["series_result"].ToString() == "0" ? "Draw" : "Won";
                        matchDetails.series_status = dt.Rows[i]["series_status"].ToString();

                        gender.Women.Add(matchDetails);
                    }
                }

                //string JsonStr = JsonConvert.SerializeObject(rootObject);
                //string FileName = JsonFilePath + "WorldCupWinners.json";
                //File.WriteAllText(FileName, JsonStr);//@"D:\BallbyBall\Data\" + 

                //string processComplete = Environment.NewLine + "==================================" + Environment.NewLine +
                //                            DateTime.Now.ToString() + Environment.NewLine + "Process Completed";
                //File.AppendAllText(LogFileName, processComplete);
                return gender;
            }
            catch (Exception ex)
            {
                string exception = DateTime.Now.ToString() + Environment.NewLine +
                                    "Error in CreateWorldCupFiles(): " + Environment.NewLine + ex.ToString() + Environment.NewLine;
                File.AppendAllText(LogFileName, exception);
                return gender;
            }
        }

        #region Commented Code
        //public void CreateT20WorldCupFiles(DataTable dt)
        //{
        //    try
        //    {
        //        RootObject rootObject = new RootObject();
        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            MatchDetails matchDetails = new MatchDetails();
        //            matchDetails.series_name = dt.Rows[i]["series_name"].ToString();
        //            matchDetails.series_id = dt.Rows[i]["series_id"].ToString();
        //            matchDetails.winning_team_name = dt.Rows[i]["winner_name"].ToString();
        //            matchDetails.winning_team_id = dt.Rows[i]["winner_id"].ToString();
        //            matchDetails.year = dt.Rows[i]["year"].ToString();
        //            matchDetails.series_result = dt.Rows[i]["series_result"].ToString();

        //            rootObject.matchDetails.Add(matchDetails);
        //        }

        //        string JsonStr = JsonConvert.SerializeObject(rootObject);
        //        string FileName = JsonFilePath + "T20WorldCupWinners.json";
        //        File.WriteAllText(FileName, JsonStr);//@"D:\BallbyBall\Data\" + 

        //        string processComplete = Environment.NewLine + "==================================" + Environment.NewLine +
        //                                    DateTime.Now.ToString() + Environment.NewLine + "Process Completed";
        //        File.AppendAllText(LogFileName, processComplete);
        //    }
        //    catch (Exception ex)
        //    {
        //        string exception = DateTime.Now.ToString() + Environment.NewLine +
        //                            "Error in CreateT20WorldCupFiles(): " + Environment.NewLine + ex.ToString() + Environment.NewLine;
        //        File.AppendAllText(LogFileName, exception);
        //    }
        //}

        //public void CreateChampionsTrophyFiles(DataTable dt)
        //{
        //    try
        //    {
        //        RootObject rootObject = new RootObject();
        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            MatchDetails matchDetails = new MatchDetails();
        //            matchDetails.series_name = dt.Rows[i]["series_name"].ToString();
        //            matchDetails.series_id = dt.Rows[i]["series_id"].ToString();
        //            matchDetails.winning_team_name = dt.Rows[i]["winner_name"].ToString();
        //            matchDetails.winning_team_id = dt.Rows[i]["winner_id"].ToString();
        //            matchDetails.year = dt.Rows[i]["year"].ToString();
        //            matchDetails.series_result = dt.Rows[i]["series_result"].ToString();

        //            rootObject.matchDetails.Add(matchDetails);
        //        }

        //        string JsonStr = JsonConvert.SerializeObject(rootObject);
        //        string FileName = JsonFilePath + "ChampionsTrophyWinners.json";
        //        File.WriteAllText(FileName, JsonStr);//@"D:\BallbyBall\Data\" + 

        //        string processComplete = Environment.NewLine + "==================================" + Environment.NewLine +
        //                                    DateTime.Now.ToString() + Environment.NewLine + "Process Completed";
        //        File.AppendAllText(LogFileName, processComplete);
        //    }
        //    catch (Exception ex)
        //    {
        //        string exception = DateTime.Now.ToString() + Environment.NewLine +
        //                            "Error in CreateChampionsTrophyFiles(): " + Environment.NewLine + ex.ToString() + Environment.NewLine;
        //        File.AppendAllText(LogFileName, exception);
        //    }
        //}

        //public void CreateAshesFiles(DataTable dt)
        //{
        //    try
        //    {
        //        RootObject rootObject = new RootObject();
        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            MatchDetails matchDetails = new MatchDetails();
        //            matchDetails.series_name = dt.Rows[i]["series_name"].ToString();
        //            matchDetails.series_id = dt.Rows[i]["series_id"].ToString();
        //            matchDetails.winning_team_name = dt.Rows[i]["winner_name"].ToString();
        //            matchDetails.winning_team_id = dt.Rows[i]["winner_id"].ToString();
        //            matchDetails.year = dt.Rows[i]["year"].ToString();
        //            matchDetails.series_result = dt.Rows[i]["series_result"].ToString();

        //            rootObject.matchDetails.Add(matchDetails);
        //        }

        //        string JsonStr = JsonConvert.SerializeObject(rootObject);
        //        string FileName = JsonFilePath + "AshesWinners.json";
        //        File.WriteAllText(FileName, JsonStr);//@"D:\BallbyBall\Data\" + 

        //        string processComplete = Environment.NewLine + "==================================" + Environment.NewLine +
        //                                    DateTime.Now.ToString() + Environment.NewLine + "Process Completed";
        //        File.AppendAllText(LogFileName, processComplete);
        //    }
        //    catch (Exception ex)
        //    {
        //        string exception = DateTime.Now.ToString() + Environment.NewLine +
        //                            "Error in CreateAshesFiles(): " + Environment.NewLine + ex.ToString() + Environment.NewLine;
        //        File.AppendAllText(LogFileName, exception);
        //    }
        //}

        //public void CreateT20LeaguesFiles(DataTable dt)
        //{
        //    try
        //    {
        //        RootObject rootObject = new RootObject();
        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            MatchDetails matchDetails = new MatchDetails();
        //            matchDetails.series_name = dt.Rows[i]["series_name"].ToString();
        //            matchDetails.series_id = dt.Rows[i]["series_id"].ToString();
        //            matchDetails.winning_team_name = dt.Rows[i]["winner_name"].ToString();
        //            matchDetails.winning_team_id = dt.Rows[i]["winner_id"].ToString();
        //            matchDetails.year = dt.Rows[i]["year"].ToString();
        //            matchDetails.series_result = dt.Rows[i]["series_result"].ToString();

        //            rootObject.matchDetails.Add(matchDetails);
        //        }

        //        string JsonStr = JsonConvert.SerializeObject(rootObject);
        //        string FileName = JsonFilePath + "T20LeagueWinners.json";
        //        File.WriteAllText(FileName, JsonStr);//@"D:\BallbyBall\Data\" + 

        //        string processComplete = Environment.NewLine + "==================================" + Environment.NewLine +
        //                                    DateTime.Now.ToString() + Environment.NewLine + "Process Completed";
        //        File.AppendAllText(LogFileName, processComplete);
        //    }
        //    catch (Exception ex)
        //    {
        //        string exception = DateTime.Now.ToString() + Environment.NewLine +
        //                            "Error in CreateT20LeaguesFiles(): " + Environment.NewLine + ex.ToString() + Environment.NewLine;
        //        File.AppendAllText(LogFileName, exception);
        //    }
        //}

        //public void CreateAsiaCupFiles(DataTable dt)
        //{
        //    try
        //    {
        //        RootObject rootObject = new RootObject();
        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            MatchDetails matchDetails = new MatchDetails();
        //            matchDetails.series_name = dt.Rows[i]["series_name"].ToString();
        //            matchDetails.series_id = dt.Rows[i]["series_id"].ToString();
        //            matchDetails.winning_team_name = dt.Rows[i]["winner_name"].ToString();
        //            matchDetails.winning_team_id = dt.Rows[i]["winner_id"].ToString();
        //            matchDetails.year = dt.Rows[i]["year"].ToString();
        //            matchDetails.series_result = dt.Rows[i]["series_result"].ToString();

        //            rootObject.matchDetails.Add(matchDetails);
        //        }

        //        string JsonStr = JsonConvert.SerializeObject(rootObject);
        //        string FileName = JsonFilePath + "AsiaCupWinners.json";
        //        File.WriteAllText(FileName, JsonStr);//@"D:\BallbyBall\Data\" + 

        //        string processComplete = Environment.NewLine + "==================================" + Environment.NewLine +
        //                                    DateTime.Now.ToString() + Environment.NewLine + "Process Completed";
        //        File.AppendAllText(LogFileName, processComplete);
        //    }
        //    catch (Exception ex)
        //    {
        //        string exception = DateTime.Now.ToString() + Environment.NewLine +
        //                            "Error in CreateAsiaCupFiles(): " + Environment.NewLine + ex.ToString() + Environment.NewLine;
        //        File.AppendAllText(LogFileName, exception);
        //    }
        //}
        #endregion

        protected override void OnStop()
        {
        }
    }
}
