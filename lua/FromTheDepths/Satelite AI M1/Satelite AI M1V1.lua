--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//
--settings start
movfram = 0 --the number of the mainframe to controll movement

crsLt = 2500 --idle altitude
cmbtLt = 2000 --combat altitude

--custom controll numbers below
-- 0-NONE | 1-T | 2-G | 3-Y | 4-H | 5-U | 6-J | 7-I |
-- 8-K | 9-O | 10-L | 11-UP | 12-DOWN | 13-LEFT | 14-RIGHT |

cntrlMoveFBEnabled = true --whether or not to move forward/back
cntrlForward = 1 --custom input to move forward
cntrlBack = 2 --custom input to move back

cntrlMoveUDEnabled = true --whether or not to move up/down
cntrlUp = 9 --custom input to move up
cntrlDown = 10 --custom input to move down

cntrlPitchEnabled = false --whether or not to pitch
cntrlPitchUp = 5 --custom input to pitch up
cntrlPitchDown = 6 --custom input to pitch down

cntrlMoveLREnabled = true --whether or not to move left/right
cntrlLeft = 13 --custom input to move left
cntrlRight = 14 --custom input to move right

cntrlTurnEnabled = false --whether or not to turn
cntrlTurnLeft = 3 --custom input to turn left
cntrlTurnRight = 7 --custom input to turn right

cntrlRollEnabled = false --whether or not to roll
cntrlRollLeft = 4 --custom input to roll left
cntrlRollRight = 8 --custom input to roll right
cntrlRollAngle = 60 --max angle to roll to

-- possible? -- speedDriveEnabled --whether or not to use custom Drive, otherwise ai will use drive set on component
-- possible? -- speedDrive = 5 --how mutch motor drive to use 0-5, wont actually change motor drive
-- possible? -- speedAdpt = true --adaptive speed on/off, if false vehicle will move at full speed even if right next to target

--settings end
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
trgt.position.y = cmbtLt
move(I,trgt.position)
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
frwrdVctr = I:GetConstructForwardVector()
I:LogToHud("frwrdVctr.x"..frwrdVctr.x)
--I:LogToHud("frwrdVctr.y"..frwrdVctr.y)
I:LogToHud("frwrdVctr.z"..frwrdVctr.z)
--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//
--[[ --old back forth code seperate x and z start
-- x start
--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//
-- pos.x > start
if pos.x > currPos.x then
if frwrdVctr.x > 0.5 then
I:RequestComplexControllerStimulus(cntrlForward)
end --forward
if 0 > frwrdVctr.x then
I:RequestComplexControllerStimulus(cntrlBack)
end --back

end -- pos.x >
-- pos.x > end
--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//
-- currPos.x > start
if currPos.x > pos.x then
if frwrdVctr.x > 0.5 then
I:RequestComplexControllerStimulus(cntrlBack)
end --forward
if 0 > frwrdVctr.x then
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
if pos.z > currPos.z then
if frwrdVctr.z > 0.5 then
I:RequestComplexControllerStimulus(cntrlForward)
end --forward
if 0 > frwrdVctr.z then
I:RequestComplexControllerStimulus(cntrlBack)
end --back

end -- pos.z >
-- pos.z > end
--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//
-- currPos.z > start
if currPos.z > pos.z then
if frwrdVctr.z > 0.5 then
I:RequestComplexControllerStimulus(cntrlBack)
end --forward
if 0 > frwrdVctr.z then
I:RequestComplexControllerStimulus(cntrlForward)
end --back

end -- currPos.z >
-- currPos.z > end
--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//
-- z end
]] --old back forth code seperate x and z end

--currPos.x/z > start
if currPos.x > pos.x and currPos.z > pos.z then
if frwrdVctr.x > 0 and frwrdVctr.z > 0 then
I:RequestComplexControllerStimulus(cntrlBack)
end --forwards
if 0 > frwrdVctr.x and 0 > frwrdVctr.z then
I:RequestComplexControllerStimulus(cntrlForward)
end --back

end -- currPos.x/z >
--currPos.x/z > end

--pos.x/z > start
if pos.x > currPos.x and pos.z > currPos.z then
if frwrdVctr.x > 0 and frwrdVctr.z > 0 then
I:RequestComplexControllerStimulus(cntrlForwards)
end --forwards
if 0 > frwrdVctr.x and 0 > frwrdVctr.z then
I:RequestComplexControllerStimulus(cntrlBack)
end --back

end -- pos.x/z >
--currPos.x/z < end

--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//

end -- forwards/back
-- forwards/back end
--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//
--I:LogToHud("origPos.x"..origPos.x)
I:LogToHud("currPos.x"..currPos.x)
I:LogToHud("pos.x"..pos.x)

--I:LogToHud("origPos.y"..origPos.y)
--I:LogToHud("currPos.y"..currPos.y)
--I:LogToHud("pos.y"..pos.y)

--I:LogToHud("origPos.z"..origPos.z)
I:LogToHud("currPos.z"..currPos.z)
I:LogToHud("pos.z"..pos.z)
--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//
-- up/down start
if cntrlMoveUDEnabled == true then
if pos.y > currPos.y then
I:RequestComplexControllerStimulus(cntrlUp)
end --up
if currPos.y > pos.y then
I:RequestComplexControllerStimulus(cntrlDown)
end --down
end -- up/down
-- up/down end
--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//
I:Log("function move end")
end --move end
--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//
--functions end
--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//