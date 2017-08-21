using Discore;
using Discore.WebSocket;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace K8Director
{
    public class Program
    {
        public static class ProgHelpers
        {
            //Begin configuration
            public static IConfigurationRoot Configuration { get; set; }
            public static string bottoken = ""; //bot token
            public static string txtversion = ""; //shown version of bot
            public static string language = ""; //en-fi
            public static string allchannel = ""; //basic commands channel
            public static string captainchannel = ""; //captain commands channel
            public static string adminchannel = ""; //admin commands channel
            public static string connstring = ""; //Connection string
            public static string owapiulr = ""; //OWAPI provider url
            public static int winpoints = 3; //How many points does a winner take (overridden from settings)
            public static int losepoints = 0; //How many points does a loser take (overridden from settings)
            public static int drawpoints = 1;//How many points does a draw give (overridden from settings)
            public static int postponelimithours = 12; //How many hours until a match cannot be cancelled any more (overridden from settings)

            //Texts (FI)
            public static string txt1 = "Käsittelyssä tapahtui virhe.";
            public static string txt2 = "Tiedot:";
            public static string txt3 = "BTag ID:";
            public static string txt4 = "Rating:";
            public static string txt5 = "Kaupunki:";
            public static string txt6 = "K/D/Heal:";
            public static string txt7 = "Nimi:";
            public static string txt8 = "Päivitetty rating:";
            public static string txt9 = "Rating päivityksessä havaittu ongelma, yritä myöhemmin uudelleen.";
            public static string txt10 = "Lisää komentoon rating, esim. 1234";
            public static string txt11 = "Battletag päivitetty onnistuneesti:";
            public static string txt12 = "Battletag päivityksessä havaittu ongelma, yritä myöhemmin uudelleen.";
            public static string txt13 = "Lisää komentoon battletag, muodossa Nimi#1234 (case-sensitive)";
            public static string txt14 = "Joukkuetta ei löytynyt järjestelmästä.";
            public static string txt15 = "Joukkueen tiedot";
            public static string txt16 = "Kaupunki:";
            public static string txt17 = "Joukkueen nimi:";
            public static string txt18 = "Kapteeni:";
            public static string txt19 = "Pelaajat:";
            public static string txt20 = "Joukkueen tiedot";
            public static string txt21 = "Kaupunki:";
            public static string txt22 = "Joukkueen nimi:";
            public static string txt23 = "Kapteeni:";
            public static string txt24 = "Lisää komentoon joukkueen nimi.";
            public static string txt25 = "Sarjataulukon löydät osoitteesta: https://envy.red/kaupunkisota";
            public static string txt26 = "Kehittäjä:";
            public static string txt27 = "Toiminnot:";
            public static string txt28 = "Katso !help";
            public static string txt29 = "Tarkoitus:";
            public static string txt30 = "Ylläpitää Kaupunkisota -liigaa";
            public static string txt31 = "Fun fact:";
            public static string txt32 = "Versio 1 sisältää 1981 riviä koodia";
            public static string txt33 = "kitsun8 DirectorBot, !help";
            public static string txt34 = "DirectorBot komentolista";
            public static string txt35 = "Infopätkä botin olemuksesta.";
            public static string txt36 = "Tulostaa nykyisen pistetilaston.";
            public static string txt37 = "Sinun tämänhetkiset tiedot tietokannassa.";

            public static string txt132 = "(!attend KAUPUNKI) Merkitsee sinut ilmoittautuneeksi turnaukseen.";
            public static string txt133 = "(!removeattend) Poistaa turnaukseen ilmoittautumisen.";
            public static string txt38 = "(!btag NIMI#1234) Lisää battletagisi botin tietoisuuteen, päivittää myös tiedot OWAPI.net rajapinnasta Tirehtöörin tietokantaan. Huomaa että Battletagin kirjainkoolla on väliä.";
            public static string txt39 = "(!rating 1234) Päivittää omavalitsemasi ratingin. Suositellaan annettavaksi todellisuutta vastaava.";
            public static string txt40 = "(!roster KAUPUNKI) Tulostaa joukkueen jäsenlistan. Huomaa että joukkueen nimi tulee kirjoittaa samassa muodossa kuin Scoreboardissa.";
            public static string txt41 = "(!teamname NIMI) Kapteeni voi muuttaa tai antaa joukkueelle nimen.";
            public static string txt42 = "(!start MATCHID) Kotijoukkueen kapteenin komento, kirjaa matsin alkaneeksi tietokantaan. Pakollinen komento ennen tulosten syöttöä.";
            public static string txt43 = "(!score KOTI VIERAS) Kotijoukkueen kapteenin komento, kirjaa tulokset tietokantaan. Huomaa pistejärjestys KOTI VIERAS, esim. !score 1 2";
            public static string txt44 = "(!add  @NIMI#1234) Kapteeni lisää pelaajat joukkueeseensa tällä. Nimenä käytetään Discord nimeä mentionina.";
            public static string txt45 = "(!remove @NIMI#1234) Vastaava kuin addmember, mutta toisinpäin.";
            public static string txt46 = "(!captain KAUPUNKI) Julistaa sinut kapteeniksi kaupunkiin, mikäli kapteenia ei ole. Kapteeni voi käyttää !add, !remove, !teamname, !start sekä !score toimintoja.";

            public static string txt47 = "Vapauttaa kapteenin valtaistuimen. Joukkueella on oltava kapteeni matsien alkaessa sekä matsin ajan.";
            public static string txt48 = "Joukkueen nimi päivitetty.";
            public static string txt49 = "Joukkueen nimen päivityksessä ongelma, joko et ole kapteeni, tai tapahtui virhe.";
            public static string txt50 = "Lisää komentoon joukkueen nimi, muodossa NIMI (välilöynnit ei sallittuja, muut välimerkit OK).";
            public static string txt51 = "Olet nyt kaupunkijoukkueen kapteeni.";
            public static string txt52 = "Kaupunkia ei löydy, tarkista #info:sta kaupunkien kirjoitusmuoto.";
            public static string txt53 = "Kaupungilla on jo kapteeni.";
            public static string txt54 = "Käyttäjäsi ei ole tietokannassa, käy laittamassa botille !btag tai !rating.";
            public static string txt55 = "Olet jo jonkin kaupungin kapteeni.";
            public static string txt56 = "Lisää komentoon joukkueen nimi.";
            public static string txt57 = "Et ole enää kaupunkijoukkueen kapteeni.";
            public static string txt58 = "Et ole minkään kaupunkijoukkueen kapteeni.";
            public static string txt59 = "Tuntematon virhe kapteeniutta pudottaessa.";
            public static string txt60 = "Käyttäjäsi ei ole tietokannassa, käy laittamassa botille !btag tai !rating.";
            public static string txt61 = "Pelaaja lisätty onnistuneesti joukkueeseen!";
            public static string txt62 = "Pelaajalla on jo joukkue!";
            public static string txt63 = "Käyttäjää ei löytynyt. Pyydä käyttäjää kirjoittamaan Botin kanavalla joko !btag NIMI#1234 tai !rating 1234";
            public static string txt64 = "Pelaajan lisäyksessä tapahtui virhe.";
            public static string txt65 = "Et ole joukkueen kapteeni.";
            public static string txt66 = "Discord nimen muoto ei näytä oikealta, kirjoitithan @nimi#1234?";
            public static string txt67 = "Lisää Pelaajan nimi.";
            public static string txt68 = "Pelaaja poistettu onnistuneesti joukkueesta!";
            public static string txt69 = "Pelaaja ei ole sinun joukkueessasi.";
            public static string txt70 = "Pelaajaa ei löytynyt.";
            public static string txt71 = "Pelaajan käsittelyssä tapahtui virhe.";
            public static string txt72 = "Käyttäjäsi ei ole järjestelmässä. Käytä komentoja !btag Nimi#1234 tai !rating 1234 lisätäksesi itsesi järjestelmään.";
            public static string txt73 = "Et ole joukkueen kapteeni.";
            public static string txt74 = "Discord nimen muoto ei näytä oikealta, kirjoitithan @nimi#1234?";
            public static string txt75 = "Lisää Pelaajan nimi.";
            public static string txt76 = "Aloitetaan matchID:";
            public static string txt77 = "Generoidaan OWDraft linkit ja lähetetään kapteeneille.";
            public static string txt78 = "Kotijoukkueen OWDraft URL:";
            public static string txt79 = "Vierasjoukkueen OWDraft URL:";
            public static string txt80 = "Ottelun OBSERVER Draft URL:";
            public static string txt81 = "hyvää matsia!";
            public static string txt82 = "Koodilla ei löydy peliä";
            public static string txt83 = "Käyttäjääsi ei ole järjestelmässä. Käytä !btag tai !rating komentoja lisätäksesi itsesi kantaan.";
            public static string txt84 = "Käyttäjäsi ei ole ottelun joukkueissa.";
            public static string txt85 = "Käyttäjäsi ei ole joukkueen kapteeni.";
            public static string txt86 = "Käyttäjäsi ei ole kotijoukkueen kapteeni.";
            public static string txt87 = "Ottelua tutkiessa tapahtui tuntematon virhe.";
            public static string txt88 = "Joukkueella 2 ei ole kapteenia, tai Peli on jo käynnissä tai päättynyt!";
            public static string txt89 = "Lisää komentoon MATCHID, tarkista annetut tiedot.";
            public static string txt90 = "Tulos lisätty onnistuneesti järjestelmään!";
            public static string txt91 = "Tulos lisätty onnistuneesti järjestelmään!";
            public static string txt92 = "Käyttäjäsi ei ole tietokannassa. Käytä !btag tai !rating komentoja lisätäksesi käyttäjän kantaan.";
            public static string txt93 = "Et ole minkään joukkueen kapteeni.";
            public static string txt94 = "Tulos lisätty onnistuneesti järjestelmään!";
            public static string txt95 = "Vain kotijoukkueen kapteeni voi lisätä tuloksen.";
            public static string txt96 = "Avointa peliä jossa olet Kotijoukkueen kapteeni ei löytynyt.";
            public static string txt97 = "Komennon muoto ei näytä oikealta.";
            public static string txt98 = "Lisää komentoon tulos.";
            public static string txt99 = "Ottelu lisätty tietokantaan!";
            public static string txt100 = "Joukkuetta 1 ei löydy järjestelmästä.";
            public static string txt101 = "Joukkuetta 2 ei löydy järjestelmästä.";
            public static string txt102 = "Komento on: +addmatch KOODI KAUPUNKI1 KAUPUNKI2";
            public static string txt103 = "KAPTEENI VAIHDETTU!";
            public static string txt104 = "KÄYTTÄJÄÄ EI LÖYDY JÄRJESTELMÄSTÄ.";
            public static string txt105 = "KAUPUNKIA EI LÖYDY JÄRJESTELMÄSTÄ.";
            public static string txt106 = "Tapahtui virhe käsitellessä kapteenia.";
            public static string txt107 = "Komento on: +forcecaptain KAUPUNKI @NIMI#1234";
            public static string txt116 = "Voitot";
            public static string txt117 = "Pelattu aika";

            public static string txt118 = "LOHKO";
            public static string txt119 = "KAUPUNKI";
            public static string txt120 = "NIMI";

            public static string txt121 = "Järjestelmässä ei vielä joukkueita!";
            public static string txt122 = "Tulostaa kaikki järjestelmässä olevat joukkueet.";


            public static string txt123 = "Päiväys väärässä muodossa, muoto on PP.KK.VVVV HH:MM";
            public static string txt124 = "(!showtime MATCHID PP.KK.VVVV HH:MM) Kotijoukkueen kapteenin komento. Määrittää ottelulle alkamisajan.";
            public static string txt125 = "Ottelun aloitusaika syötetty onnistuneesti!";
            public static string txt126 = "Ottelua ei löytynyt annetulla MATCHID:llä.";
            public static string txt127 = "Ottelua ei voi siirtää ollessa liian lähellä aiemmin annettua ajankohtaa!";
            public static string txt128 = "Et ole kotijoukkueen kapteeni!";
            public static string txt129 = "Ottelu on joko meneillään tai jo päättynyt!";
            public static string txt131 = "Kaikkia tarvittavia tietoja ei syötetty.";

            public static string txt130 = "Joukkueella on avoinna oleva peli, sulje avoin peli ensin käyttämällä !score KOTI VIERAS";

            public static string txt134 = "(+forcecaptain @NIMI#1234 KAUPUNKI), vaihtaa kaupungin kapteenin väkisin.";
            public static string txt135 = "(+addmatch KOODI KOTIKAUPUNKI VIERASKAUPUNKI), luo tietokantaan uuden ottelun koodilla.";

            public static string txt136 = "Annetulla Btag Id:llä ei löydy pelaajatunnusta!";


            //Scoreboard
            public static string txt108 = "Mikäli tuloslistasta ei saa mitään selvää, on todennäköistä että käytät liian pientä näyttöä (esim. puhelin). Voit käydä katsomassa tuloslistan myös verkkosivuilta.";
            public static string txt109 = "KAUPUNKI";
            public static string txt110 = "LOHKO";
            public static string txt111 = "PISTEET";
            public static string txt112 = "PELIT";
            public static string txt113 = "RWON";
            public static string txt114 = "RLOSE";
            public static string txt115 = "+-";



        }
            public static void Main(string[] args)
        {
            Console.WriteLine("Reading settings from appsettings.json");

            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");
            ProgHelpers.Configuration = builder.Build();
            ProgHelpers.bottoken = ProgHelpers.Configuration["Settings:BotToken"];
            ProgHelpers.txtversion = ProgHelpers.Configuration["Settings:Version"];
            ProgHelpers.language = ProgHelpers.Configuration["Settings:Language"];
            ProgHelpers.allchannel = ProgHelpers.Configuration["Settings:AllChannel"];
            ProgHelpers.captainchannel = ProgHelpers.Configuration["Settings:CaptainChannel"];
            ProgHelpers.adminchannel = ProgHelpers.Configuration["Settings:AdminChannel"];
            ProgHelpers.connstring = ProgHelpers.Configuration["Settings:ConnectionString"];
            ProgHelpers.winpoints = Int32.Parse(ProgHelpers.Configuration["Settings:WinPoints"]);
            ProgHelpers.losepoints = Int32.Parse(ProgHelpers.Configuration["Settings:LosePoints"]);
            ProgHelpers.drawpoints = Int32.Parse(ProgHelpers.Configuration["Settings:DrawPoints"]);
            ProgHelpers.postponelimithours = Int32.Parse(ProgHelpers.Configuration["Settings:PostponeLimitHours"]);
            ProgHelpers.owapiulr = ProgHelpers.Configuration["Settings:OWAPIUrlString"];

            //Print out read settings
            Console.WriteLine("START SETTINGS-----------------------------");
            Console.WriteLine("TOKEN:" + ProgHelpers.bottoken);
            Console.WriteLine("VER:" + ProgHelpers.txtversion);
            Console.WriteLine("LANG:" + ProgHelpers.language);
            Console.WriteLine("ALLCHAN:" + ProgHelpers.allchannel);
            Console.WriteLine("CAPCHAN:" + ProgHelpers.captainchannel);
            Console.WriteLine("ADMCHAN:" + ProgHelpers.adminchannel);
            Console.WriteLine("CONNSTR:" + ProgHelpers.connstring);
            Console.WriteLine("DRAWPTS:" + ProgHelpers.drawpoints);
            Console.WriteLine("WINPTS:" + ProgHelpers.winpoints);
            Console.WriteLine("LOSEPTS:" + ProgHelpers.losepoints);
            Console.WriteLine("POSTPONELIMIT:" + ProgHelpers.postponelimithours);
            Console.WriteLine("END SETTINGS-----------------------------");
            if (ProgHelpers.language == "en")
            {
                ProgHelpers.txt1 = "Encountered an error during handling.";
                ProgHelpers.txt2 = "Details:";
                ProgHelpers.txt3 = "BTag ID:";
                ProgHelpers.txt4 = "Rating:";
                ProgHelpers.txt5 = "City:";
                ProgHelpers.txt6 = "K/D/Heal:";
                ProgHelpers.txt7 = "Name:";
                ProgHelpers.txt8 = "Updated Rating:";
                ProgHelpers.txt9 = "Encountered an error during Rating update, try again later.";
                ProgHelpers.txt10 = "Specify the rating, ex. 1234.";
                ProgHelpers.txt11 = "Battletag updated successfully:";
                ProgHelpers.txt12 = "Encountered an error during Battletag update, try again later.";
                ProgHelpers.txt13 = "Specify the battletag, format Name#1234 (case-sensitive)";
                ProgHelpers.txt14 = "Team was not found from Database";
                ProgHelpers.txt15 = "Team details";
                ProgHelpers.txt16 = "City:";
                ProgHelpers.txt17 = "Team name:";
                ProgHelpers.txt18 = "Captain:";
                ProgHelpers.txt19 = "Players:";
                ProgHelpers.txt20 = "Team details";
                ProgHelpers.txt21 = "City:";
                ProgHelpers.txt22 = "Team name:";
                ProgHelpers.txt23 = "Captain:";
                ProgHelpers.txt24 = "Specify the team name.";
                ProgHelpers.txt25 = "Scoreboards can be found from our website.";
                ProgHelpers.txt26 = "Developer:";
                ProgHelpers.txt27 = "Commands:";
                ProgHelpers.txt28 = "See !help";
                ProgHelpers.txt29 = "Purpose:";
                ProgHelpers.txt30 = "To maintain tournament matches, scores and teams.";
                ProgHelpers.txt31 = "Fun fact";
                ProgHelpers.txt32 = "This version was one of the most tedious tasks I've done because of translations.";
                ProgHelpers.txt33 = "kitsun8 DirectorBot, !help";
                ProgHelpers.txt34 = "DirectorBot commands";
                ProgHelpers.txt35 = "Information about this bot.";
                ProgHelpers.txt36 = "Prints out the current scoreboards.";
                ProgHelpers.txt37 = "Your details as perceived by this bot.";
                ProgHelpers.txt38 = "(!btag NAME#1234) Adds your Battletag to the bot database. Also attempts to update your competitive stats from API. Battletag is case-sensitive.";
                ProgHelpers.txt39 = "(!rating 1234) Manually adds your rating. It is recommended to add a rating that matches reality.";
                ProgHelpers.txt40 = "(!roster TEAM) Prints out team members. Please note that the name must be written using the exact format seen in the Scoreboards.";
                ProgHelpers.txt41 = "(!teamname NAME) Captain of a team can give the team a nickname. Does not affect the !roster command team name.";
                ProgHelpers.txt42 = "(!start MATCHID) Home team Captain's command. Marks the match as started. MATCH ID:s are given out by the tournament admins for each match.";
                ProgHelpers.txt43 = "(!score HOME VISITOR) Home team Captain's command. Marks the score of the match. Please note that game result screenshots should be saved for validation reasons.";
                ProgHelpers.txt44 = "(!add @NAME#1234) Adds a member to captain's team. Used with Discord mention format.";
                ProgHelpers.txt45 = "(!remove @NAME#1234) Removes a member from captain's team. Used with Discord mention format.";
                ProgHelpers.txt46 = "(!captain TEAM) Makes you the captain of a team, if none exists.";
                ProgHelpers.txt47 = "Resigns your captainship of the team. ";
                ProgHelpers.txt48 = "Team name updated!";
                ProgHelpers.txt49 = "Encountered an error updating the name. Either you're not the captain or an error occured.";
                ProgHelpers.txt50 = "Specify the team name, format NAME.";
                ProgHelpers.txt51 = "You are now the team Captain!";
                ProgHelpers.txt52 = "Team not found, please check team name format.";
                ProgHelpers.txt53 = "That team already has a captain!";
                ProgHelpers.txt54 = "Your user is not in the bot database. Use !btag or !rating to add your Discord id to database.";
                ProgHelpers.txt55 = "You already are a team captain for another team.";
                ProgHelpers.txt56 = "Please specify the team name.";
                ProgHelpers.txt57 = "You are no longer a team captain!";
                ProgHelpers.txt58 = "You are not the captain of any team!";
                ProgHelpers.txt59 = "Encountered an error dropping the captainship.";
                ProgHelpers.txt60 = "Your user is not in the bot database. Use !btag or !rating to add your Discord id to database.";
                ProgHelpers.txt61 = "Player successfully added to the team!";
                ProgHelpers.txt62 = "That player already has a team!";
                ProgHelpers.txt63 = "Specified user is not in bot database. Ask them to use !btag or !rating to add their Discord id to database.";
                ProgHelpers.txt64 = "Encountered an error adding the player to team.";
                ProgHelpers.txt65 = "You are not the captain of the team.";
                ProgHelpers.txt66 = "Discord name format does not seem correct (Format: @name#1234)";
                ProgHelpers.txt67 = "Specify the players name.";
                ProgHelpers.txt68 = "Player successfully removed from team.";
                ProgHelpers.txt69 = "The player is not on your team.";
                ProgHelpers.txt70 = "Specified player not found.";
                ProgHelpers.txt71 = "Encountered an error handling the player.";
                ProgHelpers.txt72 = "Your user is not in the bot database. Use !btag or !rating to add your Discord id to database.";
                ProgHelpers.txt73 = "You are not the captain of the team.";
                ProgHelpers.txt74 = "Discord name format does not seem correct (Format: @name#1234)";
                ProgHelpers.txt75 = "Specify the player's name.";
                ProgHelpers.txt76 = "Starting Match ID:";
                ProgHelpers.txt77 = "Generating Draft links and sending to captains.";
                ProgHelpers.txt78 = "Home team Draft URL:";
                ProgHelpers.txt79 = "Visitor team Draft URL:";
                ProgHelpers.txt80 = "Observer Draft URL for the match:";
                ProgHelpers.txt81 = "have a good one!";
                ProgHelpers.txt82 = "Match not found with given ID.";
                ProgHelpers.txt83 = "Your user is not in the bot database. Use !btag or !rating to add your Discord id to database.";
                ProgHelpers.txt84 = "Your user is not in the teams of the match.";
                ProgHelpers.txt85 = "Your user is not a team captain.";
                ProgHelpers.txt86 = "Your user is not the Home team captain.";
                ProgHelpers.txt87 = "Encountered an error handling the match.";
                ProgHelpers.txt88 = "Visitor team does not have a captain / Match is already ongoing / Has already ended.";
                ProgHelpers.txt89 = "Specify the Match ID, check given information.";
                ProgHelpers.txt90 = "Successfully added the score!";
                ProgHelpers.txt91 = "Successfully added the score!";
                ProgHelpers.txt92 = "Your user is not in the bot database. Use !btag or !rating to add your Discord id to database.";
                ProgHelpers.txt93 = "You are not the captain of the team!";
                ProgHelpers.txt94 = "Successfully added the score!";
                ProgHelpers.txt95 = "Only the Home team captain can add the score!";
                ProgHelpers.txt96 = "An open game where you are the Home team captain was not found.";
                ProgHelpers.txt97 = "The format of the command does not seem correct.";
                ProgHelpers.txt98 = "Please specify the score.";
                ProgHelpers.txt99 = "Match added to database!";
                ProgHelpers.txt100 = "Home team not found from database.";
                ProgHelpers.txt101 = "Visitor team not found from database.";
                ProgHelpers.txt102 = "Command is: +addmatch ID HOME VISITOR";
                ProgHelpers.txt103 = "CAPTAIN CHANGED!";
                ProgHelpers.txt104 = "USER NOT FOUND FROM DATABASE.";
                ProgHelpers.txt105 = "TEAM NOT FOUND FROM DATABASE.";
                ProgHelpers.txt106 = "Encountered an error handling the captainship.";
                ProgHelpers.txt107 = "Command is: +forcecaptain TEAM @NAME#1234";
                ProgHelpers.txt108 = "If this scoreboard is looking non-sensical, use a bigger screen (eg. not a phone)";
                ProgHelpers.txt109 = "TEAM";
                ProgHelpers.txt110 = "DIVISION";
                ProgHelpers.txt111 = "PTS";
                ProgHelpers.txt112 = "MATCHES";
                ProgHelpers.txt113 = "RWON";
                ProgHelpers.txt114 = "RLOSE";
                ProgHelpers.txt115 = "+-";
                ProgHelpers.txt116 = "Winrate";
                ProgHelpers.txt117 = "Time played";

                ProgHelpers.txt118 = "Division";
                ProgHelpers.txt119 = "Team";
                ProgHelpers.txt120 = "Nickname";
                ProgHelpers.txt121 = "No teams in the database yet!";
                ProgHelpers.txt122 = "Prints out all teams in the database.";
                ProgHelpers.txt123 = "Wrong date format, use format DD.MM.YYYY HH:MM.";
                ProgHelpers.txt124 = "(!showtime MATCHID DD.MM.YYYY HH:MM) Home team Captain's command. Specifies the time for the match to start.";
                ProgHelpers.txt125 = "Starting time for match successfully inserted!";
                ProgHelpers.txt126 = "Couldn't find match with given ID!";
                ProgHelpers.txt127 = "Can not postpone match when too close to given starting time!";
                ProgHelpers.txt128 = "You are not the Home team captain.";
                ProgHelpers.txt129 = "Match is already underway or has already been settled!";
                ProgHelpers.txt131 = "Not all required information was given.";

                ProgHelpers.txt132 = "(!attend CITY/TEAM) Marks you as attending to the tournament.";
                ProgHelpers.txt133 = "(!removeattend) Removes your attending status from the tournament.";

                ProgHelpers.txt134 = "(+forcecaptain @NAME#1234 TEAM), forcefully changes the captain of the team.";
                ProgHelpers.txt135 = "(+addmatch CODE HOMETEAM VISITORTEAM), creates a new match to database.";

                ProgHelpers.txt136 = "No player profile was found with the given Btag Id!";

                ProgHelpers.txt130 = "Home team has an unresolved game pending. Use !score HOME VISITOR to give score for the pending game.";
            }

            Program program = new Program();
            program.Run().Wait();
        }

        public async Task Run()
        {
            // Create authenticator using a bot user token.
            DiscordBotUserToken token = new DiscordBotUserToken(ProgHelpers.bottoken); //token

            // Create a WebSocket application.
            DiscordWebSocketApplication app = new DiscordWebSocketApplication(token);

            // Create and start a single shard.
            Shard shard = app.ShardManager.CreateSingleShard();
            await shard.StartAsync(CancellationToken.None);

            // Subscribe to the message creation event.
            shard.Gateway.OnMessageCreated += Gateway_OnMessageCreated;
            Console.WriteLine(DateTime.Now + $" -- kitsun8's DirectorBot Started \n -------------------------");


            // Wait for the shard to end before closing the program.
            while (shard.IsRunning)
                await Task.Delay(1000);
        }

        private static async void Gateway_OnMessageCreated(object sender, MessageEventArgs e)
        {
            Shard shard = e.Shard;
            DiscordMessage message = e.Message;

            //Open helper functions
            KsHelper ksh = new KsHelper();
            
            if (message.Author == shard.User)
                // Ignore messages created by our bot.
                return;

            //-----------------------------------------------------------------------------------------TODO: Attend (Require city as info)
            //if (message.Content == "!attend")
            //{
            //    // Grab the DM or guild text channel this message was posted in from cache.
            //    ITextChannel textChannel = (ITextChannel)shard.Cache.Channels.Get(message.ChannelId);

            //    try
            //    {
            //        var x = message.Author.Id.Id;
            //        var y = message.Author.Username;
            //        //var returnTask = await ksh.Me(x, y);

            //        //if (returnTask.Count < 1)
            //        //{
            //        //    await textChannel.CreateMessage($"<@{message.Author.Id}> " + ProgHelpers.txt1 + "```");
            //        //}
            //        //else
            //        //{
            //        //    await textChannel.CreateMessage(new DiscordMessageDetails()
            //        //        .SetEmbed(new DiscordEmbedBuilder()
            //        //            .SetTitle($"{message.Author.Username} " + ProgHelpers.txt2)
            //        //            .SetColor(DiscordColor.FromHexadecimal(0xff9933))
            //        //            .AddField(ProgHelpers.txt3 + " ", returnTask[0], true)
            //        //            .AddField(ProgHelpers.txt4 + " ", returnTask[1] + "(API: " + returnTask[2] + ")", true)
            //        //            .AddField(ProgHelpers.txt5 + " ", returnTask[4] + "(" + ProgHelpers.txt7 + " " + returnTask[3] + ")", true)
            //        //            .AddField(ProgHelpers.txt6 + " ", returnTask[5] + "/" + returnTask[6] + "/" + returnTask[7], true)
            //        //            .AddField(ProgHelpers.txt116 + " ", returnTask[8] + "%", true)
            //        //            .AddField(ProgHelpers.txt117 + " ", returnTask[9] + "H", true)
            //        //            .SetThumbnail(returnTask[10])
            //        //            ));
            //        //}
            //        Console.WriteLine($"!attend - " + message.Author.Username + "-" + message.Author.Id + " --- " + DateTime.Now);
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine(ex);
            //        Console.WriteLine($"!attend - EX -" + message.Author.Username + "-" + message.Author.Id + " --- " + DateTime.Now);
            //    }
            //}

            //-----------------------------------------------------------------------------------------ME - VALMIS V1
            if (message.Content.ToLower() == "!me")
            {
                // Grab the DM or guild text channel this message was posted in from cache.
                ITextChannel textChannel = (ITextChannel)shard.Cache.Channels.Get(message.ChannelId);

                try
                {
                    var x = message.Author.Id.Id;
                    var y = message.Author.Username;
                    var returnTask = await ksh.Me(x, y);

                    if (returnTask.Count < 1)
                    {
                        await textChannel.CreateMessage($"<@{message.Author.Id}> "+ProgHelpers.txt1+"```");
                    }
                    else
                    {
                        await textChannel.CreateMessage(new DiscordMessageDetails()
                            .SetEmbed(new DiscordEmbedBuilder()
                                .SetTitle($"{message.Author.Username} "+ProgHelpers.txt2)
                                .SetColor(DiscordColor.FromHexadecimal(0xff9933))
                                .AddField(ProgHelpers.txt3+" ",returnTask[0],true)
                                .AddField(ProgHelpers.txt4 + " ",returnTask[1]+"(API: "+ returnTask[2]+")", true)
                                .AddField(ProgHelpers.txt5 + " ", returnTask[4]+"("+ ProgHelpers.txt7 + " "+ returnTask[3]+")", true)
                                .AddField(ProgHelpers.txt6 + " ", returnTask[5]+"/"+ returnTask[6]+"/"+ returnTask[7], true)
                                .AddField(ProgHelpers.txt116+" ",returnTask[8]+ "%", true)
                                .AddField(ProgHelpers.txt117 + " ", returnTask[9]+"H", true)
                                .SetThumbnail(returnTask[10])
                                ));                  
                    }
                    Console.WriteLine($"!me - " + message.Author.Username + "-" + message.Author.Id + " --- " + DateTime.Now);
                }
                catch (Exception ex) {
                    Console.WriteLine(ex);
                    Console.WriteLine($"!me - EX -" + message.Author.Username + "-" + message.Author.Id + " --- " + DateTime.Now);
                }
            }
            //-----------------------------------------------------------------------------------------TEAMS - VALMIS V1
            if (message.Content.ToLower() == "!teams")
            {
                // Grab the DM or guild text channel this message was posted in from cache.
                ITextChannel textChannel = (ITextChannel)shard.Cache.Channels.Get(message.ChannelId);

                try
                {
                    var returnTask = await ksh.Teamslist();

                    if (returnTask != "0")
                    {
                        await textChannel.CreateMessage($"```" + returnTask + "\n" + "```");
                    }
                    else
                    {
                        await textChannel.CreateMessage($"<@{message.Author.Id}> " + ProgHelpers.txt121); 
                    }
                    Console.WriteLine($"!teams - " + message.Author.Username + "-" + message.Author.Id + " --- " + DateTime.Now);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    Console.WriteLine($"!teams - EX -" + message.Author.Username + "-" + message.Author.Id + " --- " + DateTime.Now);
                }
            }
            //-----------------------------------------------------------------------------------------RATING - VALMIS V1
            if (message.Content.ToLower().StartsWith("!rating"))
            {
                var msg = message.Content;
                string[] msgsp = msg.Split(null);
                ITextChannel textChannel = (ITextChannel)shard.Cache.Channels.Get(message.ChannelId);
                int index = 1;
                if (index < msgsp.Length)
                {
                    try
                    {
                        var x = message.Author.Id.Id;
                        int y = 0;
                        string z = message.Author.Username;
                        if (Int32.TryParse(msgsp[1], out y))
                        {
                            var result = await ksh.Rating(x, y, z);
                            if (result == true)
                            {
                                await textChannel.CreateMessage($"<@{message.Author.Id}> "+ProgHelpers.txt8+" " + (msgsp[1]));
                            }
                            else
                            {
                                await textChannel.CreateMessage($"<@{message.Author.Id}> "+ProgHelpers.txt9);
                            }
                            Console.WriteLine($"!rating - " + message.Author.Username + "-" + message.Author.Id + " --- " + DateTime.Now);
                        }

                    }
                    catch (Exception)
                    {
                        Console.WriteLine($"!rating - EX - " + message.Author.Username + "-" + message.Author.Id + " --- " + DateTime.Now);
                    }
                }
                else
                {
                    await textChannel.CreateMessage($"<@{message.Author.Id}> "+ProgHelpers.txt10);
                    Console.WriteLine($"!rating - " + message.Author.Username + "-" + message.Author.Id + " --- " + DateTime.Now);
                }

            }
            //-----------------------------------------------------------------------------------------BTAG - VALMIS V1
            if (message.Content.ToLower().StartsWith("!btag"))
            {
                var msg = message.Content;
                string[] msgsp = msg.Split(null);
                ITextChannel textChannel = (ITextChannel)shard.Cache.Channels.Get(message.ChannelId);
                int index = 1;
                if (index < msgsp.Length)
                {
                    try
                    {
                        var x = message.Author.Id.Id;
                        string y = "";
                        string z = message.Author.Username;
                        y = msgsp[1];
                        var result = await ksh.Btag(x, y, z);
                        if (result == 1)
                        {
                            // Reply to the user who posted "!btag".
                            await textChannel.CreateMessage($"<@{message.Author.Id}> " + ProgHelpers.txt11 + " " + (msgsp[1]));
                        }
                        if (result == 3)
                        {
                            await textChannel.CreateMessage($"<@{message.Author.Id}> " + ProgHelpers.txt136 + " " + (msgsp[1]));
                        }
                        if (result == 0)
                        {
                            // Reply to the user who posted "!btag".
                            await textChannel.CreateMessage($"<@{message.Author.Id}> " + ProgHelpers.txt12);
                        }

                        Console.WriteLine($"!btag - " + message.Author.Username + "-" + message.Author.Id + " --- " + DateTime.Now);
                    }
                    catch (Exception) { Console.WriteLine($"!btag - EX -" + message.Author.Username + "-" + message.Author.Id + " --- " + DateTime.Now); }
                }
                else
                {
                    await textChannel.CreateMessage($"<@{message.Author.Id}> "+ProgHelpers.txt13);
                    Console.WriteLine($"!btag - " + message.Author.Username + "-" + message.Author.Id + " --- " + DateTime.Now);
                }

            }
            //-----------------------------------------------------------------------------------------ROSTER - VALMIS V1
            if (message.Content.ToLower().StartsWith("!roster"))
            {
                var msg = message.Content;
                string[] msgsp = msg.Split(null);
                ITextChannel textChannel = (ITextChannel)shard.Cache.Channels.Get(message.ChannelId);
                var y = "";
                int index = 1;
                if (index < msgsp.Length)
                {
                    y = msgsp[1].ToUpper();
                    var x = message.Author.Id.Id;
                    try
                    {
                        var returnTask = await ksh.Roster(x, y);
                        int returncount = returnTask.Count - 3;
                        if (returnTask.Count < 1)
                        {
                            await textChannel.CreateMessage($"<@{message.Author.Id}> "+ProgHelpers.txt14);
                        }
                        else
                        {

                            if (returncount > 0)
                            {
                                var playerstoSend = "";
                                for (var iz = 3; iz < returnTask.Count; iz++) { playerstoSend = playerstoSend + (returnTask[iz]) + "\n"; };
                                await textChannel.CreateMessage(new DiscordMessageDetails()
                            .SetEmbed(new DiscordEmbedBuilder()
                            .SetTitle(ProgHelpers.txt15)
                            .SetColor(DiscordColor.FromHexadecimal(0xff9933))
                            .AddField(ProgHelpers.txt16+" ", returnTask[0], true)
                            .AddField(ProgHelpers.txt17+" ", returnTask[1], true)
                            .AddField(ProgHelpers.txt18+" ", returnTask[2], true)
                            .AddField(ProgHelpers.txt19+" ", playerstoSend, false)

                            ));

                            }
                            else
                            {
                                await textChannel.CreateMessage(new DiscordMessageDetails()
                            .SetEmbed(new DiscordEmbedBuilder()
                            .SetTitle(ProgHelpers.txt20)
                            .SetColor(DiscordColor.FromHexadecimal(0xff9933))
                            .AddField(ProgHelpers.txt21+" ", returnTask[0], true)
                            .AddField(ProgHelpers.txt22+" ", returnTask[1], true)
                            .AddField(ProgHelpers.txt23+" ", returnTask[2], true)

                            ));
                                //success
                                //await textChannel.CreateMessage($"<@{message.Author.Id}> Joukkueen tiedot: \n" +
                                //    $"```Kaupunki: " + returnTask[0] + "\n" +
                                //    $"Joukkueen nimi: " + returnTask[1] + "\n" +
                                //    $"Kapteeni: " + returnTask[2] + " \n" +
                                //    $"```");
                            }
                        }
                        Console.WriteLine($"!roster - " + message.Author.Username + "-" + message.Author.Id + " --- " + DateTime.Now);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine($"!roster - " + message.Author.Username + "-" + message.Author.Id + " --- " + DateTime.Now);
                    }
                }
                else
                {
                    await textChannel.CreateMessage($"<@{message.Author.Id}> "+ProgHelpers.txt24);
                    Console.WriteLine($"!roster - " + message.Author.Username + "-" + message.Author.Id + " --- " + DateTime.Now);
                }
            }
            //-----------------------------------------------------------------------------------------STANDINGS - VALMIS V1
            if (message.Content.ToLower() == "!standings" || message.Content.ToLower() == "!scoreboard")
            {
                ITextChannel textChannel = (ITextChannel)shard.Cache.Channels.Get(message.ChannelId);

                try
                {
                    var returnlist = await ksh.Standings();
                    // Reply to the user who posted "!standings".
                    if (returnlist != null)
                    {
                        await textChannel.CreateMessage($"```"+returnlist+"\n"+ProgHelpers.txt108+"```");
                    }
                    else
                    {
                        await textChannel.CreateMessage($"<@{message.Author.Id}> Sarjataulukko on toistaiseksi tyhjä!");
                    }

                    Console.WriteLine("!scoreboard - " + message.Author.Username + "-" + message.Author.Id + " --- " + DateTime.Now);
                }
                catch (Exception) { Console.WriteLine("!scoreboard - EX -" + message.Author.Username + "-" + message.Author.Id + " --- " + DateTime.Now); }
            }
            //-----------------------------------------------------------------------------------------INFO - VALMIS V1
            if (message.Content.ToLower() == "!info")
            {
                ITextChannel textChannel = (ITextChannel)shard.Cache.Channels.Get(message.ChannelId);

                try
                {

                    // Reply to the user who posted "!info".
                    await textChannel.CreateMessage(new DiscordMessageDetails()
                     .SetEmbed(new DiscordEmbedBuilder()
                     .SetTitle($"kitsun8 DirectorBot")
                     .SetFooter("Discore (.NET Core), C#, SQL Server, "+ProgHelpers.txtversion)
                     .SetColor(DiscordColor.FromHexadecimal(0xff9933))
                     .AddField(ProgHelpers.txt26+" ", "kitsun8#4567", false)
                     .AddField(ProgHelpers.txt27+" ", ProgHelpers.txt28, false)
                     .AddField(ProgHelpers.txt29+" ", ProgHelpers.txt30, false)
                     .AddField(ProgHelpers.txt31+" ", ProgHelpers.txt32, false)
                    ));

                    Console.WriteLine($"!info - " + message.Author.Username + "-" + message.Author.Id + " --- " + DateTime.Now);
                }
                catch (Exception) { Console.WriteLine($"!info - EX -" + message.Author.Username + "-" + message.Author.Id + " --- " + DateTime.Now); }
            }
            //-----------------------------------------------------------------------------------------HELP - VALMIS V1
            if (message.Content.ToLower() == "!help")
            {
                ITextChannel textChannel = (ITextChannel)shard.Cache.Channels.Get(message.ChannelId);

                try
                {

                    //SEND !HELP LIST AS A DIRECTMESSAGE BECAUSE IT IS SO LONG

                    DiscordDMChannel dmsend = await shard.Application.HttpApi.Users.CreateDM(message.Author.Id);
                    await dmsend.CreateMessage(new DiscordMessageDetails()
                    .SetEmbed(new DiscordEmbedBuilder()
                    .SetTitle(ProgHelpers.txt33)
                    .SetFooter(ProgHelpers.txt34)
                    .SetColor(DiscordColor.FromHexadecimal(0xff9933))
                    .AddField("!info", ProgHelpers.txt35, false)
                    .AddField("!standings/scoreboard", ProgHelpers.txt36, false)
                    .AddField("!me", ProgHelpers.txt37, false)
                    .AddField("!btag", ProgHelpers.txt38, false)
                    .AddField("!rating", ProgHelpers.txt39, false)
                    //.AddField("!attend", ProgHelpers.txt132, false) //Marks person as attending
                    //.AddField("!removeattend", ProgHelpers.txt133, false) //Marks person as not attending
                    .AddField("!teams", ProgHelpers.txt122, false) //Lists all teams in database
                    .AddField("!roster", ProgHelpers.txt40, false)
                    .AddField("!teamname", ProgHelpers.txt41, false)
                    .AddField("!start", ProgHelpers.txt42, false)
                    .AddField("!showtime", ProgHelpers.txt124, false) //Starting time for the match
                    .AddField("!score", ProgHelpers.txt43, false)
                    .AddField("!add", ProgHelpers.txt44, false)
                    .AddField("!remove", ProgHelpers.txt45, false)
                    .AddField("!captain", ProgHelpers.txt46, false)
                    .AddField("!resign", ProgHelpers.txt47, false)
                    .AddField("+forcecaptain",ProgHelpers.txt134,false)
                    .AddField("+addmatch",ProgHelpers.txt135,false)
                    //.AddField("+addteam",ProgHelpers.txtxxx,false) //adds team to database
                    //.AddField("+removeteam",ProgHelpers.txtxxx,false) //removes team from database
                    //.AddField("+renameteam",ProgHelpers.txtxxx,false) //Rename the base of name (TEAM), not nickname.
                    //.AddField("+eliminateteam",ProgHelpers.txtxxx,false) //Mark team to be played out (not enough points for playoffs, or dropped off from playoffs)
                    //.AddField("+attending",ProgHelpers.txtxxx,false) //open/close attending
                    //.AddField("+division", ProgHelpers.txtxxx, false) //assign division to team
                    //.AddField("+report", ProgHelpers.txtxxx, false) //attending/??
                    ));

                    Console.WriteLine($"!help - " + message.Author.Username + "-" + message.Author.Id + " --- " + DateTime.Now);
                }
                catch (Exception) { Console.WriteLine($"!help - EX -" + message.Author.Username + "-" + message.Author.Id + " --- " + DateTime.Now); }
            }

            //-----------------------------------------------------------------------------------------#KAPTEENIT CHANNEL
            if (message.ChannelId.Id.ToString() == ProgHelpers.captainchannel)//
            {
                //-----------------------------------------------------------------------------------------SHOWTIME - CHECK IF READY V1
                if (message.Content.ToLower().StartsWith("!showtime"))
                {
                    var msg = message.Content;
                    string[] msgsp = msg.Split(null);
                    ITextChannel textChannel = (ITextChannel)shard.Cache.Channels.Get(message.ChannelId);
                    int index = 3;
                    if (index < msgsp.Length)
                    {
                        try
                        {
                            var x = message.Author.Id.Id;
                            var matchid = msgsp[1].ToUpper(); //Update 09.06.2017 - MatchID to UPPERcase
                            var datestr = msgsp[2];
                            var timestr = msgsp[3];

                            DateTime outputDateTimeValue;
                            if (DateTime.TryParseExact(datestr + " " + timestr, "dd.MM.yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out outputDateTimeValue))
                            {
                                var result = await ksh.Showtime(x,matchid, outputDateTimeValue);
                                if (result == 1)
                                {
                                    await textChannel.CreateMessage($"<@{message.Author.Id}> " + ProgHelpers.txt125);
                                    //OK
                                }
                                if (result == 2)
                                {
                                    await textChannel.CreateMessage($"<@{message.Author.Id}> " + ProgHelpers.txt126);
                                    //MATCH ei löytynyt
                                }
                                if (result == 3)
                                {
                                    await textChannel.CreateMessage($"<@{message.Author.Id}> " + ProgHelpers.txt127);
                                    //Liian lähellä deadlinea
                                }
                                if (result == 4)
                                {
                                    await textChannel.CreateMessage($"<@{message.Author.Id}> " + ProgHelpers.txt128);
                                    //Et ole Kotijoukkueen kapteeni
                                }
                                if (result == 5)
                                {
                                    await textChannel.CreateMessage($"<@{message.Author.Id}> " + ProgHelpers.txt129);
                                    //Peli meneillään tai mennyt
                                }
                            }
                            else
                            {
                                await textChannel.CreateMessage($"<@{message.Author.Id}> " + ProgHelpers.txt123);
                                Console.WriteLine("!showtime - EX - Timeformat BAD");
                            }
                        }
                        catch (Exception)
                        {

                        }

                    }
                    else
                    {
                        await textChannel.CreateMessage($"<@{message.Author.Id}> " + ProgHelpers.txt131);
                    }
                }
                //-----------------------------------------------------------------------------------------TEAMNAME - VALMIS V1
                if (message.Content.ToLower().StartsWith("!teamname"))
            {
                var msg = message.Content;
                string[] msgsp = msg.Split(null);
                ITextChannel textChannel = (ITextChannel)shard.Cache.Channels.Get(message.ChannelId);
                int index = 1;
                if (index < msgsp.Length)
                {
                    try
                    {
                        var x = message.Author.Id.Id;
                        string y = "";
                        y = msgsp[1];
                        var teamnamewithspaces = msg.Remove(0, msg.IndexOf(' ') + 1);
                        var result = await ksh.Teamname(x, teamnamewithspaces);
                        if (result == true)
                        {
                            // Reply to the user who posted "!teamname".
                            await textChannel.CreateMessage($"<@{message.Author.Id}> "+ProgHelpers.txt48);
                        }
                        else
                        {
                            // Reply to the user who posted "!teamname".
                            await textChannel.CreateMessage($"<@{message.Author.Id}> "+ProgHelpers.txt49);
                        }

                        Console.WriteLine($"!teamname - " + message.Author.Username + "-" + message.Author.Id + " --- " + DateTime.Now);
                    }
                    catch (Exception) { Console.WriteLine($"!teamname - EX -" + message.Author.Username + "-" + message.Author.Id + " --- " + DateTime.Now); }
                }
                else
                {
                    await textChannel.CreateMessage($"<@{message.Author.Id}> "+ProgHelpers.txt50);
                    Console.WriteLine($"!teamname - " + message.Author.Username + "-" + message.Author.Id + " --- " + DateTime.Now);
                }

            }
                //-----------------------------------------------------------------------------------------BECOMECAPTAIN - VALMIS V1
                if (message.Content.ToLower().StartsWith("!captain"))
                {
                    var msg = message.Content;
                    string[] msgsp = msg.Split(null);
                    ITextChannel textChannel = (ITextChannel)shard.Cache.Channels.Get(message.ChannelId);
                    var y = "";
                    int index = 1;
                    if (index < msgsp.Length)
                    {
                        y = msgsp[1].ToUpper();
                        var x = message.Author.Id.Id;
                        var z = message.Author.Username;
                        try
                        {
                            var returnTask = await ksh.Becomecaptain(x, z, y);
                            //var returnTask = 0;

                            if (returnTask == 1)
                            {
                                await textChannel.CreateMessage($"<@{message.Author.Id}> "+ProgHelpers.txt51);
                            }
                            if (returnTask == 2)
                            {
                                await textChannel.CreateMessage($"<@{message.Author.Id}> "+ProgHelpers.txt52);
                            }
                            if (returnTask == 3)
                            {
                                await textChannel.CreateMessage($"<@{message.Author.Id}> "+ProgHelpers.txt53);
                            }
                            if (returnTask == 4)
                            {
                                await textChannel.CreateMessage($"<@{message.Author.Id}> "+ProgHelpers.txt54);
                            }
                            if (returnTask == 5)
                            {
                                await textChannel.CreateMessage($"<@{message.Author.Id}> "+ProgHelpers.txt55);
                            }
                            Console.WriteLine($"!becomecaptain - " + message.Author.Username + "-" + message.Author.Id + " --- " + DateTime.Now);
                        }
                        catch (Exception)
                        {
                            Console.WriteLine($"!becomecaptain - " + message.Author.Username + "-" + message.Author.Id + " --- " + DateTime.Now);
                        }
                    }
                    else
                    {
                        await textChannel.CreateMessage($"<@{message.Author.Id}> "+ProgHelpers.txt56);
                        Console.WriteLine($"!becomecaptain - " + message.Author.Username + "-" + message.Author.Id + " --- " + DateTime.Now);
                    }
                }
                //-----------------------------------------------------------------------------------------RESIGNCAPTAIN - VALMIS V1
                if (message.Content.ToLower() == "!resign")
                {
                    ITextChannel textChannel = (ITextChannel)shard.Cache.Channels.Get(message.ChannelId);

                    var x = message.Author.Id.Id;
                    var z = message.Author.Username;
                    try
                    {
                        var returnTask = await ksh.Resigncaptain(x, z);
                        //var returnTask = 0;

                        if (returnTask == 1)
                        {
                            await textChannel.CreateMessage($"<@{message.Author.Id}> "+ProgHelpers.txt57);
                        }
                        if (returnTask == 2)
                        {
                            await textChannel.CreateMessage($"<@{message.Author.Id}> "+ProgHelpers.txt58);
                        }
                        if (returnTask == 3)
                        {
                            await textChannel.CreateMessage($"<@{message.Author.Id}> "+ProgHelpers.txt59);
                        }
                        if (returnTask == 4)
                        {
                            await textChannel.CreateMessage($"<@{message.Author.Id}> "+ProgHelpers.txt60);
                        }
                        Console.WriteLine($"!resigncaptain - " + message.Author.Username + "-" + message.Author.Id + " --- " + DateTime.Now);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine($"!resigncaptain - " + message.Author.Username + "-" + message.Author.Id + " --- " + DateTime.Now);
                    }
                }
                //-----------------------------------------------------------------------------------------ADDMEMBER - VALMIS V1
                if (message.Content.ToLower().StartsWith("!add"))
                {
                    var msg = message.Content;
                    string[] msgsp = msg.Split(null);
                    ITextChannel textChannel = (ITextChannel)shard.Cache.Channels.Get(message.ChannelId);
                    var cUpper = "";
                    int index = 1;
                    if (index < msgsp.Length)
                    {
                        cUpper = msgsp[1].ToUpper();
                        cUpper = cUpper.Replace("<", "");
                        cUpper = cUpper.Replace("@", "");
                        cUpper = cUpper.Replace(">", "");
                        if (cUpper.All(char.IsDigit))
                        {
                            try
                            {
                                var x = message.Author.Id.Id;
                                var result = await ksh.Addmember(x, cUpper);

                                if (result == 1)
                                {
                                    //success
                                    await textChannel.CreateMessage($"<@{message.Author.Id}> "+ProgHelpers.txt61);
                                }
                                if (result == 2)
                                {
                                    //alreadyinteam
                                    await textChannel.CreateMessage($"<@{message.Author.Id}> "+ProgHelpers.txt62);
                                }
                                if (result == 3)
                                {
                                    //notfound text
                                    await textChannel.CreateMessage($"<@{message.Author.Id}> "+ProgHelpers.txt63);
                                }
                                if (result == 4)
                                {
                                    //error
                                    await textChannel.CreateMessage($"<@{message.Author.Id}> "+ProgHelpers.txt64);
                                }
                                if (result == 5)
                                {
                                    //error
                                    await textChannel.CreateMessage($"<@{message.Author.Id}> "+ProgHelpers.txt65);
                                }

                                Console.WriteLine($"!addmember - " + message.Author.Username + "-" + message.Author.Id + " --- " + DateTime.Now);
                            }
                            catch (Exception)
                            {
                                Console.WriteLine($"!addmember - EX - " + message.Author.Username + "-" + message.Author.Id + " --- " + DateTime.Now);
                            }
                        }
                        else
                        {
                            await textChannel.CreateMessage($"<@{message.Author.Id}> "+ProgHelpers.txt66);
                        }


                    }
                    else
                    {
                        await textChannel.CreateMessage($"<@{message.Author.Id}> "+ProgHelpers.txt67);
                        Console.WriteLine($"!addmember - " + message.Author.Username + "-" + message.Author.Id + " --- " + DateTime.Now);
                    }
                }
                //-----------------------------------------------------------------------------------------REMOVEMEMBER - VALMIS V1
                if (message.Content.ToLower().StartsWith("!remove"))
                {
                    var msg = message.Content;
                    string[] msgsp = msg.Split(null);
                    ITextChannel textChannel = (ITextChannel)shard.Cache.Channels.Get(message.ChannelId);
                    var cUpper = "";
                    int index = 1;
                    if (index < msgsp.Length)
                    {
                        cUpper = msgsp[1].ToUpper();
                        cUpper = cUpper.Replace("<", "");
                        cUpper = cUpper.Replace("@", "");
                        cUpper = cUpper.Replace(">", "");
                        if (cUpper.All(char.IsDigit))
                        {
                            try
                            {
                                var x = message.Author.Id.Id;
                                var result = await ksh.Removemember(x, cUpper);

                                if (result == 1)
                                {
                                    //success
                                    await textChannel.CreateMessage($"<@{message.Author.Id}> "+ProgHelpers.txt68);
                                }
                                if (result == 2)
                                {
                                    //notyourplayer
                                    await textChannel.CreateMessage($"<@{message.Author.Id}> "+ProgHelpers.txt69);
                                }
                                if (result == 3)
                                {
                                    //notfound text
                                    await textChannel.CreateMessage($"<@{message.Author.Id}> "+ProgHelpers.txt70);
                                }
                                if (result == 4)
                                {
                                    //notfound text
                                    await textChannel.CreateMessage($"<@{message.Author.Id}> "+ProgHelpers.txt71);
                                }
                                if (result == 5)
                                {
                                    //notfound text
                                    await textChannel.CreateMessage($"<@{message.Author.Id}> "+ProgHelpers.txt72);
                                }
                                if (result == 6)
                                {
                                    //not captain
                                    await textChannel.CreateMessage($"<@{message.Author.Id}> "+ProgHelpers.txt73);
                                }


                                Console.WriteLine($"!removemember - " + message.Author.Username + "-" + message.Author.Id + " --- " + DateTime.Now);
                            }
                            catch (Exception) { Console.WriteLine($"!removemember - EX -" + message.Author.Username + "-" + message.Author.Id + " --- " + DateTime.Now); }
                        }
                        else
                        {
                            await textChannel.CreateMessage($"<@{message.Author.Id}> "+ProgHelpers.txt74);
                        }
                    }
                    else
                    {
                        await textChannel.CreateMessage($"<@{message.Author.Id}> "+ProgHelpers.txt75);
                        Console.WriteLine($"!removemember - " + message.Author.Username + "-" + message.Author.Id + " --- " + DateTime.Now);
                    }
                }
                //-----------------------------------------------------------------------------------------START - VALMIS V1
                if (message.Content.ToLower().StartsWith("!start"))
                {
                    var msg = message.Content;
                    string[] msgsp = msg.Split(null);
                    ITextChannel textChannel = (ITextChannel)shard.Cache.Channels.Get(message.ChannelId);
                    int index = 1;
                    if (index < msgsp.Length)
                    {
                        try
                        {
                            var code = msgsp[1].ToUpper(); //Update 09.06.2017 - Put string to uppercase
                            var x = message.Author.Id.Id;
                            var result = await ksh.Start(x, code);
                            var resultA = await ksh.OWdraftHelp(code);
                            var resultB = await ksh.OWdraftRet(code);
                            if (result == 1)
                            {
                                // Reply to the user who posted "!start".
                                await textChannel.CreateMessage($"<@{message.Author.Id}> "+ProgHelpers.txt76 +" "+ code + "\n "+ProgHelpers.txt77);
                                if (resultA.Count > 0)
                                {

                                    if (resultB.Count > 0)
                                    {
                                        var res1a = resultA[0];
                                        var res2a = resultA[1];

                                        ulong res1 = (ulong)Convert.ToInt64(res1a);
                                        ulong res2 = (ulong)Convert.ToInt64(res2a);

                                        Snowflake dm1 = new Snowflake();
                                        dm1.Id = res1;
                                        Snowflake dm2 = new Snowflake();
                                        dm2.Id = res2;

                                        DiscordDMChannel dmms1 = await shard.Application.HttpApi.Users.CreateDM(dm1);
                                        await dmms1.CreateMessage(ProgHelpers.txt78+" " + resultB[0]);
                                        DiscordDMChannel dmms2 = await shard.Application.HttpApi.Users.CreateDM(dm2);
                                        await dmms2.CreateMessage(ProgHelpers.txt79+" " + resultB[1]);

                                        await textChannel.CreateMessage(ProgHelpers.txt80+" " + resultB[2] + ", "+ProgHelpers.txt81);

                                    }
                                }
                            }
                            if (result == 2)
                            {
                                await textChannel.CreateMessage($"<@{message.Author.Id}> "+ProgHelpers.txt82);
                            }
                            if (result == 3)
                            {
                                await textChannel.CreateMessage($"<@{message.Author.Id}> "+ProgHelpers.txt83);
                            }
                            if (result == 4)
                            {
                                await textChannel.CreateMessage($"<@{message.Author.Id}> "+ProgHelpers.txt84);
                            }
                            if (result == 5)
                            {
                                await textChannel.CreateMessage($"<@{message.Author.Id}> "+ProgHelpers.txt85);
                            }
                            if (result == 6)
                            {
                                await textChannel.CreateMessage($"<@{message.Author.Id}> "+ProgHelpers.txt86);
                            }
                            if (result == 7)
                            {
                                await textChannel.CreateMessage($"<@{message.Author.Id}> "+ProgHelpers.txt87);
                                Console.WriteLine($"!start - EX - 7");
                            }
                            if (result == 8)
                            {
                                await textChannel.CreateMessage($"<@{message.Author.Id}> "+ProgHelpers.txt88);
                            }
                            if (result == 9)
                            {
                                await textChannel.CreateMessage($"<@{message.Author.Id}> " + ProgHelpers.txt130);
                            }

                            Console.WriteLine($"!start - " + message.Author.Username + "-" + message.Author.Id + " --- " + DateTime.Now);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                            Console.WriteLine($"!start - EX - " + message.Author.Username + "-" + message.Author.Id + " --- " + DateTime.Now);
                        }
                    }
                    else
                    {
                        await textChannel.CreateMessage($"<@{message.Author.Id}> "+ProgHelpers.txt89);
                        Console.WriteLine($"!start - " + message.Author.Username + "-" + message.Author.Id + " --- " + DateTime.Now);
                    }

                }
                //-----------------------------------------------------------------------------------------SCORE - VALMIS V1
                if (message.Content.ToLower().StartsWith("!score"))
                {
                    var msg = message.Content;
                    string[] msgsp = msg.Split(null);
                    ITextChannel textChannel = (ITextChannel)shard.Cache.Channels.Get(message.ChannelId);
                    int index = 2;

                    if (index < msgsp.Length)
                    {
                        //scoren lisätarkistus
                        if (msgsp[1].All(char.IsDigit) && msgsp[2].All(char.IsDigit))
                        {
                            var mr1 = "";
                            var mr2 = "";
                            mr1 = msgsp[1];
                            mr2 = msgsp[2];

                            try
                            {
                                var x = message.Author.Id.Id;
                                var result = await ksh.Score(x, mr1, mr2);
                                if (result == 1)
                                {
                                    //success
                                    await textChannel.CreateMessage($"<@{message.Author.Id}> "+ProgHelpers.txt90);
                                }
                                if (result == 2)
                                {
                                    await textChannel.CreateMessage($"<@{message.Author.Id}> "+ProgHelpers.txt91);
                                }
                                if (result == 3)
                                {
                                    await textChannel.CreateMessage($"<@{message.Author.Id}> "+ProgHelpers.txt92);
                                }
                                if (result == 4)
                                {
                                    await textChannel.CreateMessage($"<@{message.Author.Id}> "+ProgHelpers.txt93);
                                }
                                if (result == 5)
                                {
                                    await textChannel.CreateMessage($"<@{message.Author.Id}> "+ProgHelpers.txt94);
                                }
                                if (result == 6)
                                {
                                    await textChannel.CreateMessage($"<@{message.Author.Id}> "+ProgHelpers.txt95);
                                }
                                if (result == 7)
                                {
                                    await textChannel.CreateMessage($"<@{message.Author.Id}> "+ProgHelpers.txt96);
                                }
                                //get user captainship, then check active matches, then apply score. from DB, if no match, return error
                                Console.WriteLine($"!score - " + message.Author.Username + "-" + message.Author.Id + " --- " + DateTime.Now);
                            }
                            catch (Exception)
                            {
                                Console.WriteLine($"!score - EX -" + message.Author.Username + "-" + message.Author.Id + " --- " + DateTime.Now);
                            }
                        }
                        else
                        {
                            await textChannel.CreateMessage($"<@{message.Author.Id}> "+ProgHelpers.txt97);
                        }   
                    }
                    else
                    {
                        await textChannel.CreateMessage($"<@{message.Author.Id}> "+ProgHelpers.txt98);
                        Console.WriteLine($"!score - " + message.Author.Username + "-" + message.Author.Id + " --- " + DateTime.Now);
                    }
                }
            }

            //-----------------------------------------------------------------------------------------#ADMINS CHANNEL
            if (message.ChannelId.Id.ToString() == ProgHelpers.adminchannel)
            {
                //-----------------------------------------------------------------------------------------+ADDMATCH - VALMIS V1
                if (message.Content.ToLower().StartsWith("+addmatch"))
                {
                    var msg = message.Content;
                    string[] msgsp = msg.Split(null);
                    ITextChannel textChannel = (ITextChannel)shard.Cache.Channels.Get(message.ChannelId);

                    int index = 3;
                    var code = "";
                    var mt1 = "";
                    var mt2 = "";
                    if (index < msgsp.Length)
                    {
                        code = msgsp[1].ToUpper();
                        mt1 = msgsp[2].ToUpper();
                        mt2 = msgsp[3].ToUpper();
                        try
                        {
                            var x = message.Author.Id.Id;
                            var result = await ksh.Addmatch(x, code, mt1, mt2);
                            if (result == 1)
                            {
                                await textChannel.CreateMessage($"<@{message.Author.Id}> "+ProgHelpers.txt99);
                            }
                            if (result == 2)
                            {
                                await textChannel.CreateMessage($"<@{message.Author.Id}> "+ProgHelpers.txt100);
                            }
                            if (result == 3)
                            {
                                await textChannel.CreateMessage($"<@{message.Author.Id}> "+ProgHelpers.txt101);
                            }

                            Console.WriteLine($"!addmatch - " + message.Author.Username + "-" + message.Author.Id + " --- " + DateTime.Now);
                        }
                        catch
                        {
                            Console.WriteLine($"!addmatch - EX - " + message.Author.Username + "-" + message.Author.Id + " --- " + DateTime.Now);
                        }
                    }
                    else
                    {
                        await textChannel.CreateMessage($"<@{message.Author.Id}> "+ProgHelpers.txt102);
                    }
                }
                //-----------------------------------------------------------------------------------------+FORCECAPTAIN
                if (message.Content.ToLower().StartsWith("+forcecaptain"))
                {
                    var msg = message.Content;
                    string[] msgsp = msg.Split(null);
                    ITextChannel textChannel = (ITextChannel)shard.Cache.Channels.Get(message.ChannelId);

                    int index = 2;

                    if (index < msgsp.Length)
                    {
                        var city = "";
                        var cUpper = "";
                        city = msgsp[1].ToUpper();
                        cUpper = msgsp[2].ToUpper();
                        cUpper = cUpper.Replace("<", "");
                        cUpper = cUpper.Replace("@", "");
                        cUpper = cUpper.Replace(">", "");
                        if (cUpper.All(char.IsDigit))
                        {

                        }

                        try
                        {
                            var x = message.Author.Id.Id;
                            var result = await ksh.Forcecaptain(city, cUpper);
                            if (result == 1)
                            {
                                await textChannel.CreateMessage($"<@{message.Author.Id}> "+ProgHelpers.txt103);
                            }
                            if (result == 2)
                            {
                                await textChannel.CreateMessage($"<@{message.Author.Id}> "+ProgHelpers.txt104);
                            }
                            if (result == 3)
                            {
                                await textChannel.CreateMessage($"<@{message.Author.Id}> "+ProgHelpers.txt105);
                            }
                            if (result == 4)
                            {
                                await textChannel.CreateMessage($"<@{message.Author.Id}> "+ProgHelpers.txt106);
                            }

                            Console.WriteLine($"+forcecaptain - " + message.Author.Username + "-" + message.Author.Id + " --- " + DateTime.Now);
                        }
                        catch
                        {
                            Console.WriteLine($"+forcecaptain - EX - " + message.Author.Username + "-" + message.Author.Id + " --- " + DateTime.Now);
                        }
                    }
                    else
                    {
                        await textChannel.CreateMessage($"<@{message.Author.Id}> "+ProgHelpers.txt107);
                    }
                }
            }

        }
    }
}