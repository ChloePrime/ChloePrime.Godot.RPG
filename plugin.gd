@tool
extends EditorPlugin


static func _cleanup_deprecated():
	ProjectSettings.set_setting("addons/chloe_prime_rpg/global_attribute_pipeline", null)


func _enable_plugin() -> void:
	_cleanup_deprecated()
	
	var prop_name := "addons/chloe_prime_rpg/config_resource"
	ProjectSettings.set(prop_name, "uid://ca2nbg1f77xir")
	ProjectSettings.add_property_info({
		"name": prop_name,
		"type": TYPE_STRING,
	})


func _disable_plugin() -> void:
	_cleanup_deprecated()


func _enter_tree() -> void:
	# Initialization of the plugin goes here.
	pass


func _exit_tree() -> void:
	# Clean-up of the plugin goes here.
	pass
