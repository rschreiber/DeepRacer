using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using DeepRacerProcessor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DeepRacerProcessor.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RewardController : ControllerBase
    {
        private readonly ILogger<RewardController> _logger;

        public RewardController(ILogger<RewardController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [HttpPost]
        [HttpPatch]
        public double Reward([FromBody] object data)
        {
            var dataString = data.ToString();
            try
            {
                var requestParams = RewardRequestModel.FromJson(dataString);
                return RewardByDirection(requestParams) * RewardBySpeed(requestParams);
            }
            catch (Exception e)
            {
                _logger.LogError(e, dataString);
            }

            return 0.001;
        }

        private double RewardByDirection(RewardRequestModel requestParams)
        {
            const double directionThreshold = 10.0;
            var reward = 2.0;
            var directionDifference = GetVarianceFromTrackDirection(requestParams);
            if (directionDifference > directionThreshold)
            {
                reward *= 0.5;
            }

            return reward;
        }

        private double RewardBySpeed(RewardRequestModel requestParams)
        {
            if (!requestParams.AllWheelsOnTrack)
                return 0.001;

            const double speedThreshold = 4.0;

            var reward = 2.0;

            if (requestParams.Speed < speedThreshold)
            {
                reward *= 0.5;
            }

            return reward;
        }

        private double GetVarianceFromTrackDirection(RewardRequestModel requestParams)
        {
            var prevPoint = requestParams.Waypoints[requestParams.ClosestWaypoints[0]];
            var nextPoint = requestParams.Waypoints[requestParams.ClosestWaypoints[1]];
            var trackDirection = GetDirectionBetweenTwoPoints(prevPoint, nextPoint);

            if (trackDirection < 0)
                trackDirection = 360 + trackDirection;

            var directionDifference = Math.Abs(trackDirection - requestParams.Heading);

            if (directionDifference > 180)
                directionDifference = 360 - directionDifference;

            return directionDifference;
        }

        private double GetDirectionBetweenTwoPoints(double[] x, double[] y)
        {
            var radians = Math.Atan2(y[1] - x[1], y[0] - x[0]);
            var degrees = (180 / Math.PI) * radians;
            return degrees;
        }
    }
}