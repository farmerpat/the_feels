extends RigidBody2D

const MOVE_SPEED = 100
const ACCEL = 500

# grab the sprites
onready var pos_feels = get_node("pos_feels")
onready var pos_feels_animator = get_node("pos_feels").get_node("pos_feels_animator")
onready var neg_feels = get_node("neg_feels")

var screen_size = Vector2()
var vel = Vector2()
var acc = Vector2()

func _ready():
	# for now, center the player to start
	screen_size = get_viewport_rect().size
	#var pos = screen_size / 2
	#set_pos(pos)
	#set_fixed_process(true)
	set_process_input(true)

func _input(event):
	# if yer gonna use space, make better mapz!
	if event.is_action_pressed("ui_select"):
		# we might have to create the animation node on the fly to know what size we're
		# starting w/ and going to...
		pos_feels_animator.play("grower")
		var old_size = pos_feels.get_scale()
		#pos_feels.set_scale(old_size * 1.01)
	#vel.x = event.is_action_pressed("ui_right") - event.is_action_pressed("ui_left")
	#vel.y = event.is_action_pressed("ui_down") - event.is_action_pressed("ui_up")
	#apply_impulse(Vector2(), vel)
	##vel.x *= MOVE_SPEED
	##vel.y *= MOVE_SPEED

func _integrate_forces(state):
	vel.x = Input.is_action_pressed("ui_right") - Input.is_action_pressed("ui_left")
	vel.y = Input.is_action_pressed("ui_down") - Input.is_action_pressed("ui_up")
	#print(vel)
	set_applied_force(vel * MOVE_SPEED)

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