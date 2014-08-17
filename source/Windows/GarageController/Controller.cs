using System;
using System.Globalization;
using System.Threading;
using Phidgets;
using Phidgets.Events;

namespace GarageController
{
    public class Controller
    {
        private readonly InterfaceKit _interfaceKit;
        private bool isOpen = false;
        public Controller()
        {
            _interfaceKit = new InterfaceKit();

            _interfaceKit.Attach += new AttachEventHandler(ifKit_Attach);
            _interfaceKit.Detach += new DetachEventHandler(ifKit_Detach);
            _interfaceKit.Error += new ErrorEventHandler(ifKit_Error);

            _interfaceKit.open();
        }

        public void ToggleDoor1()
        {
            Console.WriteLine("Door 1 toggled");
            if (isOpen)
            {
                _interfaceKit.outputs[0] = true;
                Thread.Sleep(800);
                _interfaceKit.outputs[0] = false;
            }
        }

        public void ToggleDoor2()
        {
            Console.WriteLine("Door 2 toggled");
            if (isOpen)
            {
                _interfaceKit.outputs[1] = true;
                Thread.Sleep(800);
                _interfaceKit.outputs[1] = false;
            }
        }

        //IfKit attach event handler
        //Here we'll display the interface kit details as well as determine how many output and input
        //fields to display as well as determine the range of values for input sensitivity slider
        void ifKit_Attach(object sender, AttachEventArgs e)
        {
            isOpen = true;
            var interfaceKit = (InterfaceKit)sender;
            //TODO: Log event

            const string message1 = "InterfaceKit attached";

            var message2 = string.Format("Attached: {0}", interfaceKit.Attached.ToString());
            var message3 = string.Format("Name: {0}", interfaceKit.Name);
            var message4 = string.Format("SerialNumber: {0}", interfaceKit.SerialNumber.ToString(CultureInfo.InvariantCulture));
            var message5 = string.Format("Version: {0}", interfaceKit.Version.ToString(CultureInfo.InvariantCulture));

            var message6 = string.Format("Number of digital inputs: {0}", interfaceKit.inputs.Count.ToString(CultureInfo.InvariantCulture));
            var message7 = string.Format("Number of digital outputs: {0}", interfaceKit.outputs.Count.ToString(CultureInfo.InvariantCulture));
            var message8 = string.Format("Number of sensor inputs (analog): {0}", interfaceKit.sensors.Count.ToString(CultureInfo.InvariantCulture));

            ReportMessage(message1);
            ReportMessage(message2);
            ReportMessage(message3);
            ReportMessage(message4);
            ReportMessage(message5);
            ReportMessage(message6);
            ReportMessage(message7);
            ReportMessage(message8);
        }

        private void ReportMessage(string message)
        {
            Console.WriteLine(message);
        }

        //Ifkit detach event handler
        //Here we display the attached status, which will be false as the device is disconnected. 
        //We will also clear the display fields and hide the inputs and outputs.
        void ifKit_Detach(object sender, DetachEventArgs e)
        {
            isOpen = false;
            //var interfaceKit = (InterfaceKit)sender;

            const string message1 = "InterfaceKit detached";

            ReportMessage(message1);
        }

        //Error event handler
        void ifKit_Error(object sender, ErrorEventArgs e)
        {
            isOpen = false;
            /*
            Phidget phid = (Phidget)sender;
            switch (e.Type)
            {
                case PhidgetException.ErrorType.PHIDGET_ERREVENT_BADPASSWORD:
                    phid.close();
                    TextInputBox dialog = new TextInputBox("Error Event",
                        "Authentication error: This server requires a password.", "Please enter the password, or cancel.");
                    result = dialog.ShowDialog();
                    if (result == DialogResult.OK)
                        openCmdLine(phid, dialog.password);
                    else
                        Environment.Exit(0);
                    break;
                case PhidgetException.ErrorType.PHIDGET_ERREVENT_PACKETLOST:
                    //Ignore this error - it's not useful in this context.
                    return;
                case PhidgetException.ErrorType.PHIDGET_ERREVENT_OVERRUN:
                    //Ignore this error - it's not useful in this context.
                    return;
                default:
                    if (!errorBox.Visible)
                        errorBox.Show();
                    break;
            }
            errorBox.addMessage(DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + ": " + e.Description);
             */
        }    
    }
}
