using System;
using DeepRacerProcessor.Helpers;
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
        private const double FailureRewardValue = 1e-3;
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
                //Early exit if you're not on track
                if (!IsCarOnTrack(requestParams))
                    return FailureRewardValue;

                return RewardByDirection(requestParams) * RewardBySpeed(requestParams);


            }
            catch (Exception e)
            {
                _logger.LogError(e, dataString);
            }

            return FailureRewardValue;
        }



        private bool IsCarOnTrack(RewardRequestModel requestParams)
        {
            if (requestParams.AllWheelsOnTrack)
                return true;

            return requestParams.DistanceFromCenter < (requestParams.TrackWidth / 2);
        }

        private double RewardByDirection(RewardRequestModel requestParams)
        {
            const double directionThreshold = 10.0;
            var reward = 2.0;

            var trackDirection = GeometryHelper.GetDirectionBetweenTwoPoints(
                requestParams.Waypoints[requestParams.ClosestWaypoints[0]],
                requestParams.Waypoints[requestParams.ClosestWaypoints[1]]);

            var directionDifference = Math.Abs(trackDirection - requestParams.Heading);
            if (directionDifference > directionThreshold)
            {
                reward *= 0.5;
            }

            return reward;
        }

        public double RewardBySpeed(RewardRequestModel requestParams)
        {
            const double speedThreshold = 8;
            const int pointsToLookAhead = 4;

            var currentWayPoint = requestParams.ClosestWaypoints[0];

            var prevDirection = GeometryHelper.GetDirectionBetweenTwoPoints(
                requestParams.Waypoints[requestParams.ClosestWaypoints[0]],
                requestParams.Waypoints[requestParams.ClosestWaypoints[1]]);

            var totalDeviation = 0.0;

            var totalSegments = 0;
            for (long i = currentWayPoint + 1; i < currentWayPoint + 1 + pointsToLookAhead; i++)
            {
                totalSegments++;
                var startIndex = i;

                if (startIndex >= requestParams.Waypoints.Length)
                    startIndex -= requestParams.Waypoints.Length;

                var endIndex = startIndex + 1;
                if (endIndex >= requestParams.Waypoints.Length)
                    endIndex -= requestParams.Waypoints.Length;

                var segmentDirection = GeometryHelper.GetDirectionBetweenTwoPoints(requestParams.Waypoints[startIndex],
                    requestParams.Waypoints[endIndex]);

                var intermediateDirectionDiff = segmentDirection - prevDirection;
                
                if (intermediateDirectionDiff > 180)
                    intermediateDirectionDiff = 360 - intermediateDirectionDiff;


                if (Math.Abs(intermediateDirectionDiff) > 10)
                    break;

                totalDeviation += intermediateDirectionDiff;

                if (Math.Abs(totalDeviation) > 10)
                    break;

                prevDirection = segmentDirection;
            } 

            var reward = 2.0;

            var usableSpeedThreshold = speedThreshold * ((double)totalSegments/pointsToLookAhead);

            var speedVariance = (Math.Abs(requestParams.Speed - usableSpeedThreshold) / usableSpeedThreshold) * 100;

            if (speedVariance > 20)
            {
                reward *= 0.5;
            }

            return reward;
        }
    }
}