using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace JanssenBot
{
    public class APIThing
    {
        public static RoomState GetRoomInfo(int channel)
        {
            string result;
            string page = $"https://osu.ppy.sh/api/get_match?k={MainWindow.apiKey}&mp={channel}";

            using (WebClient wc = new WebClient())
            {
                result = wc.DownloadString(page);
            }

            RoomState room = JsonConvert.DeserializeObject<RoomState>(result);
            return room;
        }

        public static MapInfo GetMapInfo(string mapLink, int mode)
        {
            string result;
            string[] split = mapLink.Split('/');
            string mapId = split[split.Count() - 1];
            string page = $"https://osu.ppy.sh/api/get_beatmaps?k={MainWindow.apiKey}&b={mapId}&m={mode}&a=0";
            using (WebClient wc = new WebClient())
            {
                result = wc.DownloadString(page);
            }

            List<MapInfo> info = JsonConvert.DeserializeObject<List<MapInfo>>(result);
            return info[0];
        }
    }

    public class Match
    {
        public string match_id { get; set; }
        public string name { get; set; }
        public string start_time { get; set; }
        public string end_time { get; set; }
    }

    public class Score
    {
        public string slot { get; set; }
        public string team { get; set; }
        public string user_id { get; set; }
        public string score { get; set; }
        public string maxcombo { get; set; }
        public string rank { get; set; }
        public string count50 { get; set; }
        public string count100 { get; set; }
        public string count300 { get; set; }
        public string countmiss { get; set; }
        public string countgeki { get; set; }
        public string countkatu { get; set; }
        public string perfect { get; set; }
        public string pass { get; set; }
        public string enabled_mods { get; set; }
    }

    public class Game
    {
        public string game_id { get; set; }
        public string start_time { get; set; }
        public string end_time { get; set; }
        public string beatmap_id { get; set; }
        public string play_mode { get; set; }
        public string match_type { get; set; }
        public string scoring_type { get; set; }
        public string team_type { get; set; }
        public string mods { get; set; }
        public List<Score> scores { get; set; }
    }

    public class RoomState
    {
        public Match match { get; set; }
        public List<Game> games { get; set; }
    }

    public class MapInfo
    {
        public string beatmapset_id { get; set; }
        public string beatmap_id { get; set; }
        public string approved { get; set; }
        public string total_length { get; set; }
        public string hit_length { get; set; }
        public string version { get; set; }
        public string file_md5 { get; set; }
        public string diff_size { get; set; }
        public string diff_overall { get; set; }
        public string diff_approach { get; set; }
        public string diff_drain { get; set; }
        public string mode { get; set; }
        public string approved_date { get; set; }
        public string last_update { get; set; }
        public string artist { get; set; }
        public string title { get; set; }
        public string creator { get; set; }
        public string creator_id { get; set; }
        public string bpm { get; set; }
        public string source { get; set; }
        public string tags { get; set; }
        public string genre_id { get; set; }
        public string language_id { get; set; }
        public string favourite_count { get; set; }
        public string playcount { get; set; }
        public string passcount { get; set; }
        public string max_combo { get; set; }
        public string difficultyrating { get; set; }
    }
}
