--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||
--usage
--the bot assumes that a chest is behind where it was placed
--bot if runs out of power will return to where it was placed
--bot if full inventory will return to where it was placed and drop stuff of behind it preferebly in a chest
--usage
--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||
--settings
moveX = -1 --how many blocks to move x axis before digging
moveY = 0 --how many blocks to move y axis before digging
moveZ = 2 --how many blocks to move z axis before digging
digX = 3 --How mutch to dig on the x axis
digY = 3 --How mutch to dig on the y axis
digZ = 10 --How mutch to dig on the z axis
splitDirectionX = false --wheter or not to split the digX so thats the bot digs from minus half digX to half digX 
splitDirectionY = false --wheter or not to split the digY so thats the bot digs from minus half digY to half digY 
splitDirectionZ = false --wheter or not to split the digZ so thats the bot digs from minus half digZ to half digZ 
tool = "mattock" --a part of the game of the tool to use
buildMat = "cobble" --material to build with
buildSLots = 2 --how many inventory slots the robot will use for building material
powerBeforeDig = 50
powerBeforeCharge = 20
--settings
--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||
--DEVELOPER ONLY BELOW THIS LINE
--require
local comp = require("component")
local pc = comp.computer
local compu = require("computer")
local robot = require("robot")
--require
--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||
--variables
active = true --active loop

posX = 0
posY = 0
posZ = 0
facing = 1 --1[origin] -1[oposite] 2[right] -2[left] 3[up] -3[down]  --positions are relative to origin and 3 and -3 are impossible

lastPosX = 0
lastPosY = 0
lastPosZ = 0

dugX = 0
dugY = 0
dugZ = 0

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
function updatePos (reverse)
cout("function updatePos facing: "..facing.." posX: "..posX.." posZ: "..posZ.."\n")
if reverse == true then
if facing == 1 then posZ = posZ-1 end
if facing == -1 then posZ = posZ+1 end
if facing == 2 then posX = posX-1 end
if facing == -2 then posX = posX+1 end
else
if facing == 1 then posZ = posZ+1 end
if facing == -1 then posZ = posZ-1 end
if facing == 2 then posX = posX+1 end
if facing == -2 then posX = posX-1 end
end


end --updatePos
function move (dir) --move 1 block in a direction //directions 1[posZ] -1[negZ] 2[posX] -2[negX] 3[posY] -3[negY]
cout("function move "..dir .."\n")

if((((facing == 1) or (facing == -1)) and ((dir == 2) or (dir == -2))) ) then
face(2)
elseif((((facing == 2) or (facing == -2)) and ((dir == 1) or (dir == -1))) ) then
face(1)
end

if facing == dir then
if robot.forward() == true then
updatePos()
return true
else
return false
end
end
if facing == -dir then
if robot.back() == true then
updatePos(true)
return true
else
return false
end
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


end --move end
--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||
function moveDir () --move x blocks in a direction

end --movedir end
--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||
function moveTo (x,y,z)
cout("function moveTo x:"..x.." :y:"..y.." :z:"..z .."\n")
cout("currPos: x: "..posX.." :y: "..posY.." :z: "..posZ.."\n")
	
distX = x-posX
if(0>distX) then distX = distX-distX-distX end --invert dist x if negative
whilX = 0
while distX > whilX do
if(x > posX) then
move(2)
else --if x end
move(-2)
end --if x else end
whilX = whilX+1
end --whil distX end

distY = y-posY
if(0>distY) then distY = distY-distY-distY end --invert dist y if negative
whilY = 0
while distY > whilY do
if(y > posY) then
move(3)
else --if y end
move(-3)
end --if y else end
whilY = whilY+1
end --whil distX end

distZ = z-posZ
if(0>distZ) then distZ = distZ-distZ-distZ end --invert dist z if negative
whilZ = 0
while distZ > whilZ do
if(z > posZ) then
move(1)
else --if z end
move(-1)
end --if z else end

whilZ = whilZ+1
end --whil distZ end

