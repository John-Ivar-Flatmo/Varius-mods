//read carefully do not use script with timer
//this script controls rotors
//wheels must be tagged with custom info in order horizontal(X) vertical(Y) depth(Z) values are numbers from -1.1
//example wheel custom data:drive:1|-1|-1 //this wuld be the rotor that drive the wheel at right bottom back
//rotor types drive | steer | susp
//this script does not save to memory.
// in space engineers 1 sec is 60 ticks
//the script runs once eavry tick by defaoult, once evry 10 or 100 ticks can be done by putting ither 10 or 100 as argument
//options
int runSpeed = 10; //1/10/100 determines how mutch the script shuld run in ticks. Default 1
int controllType = 1; // |1: Turn|2: Tank|3: ?| 
int blockDelay = 10; //block detect delay, waits this many secounds to check for new blocks, lcds | wheels
int RPM=100; //howe mutch rpm you want
int SRPM=5; //steering rpm
double steerPrec=100; //steering precision recocmended, higher is more precise, but also potensially more wobbely
int steering=60; //max steering angle //FIXME, just redo script cuz i have absolutly fuck no idea why this no work to the left
//options end
//****//****//****//****//****//****//****//****//****//****//****//****//****//****//****//
//variables
int runCount = 0;
int runCountMax = 100;
List<IMyTextPanel> consoles = new List<IMyTextPanel>();
List<IMyMotorStator> wheels = new List<IMyMotorStator>();
List<IMyShipController> controlls = new List<IMyShipController>();

double steer=0; //stearing angle in radians, math is done further down

