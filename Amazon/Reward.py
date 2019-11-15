def reward_function(params):
    import json
    import urllib.request
    url = 'https://app-sw-dr.azurewebsites.net/reward'
    data_string = json.dumps (params)
    data = bytes(data_string,'utf-8')
    headers = {'Content-Type': 'application/json'}
    request = urllib.request.Request (url=url,data=data, headers=headers, method='POST')
    response = urllib.request.urlopen (request)
    reward = float((str(response.read(),'utf-8')))
    return float(reward)