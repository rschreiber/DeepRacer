def reward_function(params):
    reward = reward_by_speed (params) * reward_by_direction(params)
    return reward
    
def reward_by_direction(params):
    # Initialize the reward with typical value 
    reward = 2.0

    direction_diff = get_variance_from_track_direction(params)
    
    # Penalize the reward if the difference is too large
    DIRECTION_THRESHOLD = 10.0

    if direction_diff > DIRECTION_THRESHOLD:
        reward *= 0.5
        
    return reward

def reward_by_speed(params):
    # Read input variables
    all_wheels_on_track = params['all_wheels_on_track']
    speed = params['speed']

    # Set the speed threshold based your action space 
    SPEED_THRESHOLD = 3.0 
  
    reward = 2.0

    if not all_wheels_on_track:
        # Penalize if the car goes off track
        reward = 1e-3       
    elif speed < SPEED_THRESHOLD:
        # Penalize if the car goes too slow
        reward *= 0.5 

    return reward

def get_track_direction(params):
    import math
    # Get waypoints information from the parameters
    waypoints = params['waypoints']
    closest_waypoints = params['closest_waypoints']

    # Calculate the direction of the center line based on the closest waypoints
    next_point = waypoints[closest_waypoints[1]]
    prev_point = waypoints[closest_waypoints[0]]

    # Calculate the direction in radius, arctan2(dy, dx), the result is (-pi, pi) in radians
    track_direction_radians = math.atan2(next_point[1] - prev_point[1], next_point[0] - prev_point[0]) 
    # Convert to degree
    track_direction_degrees = math.degrees(track_direction_radians)
    return track_direction_degrees

def get_variance_from_track_direction(params):
    # Read input variables
    heading = params['heading']

    # Get track direction
    track_direction = get_track_direction(params)

    # Calculate the difference between the track direction and the heading direction of the car
    direction_diff = abs(track_direction - heading)
    if direction_diff > 180:
        direction_diff = 360 - direction_diff
    return direction_diff