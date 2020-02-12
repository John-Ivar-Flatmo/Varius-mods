//read carefully do not use script with timer
//custom data is set with angle and direciton agle is between 0-360 and direciton is between -1-1
//example custom data RS:60|-1 //this wuld meen that the rotor wuld stay at a 60degree angle with direction -1 the third is of no importance
//this script does not save to memory.
// in space engineers 1 sec is 60 ticks
//works with both rotors and adv rotors
//speed and torque are invesly proporsinal, the further away from the center the lower the tourqe and the higher the speed
//options

int runSpeed = 1; //1/10/100 determines how mutch the script shuld run in ticks. Default 1
int blockDelay = 10; //block detect delay, waits this many secounds to check for new blocks, lcds | wheels
//options end
//****//****//****//****//****//****//****//****//****//****//****//****//****//****//****//
//variables
int runCount = 0;
int runCountMax = 100;
List<IMyTextSurfaceProvider> consoles = new List<IMyTextSurfaceProvider>();
List<IMyShipController> controlls = new List<IMyShipController>();

List<IMyMotorSuspension> wheelsS = new List<IMyMotorSuspension>();
List<IMyMotorStator> wheelsR = new List<IMyMotorStator>();
List<IMyMotorAdvancedStator> wheelsADV = new List<IMyMotorAdvancedStator>();

Vector3 mov = new Vector3(); //move
Vector2 rot = new Vector2(); //rotation
float rol = 0; //roll

