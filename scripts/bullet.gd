extends RigidBody2D

var fire_force = Vector2()
var fire_speed = 10
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

func _on_bullet_body_enter( body ):
	if body.get_name() == "player":
		print("hit player!")
		# show an absorption animation or something
		# and fire a custom signal that tells player
		# what to expand and by how much
		queue_free()
