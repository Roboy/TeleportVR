/* 
 * This message is auto generated by ROS#. Please DO NOT modify.
 * Note:
 * - Comments from the original code will be written in their own line 
 * - Variable sized arrays will be initialized to array of size 0 
 * Please report any issues at 
 * <https://github.com/siemens/ros-sharp> 
 */



namespace RosSharp.RosBridgeClient.MessageTypes.RoboyCognition
{
    public class FacialFeatures : Message
    {
        public const string RosMessageName = "roboy_cognition_msgs/FacialFeatures";

        // only for unrecognized faces
        // is person speaking?
        public bool speaking { get; set; }
        // facial features (128x1 vector)
        public double[] ff { get; set; }

        public FacialFeatures()
        {
            this.speaking = false;
            this.ff = new double[128];
        }

        public FacialFeatures(bool speaking, double[] ff)
        {
            this.speaking = speaking;
            this.ff = ff;
        }
    }
}