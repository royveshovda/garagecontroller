import json

#cmd.SessionId = "Dummy"
#cmd.DoorNumber = 1
#cmd.Signature = "SIGN"

def parse(raw):
    return json.loads(raw)
