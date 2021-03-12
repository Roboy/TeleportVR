# The index of the following lists correspond to one end effector, example:
# ef_name[0] name of the first ef
# ef_enabled[0] enabled flag of the first ef
# ef_init_pose[0] init pose of the first ef
string[] ef_name # posible values ('left_hand'/'right_hand')
bool[] ef_enabled
geometry_msgs/Pose[] ef_init_pose

float32 operator_height