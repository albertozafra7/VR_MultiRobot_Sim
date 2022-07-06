#!/usr/bin/env python 
# coding: utf-8

import rospy
from sensor_msgs.msg import JointState
import tf_conversions
import tf2_ros
import geometry_msgs.msg
import math


def testing():
	
	joint_state = JointState()	
        joint_state.name.append("joint1")
        joint_state.name.append("joint2")
        joint_state.name.append("joint3")
        joint_state.name.append("joint4")
        joint_state.position.append(0.675599992275)                                          #values for all joints enter manually for testing
        joint_state.position.append(-0.675599992275)
        joint_state.position.append(-0.0)
        joint_state.position.append(0.675599992275)
        joint_state.velocity.append(0.0)
        joint_state.effort.append(0.0)
        joint_pub = rospy.Publisher("joint_states", JointState, queue_size=1)
        br = tf2_ros.TransformBroadcaster()
        omega = 1
        radius = 0
        rate = rospy.Rate(10) # 10hz
        tick =0
        
        while not rospy.is_shutdown():

        	    theta = omega*tick
        	    q = tf_conversions.transformations.quaternion_from_euler(0, 0, -theta)
                    t1 = geometry_msgs.msg.TransformStamped()
                    t1.header.stamp = rospy.Time.now()
                    t1.header.frame_id = "map"
                    t1.child_frame_id = "base_link"
                    t1.transform.translation.x = math.sin(theta)*radius
                    t1.transform.translation.y = math.cos(theta)*radius
                    t1.transform.translation.z = 0
                    t1.transform.rotation.x = q[0]
                    t1.transform.rotation.y = q[1]
                    t1.transform.rotation.z = q[2]
                    t1.transform.rotation.w = q[3]
                    br.sendTransform(t1)
        	    joint_state.header.stamp = rospy.Time.now()
        	    joint_pub.publish(joint_state)
        	    rate.sleep()

        



if __name__ == '__main__':
	rospy.init_node('testing_ik', anonymous=True)
	try:
	    testing()       
        except rospy.ROSInterruptException:
            pass
