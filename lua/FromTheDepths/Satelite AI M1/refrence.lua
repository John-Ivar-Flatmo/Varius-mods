--angle movement start
0-360

0-90-180

if angl > 0 and 180 > angl then

if angl >= 90 then reduce angl
if 90 > angl then increase angl

end-- > 0 nd 180 >

0-360
ngl
trgt

lft
rhgt

if ngl > 0 nd 180 >= ngl then
if trgt > 0 nd 180 >= trgt then
if trgt > ngl then
inc ngl
end
if trgt < ngl then
dec ngl
end
end
if trgt > 180 nd 270 > trgt then
inc ngl
end
if trgt > 270 nd 360 > trgt then
dec ngl
end
end

if ngl > 180 nd 360 >= ngl then
if trgt > 180 nd 360 >= trgt then
if trgt > ngl then
inc ngl
end
if trgt < ngl then
dec ngl
end
end
if trgt > 0 nd 90 > trgt then
inc ngl
end --trgt whtn 0 90
if trgt > 90 nd 180 > trgt then
dec ngl
end --trgt whtn 90 180
end

--trn start
function trn(I,yaw,trgt,vctrUp) --yaw = yaw 0-360 | trgt = 0-360 angle

if yaw > 0 and 180 >= yaw then

if trgt > 0 and 180 >= trgt then
if trgt > yaw then
trnChk(I,true,vctrUp)
end --trgt > yaw
if yaw > trgt then
trnChk(I,false,vctrUp)
end --yaw > trgt
end --trgt > 0 and 180 >= trgt //trgt wthn 0-181 end

if trgt > 180 and 270 > trgt then
trnChk(I,true,vctrUp)
end --trgt > 180 and 270 > trgt //trgt wthn 180-270 end

if trgt > 270 and 360 > trgt then
trnChk(I,false,vctrUp)
end --trgt > 270 and 360 > trgt end //trgt wthn 270-360

end --yaw > 0 and 180 >= yaw end

if yaw > 180 and 360 > yaw then

if trgt > 180 and 360 > trgt then
if trgt > yaw then
trnChk(I,true,vctrUp)
end --trgt > yaw
if yaw > trgt then
trnChk(I,false,vctrUp)
end --yaw > trgt
end --trgt > 180 and 360 > trgt //trgt wthn 180-360 end

if trgt > 0 and 90 > trgt then
trnChk(I,true,vctrUp)
end --trgt > 0 and 90 > trgt //trgt wthn 0-90 end

if trgt > 90 and 180 > trgt then
trnChk(I,false,vctrUp)
end --trgt > 90 and 180 > trgt end //trgt wthn 90-180

end --yaw > 180 and 360 > yaw end

end --trn end
--trn end
--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//
function trnChk(I,trnRght,vctrUp)
if trnRght == true then
if vctrUp.y > 0 then
I:RequestComplexControllerStimulus(cntrlTurnRight)
else --upsideup
I:RequestComplexControllerStimulus(cntrlTurnLeft)
end --upsidedown
end --trnrght true
if trnRght == false then
if vctrUp.y > 0 then
I:RequestComplexControllerStimulus(cntrlTurnLeft)
else --upsideup
I:RequestComplexControllerStimulus(cntrlTurnRight)
end --upsidedown
end --trnrght false
end --trnchk end
--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//--//
end



--angle movment end