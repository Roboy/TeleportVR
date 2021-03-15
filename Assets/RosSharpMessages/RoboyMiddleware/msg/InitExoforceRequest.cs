/* 
 * This message is auto generated by ROS#. Please DO NOT modify.
 * Note:
 * - Comments from the original code will be written in their own line 
 * - Variable sized arrays will be initialized to array of size 0 
 * Please report any issues at 
 * <https://github.com/siemens/ros-sharp> 
 */

using Newtonsoft.Json;

using RosSharp.RosBridgeClient.MessageTypes.Geometry;
using RosSharp.RosBridgeClient.MessageTypes.RoboySimulation;

namespace RosSharp.RosBridgeClient.MessageTypes.RoboyMiddleware
{
    public class InitExoforceRequest : Message
    {
        [JsonIgnore]
        public const string RosMessageName = "roboy_middleware_msgs/msg/InitExoforceRequest";

        //  The index of the following lists correspond to one end effector, example:
        //  ef_name[0] name of the first ef
        //  ef_enabled[0] enabled flag of the first ef
        //  ef_init_pose[0] init pose of the first ef
        public string[] ef_name;
        //  posible values ('left_hand'/'right_hand')
        public bool[] ef_enabled;
        public Pose[] ef_init_pose;
        public float operator_height;
        public LinkInformation[] roboy_link_information;

        public InitExoforceRequest()
        {
            this.ef_name = new string[0];
            this.ef_enabled = new bool[0];
            this.ef_init_pose = new Pose[0];
            this.operator_height = 0.0f;
            this.roboy_link_information = new LinkInformation[0];
        }

        public InitExoforceRequest(string[] ef_name, bool[] ef_enabled, Pose[] ef_init_pose, float operator_height, LinkInformation[] roboy_link_information)
        {
            this.ef_name = ef_name;
            this.ef_enabled = ef_enabled;
            this.ef_init_pose = ef_init_pose;
            this.operator_height = operator_height;
            this.roboy_link_information = roboy_link_information;
        }
    }
}