end --moveto end
--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||
--face
function face (fce)
cout("function face fce:"..fce .."\n")

if(facing == 1) then 
if(fce == -1) then 
facing = -1
robot.turnAround() end
if(fce == 2) then
facing = 2
robot.turnRight() end
if(fce == -2) then
facing = -2
robot.turnLeft() end
end --facing 1

if(facing == -1) then 
if(fce == 1) then 
facing = 1
robot.turnAround() end
if(fce == 2) then 
facing = 2
robot.turnLeft() end
if(fce == -2) then 
facing = -2
robot.turnRight() end
end --facing -1

if(facing == 2) then 
if(fce == 1) then 
facing = 1
robot.turnLeft() end
if(fce == -1) then 
facing = -1
robot.turnRight() end
if(fce == -2) then 
facing = -2
robot.turnAround() end
end --facing 2

if(facing == -2) then 
if(fce == 1) then 
facing = 1
robot.turnRight() end
if(fce == -1) then 
facing = -1
robot.turnLeft() end
if(fce == 2) then 
facing = 2
robot.turnAround() end
end --facing -2

end --face
--face
--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||
function checkPower ()
cout("function checkPower \n")
energy = compu.energy()/compu.maxEnergy()*100
if powerBeforeCharge > energy then charge() end

end --checkpower end
--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||
function charge ()
cout("function charge \n")
energy = compu.energy()/compu.maxEnergy()*100
lastPosX = posX
lastPosY = posY
lastPosZ = posZ
moveTo(0,0,0)
while powerBeforeDig > energy do
energy = compu.energy()/compu.maxEnergy()*100
pc.beep(energy*2+50,4)
end --while end
end --charge end
--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||
function checkInventory ()
cout("function checkInventory \n")
slots = robot.inventorySize()
slotsEmpty = 0
whil = 1
while not(slots == whil-1) do
if(robot.count(whil) == 0)then
slotsEmpty = slotsEmpty+1
end
whil = whil+1
end --whil end
if((slotsEmpty > 0) and (3 > slotsEmpty)) then unload() end
end --checkinventory end
--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||
function unload ()
cout("unload \n")
moveTo(0,0,0)
face(-1)
slots = robot.inventorySize()
whil = 1
while not(slots == whil-1) do
robot.select(whil)
count = robot.count()
--if(count > 0) then
robot.drop(count)
--while(robot.drop(count) == false) do
--throw("cant drop item")
--end --whil end
--end --if count > 0
whil = whil+1
end --whil end
robot.select(1)
end --unload end
--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||
function compressBlocks ()
cout("function compressBlocks \n")
	
end --compressBlocks end
--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||
--dig
function dig ()
cout("function dig x: "..digX.." :y: "..digY.." :z: "..digZ.."\n")
cout("dug x: "..dugX.." :y: "..dugY.." :z: "..dugZ.."\n")

moveTo(moveX+dugX,moveY+dugY,moveZ+dugZ)
face(1)
robot.swing()
if(digX>0)then
dugX = dugX+1
else --negative end
dugX = dugX-1
end --positive end

if (digX == dugX) then

dugX = 0
if(digY>0)then
dugY = dugY+1
else --negative end
dugY = dugY-1
end --positive end
if (digY == dugY) then

dugX = 0
dugY = 0
if(digZ>0)then
dugZ = dugZ+1
else --negative end
dugZ = dugZ-1
end --positive end
if (digZ == dugZ) then
moveTo(0,0,0)
active = false
end --dugY
end --dugZ

end --digX

--end --active

end --dig end
--dig
--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||
--misc
--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||

--functions
--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||
--runtime
init() --alert players to activating
--moveTo(moveX,moveY,moveZ)
--start of actiuve loop
while active==true do
cout("active run start \n")
checkPower()
checkInventory()
dig()
--pc.beep(100,5) --TEMP delay REMOVME
end --while active
--end of active loop
unload()
face(1)
shutdown()
--runtime
--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||--||
