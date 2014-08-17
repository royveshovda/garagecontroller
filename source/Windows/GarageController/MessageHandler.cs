namespace GarageController
{
    public class MessageHandler
    {
        private readonly Controller _controller;

        public MessageHandler()
        {
            _controller = new Controller();
        }

        public void HandleCommand(ToggleDoorCommand command)
        {
            if (command == null) return;
            if (command.DoorNumber < 1) return;
            if (command.DoorNumber > 2) return;

            //TODO Check signature

            //TODO: Check if expired

            switch (command.DoorNumber)
            {
                case 1:
                    HandleDoor1();
                    break;
                case 2:
                    HandleDoor2();
                    break;
            }
        }

        private void HandleDoor1()
        {
            _controller.ToggleDoor1();
        }

        private void HandleDoor2()
        {
            _controller.ToggleDoor2();
        }
    }
}
