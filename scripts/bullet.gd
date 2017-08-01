extends RigidBody2D

var fire_force = Vector2()
var fire_speed = 100
var need_to_fire = false
# class member variables go here, for example:
# var a = 2
# var b = "textvar"

func _ready():
	pass
	#fire(Vector2(-1, 0))

func fire(dir_vec):
	print ("in bullet fire")
	fire_force = dir_vec
	# probably firespeed should be an optional argument
	fire_force *= fire_speed
	need_to_fire = true

# must delete if off screen

func _integrate_forces(state):
	if need_to_fire:
		set_applied_force(fire_force)