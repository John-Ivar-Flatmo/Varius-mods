local args = {...}
local robot = require("robot")


--directions
if args[1] == nil then args[1] = "1" end 
if args[2] == nil then args[2] = "1" end 
whil = 0
while tonumber(args[2]) > whil do
if args[1] == "1" then
robot.forward()
end

if args[1] == "-1" then
robot.back()
end

if args[1] == "2" then
robot.turnRight()
robot.forward()
robot.turnLeft()
end

if args[1] == "-2" then
robot.turnLeft()
robot.forward()
robot.turnRight()
end

if args[1] == "3" then
robot.up()
end

if args[1] == "-3" then
robot.down()
end

whil = whil+1
end --whil end
