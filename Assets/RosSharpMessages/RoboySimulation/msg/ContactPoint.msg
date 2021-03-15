#id of roboy link in which a collision happened
int64 linkid
#position of contact point on roboy in link frame
geometry_msgs/Vector3 position
#contact normal on external body, pointing towards roboy
geometry_msgs/Vector3 contactnormal
#contact distance, positive for separation, negative for penetration
float64 contactdistance
#normal force applied during the last 'stepSimulation'
float64 normalforce