Vector3 mov = new Vector3(); //move
Vector2 rot = new Vector2(); //rotation
float rol = 0; //roll
//variables
//****//****//****//****//****//****//****//****//****//****//****//****//****//****//****//
public Program() //runs once on initialition
{
Runtime.UpdateFrequency = UpdateFrequency.Update1;
if(runSpeed==10){
Runtime.UpdateFrequency = UpdateFrequency.Update10;
};
if(runSpeed==100){
Runtime.UpdateFrequency = UpdateFrequency.Update100;
};
runCountMax = ((blockDelay*60)/runSpeed);
steer=steering*Math.PI/180; //stearing angle in radians
}
//****//****//****//****//****//****//****//****//****//****//****//****//****//****//****//
//main
public void Main(string argument) { //main script loop
runCount++;
if(runCount==runCountMax){
runCount=0;
};

mov = new Vector3(); //move
rot = new Vector2(); //rotation
rol = 0; //roll

//****//****//****//****//****//****//****//****//****//****//****//****//****//****//
List<IMyTextPanel> lcds = new List<IMyTextPanel>();
if(runCount==1){ //get blocks
//****//****//****//****//****//****//****//****//****//****//****//****//****//
//lcds start
consoles = new List<IMyTextPanel>();
GridTerminalSystem.GetBlocksOfType<IMyTextPanel>(lcds);
int whil = 0;
while(whil < lcds.Count){  //while
IMyTextPanel lcd = lcds[whil];
string lcdCD = lcd.CustomData;
string lcdCDLC = lcd.CustomData.ToLower();
//lcd cycle lcd part end
//****//****//****//****//****//****//****//****//****//****//****//****//
//consoles init
if(lcdCDLC.IndexOf("jifconsole") != -1){consoles.Add(lcd);};
//consoles init end
//****//****//****//****//****//****//****//****//****//****//****//****//
whil++;
}; //whil end
//lcds end
//****//****//****//****//****//****//****//****//****//****//****//****//****//
//wheels start
wheels = new List<IMyMotorStator>();
GridTerminalSystem.GetBlocksOfType<IMyMotorStator>(wheels);
//wheels end
//****//****//****//****//****//****//****//****//****//****//****//****//****//
//controll start
controlls = new List<IMyShipController>();
GridTerminalSystem.GetBlocksOfType<IMyShipController>(controlls);
//controll end
//****//****//****//****//****//****//****//****//****//****//****//****//****//
}; //get blocks end

//****//****//****//****//****//****//****//****//****//****//****//****//****//****/
log(consoles,"console start",true,false);
log(consoles,"runCount: "+runCount+"/"+runCountMax+" | "+"consoles: "+consoles.Count+" | "+"lcds: "+lcds.Count);
log(consoles,"Code start");

log(consoles,"steering: "+steering+"Deg"+" | "+steer+"Rad");

//controlls cycle
log(consoles,"controllers: "+controlls.Count);
int whilCtrls=0;
while(controlls.Count > whilCtrls){
var ctrl=controlls[whilCtrls];
mov=mov+ctrl.MoveIndicator;
rot=rot+ctrl.RotationIndicator;
rol=rol+ctrl.RollIndicator;
whilCtrls++;
};
log(consoles,"movment: "+mov);
log(consoles,"rotation: "+rot);
log(consoles,"roll: "+rol);
//controlls cycle end

//wheel cycle
log(consoles,"rotors: "+wheels.Count);
int whilWheel=0;
while(wheels.Count > whilWheel){
IMyMotorStator wheel = wheels[whilWheel];
log(consoles,"wheel: "+wheel.CustomData+" | "+wheel.EntityId);
string wCD = wheel.CustomData;
string wCDVal="0|0|0";
string wCDTyp="NONE";
float custRol=0;
float custRot=0;
float custMov=0;
try{
wCDVal = wCD.Substring(wCD.IndexOf(":")+1);
wCDTyp = wCD.Substring(0,wCD.IndexOf(":"));
custMov=float.Parse(wCDVal.Substring(0,wCDVal.IndexOf("|")));
custRot=float.Parse(wCDVal.Substring(wCDVal.IndexOf("|")+1,((wCDVal.IndexOf("|",wCDVal.IndexOf("|")+1))-wCDVal.IndexOf("|"))-1));
custRol=float.Parse(wCDVal.Substring(wCDVal.IndexOf("|",wCDVal.IndexOf("|")+1)+1));
}catch{};

log(consoles,"wCDVal: "+wCDVal);
log(consoles,"movment cust: "+custMov);
log(consoles,"rotation cust: "+custRot);
log(consoles,"roll cust: "+custRol);

//wheel.PropulsionOverride=mov.X*custRol;
if(wCDTyp=="drive"){
if(controllType==1){
wheel.TargetVelocityRPM=RPM*(mov.Z*custMov*custRot);
};
if(controllType==2){
if(custMov>0){
wheel.TargetVelocityRPM=(RPM*(mov.Z*custMov*custRot))+(SRPM*(mov.X*(+custMov)*custRot));
};
if(0>custMov){
wheel.TargetVelocityRPM=(RPM*(mov.Z*custMov*custRot))+(SRPM*(mov.X*(-custMov)*custRot));
};
};
};
if(wCDTyp=="steer"){
var angl = wheel.Angle;
log(consoles,"angl: "+angl);
float angMid = Convert.ToSingle(180*Math.PI/180); //180
if(controllType==1){
if(custMov>0){
if(steer>angl){wheel.TargetVelocityRPM=SRPM*(mov.X*(custMov*custRol)*custRot);}else{
wheel.TargetVelocityRPM=0;
};
};
if(0>custMov){
if(angl>-steer){wheel.TargetVelocityRPM=SRPM*(mov.X*(-custMov*custRol)*custRot);}else{
wheel.TargetVelocityRPM=0;
};
};
};
if(mov.X==0){
float angLow = Convert.ToSingle(-(((SRPM/steerPrec)*runSpeed)/10));
float angHigh = Convert.ToSingle(((SRPM/steerPrec)*runSpeed)/10);

if(angLow>angl){
wheel.TargetVelocityRPM=SRPM*(1);
}; //-angle
if(angl>angHigh){
wheel.TargetVelocityRPM=SRPM*(-1);
}; //+angle
};
};

whilWheel++;
};
//wheel cycle end
//****//****//****//****//****//****//****//****//****//****//****//****//****//****//****//
} //void main end
//****//****//****//****//****//****//****//****//****//****//****//****//****//****//****//
public static void log (List<IMyTextPanel> consoles,string strng,bool clr = false,bool nl = true) {
int whil = 0;
while(consoles.Count > whil){ //consoles > whil
IMyTextPanel console = consoles[whil];
if(clr == true){
console.WritePublicText("");
};
if(nl == true){ //nl true
console.WritePublicText(console.GetPublicText()+"\n"+strng);
}; //nl true end

if(nl == false){ //nl false
console.WritePublicText(console.GetPublicText()+strng);
}; //nl false end
whil++;
}; //consoles > whil end

} //log end
//****//****//****//****//****//****//****//****//****//****//****//****//****//****//****//
