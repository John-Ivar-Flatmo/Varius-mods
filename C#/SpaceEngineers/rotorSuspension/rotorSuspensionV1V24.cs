//read carefully do not use script with timer
//custom data is set with angle and direciton agle is between 0-360 and direciton is between -1-1
//example custom data RS:60|-1 //this wuld meen that the rotor wuld stay at a 60degree angle with direction -1 the third is of no importance
//this script does not save to memory.
// in space engineers 1 sec is 60 ticks
//works with both rotors and adv rotors
//speed and torque are invesly proporsinal, the further away from the center the lower the tourqe and the higher the speed
//options

int runSpeed = 1; //1/10/100 determines how mutch the script shuld run in ticks. //default:1
int blockDelay = 10; //block detect delay, waits this many secounds to check for new blocks, lcds | wheels
float torq = 1000000; //set to maximum rotor torque //default: 1000000
float torqM = 10; //multiplier for torque incase suspension is too weak //default: 10
float rpm = 300; //set to maximum rotor rpm //default: 300
//options end
//****//****//****//****//****//****//****//****//****//****//****//****//****//****//****//
//variables
int runCount = 0;
int runCountMax = 100;
List<IMyTextSurface> consoles = new List<IMyTextSurface>();
List<IMyTextSurface> lcds = new List<IMyTextSurface>();
List<IMyTextSurfaceProvider> lcdsProv = new List<IMyTextSurfaceProvider>();


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
consoles = new List<IMyTextSurface>();
lcds = new List<IMyTextSurface>();
lcdsProv = new List<IMyTextSurfaceProvider>();
GridTerminalSystem.GetBlocksOfType<IMyTextSurface>(lcds);
GridTerminalSystem.GetBlocksOfType<IMyTextSurfaceProvider>(lcdsProv);

int whilProv = 0;
while(lcdsProv.Count > whilProv){ //while start
int whilSurf = 0;
IMyTextSurfaceProvider prov = lcdsProv[whilProv];
while(prov.SurfaceCount > whilSurf){
IMyTextSurface lcd = prov.GetSurface(whilSurf);
string lcdCD = lcd.GetText();
string lcdCDLC = lcdCD.ToLower();
//lcd cycle lcd part end
//****//****//****//****//****//****//****//****//****//****//****//****//
//consoles init
if(lcdCDLC.IndexOf("jifconsolers") != -1){consoles.Add(lcd);};
//consoles init end
//****//****//****//****//****//****//****//****//****//****//****//****//
whilSurf++;
}; //whilesurf end
whilProv++;
}; //whileprov end

int whil = 0;
while(lcds.Count > whil){  //while
IMyTextSurface lcd = lcds[whil];
//IMyTerminalBlock lcdTerm = lcd as IMyTerminalBlock;
string lcdCD = lcd.GetText();
string lcdCDLC = lcdCD.ToLower();
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

if(anglTrgt == 0){anglTrgt = 360;}; //makes sure angl target is never 0

float trgtAngl = anglTrgt;

if(anglTrgt > 270){
anglTrgt=270-(anglTrgt-270);
}else{
if(anglTrgt > 180 ){
anglTrgt=270-(anglTrgt-270);
};
};

if(90 > anglTrgt){
anglTrgt=90+(90-anglTrgt);
}else{
if(180 > anglTrgt){
anglTrgt=90+(90-anglTrgt);
};
};

if(anglTrgt==180 &&trgtAngl==180){anglTrgt=360;};

log(consoles,"Target: "+trgtAngl);
//log(consoles,"Atrgt: "+anglTrgt);

if(wCDTyp=="RS"){
float anglR = wheel.Angle; //curr angle of rotor in radians
float anglD = Convert.ToSingle(anglR*180/Math.PI); //curr angle of rotor in degrees
float anglSHigh = 360/anglTrgt*180;
float anglS=-(anglD+anglTrgt-360);
if(0>anglS){anglS=anglS+360;};

log(consoles,"Angle: "+anglD);
//log(consoles,"currAShifted: "+anglS);
//log(consoles,"currAShiftedHigh: "+anglSHigh);

float speedRaw=anglS-180;

float speed = speedRaw/anglSHigh*rpm;
float rpmT = rpm/torqM;
float strength=speed/rpmT*torq; //torque
if(0>strength){
strength=-strength;
};

//log(consoles,"RPMRaw: "+speedRaw);
log(consoles,"RPM: "+speed);
log(consoles,"Torque: "+strength);

wheel.TargetVelocityRPM=(speed*direc);
wheel.Torque=strength;
};
whilWheelR++;
};
//rotor cycle end
//****//****//****//****//****//****//****//****//****//****//****//****//****//****//****//
} //void main end
//****//****//****//****//****//****//****//****//****//****//****//****//****//****//****//
public static void log (List<IMyTextSurface> consoles,string strng,bool clr = false,bool nl = true) {
int whil = 0;
while(consoles.Count > whil){ //consoles > whil
IMyTextSurface cnsl = consoles[whil];

if(clr == true){
cnsl.WriteText("jifconsolers\n");
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
