--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//
--settings start
movfram = 0 --the number of the mainframe to controll movement

crsLt = 2500 --idle altitude
cmbtLt = 2500 --combat altitude

acrsyMov = 10 -- how close to the target it will try to be stay where 0 is on the target
acrsyTrn = 5 -- how close to the direction to the target it will try to stay at. where 0 is directly towards the target
acrsyPtsj = 2 -- how close to the direction to the target it will try to stay at. where 0 is directly towards the target
acrsyRl = 2 -- how close to the direction to the target it will try to stay at. where 0 is directly towards the target

prsjnMov = 0.2 -- the precision of movement 0-1 where 0 is no precision and 1 is full precision. this refers to the angle between the enemy and the craft, if its 1 that means the craft must be looking directly at the target to move not even 1mm of course. wheras if this is set to 0 it will go in the general direction of the target, this affects: move
--prsjnTrn = 0 -- the precision of movement 0-1 where 0 is no precision and 1 is full precision. this refers to the angle between the enemy and the craft, if its 1 that means the craft must be looking directly at the target to move not even 1mm of course. wheras if this is set to 0 it will go in the general direction of the target, this affects: turn
--prsjnPtsj = 0.1 -- the precision of movement 0-1 where 0 is no precision and 1 is full precision. this refers to the angle between the enemy and the craft, if its 1 that means the craft must be looking directly at the target to move not even 1mm of course. wheras if this is set to 0 it will go in the general direction of the target, this affects: pitch
--prsjnRl = 0.1 -- the precision of movement 0-1 where 0 is no precision and 1 is full precision. this refers to the angle between the enemy and the craft, if its 1 that means the craft must be looking directly at the target to move not even 1mm of course. wheras if this is set to 0 it will go in the general direction of the target, this affects: roll

blnsTrn = 10 --from 0-359 the yaw to balance on, basicly the orientation of the vehicle.
--custom controll numbers below
-- 0-NONE | 1-T | 2-G | 3-Y | 4-H | 5-U | 6-J | 7-I |
-- 8-K | 9-O | 10-L | 11-UP | 12-DOWN | 13-LEFT | 14-RIGHT |

cntrlMoveFBEnabled = false --whether or not to move forward/back
cntrlForward = 1 --custom input to move forward
cntrlBack = 2 --custom input to move back

cntrlMoveUDEnabled = true --whether or not to move up/down
cntrlUp = 9 --custom input to move up
cntrlDown = 10 --custom input to move down

cntrlPitchEnabled = true --whether or not to pitch
cntrlPitchUp = 5 --custom input to pitch up
cntrlPitchDown = 6 --custom input to pitch down
cntrlPitchBlns = true --wheter or not to use pitch inputs for balancing the craft, if enabled it will balance the craft if disabled it will aim at the target

cntrlMoveLREnabled = false --whether or not to move left/right
cntrlLeft = 13 --custom input to move left
cntrlRight = 14 --custom input to move right

cntrlTurnEnabled = true --whether or not to turn (yaw)
cntrlTurnLeft = 3 --custom input to turn left
cntrlTurnRight = 7 --custom input to turn right
cntrlTurnBlns = true --wheter or not to use turn inputs for balancing the craft, if enabled it will balance the craft if disabled it will aim at the target

cntrlRollEnabled = true --whether or not to roll
cntrlRollLeft = 4 --custom input to roll left
cntrlRollRight = 8 --custom input to roll right
cntrlRollAngle = 60 --max angle to roll to
cntrlRollBlns = true --wheter or not to use roll inputs for balancing the craft, if enabled it will balance the craft if disabled it will aim at the target

-- possible? -- speedDriveEnabled --whether or not to use custom Drive, otherwise ai will use drive set on component
-- possible? -- speedDrive = 5 --how mutch motor drive to use 0-5, wont actually change motor drive
-- possible? -- speedAdpt = true --adaptive speed on/off, if false vehicle will move at full speed even if right next to target

--settings end
--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//
--todo start
--move !done
--turn
--pitch
--roll
-- smothed movement //slower the closer to target !no inbuilt way skip frames for this

