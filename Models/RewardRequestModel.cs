using Newtonsoft.Json;

namespace DeepRacerProcessor.Models
{
    public partial class RewardRequestModel
    {
        [JsonProperty("all_wheels_on_track")]
        public bool AllWheelsOnTrack { get; set; }

        [JsonProperty("x")]
        public double X { get; set; }

        [JsonProperty("y")]
        public double Y { get; set; }

        [JsonProperty("distance_from_center")]
        public long DistanceFromCenter { get; set; }

        [JsonProperty("heading")]
        public double Heading { get; set; }

        [JsonProperty("progress")]
        public long Progress { get; set; }

        [JsonProperty("steps")]
        public long Steps { get; set; }

        [JsonProperty("speed")]
        public long Speed { get; set; }

        [JsonProperty("steering_angle")]
        public long SteeringAngle { get; set; }

        [JsonProperty("track_width")]
        public double TrackWidth { get; set; }

        [JsonProperty("waypoints")]
        public double[][] Waypoints { get; set; }

        [JsonProperty("closest_waypoints")]
        public long[] ClosestWaypoints { get; set; }

        [JsonProperty("is_left_of_center")]
        public bool IsLeftOfCenter { get; set; }

        [JsonProperty("is_reversed")]
        public bool IsReversed { get; set; }
    }

    public partial class RewardRequestModel
    {
        public static RewardRequestModel FromJson(string json) => JsonConvert.DeserializeObject<RewardRequestModel>(json);
    }
}