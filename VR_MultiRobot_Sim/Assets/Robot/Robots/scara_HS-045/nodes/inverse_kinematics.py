#!/usr/bin/env python 
# coding: utf-8 
import rospy
from geometry_msgs.msg import Pose
from ros_learning.msg import ik
import tf_conversions
import math
import tf2_ros

def listener():
    a1, a2 = 0.225, 0.225
    b1, b2, b4 = 0.301, 0.100, 0.194
    tfBuffer = tf2_ros.Buffer()
    listener = tf2_ros.TransformListener(tfBuffer)
    p = Pose()
    i = ik()
    tool_pub = rospy.Publisher('tool_pose', Pose, queue_size=1)
    joint_pub = rospy.Publisher('joint_pose', ik, queue_size=1)
    rate = rospy.Rate(10.0)
    while not rospy.is_shutdown():
        try:
            # Listening the pose of the tool frame , represented by Quaternions
            trans = tfBuffer.lookup_transform('base_link', 'tool', rospy.Time())
        except (tf2_ros.LookupException, tf2_ros.ConnectivityException, tf2_ros.ExtrapolationException):
            rate.sleep()
            continue
        p.position.x = trans.transform.translation.x
        p.position.y = trans.transform.translation.y
        p.position.z = trans.transform.translation.z
        p.orientation.x = trans.transform.rotation.x
        p.orientation.y = trans.transform.rotation.y
        p.orientation.z = trans.transform.rotation.z
        p.orientation.w = trans.transform.rotation.w
        
        # tool_frame's position : px,py,pz
        x = trans.transform.translation.x
        y = trans.transform.translation.y
        z = trans.transform.translation.z
        # tool_frame's orientation : alpha = roll, beta = pitch, gamma = yaw
        (alpha,beta,gamma) = tf_conversions.transformations.euler_from_quaternion([trans.transform.rotation.x, trans.transform.rotation.y, trans.transform.rotation.z, trans.transform.rotation.w])
        #Value of phi
        phi = math.atan2((math.sin(alpha)*math.sin(beta)*math.cos(gamma))+(math.cos(alpha)*math.sin(gamma)),math.cos(beta)*math.cos(gamma))
        #phi = math.atan2(y,x)
        #Value of theta 2
        c2 = (math.pow(x,2)+math.pow(y,2)-(math.pow(a1,2)+math.pow(a2,2)))/(2*a1*a2)
        s2 = -(math.sqrt(abs(1-math.pow(c2,2))))
        i.theta2 = math.atan2(s2,c2)

        #Value of theta 1
        c1 = (((a1+(a2*c2))*x)+((a2*s2)*y))/(math.pow((a1+(a2*c2)),2)-math.pow(a2*s2,2))
        s1 = (((a1+(a2*c2))*y)-((a2*s2)*x))/(math.pow((a1+(a2*c2)),2)-math.pow(a2*s2,2))
        i.theta1 = math.atan2(s1,c1)
        
        #Value of theta 4
        i.theta4 = phi - (i.theta1+i.theta2)
        #Value of b3
        i.b3 = -(float(format(b1+b2-b4-z,'.3f')))


        
        joint_pub.publish(i)           #Publish joint's pose
        tool_pub.publish(p)            #Publish  tool frame's pose 

                        

if __name__ == '__main__':
    rospy.init_node('pose_listener', anonymous=True)
    try:
        listener()
    except rospy.ROSInterruptException:
        pass