--todo end
--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//
--var start
origPos = false
--var end
--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//
--functions start
--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//
--Update start
function Update(I)
I:Log("function Update start")
--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//
if origPos == false then
origPos = I:GetConstructPosition()
end --origPos

if I:IsDocked() == false then
if I.AIMode ~= "off" then
I:TellAiThatWeAreTakingControl()
run(I)
--else
--I:LogToHud("ai off")
end --ai not off
--else
--I:LogToHud("ai off")
end --docked
--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//
I:Log("function Update end")
end --Update end
--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//
--run start
function run(I)
I:Log("function run start") 
if I:GetNumberOfTargets(movfram) ~= 0 then
trgt = I:GetTargetInfo(movfram,0)
if trgt.Valid == true then
--if I.AIMode == "combat" then
--trgt.position.y = crsLt
--end --combat mode
--trgt.position.y = cmbtLt
nTrgt = {}
nTrgt.x = trgt.Position.x
nTrgt.y = cmbtLt
nTrgt.z = trgt.Position.z
move(I,nTrgt)
end -- target valid
else
--trgt = I:GetConstructPosition()
--trgt = origPos
trgt = {}
trgt.x = origPos.x
trgt.y = crsLt
trgt.z = origPos.z
move(I,trgt)
end -- getnumtargets > 0
I:Log("function run end")
end --run end
--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//
--move start
function move(I,pos)
I:Log("function move start")
currPos = I:GetConstructPosition()
--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//
-- forwards/back start
if cntrlMoveFBEnabled == true then
vctr = I:GetConstructForwardVector()
--I:LogToHud("vctr.x: "..vctr.x)
--I:LogToHud("vctr.y: "..vctr.y)
--I:LogToHud("vctr.z: "..vctr.z)
--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//
-- x start
--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//
-- pos.x > start
if pos.x > currPos.x+acrsyMov then
if vctr.x > prsjnMov then
I:RequestComplexControllerStimulus(cntrlForward)
end --forward
if prsjnMov > vctr.x then
I:RequestComplexControllerStimulus(cntrlBack)
end --back

end -- pos.x >
-- pos.x > end
--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//
-- currPos.x > start
if currPos.x > pos.x+acrsyMov then
if vctr.x > prsjnMov then
I:RequestComplexControllerStimulus(cntrlBack)
end --forward
if prsjnMov > vctr.x then
I:RequestComplexControllerStimulus(cntrlForward)
end --back

end -- currPos.x >
-- currPos.x > end
--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//
-- x end
--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//
-- z start
--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//

-- pos.z > start
if pos.z > currPos.z+acrsyMov then
if vctr.z > prsjnMov then
I:RequestComplexControllerStimulus(cntrlForward)
end --forward
if prsjnMov > vctr.z then
I:RequestComplexControllerStimulus(cntrlBack)
end --back

end -- pos.z >
-- pos.z > end
--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//
-- currPos.z > start
if currPos.z > pos.z+acrsyMov then
if vctr.z > prsjnMov then
I:RequestComplexControllerStimulus(cntrlBack)
end --forward
if prsjnMov > vctr.z then
I:RequestComplexControllerStimulus(cntrlForward)
end --back

end -- currPos.z >
-- currPos.z > end
--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//
-- z end
--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//
end -- forwards/back
-- forwards/back end
--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//

--I:LogToHud("origPos.x: "..origPos.x)
--I:LogToHud("currPos.x: "..currPos.x)
--I:LogToHud("pos.x: "..pos.x)

--I:LogToHud("origPos.y: "..origPos.y)
--I:LogToHud("currPos.y: "..currPos.y)
--I:LogToHud("pos.y: "..pos.y)

--I:LogToHud("origPos.z: "..origPos.z)
--I:LogToHud("currPos.z: "..currPos.z)
--I:LogToHud("pos.z: "..pos.z)
--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//
-- up/down start
if cntrlMoveUDEnabled == true then
vctr = I:GetConstructUpVector()
--I:LogToHud("vctr.x: "..vctr.x)
--I:LogToHud("vctr.y: "..vctr.y)
--I:LogToHud("vctr.z: "..vctr.z)

