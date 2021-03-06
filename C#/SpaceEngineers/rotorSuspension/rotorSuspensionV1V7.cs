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
float torq = 1000000; //set to maximum rotor torque //default: 1000000
float torqM = 2; //multiplier for torque incase suspension is too weak
float rpm = 10; //set to maximum rotor rpm //default: 30
//options end
//****//****//****//****//****//****//****//****//****//****//****//****//****//****//****//
//variables
int runCount = 0;
int runCountMax = 100;
List<IMyTextSurfaceProvider> consoles = new List<IMyTextSurfaceProvider>();
List<IMyTextSurfaceProvider> lcds = new List<IMyTextSurfaceProvider>();


List<IMyMotorSuspension> wheelsS = new List<IMyMotorSuspension>();
List<IMyMotorStator> wheelsR = new List<IMyMotorStator>();
List<IMyMotorAdvancedStator> wheelsADV = new List<IMyMotorAdvancedStator>();

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
if(runCount==1){ //get blocks
//****//****//****//****//****//****//****//****//****//****//****//****//****//
//lcds start
consoles = new List<IMyTextSurfaceProvider>();
lcds = new List<IMyTextSurfaceProvider>();
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
if(lcdCDLC.IndexOf("jifconsolers") != -1){consoles.Add(lcd);};
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
}; //get blocks end

//****//****//****//****//****//****//****//****//****//****//****//****//****//****/
log(consoles,"[console start]",true,false);
log(consoles,"runCount: "+runCount+"/"+runCountMax+" | "+"consoles: "+consoles.Count+" | "+"lcds: "+lcds.Count);
log(consoles,"[Code start]");

//rotor cycle
log(consoles,"(rotors: "+wheelsR.Count+")");
int whilWheelR=0;
while(wheelsR.Count > whilWheelR){
IMyMotorStator wheel = wheelsR[whilWheelR];
string wCN = wheel.CustomName;
string wCNLC = wCN.ToLower();
string wCD = wheel.CustomData;
log(consoles,whilWheelR+": "+wCN+" | "+wCD);

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

if(anglTrgt==0){anglTrgt=360;};
log(consoles,"trgtA: "+anglTrgt);
if(wCDTyp=="RS"){
float anglR = wheel.Angle; //curr angle of rotor in radians
float anglD = Convert.ToSingle(anglR*180/Math.PI); //curr angle of rotor in degrees
float angl = anglD/180*anglTrgt; //curr angle of rotor in degrees, using the target angle as the middle point instead of 180
float anglS = anglD/anglTrgt*180; //current angle shifted to match 180 when degrees matches target
float anglSHigh =360/anglTrgt*180; //maximun shifted angle

log(consoles,"currA: "+anglD);
//log(consoles,"currAShifted: "+anglS);
//log(consoles,"currAShiftedMax: "+anglSHigh);

//inversly inverse proposional start
//float speedRaw=180-anglS;
//float speed = speedRaw/anglSHigh*rpm;

//float strength=speed/rpm*torq-torq; //torque
//if(0>strength){
//strength=-strength;
//};
//inversly inverse proposional end

//inverse proposional start
//float strengthRaw=180-anglS;
//float strength = strengthRaw/anglSHigh*torq;

//float speed=strength/torq*rpm-rpm; //torque
//if(0>speed){
//speed=-speed;
//};
//inverse proposional end

//proposional start
float speedRaw=180-anglS;
float speed = speedRaw/anglSHigh*rpm;
float rpmT = rpm/torqM;
float strength=speed/rpmT*torq; //torque
if(0>strength){
strength=-strength;
};
//proposional end

//log(consoles,"RPMRaw: "+speedRaw);
log(consoles,"RPM: "+speed);
log(consoles,"Torque: "+strength);

wheel.TargetVelocityRPM=(speed*direc);
wheel.Torque=strength;
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
