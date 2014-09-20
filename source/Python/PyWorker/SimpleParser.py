import GarageControllerMessages_pb2


def main():
    cmd = GarageControllerMessages_pb2.ToggleDoorCommand()
    cmd.SessionId = "Dummy"
    cmd.DoorNumber = 1
    cmd.Signature = "SIGN"
    cmd.Clear()
    """msg = cmd.SerializeToString()
    print(msg)"""

if __name__ == '__main__':
    main()