-- y start
--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//

-- pos.y > start
if pos.y > currPos.y+acrsyMov then
if vctr.y > prsjnMov then
I:RequestComplexControllerStimulus(cntrlUp)
end --up
if prsjnMov > vctr.y then
I:RequestComplexControllerStimulus(cntrlDown)
end --down

end -- pos.y >
-- pos.y > end
--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//
-- currPos.y > start
if currPos.y > pos.y+acrsyMov then
if vctr.y > prsjnMov then
I:RequestComplexControllerStimulus(cntrlDown)
end --up
if prsjnMov > vctr.y then
I:RequestComplexControllerStimulus(cntrlUp)
end --down

end -- currPos.y >
-- currPos.y > end
--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//
-- y end
end -- up/down
-- up/down end
--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//
-- left/right start
if cntrlMoveLREnabled == true then
vctr = I:GetConstructRightVector()
--I:LogToHud("vctr.x: "..vctr.x)
--I:LogToHud("vctr.y: "..vctr.y)
--I:LogToHud("vctr.z: "..vctr.z)
--I:Log("vctr.x: "..vctr.x)
--I:Log("vctr.y: "..vctr.y)
--I:Log("vctr.z: "..vctr.z)
--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//
-- x start
--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//
-- pos.x > start
if pos.x > currPos.x+acrsyMov then
if vctr.x > prsjnMov then
I:RequestComplexControllerStimulus(cntrlRight)
end --right
if prsjnMov > vctr.x then
I:RequestComplexControllerStimulus(cntrlLeft)
end --left

end -- pos.x >
-- pos.x > end
--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//
-- currPos.x > start
if currPos.x > pos.x+acrsyMov then
if vctr.x > prsjnMov then
I:RequestComplexControllerStimulus(cntrlLeft)
end --right
if prsjnMov > vctr.x then
I:RequestComplexControllerStimulus(cntrlRight)
end --left

end -- currPos.x >
-- currPos.x > end
--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//
-- x end
--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//
-- z start
--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//

-- pos.z > start
if pos.z > currPos.z+acrsyMov then
if vctr.z > prsjnMov then
I:RequestComplexControllerStimulus(cntrlRight)
end --right
if prsjnMov > vctr.z then
I:RequestComplexControllerStimulus(cntrlLeft)
end --left

end -- pos.z >
-- pos.z > end
--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//
-- currPos.z > start
if currPos.z > pos.z+acrsyMov then
if vctr.z > prsjnMov then
I:RequestComplexControllerStimulus(cntrlLeft)
end --right
if prsjnMov > vctr.z then
I:RequestComplexControllerStimulus(cntrlRight)
end --left

end -- currPos.z >
-- currPos.z > end
--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//
-- z end
--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//
end -- left/right
-- left/right end
--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//
-- left/right start
if cntrlTurnEnabled == true then
yaw = I:GetConstructYaw()
I:LogToHud("yaw: "..yaw)
--I:Log("yaw: "..yaw)
vctrFrwrd = I:GetConstructForwardVector()
--I:LogToHud("vctrFrwrd.x: "..vctrFrwrd.x)
--I:LogToHud("vctrFrwd.y: "..vctrFrwrd.y)
--I:LogToHud("vctrFrwrd.z: "..vctrFrwrd.z)
vctrUp = I:GetConstructUpVector()
--I:LogToHud("vctrUp.x: "..vctrUp.x)
--I:LogToHud("vctrUp.y: "..vctrUp.y)
--I:LogToHud("vctrUp.z: "..vctrUp.z)
--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//
-- pos.x > start
--if pos.x > currPos.x+acrsy then

--if -1/2 > 1 then

--end

