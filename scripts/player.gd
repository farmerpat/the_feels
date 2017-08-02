extends RigidBody2D

signal player_shot(feels_type)

const MOVE_SPEED = 100
const ACCEL = 500

onready var bullet = preload("res://scenes/bullet.tscn")

# grab the sprites
onready var pos_feels = get_node("pos_feels")
onready var pos_feels_animator = get_node("pos_feels").get_node("pos_feels_animator")
onready var neg_feels_animator = get_node("neg_feels").get_node("neg_feels_animator")
onready var neg_feels = get_node("neg_feels")

var feels_fire = "pos"
var screen_size = Vector2()
var vel = Vector2()
var acc = Vector2()
var fire_vel = Vector2()

func _ready():
	# for now, center the player to start
	screen_size = get_viewport_rect().size
	#var pos = screen_size / 2
	#set_pos(pos)
	#set_fixed_process(true)
	set_process_input(true)

func _input(event):
	# if yer gonna use space, make better mapz!
	if event.is_action_pressed("player_toggle_feels"):
		if feels_fire == "pos":
			feels_fire = "neg"
		else:
			feels_fire = "pos"
	fire_vel.x = event.is_action_pressed("player_fire_right") - \
				event.is_action_pressed("player_fire_left")
	fire_vel.y = event.is_action_pressed("player_fire_down") - \
				event.is_action_pressed("player_fire_up")

	print (fire_vel)
	# LOOK INTO is_action_released AND FIRE RATE, ETC
	
	# prevent from failing player collision:
	#https://godotengine.org/qa/4010/whats-difference-between-collision-layers-collision-masks
	#http://docs.godotengine.org/en/stable/classes/class_physicsbody2d.html

	if fire_vel.x != 0 || fire_vel.y != 0:
		shoot()


		#var old_size = pos_feels.get_scale()
		#pos_feels.set_scale(old_size * 1.01)
	#vel.x = event.is_action_pressed("ui_right") - event.is_action_pressed("ui_left")
	#vel.y = event.is_action_pressed("ui_down") - event.is_action_pressed("ui_up")
	#apply_impulse(Vector2(), vel)
	##vel.x *= MOVE_SPEED
	##vel.y *= MOVE_SPEED

func shoot():
	var b = bullet.instance()
	b.set_player_fire()
	add_child(b)

	if feels_fire == "pos":
		b.make_pos()
	elif feels_fire == "neg":
		b.make_neg()

	b.set_global_pos(get_pos())
	b.fire(fire_vel)

func _integrate_forces(state):
	vel.x = Input.is_action_pressed("ui_right") - Input.is_action_pressed("ui_left")
	vel.y = Input.is_action_pressed("ui_down") - Input.is_action_pressed("ui_up")
	#print(vel)
	set_applied_force(vel * MOVE_SPEED)

func grow_pos_punk_out():
	pass

func grow_pos():
	print("in grow pos")
	
	# I want to be able to do it this way, but
	# it will probably be faster to programatically edit the values
	# of the four keyframes
	# ala https://www.youtube.com/watch?v=5m9eB6bq3t0

	var anim = Animation.new()
	anim.set_length(3.1)
	var track = anim.add_track(Animation.TYPE_VALUE)
	anim.track_set_path(track, ".:transform/scale")

	var current_scale = pos_feels.get_scale()
	print (current_scale)
	anim.track_insert_key(track, 0.0, current_scale)
	anim.track_insert_key(track, 1.0, Vector2(current_scale.x + .03, current_scale.y + .03))
	anim.track_insert_key(track, 2.0, Vector2(current_scale.x + .06, current_scale.y + .06))
	anim.track_insert_key(track, 3.0, Vector2(current_scale.x + .1, current_scale.y + .1))

	print (anim.get_track_count())
	print(anim.get_step())
	print(anim.track_get_type(0))
	print(anim.track_get_path(0))
	print(anim.track_get_key_value(0, 0))
	print(anim.track_get_key_value(0,1))

	print(pos_feels_animator.get_animation_list())
	pos_feels_animator.add_animation("dis_grower", anim)
	pos_feels_animator.play("dis_grower")
	
	#pos_feels_animator.remove_animation("dis_grower")


func grow_neg():
	# this is a log of duplicated code.  shoudl consolodate
	# into one function
	# also, the AnimationPlayers on pos_feels and neg_feels
	# are probably unnecessary..
	# should be able to create animation player on the fly and add to player
	# node, then rm when finished.
	var anim = Animation.new()
	anim.set_length(3.1)
	var track = anim.add_track(Animation.TYPE_VALUE)
	anim.track_set_path(track, ".:transform/scale")

	var current_scale = neg_feels.get_scale()
	print (current_scale)
	anim.track_insert_key(track, 0.0, current_scale)
	anim.track_insert_key(track, 1.0, Vector2(current_scale.x + .03, current_scale.y + .03))
	anim.track_insert_key(track, 2.0, Vector2(current_scale.x + .06, current_scale.y + .06))
	anim.track_insert_key(track, 3.0, Vector2(current_scale.x + .1, current_scale.y + .1))

	neg_feels_animator.add_animation("dis_grower", anim)
	neg_feels_animator.play("dis_grower")

#func _fixed_process(delta):
#	print(vel)
	##acc.x = Input.is_action_pressed("ui_right") - Input.is_action_pressed("ui_left")
	##acc.y = Input.is_action_pressed("ui_down") - Input.is_action_pressed("ui_up")
	##vel += acc * delta
	#vel.x = Input.is_action_pressed("ui_right") - Input.is_action_pressed("ui_left")
	#vel.y = Input.is_action_pressed("ui_down") - Input.is_action_pressed("ui_up")
	##add_force(Vector2(0,0), vel)
	#apply_impulse(Vector2(0,0), vel)
	##move(vel * delta)

func _on_player_body_enter( body ):
	# figure out what to do about the "contacts_reported" field
	# does it need to be larger than 1? similarly for bullet
	if body.is_in_group("bullet"):
		if body.get_node("neg").is_visible():
			print("das a neg buleet")
			emit_signal("player_shot", "neg")
		elif body.get_node("pos").is_visible():
			print("das a pos buleet")
			emit_signal("player_shot", "pos")
