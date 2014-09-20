using ProtoBuf;

namespace GarageController
{
    [ProtoContract]
    public class ToggleDoorCommand
    {
        [ProtoMember(1)]
        public System.String SessionId;

        [ProtoMember(2)]
        public System.Int32 DoorNumber;

        [ProtoMember(3)]
        public System.String Created;

        [ProtoMember(4)]
        public System.String? Expiry;
                
        [ProtoMember(5)]
        public System.String Signature;
    }
}