if cntrlRollBlns == true then
--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//
--[[
--x start
if prsjnTrn > yaw then
if vctrUp.y > 0 then
I:RequestComplexControllerStimulus(cntrlTurnRight)
else --upsideup
I:RequestComplexControllerStimulus(cntrlTurnLeft)
end --upsidedown
end --prsjsntrn/2 >

if yaw > prsjnTrn then
if vctrUp.y > 0 then
I:RequestComplexControllerStimulus(cntrlTurnLeft)
else --upsideup
I:RequestComplexControllerStimulus(cntrlTurnRight)
end --upsidedown
end --prsjsntrn/2 >
--x end
]]
--x start
--if yaw ~= prsjnTrn then
I:LogToHud("blnsTrn: "..blnsTrn)

blnsAng1 = blnsTrn+acrsyTrn
--if blnsAng1 > 360 then blnsAng1 = blnsAng1-360 end
--if 0 > blnsAng1 then blnsAng1 = 360+blnsAng1 end
if blnsAng1 > 360 then blnsAng1 = blnsTrn end
if 0 > blnsAng1 then blnsAng1 = blnsTrn end
I:LogToHud("blnsAng1: "..blnsAng1)

blnsAng2 = blnsTrn-acrsyTrn
--if blnsAng2 > 360 then blnsAng2 = blnsAng2-360 end
--if 0 > blnsAng2 then blnsAng2 = 360+blnsAng2 end
if blnsAng2 > 360 then blnsAng2 = blnsTrn end
if 0 > blnsAng2 then blnsAng2 = blnsTrn end
I:LogToHud("blnsAng2: "..blnsAng2)

if yaw > blnsAng1 then

if yaw > 180 then
if vctrUp.y > 0 then
I:RequestComplexControllerStimulus(cntrlTurnRight)
else --upsideup
I:RequestComplexControllerStimulus(cntrlTurnLeft)
end --upsidedown
end --yaw > 180

if 180 > yaw then
if vctrUp.y > 0 then
I:RequestComplexControllerStimulus(cntrlTurnLeft)
else --upsideup
I:RequestComplexControllerStimulus(cntrlTurnRight)
end --upsidedown
end --180 > yaw

end --yaw > prsjnTrn

if blnsAng2 > yaw then

if yaw > 180 then
if vctrUp.y > 0 then
I:RequestComplexControllerStimulus(cntrlTurnLeft)
else --upsideup
I:RequestComplexControllerStimulus(cntrlTurnRight)
end --upsidedown
end --yaw > 180

if 180 > yaw then
if vctrUp.y > 0 then
I:RequestComplexControllerStimulus(cntrlTurnRight)
else --upsideup
I:RequestComplexControllerStimulus(cntrlTurnLeft)
end --upsidedown
end --180 > yaw

end --prsjnTrn > yaw


--end -- yaw ~= prsjnTrn

--[[
if yaw ~= prsjnTrn then
if vctrUp.y > 0 then
if 180 > prsjnTrn then
I:RequestComplexControllerStimulus(cntrlTurnRight)
else --180 >
I:RequestComplexControllerStimulus(cntrlTurnLeft)
end --> 180
else --upsideup
if prsjnTrn > 180 then
I:RequestComplexControllerStimulus(cntrlTurnLeft)
else --180 >
I:RequestComplexControllerStimulus(cntrlTurnRight)
end --> 180
end --upsidedown
end --yaw ~=
]]
--x end
--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//
end --blns
--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//

--[[
if prsjnTrn > vctrFrwrd.x then
if vctrUp.y > 0 then
I:RequestComplexControllerStimulus(cntrlTurnRight)
else --upsideup
I:RequestComplexControllerStimulus(cntrlTurnLeft)
end --upsidedown
end --right
--prsjnTrnNeg = prsjnTrn - prsjnTrn - prsjnTrn
if vctrFrwrd.x > prsjnTrn then
if vctrUp.y > 0 then
I:RequestComplexControllerStimulus(cntrTurnlLeft)
else --upsideup
I:RequestComplexControllerStimulus(cntrlTurnRight)
end --upsidedown
end --left
]]
--end -- pos.x >
-- pos.x > end
--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//
--left start
--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//

end -- turn
-- turn end
--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//


I:Log("function move end")
end --move end
--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//
--functions end
--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//