using Cricket.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Linq;

namespace Cricket.Controllers
{
    public class HomeController : Controller
    {
        private CrickDataContext context;
        public HomeController()
        {
            context = new CrickDataContext();
        }

        public FileResult Show(int id)
        {
            //if(id==0)
            //{
            //    return File((byte[])null, "image/jpg");
            //}
            byte[] imageData = (from m in context.TeamDetails
                                where m.TeamId == id
                                select m.Flag).First().ToArray();
            return File(imageData, "image/jpg");
        }

        public ActionResult Create()
        {
            TeamDetailModel tdm = new TeamDetailModel();
            return View(tdm);
        }
        [HttpPost]
        public ActionResult Create(TeamDetailModel tdm, HttpPostedFileBase image1)
        {
            try
            {
                //if (image1 != null)

                tdm.Flag = new byte[image1.ContentLength];
                image1.InputStream.Read(tdm.Flag, 0, image1.ContentLength);
                TeamDetail TM = new TeamDetail()
                {
                    TeamName = tdm.TeamName,
                    Flag = tdm.Flag
                };
                context.TeamDetails.InsertOnSubmit(TM);
                context.SubmitChanges();
                return RedirectToAction("mytest");

            }
            catch
            {
                return RedirectToAction("Create");
            }
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [OutputCache(Duration = 600)]
        public ActionResult mytest()
        {
            TestTeamLeagueModel ttlm = new TestTeamLeagueModel();

            ttlm.testteamdetail = teamdetailmodelfunction();
            ttlm.LeagueCollection = leaguedetailmodelclass();

            // ViewBag.tdlength = tdm.Count();
            return View(ttlm);
        }
        [HttpPost]
        public ActionResult mytest(TestTeamLeagueModel model)
        {
            int? leagueid = model.SelectedLeagueId;
            TempData["leagueid"] = (int)leagueid;
            string selectedtext = model.SelectedLeagueName;

            model.testteamdetail = teamdetailmodelfunction();
            model.LeagueCollection = leaguedetailmodelclass();

            return View(model);
        }

        //[ChildActionOnly]
        public ActionResult Details(int id)
        {
            int leagueselectedid = (int)TempData["leagueid"];
            TempData.Keep();
            var query = from pei in context.PlayerInfos
                        join td in context.TeamDetails
                        on pei.TeamId equals td.TeamId
                        join ld in context.LeagueDetails
                        on pei.LeagueId equals ld.LeagueId
                        join cd in context.CategoryDetails
                        on pei.CategoryId equals cd.CategoryId
                        where pei.TeamId == id && pei.LeagueId == leagueselectedid
                        select new
                        {
                            PlayerName = pei.PlayerName,
                            CategoryName = cd.CategoryName,
                            TeamName = td.TeamName,
                            CategoryId = cd.CategoryId
                        };
            IList<PlayerInfo_League_CategoryModel> pilct = new List<PlayerInfo_League_CategoryModel>();
            foreach (var pi in query)
            {
                pilct.Add(new PlayerInfo_League_CategoryModel()
                {
                    PlayerName = pi.PlayerName,
                    CategoryName = pi.CategoryName,
                    TeamName = pi.TeamName,
                    CategoryId = pi.CategoryId
                });
            }
            ViewBag.leagueselectedname = context.LeagueDetails.Where(x => x.LeagueId == leagueselectedid).Select(x => x.LeagueName).FirstOrDefault();
            ViewBag.teamname = context.TeamDetails.Where(x => x.TeamId == id).Select(x => x.TeamName).FirstOrDefault();
            TempData["teamid"] = id;
            TempData["leagueid"] = leagueselectedid;
            return View(pilct);
        }

        public ActionResult DetailsTest(int id)
        {
            int leagueselectedid = (int)TempData["leagueid"];
            TempData.Keep();
            ViewBag.leagueselectedname = context.LeagueDetails.Where(x => x.LeagueId == leagueselectedid).Select(x => x.LeagueName).FirstOrDefault();
            ViewBag.teamname = context.TeamDetails.Where(x => x.TeamId == id).Select(x => x.TeamName).FirstOrDefault();
            TempData["teamid"] = id;
            TempData["leagueid"] = leagueselectedid;
            ViewBag.TeamId = id;

            var querymainplayerinfo = (from md in context.MatchDetails
                                       join pd in context.PlayerDetails
                                       on md.MatchId equals pd.MatchId
                                       join pirr in context.PlayerInfos
                                       on pd.PlayerId equals pirr.PlayerId
                                       join cd in context.CategoryDetails
                                       on pirr.CategoryId equals cd.CategoryId
                                       where md.LeagueId == leagueselectedid && md.TeamAId == id
                                       group pd by new { pirr.PlayerName, pd.PlayerId, cd.CategoryId }
                                       into grp
                                       select new
                                       {
                                           PlayerName = grp.Key.PlayerName,
                                           PlayerId = grp.Key.PlayerId,
                                           CategoryId = grp.Key.CategoryId,
                                           MatchesPlayed = grp.Select(x => x.MatchId).Count()
                                       }).ToList();
            var querymainrunwickets = (from pd in context.PlayerDetails
                                       join pirr in context.PlayerInfos
                                       on pd.PlayerId equals pirr.PlayerId
                                       where pirr.LeagueId == leagueselectedid && pirr.TeamId == id
                                       group pd by new { pd.PlayerId, pirr.PlayerName }
                                       into grp
                                       select new
                                       {
                                           PlayerId = grp.Key.PlayerId,
                                           PlayerName = grp.Key.PlayerName,
                                           MaxRuns = grp.Select(x => x.PlayerRun).Max(),
                                           AvgSR = grp.Select(x => x.SR).Average(),
                                           MaxWickets = grp.Select(x => x.PlayerWickets).Max(),
                                           AvgER = grp.Select(x => x.ER).Average()
                                       }).ToList();

            var querydistinctplayerid = (from pd in context.PlayerDetails
                                         join pirr in context.PlayerInfos
                                         on pd.PlayerId equals pirr.PlayerId
                                         where pirr.LeagueId == leagueselectedid && pirr.TeamId == id
                                         select new
                                         {
                                             //totalplayers = context.PlayerInfos.Count(c =>
                                             //c.PlayerId == pirr.PlayerId)
                                             totalplayers = pd.PlayerId
                                         }).Distinct().Count();

            var querymainreviewinfo = context.PlayerDetails.Join(context.PlayerInfos,
                pd => pd.PlayerId,
                pirr => pirr.PlayerId,
                (pd, pirr) => new
                {
                    PlayerInfoId = pd.PlayerInfoId,
                    PlayerId = pd.PlayerId,
                    Review = pd.Review,
                    LeagueId = pirr.LeagueId,
                    TeamId = pirr.TeamId
                })
                .Where(x => x.TeamId == id && x.LeagueId == leagueselectedid)
                .OrderByDescending(x => x.PlayerInfoId).Take(querydistinctplayerid);

            var queryfinaldata = (from mainplayerinfo in querymainplayerinfo
                                  join mainreviewinfo in querymainreviewinfo
                                  on mainplayerinfo.PlayerId equals mainreviewinfo.PlayerId
                                  join mainrunswickets in querymainrunwickets
                                  on mainplayerinfo.PlayerId equals mainrunswickets.PlayerId
                                  select new
                                  {
                                      PlayerId = mainplayerinfo.PlayerId,
                                      PlayerName = mainplayerinfo.PlayerName,
                                      MatchesPlayed = mainplayerinfo.MatchesPlayed,
                                      CategoryId = mainplayerinfo.CategoryId,
                                      PlayerInfoId = mainreviewinfo.PlayerInfoId,
                                      MaxRuns = mainrunswickets.MaxRuns,
                                      AvgSR = mainrunswickets.AvgSR,
                                      MaxWickets = mainrunswickets.MaxWickets,
                                      AvgER = mainrunswickets.AvgER,
                                      Review = mainreviewinfo.Review
                                  }).ToList();
            List<DetailModel> dm = new List<DetailModel>();
            foreach (var finaldata in queryfinaldata)
            {
                dm.Add(new DetailModel()
                {
                    PlayerId = (int)finaldata.PlayerId,
                    PlayerName = finaldata.PlayerName,
                    MatchesPlayed = finaldata.MatchesPlayed,
                    CategoryId = finaldata.CategoryId,
                    PlayerInfoId = finaldata.PlayerInfoId,
                    MaxRuns = (int)finaldata.MaxRuns,
                    AvgSR = (decimal)finaldata.AvgSR,
                    MaxWickets = (int)finaldata.MaxWickets,
                    AvgER = (decimal)finaldata.AvgER,
                    Review = finaldata.Review
                });

            }



            return View(dm);
        }

        public ActionResult AddPlayer()
        {
            PlayerInfoModel pim = new PlayerInfoModel();
            pim.CategoryCollection = categorydetailmodelclass();

            return View(pim);
        }
        [HttpPost]
        public ActionResult AddPlayer(PlayerInfoModel model)
        {
            int teamiddata = (int)TempData["teamid"];
            TempData.Keep();
            int leagueiddata = (int)TempData["leagueid"];
            TempData.Keep();
            
            PlayerInfo pio;
            try
            {
                pio = new PlayerInfo()
                {
                    TeamId = teamiddata,
                    PlayerName = model.PlayerName,
                    LeagueId = leagueiddata,
                    CategoryId = model.SelectedCategoryId
                };
                context.PlayerInfos.InsertOnSubmit(pio);
                context.SubmitChanges();

                //-----------------------------------------------------------
                //MatchDetail md = new MatchDetail()
                //{
                //    TeamAId = teamiddata,
                //    TeamBId = null,
                //    Venue = null,
                //    MatchDate = null,
                //    LeagueId = leagueiddata
                //};
                //context.MatchDetails.InsertOnSubmit(md);
                //context.SubmitChanges();

                //var recentplayerid = context.PlayerInfos.Where(x=>x.PlayerName==model.PlayerName)
                //    .Select(x => x.PlayerId).FirstOrDefault();
                //var recentmatchid = context.MatchDetails.OrderByDescending(x => x.MatchId).Select(x => x.MatchId).Take(1).FirstOrDefault();
                //PlayerDetail pd = new PlayerDetail()
                //{
                //    PlayerId = recentplayerid,
                //    MatchId = recentmatchid,
                //    CategoryId = model.SelectedCategoryId,
                //    PlayerRun = null,
                //    SR = null,
                //    PlayerWickets = null,
                //    Overs = null,
                //    ER = null,
                //    Review = null
                //};
                        
                //context.PlayerDetails.InsertOnSubmit(pd);
                //context.SubmitChanges();
                //-----------------------------------------------------------

                return RedirectToAction("DetailsTest", new { id = teamiddata });
            }
            catch
            {
                return View(model);
            }
        }

        public ActionResult AddMatch()
        {
            MatchDetail_PlayerDetail_Model mdpdm = new MatchDetail_PlayerDetail_Model();
            mdpdm.TeamCollection = TeamCollectionModelFunction();
            mdpdm.PlayerCollection = PlayerCollectionModelFunction();
            mdpdm.PlayerScoringCollection = PlayerScoringCollectionModelFunction();

            return View(mdpdm);
        }
        [HttpPost]
        public ActionResult AddMatch(MatchDetail_PlayerDetail_Model model)
        {
            int teamiddata = (int)TempData["teamid"];
            TempData.Keep();
            int leagueiddata = (int)TempData["leagueid"];
            TempData.Keep();

            try
            {
                MatchDetail md = new MatchDetail()
                {
                    TeamAId = teamiddata,
                    TeamBId = model.SelectedTeamId,
                    Venue = model.Venue,
                    MatchDate = DateTime.Now,
                    LeagueId = leagueiddata
                };
                context.MatchDetails.InsertOnSubmit(md);
                context.SubmitChanges();

                var recentmatchid = context.MatchDetails.OrderByDescending(x => x.MatchId).Select(x => x.MatchId).Take(1).FirstOrDefault();

                IList<PlayerDetail> Ipd = new List<PlayerDetail>();

                foreach (var listmodel in model.PlayerScoringCollection)
                {
                    Ipd.Add(new PlayerDetail()
                    {
                        PlayerId = listmodel.PlayerId,
                        MatchId = recentmatchid,
                        CategoryId = listmodel.CategoryId,
                        PlayerRun = listmodel.PlayerRun,
                        SR = listmodel.SR,
                        PlayerWickets = listmodel.PlayerWicket,
                        Overs = listmodel.Overs,
                        ER = listmodel.ER,
                        Review = listmodel.Review
                    });
                }

                context.PlayerDetails.InsertAllOnSubmit(Ipd);
                context.SubmitChanges();

                return RedirectToAction("DetailsTest", new { id = teamiddata });
            }
            catch
            {
                return View(model);
            }

        }

        public ActionResult ShowPlayer(int playerid, int categoryid)
        {
            var queryresult = (from pd in context.PlayerDetails
                               join pirr in context.PlayerInfos
                               on pd.PlayerId equals pirr.PlayerId
                               join cd in context.CategoryDetails
                               on pirr.CategoryId equals cd.CategoryId
                               join md in context.MatchDetails
                               on pd.MatchId equals md.MatchId
                               join td in context.TeamDetails
                               on md.TeamBId equals td.TeamId
                               where pd.PlayerId == playerid
                               select new ShowPartialPlayer
                               {
                                   PlayerId = (int)pd.PlayerId,
                                   PlayerName = pirr.PlayerName,
                                   PlayerRun = (int)pd.PlayerRun,
                                   PlayerWickets = (int)pd.PlayerWickets,
                                   MatchId = (int)pd.MatchId,
                                   TeamName = td.TeamName,
                                   CategoryId = cd.CategoryId,
                                   CategoryName = cd.CategoryName
                               });
            ViewBag.PlayerName = context.PlayerInfos.Where(x => x.PlayerId == playerid).Select(x => x.PlayerName).FirstOrDefault();
            ViewBag.CategoryName = context.CategoryDetails.Where(x => x.CategoryId == categoryid).Select(x => x.CategoryName).FirstOrDefault();
            ViewBag.CategoryId = categoryid;
            if (categoryid == 1 || categoryid == 4)
            {
                var queryshowplayer = queryresult.OrderByDescending(x => x.PlayerRun).Take(3);
                return PartialView(queryshowplayer);
            }
            else if (categoryid == 3)
            {

                var queryshowplayer = queryresult.OrderByDescending(x => x.PlayerWickets).Take(3);
                return PartialView(queryshowplayer);
            }
            else
            {

                var queryshowplayer = queryresult.OrderByDescending(x => x.PlayerRun)
                    .OrderByDescending(x=>x.PlayerWickets).Take(3);
                return PartialView(queryshowplayer);
            }


        }

        public ActionResult RecordView()
        {
            int teamiddata = (int)TempData["teamid"];
            TempData.Keep();
            int leagueiddata = (int)TempData["leagueid"];
            TempData.Keep();
            MatchPlayerRecordDisplay mprd = new MatchPlayerRecordDisplay();
            mprd.PlayerDetailModel = PlayerDetailTestModelCollectionFunction();
            mprd.PlyerInfoModel = PlayerInfroModelCollectionFunction();
            mprd.MatchDetailModel = MatchDetailModelCollectionFunction();
            var querymatchdetailid = context.MatchDetails.Where(x => x.TeamAId == teamiddata && x.LeagueId == leagueiddata)
                .OrderByDescending(x => x.MatchId).Select(x => x.MatchId).Take(2);
            ViewBag.matchids = querymatchdetailid;
            return View(mprd);
        }

        public ActionResult RecordViewTest(int id)
        {
            //int teamiddata = (int)TempData["teamid"];
            //TempData.Keep();
            int teamiddata = id;
            int leagueiddata = (int)TempData["leagueid"];
            TempData.Keep();
            MatchPlayerRecordDisplay mprd = new MatchPlayerRecordDisplay();
            mprd.PlayerDetailModel = PlayerDetailTestModelCollectionFunction();
            mprd.PlyerInfoModel = PlayerInfroModelCollectionFunction();
            mprd.MatchDetailModel = MatchDetailModelCollectionFunction();
            var querymatchdetailid = context.MatchDetails.Where(x => x.TeamAId == teamiddata && x.LeagueId == leagueiddata)
                .OrderByDescending(x => x.MatchId).Select(x => x.MatchId).Take(3);
            //int matchid = querymatchdetailid.First();
            //ViewBag.matchids = querymatchdetailid;
            List<int> testmatchid = new List<int>();
           foreach(var matchdetailid in querymatchdetailid)
            {
                testmatchid.Add(matchdetailid); 
            }
            ViewBag.matchids = testmatchid;
            return View(mprd);
        }

        public ActionResult AddTest()
        {
            
            MatchDetail_PlayerDetail_Model mdpdm = new MatchDetail_PlayerDetail_Model();
            mdpdm.TeamCollection = TeamCollectionModelFunction();
            
            mdpdm.PlayerScoringCollection = PlayerScoringCollectionModelFunction();

            return View(mdpdm);
        }

        [OutputCache(Duration =600)]
        public List<TeamDetailModel> teamdetailmodelfunction()
        {
            var query = (from td in context.TeamDetails
                         select td).ToArray();
            List<TeamDetailModel> tdm = new List<TeamDetailModel>();

            foreach (var teamdetailmodel in query)
            {
                tdm.Add(new TeamDetailModel()
                {
                    TeamId = teamdetailmodel.TeamId,
                    TeamName = teamdetailmodel.TeamName,
                    Flag = (byte[])teamdetailmodel.Flag.ToArray()
                });

            }

            return tdm;
        }

        [OutputCache(Duration =600)]
        public List<LeagueDetailModel> leaguedetailmodelclass()
        {
            var query1 = (from ld in context.LeagueDetails
                          select ld).ToList();
            List<LeagueDetailModel> ldm = new List<LeagueDetailModel>();
            foreach (var leaguedetailmodel in query1)
            {
                ldm.Add(new LeagueDetailModel()
                {
                    LeagueId = leaguedetailmodel.LeagueId,
                    LeagueName = leaguedetailmodel.LeagueName
                });
            }
            return ldm;
        }
        public List<CategoryDetailModel> categorydetailmodelclass()
        {
            var query2 = (from cd in context.CategoryDetails
                          select cd).ToList();
            List<CategoryDetailModel> cdm = new List<CategoryDetailModel>();
            foreach (var categorydetailmodel in query2)
            {
                cdm.Add(new CategoryDetailModel()
                {
                    CategoryId = categorydetailmodel.CategoryId,
                    CategoryName = categorydetailmodel.CategoryName
                });
            }
            return cdm;
        }
        public IList<PlayerScoringRecordsModel> PlayerScoringCollectionModelFunction()
        {
            int teamiddata = (int)TempData["teamid"];
            TempData.Keep();
            int leagueiddata = (int)TempData["leagueid"];
            TempData.Keep();
            var queryplayercollection = (from pi in context.PlayerInfos
                                         where pi.TeamId == teamiddata && pi.LeagueId == leagueiddata
                                         select new
                                         {
                                             pi.PlayerId,
                                             pi.PlayerName,
                                             pi.CategoryId

                                         }).ToList();

            List<PlayerScoringRecordsModel> psrm = new List<PlayerScoringRecordsModel>();
            foreach (var playercollection in queryplayercollection)
            {
                psrm.Add(new PlayerScoringRecordsModel()
                {
                    PlayerId = playercollection.PlayerId,
                    PlayerName = playercollection.PlayerName,
                    CategoryId= (int)playercollection.CategoryId
                });
            }
            return psrm;

        }

        public List<PlayerNameIdModel> PlayerCollectionModelFunction()
        {
            int teamiddata= (int)TempData["teamid"];
            int leagueiddata = (int)TempData["leagueid"];

            var queryplayercollection = (from pi in context.PlayerInfos
                                         where pi.TeamId==teamiddata && pi.LeagueId== leagueiddata
                                         select new
                                         {
                                             pi.PlayerId,
                                             pi.PlayerName

                                         }).ToList();

            List<PlayerNameIdModel> pnim = new List<PlayerNameIdModel>();
            foreach(var playercollection in queryplayercollection)
            {
                pnim.Add(new PlayerNameIdModel()
                {
                    PlayerId = playercollection.PlayerId,
                    PlayerName = playercollection.PlayerName
                });
            }
            return pnim;

        }
        public List<TeamDetailModel> TeamCollectionModelFunction()
        {
            var queryteamcollection = (from tc in context.TeamDetails
                                         select new
                                         {
                                             tc.TeamId,
                                             tc.TeamName
                                         }).ToList();

            List<TeamDetailModel> tdm = new List<TeamDetailModel>();

            foreach (var playercollection in queryteamcollection)
            {
                tdm.Add(new TeamDetailModel()
                {
                    TeamId = playercollection.TeamId,
                    TeamName = playercollection.TeamName
                });
            }
            return tdm;
        }

        public List<PlayerDetailTestModel> PlayerDetailTestModelCollectionFunction()
        {
            int teamiddata = (int)TempData["teamid"];
            TempData.Keep();
            int leagueiddata = (int)TempData["leagueid"];
            TempData.Keep();

            var queryplayeridcollection = context.PlayerInfos.Where(x => x.TeamId == teamiddata && x.LeagueId == leagueiddata)
                .Select(x => x.PlayerId).ToList();
            var querymatchidcollection = context.MatchDetails.Where(x => x.TeamAId == teamiddata && x.LeagueId == leagueiddata)
                .OrderByDescending(x => x.MatchId).Select(x => x.MatchId).Take(3);

            var queryplayercollection = (from pd in context.PlayerDetails
                                    join pir in context.PlayerInfos
                                    on pd.PlayerId equals pir.PlayerId
                                    join md in context.MatchDetails
                                    on pd.MatchId equals md.MatchId
                                    join tad in context.TeamDetails
                                    on md.TeamAId equals tad.TeamId
                                    join tbd in context.TeamDetails
                                    on md.TeamBId equals tbd.TeamId
                                    join cd in context.CategoryDetails
                                    on pd.CategoryId equals cd.CategoryId
                                    where queryplayeridcollection.Contains(pir.PlayerId)
                                    where querymatchidcollection.Contains(md.MatchId)
                                    orderby pd.MatchId descending
                                         select new
                                    {
                                        PlayerInfoId=pd.PlayerInfoId,
                                        PlayerId=pir.PlayerId,
                                        PlayerName=pir.PlayerName,
                                        MatchdId=md.MatchId,
                                        TeamAId=md.TeamAId,
                                        TeamAName = tad.TeamName,
                                        TeamBId =md.TeamBId,
                                        TeamBName=tbd.TeamName,
                                        Venue=md.Venue,
                                        MatchDate=md.MatchDate, 
                                        PlayerRun=pd.PlayerRun,
                                        SR=pd.SR,
                                        PlayerWickets=pd.PlayerWickets,
                                        ER=pd.ER,
                                        Overs=pd.Overs,
                                        CategoryId=cd.CategoryId,
                                        CategoryName=cd.CategoryName
                                    }).ToList();

            List<PlayerDetailTestModel> tdtm = new List<PlayerDetailTestModel>();

            foreach(var playercollection in queryplayercollection)
            {
                tdtm.Add(new PlayerDetailTestModel()
                {
                    PlayerInfoId= playercollection.PlayerInfoId,
                    PlayerId=playercollection.PlayerId,
                    PlayerName=playercollection.PlayerName,
                    MatchId=playercollection.MatchdId,
                    TeamAId=(int)playercollection.TeamAId,
                    TeamAName=playercollection.TeamAName,
                    TeamBId=(int)playercollection.TeamBId,
                    TeamBName=playercollection.TeamBName,
                    Venue=playercollection.Venue,
                    MatchDate=(DateTime)playercollection.MatchDate,
                    PlayerRun=(int)playercollection.PlayerRun,
                    SR=(Decimal)playercollection.SR,
                    PlayerWickets=(int)playercollection.PlayerWickets,
                    ER=(Decimal)playercollection.ER,
                    Overs=(int)playercollection.Overs,
                    CategoryId=playercollection.CategoryId,
                    CategoryName=playercollection.CategoryName
                });
            }
            return tdtm;
        }

        public List<PlayerInfoTestModel> PlayerInfroModelCollectionFunction()
        {
            int teamiddata = (int)TempData["teamid"];
            TempData.Keep();
            int leagueiddata = (int)TempData["leagueid"];
            TempData.Keep();

            var queryplayerinfocollection = (from pirr in context.PlayerInfos                               
                                         where pirr.TeamId == teamiddata && pirr.LeagueId == leagueiddata
                                         select pirr).ToList();
            List<PlayerInfoTestModel> pim = new List<PlayerInfoTestModel>();

            foreach(var playerinfocollection in queryplayerinfocollection)
            {
                pim.Add(new PlayerInfoTestModel()
                {
                    PlayerId=playerinfocollection.PlayerId,
                    TeamId=(int)playerinfocollection.TeamId,
                    PlayerName=playerinfocollection.PlayerName,
                    Leagueid=(int)playerinfocollection.LeagueId,
                    CategoryId=(int)playerinfocollection.CategoryId
                });
            }

            return pim;
        }

        public List<MatchDetailModel> MatchDetailModelCollectionFunction()
        {
            int teamiddata = (int)TempData["teamid"];
            TempData.Keep();
            int leagueiddata = (int)TempData["leagueid"];
            TempData.Keep();

            var querymatchdetailcollection = context.MatchDetails.Join(context.TeamDetails,
                md=>md.TeamBId,
                td=>td.TeamId,
                (md, td)=> new
                {
                    TeamAId = md.TeamAId,
                    TeamBId=md.TeamBId,
                    TeamBName=td.TeamName,
                    MatchId=md.MatchId,
                    Venue=md.Venue,
                    MatchDate=md.MatchDate,
                    LeagueId=md.LeagueId
                })
                .Where(x => x.TeamAId == teamiddata && x.LeagueId == leagueiddata)
                .OrderByDescending(x => x.MatchId).Take(3);
            
            
            List<MatchDetailModel> mdm = new List<MatchDetailModel>();

            foreach (var matchdetailcollection in querymatchdetailcollection)
            {
                mdm.Add(new MatchDetailModel()
                {
                    MatchId=matchdetailcollection.MatchId,
                    TeamAId= (int)matchdetailcollection.TeamAId,
                    TeamBId= (int)matchdetailcollection.TeamBId,
                    Venue=matchdetailcollection.Venue,
                    MatchDate=(DateTime)matchdetailcollection.MatchDate,
                    LeagueId=(int)matchdetailcollection.LeagueId,
                    TeamBName=matchdetailcollection.TeamBName
                });
            }

            return mdm;
        }

        

    }
}