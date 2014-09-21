import json

def parse(raw):
    data = json.loads(raw)
    data.setdefault("Signature", "")
    data.setdefault("Created", "")
    data.setdefault("Expiry", "")
    data.setdefault("SessionId", "")
    SessionId = data["SessionId"]
    DoorNumber = data["DoorNumber"]
    Signature = data["Signature"]
    Created = data["Created"]
    Expiry = data["Expiry"]
    return SessionId, DoorNumber, Created, Expiry, Signature

if __name__ == '__main__':
    raw = "{\"SessionId\" : \"123\",\"DoorNumber\": \"2\", \"Signature\": \"SIGN\" }"
    sessionId, door, created, expiry, signature = parse(raw)
