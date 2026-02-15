extends Control


const AttributeModifier = preload("uid://dlpibhpxqbeh0")
const AttributeInstance = preload("uid://i7djb7l1dy4h")
const AttributeTable = preload("uid://b4f4tsvg2ht2t")
const test_attribute := preload("uid://cr4yputsgv380")


func _test_basic_use_and_merge(
	attr_table: AttributeTable, 
	attr_instance: AttributeInstance
) -> void:
	var add_modifier := AttributeModifier.new()
	add_modifier.Name = "test"
	add_modifier.Operation = BuiltinAttributeModifierOperations.AddToBase()
	add_modifier.Amount = 30
	attr_instance.AddModifier(add_modifier)
	assert(is_equal_approx(attr_table.GetValue(test_attribute), 80))
	
	# Attribute modifiers are copied when adding, so mutating its property is safe.
	add_modifier.Amount = 60
	add_modifier.Name = "test2"
	attr_instance.AddModifier(add_modifier)
	assert(is_equal_approx(attr_table.GetValue(test_attribute), 50 + 30 + 60))
	assert(is_equal_approx(attr_instance.MergedCache[add_modifier.Operation], 30 + 60))
	
	attr_instance.RemoveModifier("test")
	assert(attr_instance.MergedCache.get(add_modifier.Operation) == null)
	assert(is_equal_approx(attr_table.GetValue(test_attribute), 50 + 60))


func _test_mul_and_add_total(
	attr_table: AttributeTable,
	attr_instance: AttributeInstance
) -> void:
	# Attribute value should be 50 (base) + 60 (add to base)
	assert(is_equal_approx(attr_table.GetValue(test_attribute), 50 + 60))
	
	# Mul Base
	var mul_modifier := AttributeModifier.new()
	mul_modifier.Operation = BuiltinAttributeModifierOperations.MultiplyBase()
	mul_modifier.Name = "test_mul_base_0"
	mul_modifier.Amount = 0.5
	attr_instance.AddModifier(mul_modifier)
	assert(is_equal_approx(attr_table.GetValue(test_attribute), (50 + 60) * (1 + 0.5)))
	
	mul_modifier.Name = "test_mul_base_1"
	mul_modifier.Amount = 0.7
	attr_instance.AddModifier(mul_modifier)
	assert(is_equal_approx(attr_table.GetValue(test_attribute), (50 + 60) * (1 + 0.5 + 0.7)))
	
	# Mul Total
	var base_value_before_mul_total := (50 + 60) * (1 + 0.5 + 0.7)
	mul_modifier.Operation = BuiltinAttributeModifierOperations.MultiplyTotal()
	mul_modifier.Name = "test_mul_total_0"
	mul_modifier.Amount = 0.91
	attr_instance.AddModifier(mul_modifier)
	assert(is_equal_approx(attr_table.GetValue(test_attribute), base_value_before_mul_total * 1.91))
	
	mul_modifier.Name = "test_mul_total_1"
	mul_modifier.Amount = 1.73
	attr_instance.AddModifier(mul_modifier)
	assert(is_equal_approx(attr_table.GetValue(test_attribute), base_value_before_mul_total * 1.91 * 2.73))
	
	
	# Add To Total
	var base_value_before_add_to_total := base_value_before_mul_total * 1.91 * 2.73
	var add_modifier := AttributeModifier.new()
	add_modifier.Operation = BuiltinAttributeModifierOperations.AddToTotal()
	add_modifier.Name = "test_add_to_total_0"
	add_modifier.Amount = 225
	attr_instance.AddModifier(add_modifier)
	assert(is_equal_approx(attr_table.GetValue(test_attribute), base_value_before_add_to_total + 225))
	
	
func _test_min_max_lock(
	attr_table: AttributeTable,
	attr_instance: AttributeInstance
) -> void:
	# This value should be about 1486.860600
	var original_value := attr_table.GetValue(test_attribute) 
	print("original value before min max lock is %f" % original_value)
	
	# Test min modifier
	var min_modifier := AttributeModifier.Create(
		"test_min_0",
		BuiltinAttributeModifierOperations.Min(),
		2500,
	)
	attr_instance.AddModifier(min_modifier)
	assert(is_equal_approx(attr_table.GetValue(test_attribute), 2500))
	
	# Test multiple min modifier
	min_modifier.Name = "test_min_1"
	min_modifier.Amount = 4500
	attr_instance.AddModifier(min_modifier)
	assert(is_equal_approx(attr_table.GetValue(test_attribute), 4500))
	
	attr_instance.RemoveModifier("test_min_0")
	attr_instance.RemoveModifier("test_min_1")
	min_modifier.Name = "test_min_2"
	min_modifier.Amount = 1250 #  < original_value
	attr_instance.AddModifier(min_modifier)
	assert(is_equal_approx(attr_table.GetValue(test_attribute), original_value))
	
	# Test max modifier
	var max_modifier := AttributeModifier.Create(
		"test_max_0",
		BuiltinAttributeModifierOperations.Max(),
		-1500,
	)
	attr_instance.AddModifier(max_modifier)
	assert(is_equal_approx(attr_table.GetValue(test_attribute), -1500))
	
	# Test multiple max modifier
	max_modifier.Name = "test_max_1"
	max_modifier.Amount = -1566
	attr_instance.AddModifier(max_modifier)
	assert(is_equal_approx(attr_table.GetValue(test_attribute), -1566))
	
	attr_instance.RemoveModifier("test_max_0")
	attr_instance.RemoveModifier("test_max_1")
	max_modifier.Name = "test_max_2"
	max_modifier.Amount = 1500 #  > original_value
	attr_instance.AddModifier(max_modifier)
	assert(is_equal_approx(attr_table.GetValue(test_attribute), original_value))
	
	var test_lock_modifier = func(value: float):
		var lock_modifier := AttributeModifier.Create(
			"test_lock_" + str(randi()),
			BuiltinAttributeModifierOperations.Lock(),
			value,
		)
		attr_instance.AddModifier(lock_modifier)
		assert(is_equal_approx(attr_table.GetValue(test_attribute), value))
	
	# Test lock modifier
	test_lock_modifier.call(-91)
	test_lock_modifier.call(91)
	
	# Test stability of lock modifier
	for i in range(1001):
		test_lock_modifier.call(i - 500)


func _ready() -> void:
	var attr_table = $"Attribute Container".Data
	var attr_instance = attr_table.GetInstance(test_attribute)
	assert(attr_table.GetValue(test_attribute) == 50)
	assert(attr_instance.Value == 50)
	
	_test_basic_use_and_merge(attr_table, attr_instance)
	_test_mul_and_add_total(attr_table, attr_instance)
	_test_min_max_lock(attr_table, attr_instance)
	
	print("TEST SUCCESS")


func _process(_delta: float) -> void:
	pass
