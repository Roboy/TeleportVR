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
    public class Faces : Message
    {
        public const string RosMessageName = "roboy_cognition_msgs/Faces";

        public long[] ids { get; set; }
        public string[] names { get; set; }
        public double[] confidence { get; set; }
        public FacialFeatures[] face_encodings { get; set; }

        public Faces()
        {
            this.ids = new long[0];
            this.names = new string[0];
            this.confidence = new double[0];
            this.face_encodings = new FacialFeatures[0];
        }

        public Faces(long[] ids, string[] names, double[] confidence, FacialFeatures[] face_encodings)
        {
            this.ids = ids;
            this.names = names;
            this.confidence = confidence;
            this.face_encodings = face_encodings;
        }
    }
}