float torq = 1000000; //set to maximum rotor torqe
float rpm = 60; //set to maximum rotor rpm

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
}
//****//****//****//****//****//****//****//****//****//****//****//****//****//****//****//
//main
public void Main(string argument) { //main script loop
runCount++;
if(runCount==runCountMax){
runCount=0;
};

//****//****//****//****//****//****//****//****//****//****//****//****//****//****//
List<IMyTextSurfaceProvider> lcds = new List<IMyTextSurfaceProvider>();
if(runCount==1){ //get blocks
//****//****//****//****//****//****//****//****//****//****//****//****//****//
//lcds start
consoles = new List<IMyTextSurfaceProvider>();
GridTerminalSystem.GetBlocksOfType<IMyTextSurfaceProvider>(lcds);
int whil = 0;
while(whil < lcds.Count){  //while
IMyTextSurfaceProvider lcd = lcds[whil];
IMyTerminalBlock lcdTerm = lcd as IMyTerminalBlock;
string lcdCD = lcdTerm.CustomData;
string lcdCDLC = lcdTerm.CustomData.ToLower();
//lcd cycle lcd part end
//****//****//****//****//****//****//****//****//****//****//****//****//
//consoles init
if(lcdCDLC.IndexOf("jifconsoleRS") != -1){consoles.Add(lcd);};
//consoles init end
//****//****//****//****//****//****//****//****//****//****//****//****//
whil++;
}; //whil end
//lcds end
//****//****//****//****//****//****//****//****//****//****//****//****//****//
//wheels start
wheelsR = new List<IMyMotorStator>();
GridTerminalSystem.GetBlocksOfType<IMyMotorStator>(wheelsR);
wheelsADV = new List<IMyMotorAdvancedStator>();
GridTerminalSystem.GetBlocksOfType<IMyMotorAdvancedStator>(wheelsADV);
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

//controlls cycle
log(consoles,"controllers: "+controlls.Count);
int whilCtrls=0;
while(controlls.Count > whilCtrls){
var ctrl=controlls[whilCtrls];
whilCtrls++;
};
//controlls cycle end

//rotor cycle
log(consoles,"rotors: "+wheelsR.Count);
int whilWheelR=0;
while(wheelsR.Count > whilWheelR){
IMyMotorStator wheel = wheelsR[whilWheelR];
string wCN = wheel.CustomName;
string wCNLC = wCN.ToLower();
string wCD = wheel.CustomData;
log(consoles,"wheel: "+wCN+" | "+wCD+" | "+wheel.EntityId);

string wCDVal="0|0";
string wCDTyp="NONE";
float anglTrgt=0;
float direc=0;
try{
wCDVal = wCD.Substring(wCD.IndexOf(":")+1);
wCDTyp = wCD.Substring(0,wCD.IndexOf(":"));
anglTrgt=float.Parse(wCDVal.Substring(0,wCDVal.IndexOf("|")));
direc=float.Parse(wCDVal.Substring(wCDVal.IndexOf("|")+1));
}catch{};

if(wCDTyp=="RS"){
float anglR = wheel.Angle; //curr angle of rotor in radians
//float anglD = anglR*180/Math.PI; //curr angle of rotor in degrees
float angl = Convert.ToSingle(anglR*anglTrgt/Math.PI); //curr angle of rotor in degrees, using the target angle as the middle point instead of 180
float anglLow = 0; //lowest and agle can be
float anglHigh = anglTrgt*2; //highest an angle can be 2*middle angle
//float angMid = Convert.ToSingle(180*Math.PI/180); //180Deg in radians
int angDir=0;

if(angl>anglTrgt){
angDir=1;
};

if(anglTrgt>angl){
angDir=-1;
};

//float speed=angl-anglTrgt/anglTrgt*rpm; //RPM
float speed=angl/anglTrgt*rpm;
float strength=speed/rpm*torq-torq; //torque
if(0>strength){
strength=-strength;
};

wheel.TargetVelocityRPM=(speed*angDir*direc);
//wheel.TargetVelocityRPM=(speed*direc);
//wheel.Torque=(strength);
};
whilWheelR++;
};
//rotor cycle end

//ADVrotor cycle
log(consoles,"ADVrotors: "+wheelsADV.Count);
int whilWheelADV=0;
while(wheelsADV.Count > whilWheelADV){
IMyMotorAdvancedStator wheel = wheelsADV[whilWheelADV];
string wCN = wheel.CustomName;
string wCNLC = wCN.ToLower();
string wCD = wheel.CustomData;
log(consoles,"wheel: "+wCN+" | "+wCD+" | "+wheel.EntityId);

string wCDVal="0|0";
string wCDTyp="NONE";
float anglTrgt=0;
float direc=0;
try{
wCDVal = wCD.Substring(wCD.IndexOf(":")+1);
wCDTyp = wCD.Substring(0,wCD.IndexOf(":"));
anglTrgt=float.Parse(wCDVal.Substring(0,wCDVal.IndexOf("|")));
direc=float.Parse(wCDVal.Substring(wCDVal.IndexOf("|")+1));
}catch{};

if(wCDTyp=="RS"){
float anglR = wheel.Angle; //curr angle of rotor in radians
//float anglD = anglR*180/Math.PI; //curr angle of rotor in degrees
float angl = Convert.ToSingle(anglR*anglTrgt/Math.PI); //curr angle of rotor in degrees, using the target angle as the middle point instead of 180
float anglLow = 0; //lowest and agle can be
float anglHigh = anglTrgt*2; //highest an angle can be 2*middle angle
//float angMid = Convert.ToSingle(180*Math.PI/180); //180Deg in radians
//int angDir=0;

//if(angl>anglTrgt){
//angDir=1;
//};

//if(anglTrgt>angl){
//angDir=-1;
//};

float speed=angl-anglTrgt/anglTrgt*rpm; //RPM
float strength=speed/rpm*torq-torq; //torque

//wheel.TargetVelocityRPM=(speed*angDir*direc);
wheel.TargetVelocityRPM=(speed*direc);
//wheel.Torque=(strength);
};
whilWheelADV++;
};
//ADVrotor cycle end
//****//****//****//****//****//****//****//****//****//****//****//****//****//****//****//
} //void main end
//****//****//****//****//****//****//****//****//****//****//****//****//****//****//****//
public static void log (List<IMyTextSurfaceProvider> consoles,string strng,bool clr = false,bool nl = true) {
int whil = 0;
while(consoles.Count > whil){ //consoles > whil
IMyTextSurfaceProvider console = consoles[whil];
IMyTextSurface cnsl = console.GetSurface(0); //get first surface of surface provider
if(clr == true){
cnsl.WriteText("");
};
if(nl == true){ //nl true
cnsl.WriteText(cnsl.GetText()+"\n"+strng);
}; //nl true end

if(nl == false){ //nl false
cnsl.WriteText(cnsl.GetText()+strng);
}; //nl false end
whil++;
}; //consoles > whil end

} //log end
//****//****//****//****//****//****//****//****//****//****//****//****//****//****//****//
