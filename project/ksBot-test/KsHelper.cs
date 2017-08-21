using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore.Internal;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using HtmlAgilityPack;
using ConsoleTables;
using static K8Director.Program;
using K8Director.Models;

namespace K8Director
{
    class KsHelper
    {
        private ksBotSQLContext db = new ksBotSQLContext();
        //---------------------------------------------------------------------------------------!attend <KAUPUNKI>                             TODO V2

        //---------------------------------------------------------------------------------------!me (list string)                              VALMIS V1
        public async Task<List<string>> Me(ulong i, string name)
        {
            //Update !me stats (CurrentSR,Stats) before running !me query
            using (var dbupd = new ksBotSQLContext())
            {
                var discordId = i.ToString().Trim();
                var resultStatUpdate = dbupd.User.SingleOrDefault(b => b.DiscordId == discordId);
                if (resultStatUpdate != null)
                {
                    if (resultStatUpdate.BtagId != null)
                    {
                        await OWstats(discordId, resultStatUpdate.BtagId); //Add API update to !me command
                    } 
                }
              
            }

            //run !me query
            var li = new List<string>();
            using (var db = new ksBotSQLContext())
            {
                var namehold = "";
                var discordId = i.ToString().Trim();
                namehold = name;
                
                //Placeholderit joita käytetään mikäli muuta tietoa ei saatavilla.
                var retBtag = "-";
                var retRating = "-";
                var retAPIrating = "-";
                var retTeam = "-";
                var retCity = "-";
                var retKav = "-";
                var retDav = "-";
                var retHav = "-";

                var retTime = "-";
                var retWr = "-";
                var retAvatar = "https://blzgdapipro-a.akamaihd.net/game/unlocks/0x0250000000000D58.png";
                


                var result = db.User.SingleOrDefault(b => b.DiscordId == discordId);
                
                if (result != null)
                    {
                        var resultT = db.Team.SingleOrDefault(a => a.Id == result.TeamId);

                    if (result.Username != namehold) //If username is changed, update it to DB. (New feature June 2017)
                    {
                        result.Username = namehold;
                        await db.SaveChangesAsync();
                    }
                    //
                    if (result.BtagId != null)
                    {
                        retBtag = result.BtagId;
                    }
                    //
                    if (result.CurrentSr != null)
                    {
                        retRating = result.CurrentSr.ToString();
                    }
                    // 
                    if (result.ApicurrentSr != null)
                    {
                        retAPIrating = result.ApicurrentSr.ToString();
                    }
                    // 
                    if (result.ApideathAvg != null)
                    {
                        retDav = result.ApideathAvg.ToString();
                    }
                    // 
                    if (result.ApihealAvg != null)
                    {
                        retHav = result.ApihealAvg.ToString();
                    }
                    // 
                    if (result.ApikillAvg != null)
                    {
                        retKav = result.ApikillAvg.ToString();
                    }
                    //
                    if (result.ApitimePlayed != null)
                    {
                        retTime = result.ApitimePlayed.ToString();
                    }
                    //
                    if (result.ApiwinRate != null)
                    {
                        retWr = result.ApiwinRate.ToString();
                    }
                    //
                    if (result.ApiavatarUrl != null)
                    {
                        retAvatar = result.ApiavatarUrl.ToString();
                    }
                    // 
                    if (resultT != null)
                    {
                        if (resultT.TeamName != null)
                        {
                            retTeam = resultT.TeamName;
                        }
                        // 
                        if (resultT.CityName != null)
                        {
                            retCity = resultT.CityName;
                        }
                    }
                    
                    li.Add(retBtag);
                    li.Add(retRating);
                    li.Add(retAPIrating);
                    li.Add(retTeam);
                    li.Add(retCity);
                    li.Add(retKav);
                    li.Add(retDav);
                    li.Add(retHav);

                    li.Add(retWr);
                    li.Add(retTime);
                    li.Add(retAvatar);

                    return li;
                }
                else
                {
                    //Luo käyttäjärivi kantaan, jonka jälkeen sama kuin yllä.
                    var cu = await Declareuser(discordId, namehold);
                    if (cu == true)
                    {
                        var result2 = db.User.SingleOrDefault(b => b.DiscordId == discordId);
                        if (result2 != null)
                        {
                            var result2T = db.Team.SingleOrDefault(a => a.Id == result2.TeamId);
                            //
                            if (result2.BtagId != null)
                            {
                                retBtag = result2.BtagId;
                            }
                            //
                            if (result2.CurrentSr != null)
                            {
                                retRating = result2.CurrentSr.ToString();
                            }
                            // 
                            if (result2.ApicurrentSr != null)
                            {
                                retAPIrating = result2.ApicurrentSr.ToString();
                            }
                            if(result2T != null)
                            {
                                // 
                                if (result2T.TeamName != null)
                                {
                                    retTeam = result2T.TeamName;
                                }
                                // 
                                if (result2T.CityName != null)
                                {
                                    retCity = result2T.CityName;
                                }
                            }

                            li.Add(retBtag);
                            li.Add(retRating);
                            li.Add(retAPIrating);
                            li.Add(retTeam);
                            li.Add(retCity);
                            li.Add(retKav);
                            li.Add(retDav);
                            li.Add(retHav);

                            return li;
                        }
                        else
                        {
                            return li;
                        }
                    }
                }
                return li;
            }

        }
        //---------------------------------------------------------------------------------------!teams                                         VALMIS V1 (Confirm?)
        public async Task<string> Teamslist()
        {
            using (var db = new ksBotSQLContext())
            {
                var result = db.Team
                    .OrderBy(x => x.Division)
                    .ThenByDescending(x => x.TeamName)
                    .ToList();

                var txt = "0";
                var consoletable = new ConsoleTable(ProgHelpers.txt118, ProgHelpers.txt119, ProgHelpers.txt120);
                if (result.Count > 0)
                {
                    for (var iz = 0; iz < result.Count; iz++)
                    {
                        consoletable.AddRow(result[iz].Division, result[iz].CityName, result[iz].TeamName);
                    }
                    txt = consoletable.ToStringAlternative();
                }
                return txt;
                

            }
        }
        //---------------------------------------------------------------------------------------!btag <BATTLETAG#1234>                         VALMIS V1
        public async Task<int> Btag(ulong i, string st, string name)
        {
            using (var db = new ksBotSQLContext())
            {
                var battleTag = "";
                var namehold = "";
                var discordId = i.ToString().Trim();
                namehold = name;
                battleTag = st;
                var result = db.User.SingleOrDefault(b => b.DiscordId == discordId);
                if (result != null)
                {
                   
                    var goAPI = await OWstats(discordId,battleTag);
                    if (goAPI == 1)
                    {
                        //update db because the status is ok
                        result.BtagId = battleTag;
                        result.Username = namehold;
                        await db.SaveChangesAsync();

                        return 1;
                    }
                    if (goAPI == 0)
                    {
                        //update db because the status is "unsure"
                        result.BtagId = battleTag;
                        result.Username = namehold;
                        await db.SaveChangesAsync();

                        return 0;
                    }
                    if (goAPI == 3)
                    {
                        //return without updating
                        return 3;
                    }
                    return 1;  
                }
                else
                {
                    //Luo käyttäjärivi kantaan, jonka jälkeen sama kuin yllä.
                    var cu = await Declareuser(discordId,namehold);
                    if (cu == true)
                    {
                        var result2 = db.User.SingleOrDefault(b => b.DiscordId == discordId);
                        if (result2 != null)
                        {
                            
                            var goAPI = await OWstats(discordId, battleTag);
                            if (goAPI == 1)
                            {
                                //update db because the status is ok
                                result2.BtagId = battleTag;
                                result2.Username = namehold;
                                await db.SaveChangesAsync();

                                return 1;
                            }
                            if (goAPI == 0)
                            {
                                //update db because the status is "unsure"
                                result2.BtagId = battleTag;
                                result2.Username = namehold;
                                await db.SaveChangesAsync();

                                return 0;
                            }
                            if (goAPI == 3)
                            {
                                //return without updating
                                return 3;
                            }
                            return 1;
                        }
                        else
                        {
                            return 0;
                        }
                    }
                }
                return 0;
            }
        }
        //---------------------------------------------------------------------------------------!rating <ARVO>                                 VALMIS V1
        public async Task<bool> Rating(ulong i, int it, string name)
        {
            using (var db = new ksBotSQLContext())
            {
                var currentSR = 0;
                var namehold = "";
                var discordId = i.ToString().Trim();
                currentSR = it;
                namehold = name;
                var result = db.User.SingleOrDefault(b => b.DiscordId == discordId);
                if (result != null)
                {
                    result.CurrentSr = currentSR;
                    result.Username = namehold;
                    await db.SaveChangesAsync();
                    return true;
                }
                else
                {
                    //Luo käyttäjärivi kantaan, jonka jälkeen sama kuin yllä.
                    var cu = await Declareuser(discordId,namehold);
                    if (cu == true)
                    {
                        var result2 = db.User.SingleOrDefault(b => b.DiscordId == discordId);
                        if (result2 != null)
                        {
                            result2.CurrentSr = currentSR;
                            result2.Username = namehold;
                            await db.SaveChangesAsync();
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                return false;
            }
        }
        //---------------------------------------------------------------------------------------!roster <JOUKKUE>                              VALMIS V1
        public async Task<List<string>> Roster(ulong i, string st)
        {
            var discordId = i.ToString().Trim();
            var queryName = st;
            var li = new List<string>();

            //muuttujat
            var retCity = "-";
            var retTeamName = "-";
            var retCaptain = "-";
            var returnList = new List<string>();
            //helpermuuttujat
            var captainsearch = (int?)0;
            var playerteamidsearch = (int?)0;
            

            using (var db = new ksBotSQLContext())
            {
                var result = db.Team.SingleOrDefault(b => b.CityName == queryName);
                if (result != null)
                {
                    if(result.CaptainUserId != null)
                    {
                        captainsearch = result.CaptainUserId;
                        var result2 = db.User.SingleOrDefault(c => c.Id == captainsearch);
                        if(result2 != null)
                        {
                            retCaptain = result2.Username + " ("+result2.BtagId+")";
                        }
                    }
                    if(result.CityName != null)
                    {
                        retCity = result.CityName;
                    }
                    if(result.TeamName != null)
                    {
                       if (result.TeamName == "")
                        {
                            retTeamName = "-";
                        }
                        else
                        {
                            retTeamName = result.TeamName;
                        }
                    }

                    li.Add(retCity);
                    li.Add(retTeamName);
                    li.Add(retCaptain);

                    playerteamidsearch = result.Id;
                    using (var context = new ksBotSQLContext())
                    {
                        var playerlist = context.User
                            .Where(d => d.TeamId == playerteamidsearch)
                            .Select(row => new { row.Username, row.BtagId })
                            .ToList();

                        var playerlistcomb = new List<string>();
                            for (var iz = 0; iz < playerlist.Count; iz++)
                        {
                            playerlistcomb.Add(playerlist[iz].Username+" - "+playerlist[iz].BtagId);
                        }
                        li.AddRange(playerlistcomb);
                    }

                    return li;
                    
                }
                return li;

            }
        }
        //---------------------------------------------------------------------------------------!teamname <NIMI>                               VALMIS V1
        public async Task<bool> Teamname(ulong i, string st)
        {
            using (var db = new ksBotSQLContext())
            {
                var namehold = "";
                var discordId = i.ToString().Trim();
                namehold = st;

                var result = db.User.SingleOrDefault(b => b.DiscordId == discordId);
                if (result != null)
                {
                    var uidSearch = result.Id;
                    var result2 = db.Team.SingleOrDefault(b => b.CaptainUserId == uidSearch);
                    if (result2 != null)
                    {
                        result2.TeamName = namehold;
                        await db.SaveChangesAsync();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }
        //---------------------------------------------------------------------------------------!scoreboard                                    VALMIS V2 
        public async Task<string> Standings() //Task<List<string>>
        {
            var dblist = db.Scoreboard
                .OrderBy(x => x.Division)
                .ThenByDescending(x => x.StdPoints)
                .ToList();

            //var returnlist = new List<string>();
            var txtv2 = "";
            var consoletable = new ConsoleTable(ProgHelpers.txt109, ProgHelpers.txt110, ProgHelpers.txt111, ProgHelpers.txt112, ProgHelpers.txt113, ProgHelpers.txt114, ProgHelpers.txt115);
            if (dblist.Count > 0)
            {
                for (var iz = 0; iz < dblist.Count; iz++)
                {
                    consoletable.AddRow(dblist[iz].CityName, dblist[iz].Division.ToString(), dblist[iz].StdPoints.ToString(), dblist[iz].GamesPlayed.ToString(), dblist[iz].RndWon.ToString(), dblist[iz].RndLose.ToString(), ((int)dblist[iz].RndWon - (int)dblist[iz].RndLose).ToString("+#;-#;+-0"));
                }
            }
            
            //consoletable.Write(Format.Alternative);
            //Console.WriteLine();
            txtv2 = consoletable.ToStringAlternative();
            //Console.WriteLine(txtv2);

            return txtv2;


            //for (var iz = 0; iz < dblist.Count; iz++)
            //{
            //    returnlist.Add(dblist[iz].CityName+"\t\t" + dblist[iz].Division.ToString()+ "\t\t" + dblist[iz].StdPoints.ToString() + "\t\t" + dblist[iz].GamesPlayed.ToString() + "\t\t" + dblist[iz].RndWon.ToString() + "\t\t" + dblist[iz].RndLose.ToString() + "\t\t" + ((int)dblist[iz].RndWon - (int)dblist[iz].RndLose).ToString("+#;-#;+-0"));
            //    //returnlist.Add(dblist[iz].CityName.PadRight(25) + dblist[iz].StdPoints.ToString().PadRight(10) + "------" + dblist[iz].GamesPlayed.ToString().PadRight(5) + " | " + dblist[iz].RndWon.ToString().PadRight(5) + " | " + dblist[iz].RndLose.ToString().PadRight(5) + " | " + (dblist[iz].RndWon - dblist[iz].RndLose).ToString().PadRight(5) + " |");
            //}

            //return returnlist;
        }

        //#kapteenit kanavan komennot
        //TODO: Taskien palautusformaatit, taskit.

        //---------------------------------------------------------------------------------------!showtime <UID, DATETIME>                      VALMIS V1 (Confirm?)
        public async Task<int> Showtime(ulong i, string aa, DateTime atime)
        {
            using (var db = new ksBotSQLContext())
            {
                var discordId = i.ToString().Trim();
                var code = aa;
                var gotime = atime;
                var uId = 0; //pyytäjän userid
                var utId = 0; //pyytäjän teamid
                var mId = 0; //target pelin id
                var mTid = 0; //target pelin teamid

                var resUser = db.User.SingleOrDefault(b => b.DiscordId == discordId);
                if (resUser != null)
                {
                    uId = resUser.Id;
                    utId = (int)resUser.TeamId;

                    var resMatch = db.Match.SingleOrDefault(b => b.Matchcode == code);
                    if (resMatch != null)
                    {
                        mId = resMatch.Id;
                        mTid = (int)resMatch.Team1Id;

                        var resTeam = db.Team.SingleOrDefault(b => b.Id == mTid);
                        if (resTeam != null)
                        {
                            if(resTeam.CaptainUserId == uId)
                            {
                                if(resMatch.Started == true)
                                {
                                    //käynnistetty jo
                                    return 5;
                                }
                                else
                                {
                                    if (resMatch.Projectedstarttime == null)
                                    {
                                        resMatch.Projectedstarttime = gotime;
                                        resMatch.Modified = DateTime.Now;
                                        await db.SaveChangesAsync();
                                        return 1;
                                    }
                                    else
                                    {
                                        DateTime dbdatetime = (DateTime)resMatch.Projectedstarttime;
                                        var hours = (dbdatetime - DateTime.Now).TotalHours;
                                        Console.WriteLine("Comparing Hours to move match: " + hours + ((gotime - dbdatetime).TotalHours));
                                        if (hours < ProgHelpers.postponelimithours)
                                        {
                                            Console.WriteLine("Too near the original starting time, can't move anymore! --- " + DateTime.Now.ToString());
                                            return 3;
                                        }
                                        else
                                        {
                                            Console.WriteLine("Moving starting time! ---" + DateTime.Now.ToString());
                                            resMatch.Projectedstarttime = gotime;
                                            resMatch.Modified = DateTime.Now;
                                            await db.SaveChangesAsync();
                                            return 1;
                                        }
                                        //ei ole null, tarkista
                                    }
                                }
                            }
                            else
                            {
                                return 4;
                                //ei kapu
                            }
                        }
                        else
                        {
                            return 4;
                            //et ole kotijoukkueesta
                        }
                    }
                    else
                    {
                        return 2;
                        //matsia ei löydy
                    }
                }
                else
                {
                    return 4;
                    //käyttäjää ei löydy
                }

                //tarkista user

                //tarkista joukkue
                //tarkista kapteeni
                //tarkista matchid & kapteeni

                //tarkista onko showtime olemassa
                //jos on, onko alle limitin 

            }
        }
        //---------------------------------------------------------------------------------------!start <MATCHID>                               VALMIS V2 Updated 06/2017
        public async Task<int> Start(ulong i, string st)
        {
            using (var db = new ksBotSQLContext())
            {
                var discordId = i.ToString().Trim();
                var code = "";
                code = st;
                var mt1id = 0; //joukkue1 id
                var mt2id = 0; //joukkue2 id
                var uId = 0; //pyytäjän userid
                var utId = 0; //pyytäjän teamid
                var mId = 0; //target pelin id
                var t1cap = 0; //joukkue1 cap id
                var t2cap = 0; //joukkue2 cap id
                var drurl1 = ""; //drafturls
                var drurl2 = ""; //drafturls
                var resultM = db.Match.SingleOrDefault(b => b.Matchcode == code);
                var resultU = db.User.SingleOrDefault(b => b.DiscordId == discordId);
                if (resultM != null)
                {
                    if (resultU != null)
                    {
                        uId = resultU.Id;
                        if (resultU.TeamId != null)
                        {
                            utId = (int)resultU.TeamId;
                            var resultT1 = db.Team.SingleOrDefault(b => b.CaptainUserId == uId); 
                            if (resultT1 != null)
                            {
                                mt1id = resultT1.Id;
                                mt2id = (int)resultM.Team2Id;
                                t1cap = uId;

                                //Update 09.06.2017 - Tarkista onko useampia pelejä auki
                                var matchlistdouble = db.Match
                                    .Where(d => d.Team1Id == mt1id)
                                    .Where(d => d.Started == true)
                                    .Where(d => d.Closed == false)
                                    .FirstOrDefault();
                                if (matchlistdouble != null)
                                {
                                    return 9; // Joukkueella on käynnissä vielä edellinen ottelu!
                                }//End Update 09.06.2017

                                if (resultM.Team1Id == mt1id)
                                {
                                    mId = resultM.Id;
                                    var resultT2 = db.Team.SingleOrDefault(b => b.Id == mt2id);
                                    if (resultT2 != null)
                                    {
                                        if (resultT2.CaptainUserId != null) // if (resultT2.TeamCaptainUser != null) , vaihdettu //tarkistaa onko kapteenia joukkueessa
                                        {
                                           


                                            var resultMTx = db.MatchTeams.FirstOrDefault(b => b.MatchId == mId && b.TeamId == mt1id); //team1
                                            if (resultMTx != null) //tarkista onko match id jo olemassa
                                            {
                                                t2cap = (int)resultT2.CaptainUserId;
                                                //owdraft luonti 
                                                var owdraftgo = await OWdraft(resultT1.CityName,resultT2.CityName);

                                                //team1
                                                resultMTx.TeamId1CaptainUserId = t1cap;
                                                resultMTx.TeamId2CaptainUserId = t2cap;
                                                if (owdraftgo.Count > 2)
                                                {
                                                    drurl1 = owdraftgo[0];
                                                    drurl2 = owdraftgo[1];
                                                    resultMTx.TeamId1DraftUrl = drurl1;
                                                    resultMTx.TeamId2DraftUrl = drurl2;

                                                    await db.SaveChangesAsync();
                                                }

                                                await db.SaveChangesAsync();

                                                var resultMTx2 = db.MatchTeams.FirstOrDefault(b => b.MatchId == mId && b.TeamId == mt2id); //team2
                                                if(resultMTx2 != null)
                                                {
                                                    resultMTx2.TeamId1CaptainUserId = t1cap;
                                                    resultMTx2.TeamId2CaptainUserId = t2cap;
                                                    resultMTx2.TeamId1DraftUrl = drurl1;
                                                    resultMTx2.TeamId2DraftUrl = drurl2;

                                                    await db.SaveChangesAsync();
                                                }
                                                
                                                //Lisää observerlinkki 19.04.2017
                                                var resultMa = db.Match.FirstOrDefault(b => b.Id == mId);
                                                if(resultMa != null)
                                                {
                                                    var drurlo = "";
                                                    if (owdraftgo.Count > 2)
                                                    {
                                                        drurlo = owdraftgo[2];
                                                        resultMa.ObsDraftUrl = drurlo;

                                                        await db.SaveChangesAsync();
                                                    }
                                                }

                                                var resultMT = db.MatchTeams.FirstOrDefault(b => b.MatchId == mId);
                                                if (resultMT != null)
                                                {
                                                    if (resultM.Started == false)
                                                    {
                                                        if (resultM.Closed == false)
                                                        {
                                                            
                                                            //-> Kysely Users taulusta  id kaikki joiden teamid on team1: id => lisätään Matchuserstauluun user_id sekä match_id

                                                            var mat12x = new List<MatchUsers>();

                                                            var items = db.User
                                                                .Where(x => x.TeamId == mt1id);
                                                            foreach(var item in items)
                                                            {
                                                                var mu = new MatchUsers();
                                                                mu.UserId = item.Id;
                                                                mu.MatchId = mId;
                                                                
                                                                mat12x.Add(mu);
                                                            }
                                                            db.MatchUsers.AddRange(mat12x);
                                                            await db.SaveChangesAsync();

                                                            var mat12x1 = new List<MatchUsers>();
                                                            var items2 = db.User
                                                               .Where(x => x.TeamId == mt2id);
                                                            foreach (var item in items2)
                                                            {
                                                                var mux = new MatchUsers();
                                                                mux.UserId = item.Id;
                                                                mux.MatchId = mId;
                                                                
                                                                mat12x1.Add(mux);
                                                            }
                                                            db.MatchUsers.AddRange(mat12x1);
                                                            //await db.SaveChangesAsync();
                                                            resultM.Started = true;
                                                            //Update 09.06.2017 - Add modified time
                                                            resultM.Modified = DateTime.Now;

                                                            await db.SaveChangesAsync();

                                                            foreach (var item in db.MatchTeams.Where(d => d.MatchId == resultMT.MatchId))
                                                            {
                                                                item.TeamId1CaptainUserId = t1cap;
                                                                item.TeamId2CaptainUserId = t2cap;
                                                                //await db.SaveChangesAsync();
                                                            }
                                                            //resultMT.TeamId1CaptainUserId = t1cap;
                                                            //resultMT.TeamId2CaptainUserId = t2cap;
                                                            
                                                            await db.SaveChangesAsync(); //committaa kantaan
                                                            return 1; //palauta arvo botille
                                                        }
                                                        else
                                                        {
                                                            return 9;//peli on jo pelattu
                                                        }
                                                    }
                                                    else
                                                    {
                                                        return 8;//peli on jo käynnissä
                                                    }
                                                }
                                                else
                                                {
                                                    return 7;//Kenttää ei löytynyt Matchteamsista?? - Tekninen virhe
                                                }
                                            }
                                            else
                                            {
                                                return 8;
                                            }
                                            
                                        }
                                        else
                                        {
                                            return 8; // Joukkueella 2 ei ole kapteenia
                                        }
                                    }
                                    else
                                    {
                                        return 7; //"mahdoton virhe", vierasjoukkuetta ei löydy - Tekninen virhe
                                    }
                                }
                                else
                                {
                                    return 6; //Et ole kotijoukkueen kapteeni
                                }
                            }
                            else
                            {
                                return 5; //käyttäjä ei ole joukkueen kapteeni
                            }
                        }
                        else
                        {
                            return 4; // Pyytäjä ei joukkueessa
                        }
                    }
                    else
                    {
                        return 3; //Pyytäjää ei kannassa
                    }
                }
                else
                {
                    return 2; //Matchcodea ei löydy
                }
            }
        }
        //---------------------------------------------------------------------------------------!score <KOTI-VIERAS>                           VALMIS V2 Updated 06/2017
        public async Task<int> Score(ulong i, string si, string sb)
        {
            using (var db = new ksBotSQLContext())
            {
                var mr1 = 0;
                var mr2 = 0;
                var discordId = i.ToString().Trim();

                if (Int32.TryParse(si, out mr1))
                {
                    if (Int32.TryParse(sb, out mr2))
                    {
                        
                        var resultUX = db.User.SingleOrDefault(b => b.DiscordId == discordId);
                        if (resultUX != null)
                        {
                            //onko useria
                            var resultU1 = db.Team.SingleOrDefault(b => b.CaptainUserId == resultUX.Id);
                            if (resultU1 != null)
                            {
                                //matchid selvitys
                                var matchlistPREid = 0;
                                var matchlistPRE = db.Match
                                            .Where(d => d.Team1Id == resultU1.Id)
                                            .Where(d => d.Started == true)
                                            .Where(d => d.Closed == false)
                                            .FirstOrDefault();
                                if (matchlistPRE != null)
                                {
                                    matchlistPREid = matchlistPRE.Id;
                                    //onko team1 kapteeni
                                    //var resultU2 = db.MatchTeams.SingleOrDefault(b => b.TeamId1CaptainUserId == resultUX.Id);
                                    var resultU2 = db.MatchTeams.FirstOrDefault(b => b.TeamId1CaptainUserId == resultUX.Id && b.MatchId == matchlistPREid);
                                    if (resultU2 != null)
                                    {
                                        var matchlist = db.Match
                                            .Where(d => d.Team1Id == resultU1.Id)
                                            .Where(d => d.Started == true)
                                            .Where(d => d.Closed == false)
                                            .FirstOrDefault();
                                        
                                        if (matchlist != null)
                                        {
                                            var winteamId = 0;
                                            //löytyi peli, aloita tulosten syöttö
                                            if (mr1 > mr2)
                                            {
                                                winteamId = (int)matchlist.Team1Id;
                                            }
                                            if (mr1 < mr2)
                                            {
                                                winteamId = (int)matchlist.Team2Id;
                                            }

                                            //tarkista onko riviä jo sarjataulukossa -> syötä team1 ja team2 pisteet
                                            var scNamecheck1 = db.Team.SingleOrDefault(v => v.Id == matchlist.Team1Id); //team1 (koti)
                                            var scNamecheck2 = db.Team.SingleOrDefault(v => v.Id == matchlist.Team2Id); //team2 (vieras)
                                            var scCheck1 = db.Scoreboard.SingleOrDefault(b => b.TeamId == matchlist.Team1Id); //team1 (koti)
                                            var scCheck2 = db.Scoreboard.SingleOrDefault(b => b.TeamId == matchlist.Team2Id); //team2 (vieras)
                                            if (scCheck1 != null) //team1 (koti)
                                            {
                                                var pointst1 = 1;
                                                var divisiont1 = 0;
                                                if (scNamecheck1.Division != null)
                                                {
                                                    divisiont1 = (int)scNamecheck1.Division;
                                                }
                                                if (mr1 > mr2)
                                                {
                                                    pointst1 = ProgHelpers.winpoints; //voitto
                                                }
                                                if (mr1 < mr2)
                                                {
                                                    pointst1 = ProgHelpers.losepoints; //tappio
                                                }
                                                if (mr1 == mr2)
                                                {
                                                    pointst1 = ProgHelpers.drawpoints; //tasapeli
                                                }
                                                await Editscoreboardrow((int)matchlist.Team1Id, pointst1, mr1, mr2,divisiont1);
                                            }
                                            else
                                            {
                                                var pointst1 = 1;
                                                var divisiont1 = 0;
                                                if (scNamecheck1.Division != null)
                                                {
                                                    divisiont1 = (int)scNamecheck1.Division;
                                                }
                                                if (mr1 > mr2)
                                                {
                                                    pointst1 = ProgHelpers.winpoints;
                                                }
                                                if (mr1 < mr2)
                                                {
                                                    pointst1 = ProgHelpers.losepoints;
                                                }
                                                if (mr1 == mr2)
                                                {
                                                    pointst1 = ProgHelpers.drawpoints;
                                                }
                                                await Newscoreboardrow((int)matchlist.Team1Id,scNamecheck1.CityName, pointst1, mr1, mr2,divisiont1);
                                            }

                                            if (scCheck2 != null) //team2 (vieras)
                                            {
                                                var pointst2 = 1;
                                                var divisiont2 = 0;
                                                if (scNamecheck2.Division != null)
                                                {
                                                    divisiont2 = (int)scNamecheck2.Division;
                                                }
                                                if (mr2 > mr1)
                                                {
                                                    pointst2 = ProgHelpers.winpoints;
                                                }
                                                if (mr2 < mr1)
                                                {
                                                    pointst2 = ProgHelpers.losepoints;
                                                }
                                                if (mr2 == mr1)
                                                {
                                                    pointst2 = ProgHelpers.drawpoints;
                                                }
                                                await Editscoreboardrow((int)matchlist.Team2Id, pointst2, mr2, mr1,divisiont2);
                                            }
                                            else
                                            {
                                                var pointst2 = 1;
                                                var divisiont2 = 0;
                                                if (scNamecheck2.Division != null)
                                                {
                                                    divisiont2 = (int)scNamecheck2.Division;
                                                }
                                                if (mr2 > mr1)
                                                {
                                                    pointst2 = ProgHelpers.winpoints;
                                                }
                                                if (mr2 < mr1)
                                                {
                                                    pointst2 = ProgHelpers.losepoints;
                                                }
                                                if (mr2 == mr1)
                                                {
                                                    pointst2 = ProgHelpers.drawpoints;
                                                }
                                                await Newscoreboardrow((int)matchlist.Team2Id, scNamecheck2.CityName, pointst2, mr2, mr1,divisiont2);
                                            }
                                            //await db.SaveChangesAsync();
                                            var resU2 = resultU2.MatchId; //testi
                                            resultU2 = null; //testi
                                            matchlist.Closed = true;
                                            matchlist.WinnerTeamId = winteamId;
                                            //lisää modified aika
                                            matchlist.Modified = DateTime.Now;

                                            db.Entry(matchlist).State = EntityState.Modified;
                                            await db.SaveChangesAsync();
                                            
                                            try
                                            {
                                                using (var dbgo = new ksBotSQLContext())
                                                {
                                                    foreach (var item in dbgo.MatchTeams.Where(d => d.MatchId == resU2).ToList())
                                                    {
                                                        item.TeamId1Points = mr1;
                                                        item.TeamId2Points = mr2;
                                                        await dbgo.SaveChangesAsync();
                                                    }
                                                }       
                                            }
                                            catch (Exception ex)
                                            {
                                                Console.WriteLine(ex);
                                            }
                                            
                                            //resultU2.TeamId1Points = mr1;
                                            //resultU2.TeamId2Points = mr2;
                                            
                                            return 1;
                                        }
                                        else
                                        {
                                            //ei löytynyt aloitettua peliä
                                            return 7;
                                        }

                                    }
                                    else
                                    {
                                        //ei ole team1 kapteeni
                                        return 6;
                                    }
                                }
                                else
                                {
                                    return 7;
                                }
                                
                            }
                            else
                            {
                                //ei ole kapteeni
                                return 4;
                            }
                        }
                        else
                        {
                            //user ei kannassa
                            return 3;
                        }
                    }
                }
                else
                {
                    return 5;
                }
                return 4;
            }

        }
        //---------------------------------------------------------------------------------------!add <@discordnimi#1234>                       VALMIS V1
        public async Task<int> Addmember(ulong i, string st)
        {
            using (var db = new ksBotSQLContext())
            {
                //lisättävä
                var namehold = "";
                namehold = st;
                var nameholdId = 0;
                //lisääjä
                var discordId = i.ToString().Trim();
                //var nameholdIdA = 0;


                var resultT = db.User.SingleOrDefault(b => b.DiscordId == namehold);
                var resultS = db.User.SingleOrDefault(b => b.DiscordId == discordId);
                if (resultT != null)
                {
                    nameholdId = resultT.Id;
                    if(resultS != null)
                    {
                        //nameholdIdA = resultS.Id;
                        
                        //on jo sinun joukkueessa
                        if(resultS.TeamId == resultT.TeamId || resultT.TeamId != null)
                        {
                            return 2;
                        }
                        else
                        {
                            var resultST = db.Team.SingleOrDefault(b => b.CaptainUserId == resultS.Id);
                            if (resultST != null)
                            {
                                resultT.TeamId = resultS.TeamId;
                                await db.SaveChangesAsync();
                                //OK
                                return 1;
                            }
                            else
                            {
                                //Ei oikeuksia
                                return 5;
                            }
                       
                        }
                    }
                    else
                    {
                        //Sinua ei löydy kannasta
                        return 3;
                    }
                }
                else
                {
                    return 3;
                }

            }
        }
        //---------------------------------------------------------------------------------------!remove <@discordnimi#1234>                    VALMIS V1
        public async Task<int> Removemember(ulong i, string st)
        {
            using (var db = new ksBotSQLContext())
            {
                //poistettava
                var namehold = "";
                namehold = st;
                var nameholdId = 0;
                //poistaja
                var discordId = i.ToString().Trim();

                var resultT = db.User.SingleOrDefault(b => b.DiscordId == namehold);
                var resultS = db.User.SingleOrDefault(b => b.DiscordId == discordId);
                if (resultT != null)//löytyykö kohde
                {
                    nameholdId = resultT.Id;
                    if(resultS != null)//löytyykö pyytäjä
                    {
                        if (resultS.TeamId == resultT.TeamId)//sama joukkue?
                        {
                            var resultST = db.Team.SingleOrDefault(b => b.CaptainUserId == resultS.Id);
                            if (resultST != null)//onko kapteeni joukkueessa
                            {
                                if (resultT.TeamId == resultS.TeamId)//onko kapteeni poistamassa itseään?
                                {
                                    var resulttc = db.TeamCaptainUser.SingleOrDefault(b => b.UserId == resultS.TeamId);
                                    if (resulttc != null)//kapteeni poistamassa itseään, poistetaan liittyvät tiedot
                                    {
                                        resultST.CaptainUserId = null;
                                        db.Entry(resulttc).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                                        resultT.TeamId = null;
                                        await db.SaveChangesAsync();

                                    }
                                    resultT.TeamId = null;
                                    await db.SaveChangesAsync();
                                    return 1;
                                }
                                else
                                {
                                    resultT.TeamId = null;
                                    await db.SaveChangesAsync();
                                    return 1;
                                }        
                            }
                            else
                            {
                                return 6;
                            }
                        }
                        else
                        {
                            return 2;
                        }
                    }
                    else
                    {
                        return 5;
                    }
                }
                else
                {
                    return 4;
                }
                return 4;
            }
        }
        //---------------------------------------------------------------------------------------!captain <KAUPUNKI>                            VALMIS V1
        public async Task<int> Becomecaptain(ulong i, string sn, string st)
        {
            using (var db = new ksBotSQLContext())
            {
                var namehold = "";
                var discordId = i.ToString().Trim();
                namehold = st;
                var teamid = 0;
                var result = db.User.SingleOrDefault(b => b.DiscordId == discordId);
                if (result != null)
                {
                    var uidSearch = result.Id;
                    var result2 = db.Team.SingleOrDefault(b => b.CityName == namehold);
                    //var teamcaptain = db.TeamCaptainUser.ToList();
                    if (result2 != null)
                    {
                        teamid = result2.Id;
                        if (result2.CaptainUserId != null)
                        {
                            //kaupungilla on jo kapteeni
                            return 3;
                        }
                        else
                        {
                            result2.CaptainUserId = uidSearch;
                            result.TeamId = teamid;
                            var resulttct = db.TeamCaptainUser.SingleOrDefault(b => b.UserId == uidSearch);
                            var resulttc = db.TeamCaptainUser.SingleOrDefault(b => b.TeamId == teamid);

                            await db.SaveChangesAsync();

                            if (resulttc != null) //onko taulussa teamidtä
                            {
                                if (resulttct != null) //onko taulussa userid
                                {
                                    return 5; //olet jo jonkun mestan kapteeni
                                }
                                else
                                {
                                    //poista vanha kapurivi, lisää uusi
                                    if (resulttc != null)
                                    {
                                        db.Entry(resulttc).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                                        await db.SaveChangesAsync();
                                    }

                                    var teamcap = new TeamCaptainUser();
                                    teamcap.UserId = uidSearch;
                                    teamcap.TeamId = teamid;
                                    db.Entry(teamcap).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                                    await db.SaveChangesAsync();
                                }
                            }
                            else //taulussa ei teamidtä
                            {
                                if (resulttct != null) //onko taulussa userid
                                {
                                    return 5; //olet jo jonkun mestan kapteeni
                                }
                                else
                                {
                                    //poista vanha kapurivi, lisää uusi
                                    if (resulttc != null)
                                    {
                                        db.Entry(resulttc).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                                        await db.SaveChangesAsync();
                                    }

                                    var teamcap = new TeamCaptainUser();
                                    teamcap.UserId = uidSearch;
                                    teamcap.TeamId = teamid;
                                    db.Entry(teamcap).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                                    await db.SaveChangesAsync();

                                }
                            }

                            return 1;
                        }
                    }
                    else
                    {
                        return 2;
                    }
                }
                else
                {
                    //await Declareuser(discordId, sn);
                    //var resultx = db.User.SingleOrDefault(b => b.DiscordId == discordId);
                    //if (resultx != null)
                    //{
                    //    var uidSearch = result.Id;
                    //    var result2x = db.Team.SingleOrDefault(b => b.CityName == namehold);
                    //    if (result2x != null)
                    //    {
                    //        teamid = result2x.Id;
                    //        if (result2x.CaptainUserId != null)
                    //        {
                    //            return 3;
                    //        }
                    //        else
                    //        {

                    //            var resulttc2 = db.TeamCaptainUser.SingleOrDefault(b => b.TeamId == teamid);
                    //            if (resulttc2 != null)
                    //            {
                    //                resulttc2.UserId = uidSearch;
                    //            }
                    //            else
                    //            {
                    //                var teamcap2 = new TeamCaptainUser();
                    //                result2x.CaptainUserId = uidSearch;
                    //                resultx.TeamId = teamid;
                    //                db.Entry(teamcap2).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                    //            }

                    //            await db.SaveChangesAsync();
                    //            return 1;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        return 2;
                    //    }
                    //}
                    return 4;
                }
            }
        }
        //---------------------------------------------------------------------------------------!resign <KAUPUNKI>                             VALMIS V1
        public async Task<int> Resigncaptain(ulong i,string sn)
        {
            using (var db = new ksBotSQLContext())
            {
                var discordId = i.ToString().Trim();

                var result = db.User.SingleOrDefault(b => b.DiscordId == discordId);
                if (result != null)
                {
                    var uidSearch = result.Id;
                    var result2 = db.Team.SingleOrDefault(b => b.CaptainUserId == uidSearch);
                    if (result2 != null)
                    {
                        result2.CaptainUserId = null;

                        var resulttc = db.TeamCaptainUser.SingleOrDefault(b => b.UserId == uidSearch);
                        if (resulttc != null)
                        {
                            db.Entry(resulttc).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                            await db.SaveChangesAsync();

                        }

                        await db.SaveChangesAsync();
                        return 1;
                    }
                    else
                    {
                        return 2;
                    }
                }
                else
                {
                    //await Declareuser(discordId, sn);
                    //var resultx = db.User.SingleOrDefault(b => b.DiscordId == discordId);
                    //if (resultx != null)
                    //{
                    //    var uidSearch = result.Id;
                    //    var result2x = db.Team.SingleOrDefault(b => b.CaptainUserId == uidSearch);
                    //    if (result2x != null)
                    //    {
                    //        result2x.CaptainUserId = null;
                    //        var resulttc2 = db.TeamCaptainUser.SingleOrDefault(b => b.UserId == uidSearch);
                    //        if (resulttc2 != null)
                    //        {
                    //            db.Entry(resulttc2).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;

                    //        }
                    //        await db.SaveChangesAsync();
                    //        return 1;
                    //    }
                    //    else
                    //    {
                    //        return 2;
                    //    }
                    //}
                    return 4;
                }
            }
        }

        // #admin kanavan komennot

        //---------------------------------------------------------------------------------------+addteam <KAUPUNKI>                                TODO V2
        //---------------------------------------------------------------------------------------+removeteam <KAUPUNKI> (Vain jos ei yhtään peliä)  TODO V2
        //---------------------------------------------------------------------------------------+renameteam <KAUPUNKI>                             TODO V2
        //---------------------------------------------------------------------------------------+division <KAUPUNKI><1,2 jne>                      TODO V2
        //---------------------------------------------------------------------------------------+attending <OPEN/CLOSED>                           TODO V2
        //---------------------------------------------------------------------------------------+reportattending                                   TODO V2


        //---------------------------------------------------------------------------------------+addmatch <MATCHID> <KAUPUNKI1> <KAUPUNKI2>    VALMIS V1
        public async Task<int> Addmatch(ulong i, string sid, string st, string stb)
        {
            using (var db = new ksBotSQLContext())
            {
                var discordId = i.ToString().Trim();
                var mid = "";
                var mt1 = "";
                var mt2 = "";
                mid = sid;
                mt1 = st;
                mt2 = stb;
                var mt1id = 0;
                var mt2id = 0;

                var resultMt1 = db.Team.SingleOrDefault(b => b.CityName == mt1);
                var resultMt2 = db.Team.SingleOrDefault(b => b.CityName == mt2);
                if (resultMt1 != null)
                {
                    mt1id = resultMt1.Id;
                    if (resultMt2 != null)
                    {
                        //kaikki OK, laita kantaan.
                        mt2id = resultMt2.Id;
                        var match = new Match();
                        match.Team1Id = mt1id;
                        match.Team2Id = mt2id;
                        match.Matchcode = mid;
                        //Lisää myös divisioona ja modified aika
                        match.Team1Division = resultMt1.Division;
                        match.Team2Division = resultMt2.Division;
                        match.Modified = DateTime.Now;

                        db.Entry(match).State = Microsoft.EntityFrameworkCore.EntityState.Added;

                        await db.SaveChangesAsync();

                        //team1
                        var matchteams = new MatchTeams();

                        var matchidvar = match.Id;

                        matchteams.MatchId = matchidvar;
                        matchteams.TeamId = mt1id;

                        db.Entry(matchteams).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                        await db.SaveChangesAsync();

                        //team2
                        var matchteams2 = new MatchTeams();

                        matchteams2.MatchId = matchidvar;
                        matchteams2.TeamId = mt2id;

                        db.Entry(matchteams2).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                        await db.SaveChangesAsync();


                        return 1;
                    }
                    else
                    {
                        //joukkue2 ei löydy
                        return 3;
                    }
                }
                else
                {
                    //joukkue1 ei löydy
                    return 2;
                }
            }

        }
        //---------------------------------------------------------------------------------------+forcecaptain <KAUPUNKI><@discordnimi#1234>    VALMIS V1
        public async Task<int> Forcecaptain(string city, string name)
        {
            using (var db = new ksBotSQLContext())
            {
                var discordId = name.ToString().Trim();
                var cityhold = "";
                cityhold = city;

                var resultD = db.User.SingleOrDefault(b => b.DiscordId == discordId);
                var resultT = db.Team.SingleOrDefault(b => b.CityName == cityhold);
                if (resultD != null)
                {
                    var resUid = resultD.Id;
                    if (resultT != null)
                    {
                        var resTid = resultT.Id;
                        //käyttäjä ja joukkue löytyy, aloitetaan yliajo.
                        resultD.TeamId = resTid;
                        resultT.CaptainUserId = resUid;

                        await db.SaveChangesAsync();

                        var resultTCU = db.TeamCaptainUser.SingleOrDefault(b => b.TeamId == resTid);
                        if (resultTCU != null)
                        {
                            db.Entry(resultTCU).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                            await db.SaveChangesAsync();

                            var teamcap = new TeamCaptainUser();
                            teamcap.UserId = resUid;
                            teamcap.TeamId = resTid;
                            db.Entry(teamcap).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                            await db.SaveChangesAsync();

                            return 1;
                        }
                        else
                        {
                            var teamcap = new TeamCaptainUser();
                            teamcap.UserId = resUid;
                            teamcap.TeamId = resTid;
                            db.Entry(teamcap).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                            await db.SaveChangesAsync();

                            return 1;
                        }
                    }
                    else
                    {
                        return 3; // kaupunkia ei löydy
                    }
                }
                else
                {
                    return 2; //käyttäjää ei löydy
                }

            }
        }
        //API osuudet
        //---------------------------------------------------------------------------------------!OWAPI.net                                     VALMIS V1 Updated 06/2017 (Custom OWApi URL)
        public async Task<int> OWstats(string i, string st)
    {
        string stnew = "";
        stnew = st.Replace("#", "-");
        string url = ProgHelpers.owapiulr + stnew + "/stats";
        try
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Encoding", "gzip, deflate"); //This might be unnecessary, even wrong! Keep in mind.
                client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 6.2; WOW64; rv:19.0) Gecko/20100101 Firefox/19.0");
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");  

                try
                {
                    HttpResponseMessage responseweb = await client.GetAsync(url);
                    if (!(responseweb.IsSuccessStatusCode))
                    {
                        return 3;
                    }
                    var responseText = await client.GetStringAsync(url);
                    
                    //dynamic data = JsonConvert.DeserializeObject(responseText);
                    dynamic data = responseText;
                    JObject o = JObject.Parse(data);

                    //var hero1 = "";
                    //var hero2 = "";
                    //var hero3 = "";

                    //int sr = (int)o.SelectToken("eu.stats.competitive.overall_stats.comprank");
                    //double killavg = (double)o.SelectToken("eu.stats.competitive.average_stats.eliminations_avg");
                    //double healavg = (double)o.SelectToken("eu.stats.competitive.average_stats.healing_done_avg");
                    //double deathavg = (double)o.SelectToken("eu.stats.competitive.average_stats.deaths_avg");

                    double killavg = 0;
                    double healavg = 0;
                    double deathavg = 0;
                    double winrate = 0;
                    double timeplayed = 0;
                    string avatarurl = "";
                    
                    //06.08.2017 OWAPI Workaround, Blizzard removed average kills deaths and healing, but totals are visible, so...
                    double gamesPlayed = 0;
                    double placehKills = 0;
                    double placehDeaths = 0;
                    double placehHeal = 0;


                    int sr = 0;


                    if (o.SelectToken("eu.stats.competitive.game_stats.games_played") != null)
                    {
                        gamesPlayed = (double)o.SelectToken("eu.stats.competitive.game_stats.games_played");

                        if (o.SelectToken("eu.stats.competitive.game_stats.eliminations") != null)
                        {
                                placehKills = (double)o.SelectToken("eu.stats.competitive.game_stats.eliminations");
                                killavg = Math.Round(placehKills / gamesPlayed, 1);
                        }
                        if (o.SelectToken("eu.stats.competitive.game_stats.deaths") != null)
                        {
                                placehDeaths = (double)o.SelectToken("eu.stats.competitive.game_stats.deaths");
                                deathavg = Math.Round(placehDeaths / gamesPlayed, 1);
                        }
                        if (o.SelectToken("eu.stats.competitive.game_stats.healing_done") != null)
                        {
                                placehHeal = (double)o.SelectToken("eu.stats.competitive.game_stats.healing_done");
                                healavg = Math.Round(placehHeal / gamesPlayed, 1);
                        }
                    }


                        //if (o.SelectToken("eu.stats.competitive.average_stats.eliminations_avg") != null)
                        //{
                        //    killavg = (double)o.SelectToken("eu.stats.competitive.average_stats.eliminations_avg");
                        //}
                        //if (o.SelectToken("eu.stats.competitive.average_stats.healing_done_avg") != null)
                        //{
                        //    healavg = (double)o.SelectToken("eu.stats.competitive.average_stats.healing_done_avg");
                        //}
                        //if (o.SelectToken("eu.stats.competitive.average_stats.deaths_avg") != null)
                        //{
                        //    deathavg = (double)o.SelectToken("eu.stats.competitive.average_stats.deaths_avg");
                        //}

                    if (o.SelectToken("eu.stats.competitive.overall_stats.comprank") != null)
                    {
                        sr = (int)o.SelectToken("eu.stats.competitive.overall_stats.comprank");
                    }
                    if (o.SelectToken("eu.stats.competitive.overall_stats.win_rate") != null)
                    {
                         winrate = (double)o.SelectToken("eu.stats.competitive.overall_stats.win_rate");
                    }
                    if (o.SelectToken("eu.stats.competitive.overall_stats.avatar") != null)
                    {
                        avatarurl = (string)o.SelectToken("eu.stats.competitive.overall_stats.avatar");
                    }
                    if (o.SelectToken("eu.stats.competitive.game_stats.time_played") != null)
                    {
                        timeplayed = (double)o.SelectToken("eu.stats.competitive.game_stats.time_played");
                    }


                        using (var db = new ksBotSQLContext())
                    {
                        var result = db.User.SingleOrDefault(b => b.DiscordId == i);
                        if (result != null)
                        {
                            //result.Apihero1 = hero1;
                            //result.Apihero2 = hero2;
                            //result.Apihero3 = hero3;
                            result.ApicurrentSr = sr;
                            result.ApikillAvg = killavg;
                            result.ApideathAvg = deathavg;
                            result.ApihealAvg = healavg;
                            result.ApiavatarUrl = avatarurl;
                            result.ApiwinRate = winrate;
                            result.ApitimePlayed = timeplayed;

                            await db.SaveChangesAsync();
                            Console.WriteLine("OWAPI-UPDATE OK");
                            //return 1;
                            }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("OWAPI 404 EX");
                        Console.WriteLine(ex);
                    return 0;
                }
                        
            }
                return 1;
        }
        catch (Exception)
        {
            Console.WriteLine("OWAPI-UPDATE -- EX");
            return 0;
        }

    }
        //---------------------------------------------------------------------------------------!Owdraft FETCH                                 VALMIS V1
        public async Task<List<string>> OWdraft(string st, string stb)
        {
            try
            {
                string url = "http://owdraft.com/create"; //or: check where the form goes
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 6.2; WOW64; rv:19.0) Gecko/20100101 Firefox/19.0");

                    var id1 = "";
                    var id2 = "";
                    var name = "";
                    id1 = st;
                    id2 = stb;
                    name = "KaupunkisotaOttelu_" + id1 + "_" + id2 + "_" + DateTime.Now.ToString("yyyyMMdd_HHmm");
                    
                    //kasaa POST
                    var values = new Dictionary<string, string>
                    {
                       { "tournament[name]", name },
                       { "teams[1][name]", id1 },
                       { "teams[2][name]:", id2 },
                       { "map[draft_rule_set][first_round_options][]", "FIRSTPICK_TEAM_ONE" },
                       { "map[type]","MAPPICK_DRAFT"},
                       { "map[draft_rule_set][pick_ban_pattern]", "1" },
                       { "map[draft_rule_set][timer_preset]", "1" },
                       { "hero[type]", "HEROPICK_NONE" }
                    };
                    var content = new FormUrlEncodedContent(values);

                    //lähetä POST
                    var response = await client.PostAsync(url, content);
                    Console.WriteLine("OWDraft API Attempt");

                    var responseString = await response.Content.ReadAsStringAsync();
                    HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
                    htmlDoc.LoadHtml(responseString);


                    //Kerää kaikki linkit (1. team, 2. team, observer) tuloksesta
                    List<string> hrefTags = new List<string>();
                    foreach (HtmlNode link in htmlDoc.DocumentNode.SelectNodes("//input[@value]"))
                    {
                        HtmlAttribute att = link.Attributes["value"];
                        hrefTags.Add(att.Value);
                    }

                    //foreach (object o in hrefTags)
                    //{
                    //    Console.WriteLine(o);
                    //}
                    
                    return hrefTags;
                }
            }
            catch (Exception)
            {
                Console.WriteLine("OWDraft API Attempt - EX - Down?");
                List<string> hrefTags = new List<string>();
                return hrefTags;
            }
            
        }
        //---------------------------------------------------------------------------------------!Owdraft RETURN                                VALMIS V1
        public async Task<List<string>> OWdraftRet(string sta)
        {
            try {
                var mis = "";
                mis = sta;
                var mId = 0;
                var t1id = 0;
                var t2id = 0;

                var url1 = "";
                var url2 = "";
                var urlo = "";

                List<string> retlist = new List<string>();

                var resultMa = db.Match.FirstOrDefault(b => b.Matchcode == mis);
                if (resultMa != null)
                {
                    mId = resultMa.Id;
                    t1id = (int)resultMa.Team1Id;
                    t2id = (int)resultMa.Team2Id;
                    urlo = resultMa.ObsDraftUrl;

                    var resT1a = db.MatchTeams.FirstOrDefault(b => b.MatchId == mId);
                    if (resT1a != null)
                    {
                        url1 = resT1a.TeamId1DraftUrl;
                        url2 = resT1a.TeamId2DraftUrl;
                    }

                    retlist.Add(url1);
                    retlist.Add(url2);
                    retlist.Add(urlo);

                    return retlist;

                }
                return retlist;


            }
            catch (Exception)
            {
                List<string> retlist = new List<string>();
                return retlist;
            }
        }
        //---------------------------------------------------------------------------------------!Owdraft Helper, return captain ids            VALMIS V1
        public async Task<List<string>> OWdraftHelp(string sta)
        {
            try
            {
                var mis = "";
                mis = sta;
                var mId = 0;
                var t1id = 0;
                var t2id = 0;

                var ct1id = 0;
                var ct2id = 0;

                var usr1 = "";
                var usr2 = "";


                List<string> retlist = new List<string>();

                var resultMa = db.Match.FirstOrDefault(b => b.Matchcode == mis);
                if (resultMa != null)
                {
                    mId = resultMa.Id;
                    t1id = (int)resultMa.Team1Id;
                    t2id = (int)resultMa.Team2Id;

                    var resT1a = db.Team.FirstOrDefault(b => b.Id == t1id);
                    var resT2a = db.Team.FirstOrDefault(b => b.Id == t2id);

                    if (resT1a != null)
                    {
                        ct1id = (int)resT1a.CaptainUserId;

                        if (resT2a != null)
                        {
                            ct2id = (int)resT2a.CaptainUserId;

                            var resT1cd = db.User.FirstOrDefault(b => b.Id == ct1id);
                            var resT2cd = db.User.FirstOrDefault(b => b.Id == ct2id);
                            if (resT1cd != null)
                            {
                                usr1 = resT1cd.DiscordId;
                                if (resT2cd != null)
                                {
                                    usr2 = resT2cd.DiscordId;

                                    retlist.Add(usr1);
                                    retlist.Add(usr2);
                                }
                            }
                        }
                    }

                    return retlist;

                }
                return retlist;

            }
            catch (Exception)
            {
                List<string> retlist = new List<string>();
                return retlist;
            }
        }



        //HELPER
        //---------------------------------------------------------------------------------------Perusta uusi käyttäjä kantaan                  VALMIS V1
        public async Task<bool> Declareuser(string i,string name)
    {
        using (var db = new ksBotSQLContext())
        {

            //var dId = i.ToString().Trim();
            var discordId = i;
            var newuser = new User();
            newuser.DiscordId = discordId;
            newuser.Username = name;
            db.Entry(newuser).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            await db.SaveChangesAsync();

            return true;
        }
    }
        //---------------------------------------------------------------------------------------Lisää uusi joukkue tuloslistalle               VALMIS V1
        public async Task<bool> Newscoreboardrow(int id, string name, int stdpoints, int rndwon, int rndlose, int division)
        {
            using (var dbn1 = new ksBotSQLContext())
            {
                try
                {
                    var newrow = new Scoreboard();
                    newrow.TeamId = id;
                    newrow.CityName = name;
                    newrow.StdPoints = stdpoints;
                    newrow.RndWon = rndwon;
                    newrow.RndLose = rndlose;
                    newrow.GamesPlayed = 1;
                    newrow.Division = division;

                    if (stdpoints == ProgHelpers.winpoints) //3
                    {
                        newrow.RndWins = 1;
                    }
                    if (stdpoints == ProgHelpers.drawpoints) //1
                    {
                        newrow.RndDraws = 1;
                    }
                    if (stdpoints == ProgHelpers.losepoints) //0
                    {
                        newrow.RndLoses = 1;
                    }

                    dbn1.Entry(newrow).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                    await dbn1.SaveChangesAsync();

                    Console.WriteLine("Newscoreboardrow - OK");
                    return true;
                }
                catch (Exception)
                {
                    Console.WriteLine("Newscoreboardrow - EX - !!!");
                    return false;
                }
                
            }
               
        }
        //---------------------------------------------------------------------------------------Muokkaa joukkueen tulostietoja                 VALMIS V1
        public async Task<bool> Editscoreboardrow(int id, int stdpoints, int rndwon, int rndlose, int division)
        {
            using (var dbn2 = new ksBotSQLContext())
            {
                try
                {
                var editrow = dbn2.Scoreboard
                .Where(d => d.TeamId == id)
                .FirstOrDefault();

                    if (editrow != null)
                    {
                        var stdpointsedit = editrow.StdPoints;
                        var rndwonedit = editrow.RndWon;
                        var rndloseedit = editrow.RndLose;

                        var rndwinrounds = editrow.RndWins;
                        var rnddrawrounds = editrow.RndDraws;
                        var rndloserounds = editrow.RndLoses;

                        editrow.StdPoints = stdpointsedit + stdpoints;
                        editrow.RndWon = rndwonedit + rndwon;
                        editrow.RndLose = rndloseedit + rndlose;
                        editrow.GamesPlayed = editrow.GamesPlayed + 1;
                        editrow.Division = division;

                        if (stdpoints == ProgHelpers.winpoints) //3
                        {
                            editrow.RndWins = rndwinrounds + 1;
                        }
                        if (stdpoints == ProgHelpers.drawpoints) //1
                        {
                            editrow.RndDraws = rnddrawrounds + 1;
                        }
                        if (stdpoints == ProgHelpers.losepoints) //0
                        {
                            editrow.RndDraws = rndloserounds + 1;
                        }

                        dbn2.Entry(editrow).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        await dbn2.SaveChangesAsync();

                        Console.WriteLine("Editscoreboardrow - OK");
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Editscoreboardrow - EX - !!!");
                        return false;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Editscoreboardrow - EX - !!!");
                    return false;
                }
                
            }

        }

    }
}
