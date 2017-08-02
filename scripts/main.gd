extends Node

onready var player = get_node("player")
onready var bullet = preload("res://scenes/bullet.tscn")

func _ready():
	player.connect("player_shot", self, "_on_player_shot")
	print ("main connected to player")
	# Called every time the node is added to the scene.
	# Initialization here
	pass

func _on_player_shot (feels_type):
	print("player shot, <3 main")
	# grow the correct player feels orb
	if feels_type == "neg":
		print ("feels bad man")
		player.grow_neg()
	elif feels_type == "pos":
		print ("feels good man")
		player.grow_pos()