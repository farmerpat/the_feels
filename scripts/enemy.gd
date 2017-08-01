extends RigidBody2D

onready var bullet = preload("res://scenes/bullet.tscn")

var counter = 0

func _ready():
	set_process(true)

func _process(delta):
	if counter % 400 == 0:
		print("enemy get_pos:")
		print(get_pos())
		var b = bullet.instance()
		add_child(b)
		b.set_global_pos(get_pos())

		print("bullet get_pos:")
		print(b.get_pos())
		
		b.fire(Vector2(-1,0))
		#remove_child(b)

	counter += 1