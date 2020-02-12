local args = {...}
local robot = require("robot")


--directions 

if args[1] == "1" then
robot.forward()
end

if args[1] == "-1" then
robot.back()
end

if args[1] == 2 then
robot.turnRight()
robot.forward()
robot.turnLeft()
end

if args[1] == -2 then
robot.turnLeft()
robot.Forward()
robot.turnRight()
end

if args[1] == 3 then
robot.Up()
end

if args[1] == -3 then
robot.Down()
end