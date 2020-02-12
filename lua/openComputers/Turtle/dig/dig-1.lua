
--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||
--usage

--the bot assumes that a chest is behind where it was placed
--bot if runs out of power will return to where it was placed
--bot if full inventory will return to where it was placed and drop stuff of behind it preferebly in a chest

--usage
--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||

--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||
--settings
moveX = -1 --how many blocks to move x axis before digging
moveY = 0 --how many blocks to move y axis before digging
moveZ = 2 --how many blocks to move z axis before digging
digX = 1 --How mutch to dig on the x axis
digY = 1 --How mutch to dig on the y axis
digZ = 10 --How mutch to dig on the z axis
splitDirectionX = false --wheter or not to split the digX so thats the bot digs from minus half digX to half digX 
splitDirectionY = false --wheter or not to split the digY so thats the bot digs from minus half digY to half digY 
splitDirectionZ = false --wheter or not to split the digZ so thats the bot digs from minus half digZ to half digZ 

tool = "mattock" --a part of the game of the tool to use
buildMat = "cobble" --material to build with
buildSLots = 2 --how many inventory slots the robot will use for building material

--settings
--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||
--DEVELOPER ONLY BELOW THIS LINE
--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||
--require
local comp = require("component")
local pc = comp.computer
local robot = require("robot")
--require
--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||
--variables
active = true --active loop

posX = 0
posY = 0
posZ = 0
facing = 1 --1[origin] -1[oposite] 2[right] -2[left] 3[up] -3[down]  --positions are relative to origin and 3 and -3 are impossible



--variables
--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||
--functions

--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||
--misc
--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||
function throw (strng)
pc.beep(250,0.15)
pc.beep(100,0.4)
io.stderr:write(strng)
end --throw end
--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||
function cout (strng)
io.stdout:write(strng)
end --cout end
--cout end
--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||
function init ()
pc.beep(50,0.5)
pc.beep(100,0.5)
pc.beep(150,0.5)
end --init end
--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||
function shutdown ()
active = false
pc.beep(150,0.5)
pc.beep(100,0.5)
pc.beep(50,0.5)
pc.stop()
end --init end
--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||
function updatePos ()
cout("function updatePos facing: "..facing.." posX: "..posX.." posZ: "..posZ)
if facing == 1 then posZ = posZ+1 end
if facing == -1 then posZ = posZ-1 end
if facing == 2 then posX = posX+1 end
if facing == -2 then posX = posX-1 end
end --updatePos
function move (dir) --move 1 block in a direction //directions 1[posZ] -1[negZ] 2[posX] -2[negX] 3[posY] -3[negY]
cout("function move "..dir .."\n")
if facing == dir then
updatePos()
return robot.forward()
end
if facing == -dir then
updatePos()
return robot.back()
end

if dir == 3 then
if robot.up() == true then
posY = posY+1
return true
else return false
end
end

if dir == -3 then
if robot.down() == true then
posY = posY-1
return true
else return false
end
end

if facing == 1 then
robot.turnRight()
facing = 2
elseif facing == 2 then
robot.turnLeft()
facing = 1
end

if facing == -1 then
robot.turnRight()
facing = -2
elseif facing == -2 then
robot.turnLeft()
facing = -1
end

if facing == dir then
updatePos()
return robot.forward()
end
if facing == -dir then
updatePos()
return robot.back()
end

end --move end
--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||
function moveDir () --move x blocks in a direction

end --movedir end
--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||
function moveTo (x,y,z)
cout("function moveTo x:"..x.." y:"..y.." z:"..z .."\n")

distX = x-posX
if 0 > distX then
distX = distX - distX - distX --invert distX
end
while distX > posX do
if y > 0 then
move(2)
else --positive end
move(-2)
end --negative end
end --while x

distY = y-posY
if 0 > distY then
distY = distY - distY - distY --invert distX
end
while distY > posY do
if x > 0 then
move(3)
else --positive end
move(-3)
end --negative end
end --while z

distZ = z-posZ
if 0 > distZ then
distZ = distZ - distZ - distZ --invert distX
end
while distZ > posZ do
if z > 0 then
move(1)
else --positive end
move(-1)
end --negative end
end --while z

end --moveto end
--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||
function checkPower ()

end --checkpower end
--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||
function checkInventory ()

end --checkinventory end
--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||
function compressBlocks ()

end --compressBlocks end
--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||
--misc
--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||

--functions
--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||
--runtime
init() --alert players to activating
moveTo(moveX,moveY,moveZ)
--start of actiuve loop
while active==true do
active=false

end --while active
--end of active loop

shutdown()
--runtime
--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||
