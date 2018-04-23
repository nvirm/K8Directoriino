# K8Directoriino
![alt text](https://github.com/kitsun8/K8Directoriino/blob/master/screenshots/directoriino1.PNG)

# !! Please note: Development version, some functions do not work.!!
- You're free to use this as a foundation for your project, but keep in mind that it is not complete.

An Overwatch custom tournament/league bot for Discord
Includes: Regular season point system, Team management, Map draft link generating, Player Competitive stats fetching and storing

# What it does

- Team management
- Scoreboarding for regular season, allows use of multiple divisions
- Map draft URL generation
- Overwatch Competitive stats loading for users 
- Discord user profiling
- Match logging and listing

# Work in progress
- Playoffs system
- Elimination system for eliminated teams from regular season and playoffs
- Attending system for Pre-Season
- Possibly moving from OWDraft to another map drafting system (because of instability in OWDraft)
- More Admin commands, making SQL server updates less needed.

# Details

Bot is running on Discore 2.4.1 (https://github.com/BundledSticksInkorperated/Discore)

Bot utilizes OW API (https://github.com/SunDwarf/OWAPI) instance that you can either host yourself or use the API the developer provides.

Project is running on .NET Core 1.1 / .NET Standard 1.6

Language support added for english, default/original language was finnish. Can be changed from appsettings.json

