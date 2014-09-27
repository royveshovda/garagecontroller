import json


def parse(raw_json):
    data = json.loads(raw_json)
    data.setdefault("Signature", "")
    data.setdefault("Created", "")
    data.setdefault("Expiry", "")
    data.setdefault("SessionId", "")
    session_id = data["SessionId"]
    door_number = data["DoorNumber"]
    signature_data = data["Signature"]
    created_data = data["Created"]
    expiry_data = data["Expiry"]
    return session_id, door_number, created_data, expiry_data, signature_data


if __name__ == '__main__':
    raw = "{\"SessionId\" : \"123\",\"DoorNumber\": \"2\", \"Signature\": \"SIGN\" }"
    sessionId, door, created, expiry, signature = parse(raw)
