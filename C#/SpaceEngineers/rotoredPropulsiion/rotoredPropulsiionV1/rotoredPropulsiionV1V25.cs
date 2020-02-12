//read carefully do not use script with timer
//this script controls rotors
//wheels must be tagged with custom info in order horizontal(X) vertical(Y) depth(Z) values are numbers from -1.1
//example wheel custom data:drive:1|-1|-1 //this wuld be the rotor that drive the wheel at right bottom back
//rotor types drive | steer | susp
//this script does not save to memory.
// NOTE replace balance with rotorSuspension scirpt method
// in space engineers 1 sec is 60 ticks
//the script runs once eavry tick by defaoult, once evry 10 or 100 ticks can be done by putting ither 10 or 100 as argument
//options
int runSpeed = 1; //1/10/100 dalay in ticks between each run. Default 1
int controllType = 1; // |1: Turn|2: Tank|3: ?| 
int blockDelay = 10; //block detect delay, waits this many secounds to check for new blocks, lcds | wheels
int RPM=100; //howe mutch rpm you want
int SRPM=5; //steering rpm
double steerPrec=100; //steering precision recocmended, higher is more precise, but also potensially more wobbely
int steering=40; //max steering angle
float speedLimit = 100; //this script uses its own speed limit and changes the one on the wheels so use this.
//options end
//****//****//****//****//****//****//****//****//****//****//****//****//****//****//****//
//variables
int runCount = 0;
int runCountMax = 100;
List<IMyTextSurface> consoles = new List<IMyTextSurface>();
List<IMyTextSurface> lcds = new List<IMyTextSurface>();

List<IMyMotorSuspension> wheelsS = new List<IMyMotorSuspension>();
List<IMyMotorStator> wheelsR = new List<IMyMotorStator>();
List<IMyMotorAdvancedStator> wheelsADV = new List<IMyMotorAdvancedStator>();
List<IMyShipController> controlls = new List<IMyShipController>();

int lastBrek = 1;
bool brek = false; //break toggle

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
brek = false;

mov = new Vector3(); //move
rot = new Vector2(); //rotation
rol = 0; //roll

//****//****//****//****//****//****//****//****//****//****//****//****//****//****//
if(runCount==1){ //get blocks
//****//****//****//****//****//****//****//****//****//****//****//****//****//
//lcds start
consoles = new List<IMyTextSurface>();
lcds = new List<IMyTextSurface>();
List<IMyTextSurfaceProvider> lcdsProv = new List<IMyTextSurfaceProvider>();
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
if(lcdCDLC.IndexOf("jifconsolerp") != -1){consoles.Add(lcd);};
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
if(lcdCDLC.IndexOf("jifconsolerp") != -1){consoles.Add(lcd);};
//consoles init end
//****//****//****//****//****//****//****//****//****//****//****//****//
whil++;
}; //whil end
//lcds end
//****//****//****//****//****//****//****//****//****//****//****//****//****//
//wheels start
wheelsS = new List<IMyMotorSuspension>();
GridTerminalSystem.GetBlocksOfType<IMyMotorSuspension>(wheelsS);

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

log(consoles,"steering: "+steering+"Deg"+" | "+steer+"Rad");

//controlls cycle
log(consoles,"controllers: "+controlls.Count);
int whilCtrls=0;
while(controlls.Count > whilCtrls){
var ctrl=controlls[whilCtrls];
mov=mov+ctrl.MoveIndicator;
rot=rot+ctrl.RotationIndicator;
rol=rol+ctrl.RollIndicator;
if(ctrl.HandBrake==true){brek=true;};
whilCtrls++;
};
log(consoles,"movment: "+mov);
log(consoles,"rotation: "+rot);
log(consoles,"roll: "+rol);
//controlls cycle end

//rotor cycle
log(consoles,"rotors: "+wheelsR.Count);
int whilWheelR=0;
while(wheelsR.Count > whilWheelR){
IMyMotorStator wheel = wheelsR[whilWheelR];
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
log(consoles,"steer");
var angl = wheel.Angle;
log(consoles,"angl: "+angl);
float angMid = Convert.ToSingle(180*Math.PI/180); //180
double anglDeg = angl*180/Math.PI; //curr angle in degrees
log(consoles,"anglDeg: "+anglDeg);
wheel.TargetVelocityRPM=0;
//if(mov.X==0){
float angLow = Convert.ToSingle(-(((SRPM/steerPrec)*runSpeed)/10));
float angHigh = Convert.ToSingle(((SRPM/steerPrec)*runSpeed)/10);

if(angLow>angl){
wheel.TargetVelocityRPM=SRPM*(1);
}; //-angle
if(angl>angHigh){
wheel.TargetVelocityRPM=SRPM*(-1);
}; //+angle
//};

if(controllType==1){
if(mov.X!=0){
if(steering>anglDeg&&anglDeg>-steering){wheel.TargetVelocityRPM=SRPM*(mov.X*(custMov*custRol)*custRot);};
};
};

}; //steer end

if(wCDTyp=="balance"){
log(consoles,"balance");
var angl = wheel.Angle;
log(consoles,"angl: "+angl);
var rotSpd = angl;
log(consoles,"rotSpd: "+rotSpd);
wheel.TargetVelocityRad=-rotSpd;
};

whilWheelR++;
};
//rotor cycle end

//wheel cycle
log(consoles,"wheels: "+wheelsS.Count);
int whilWheelS=0;
while(wheelsS.Count > whilWheelS){
IMyMotorSuspension wheel = wheelsS[whilWheelS];
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

//break
int compBrek = lastBrek;
//float brekMov = lastBrek*BRPM;
float brekMov = lastBrek;
if(brek==true){
//brekMov = lastBrek;
if(compBrek==-1){
brekMov = -0.8f;
lastBrek=1;
};
if(compBrek==1){
brekMov = 0.8f;
lastBrek=-1;
};
};
//break

if(wCDTyp=="drive"){
if(controllType==1){
wheel.SetValue("Propulsion override",((RPM/60)*-mov.Z*custMov));
};
if(controllType==2){
if(custMov!= 0){
wheel.SetValue("Propulsion override",((RPM/60)*-mov.Z*custMov)+((SRPM/60)*mov.X));
};
};

if(brek==true){
wheel.SetValue("Speed Limit",0.00f);
wheel.SetValue("Propulsion override",0f);
}else{
wheel.SetValue("Speed Limit",speedLimit);
};

};

whilWheelS++;
};
//wheel cycle end

//****//****//****//****//****//****//****//****//****//****//****//****//****//****//****//
} //void main end
//****//****//****//****//****//****//****//****//****//****//****//****//****//****//****//
public static void log (List<IMyTextSurface> consoles,string strng,bool clr = false,bool nl = true) {
int whil = 0;
while(consoles.Count > whil){ //consoles > whil
IMyTextSurface cnsl = consoles[whil];

if(clr == true){
cnsl.WriteText("jifconsolerp\n");
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
