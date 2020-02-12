//read carefully do not use script with timer
//this script controls suspension wheels and rotors
//there is handbrake support, it work same as other wheels
//this script does not save to memory.
// in space engineers 1 sec is 60 ticks
//example custom data TS //is needed to enable the script for the wheel/rotor
//suspension wheels may wobble when parked
//options
int runSpeed = 1; //1/10/100 determines how mutch the script shuld run in ticks. Default 1
int blockDelay = 10; //block detect delay, waits this many secounds to check for new blocks, lcds | wheels
int controllBlock = 1; //1:SuspensionWheel 2:Rotor 3:ADVRotor
float RPM = 0.8f; //wheel propulsion where 0 is 0% and 1 is 100% //works for rotors aswell it assumes 60RPM is 100% for them
float SRPM = 0.8f; //wheel propulsion modifier when turning where 0 is 0% and 1 is 100%
float BRPM = 0.2f; //propulsion used for breaking, only applies to suspension wheels if handbrake is on 1.0f is used regardless of this value.
float speedLimit = 100; //this script uses its own speed limit and changes the one on the wheels so use this.
//options end
//****//****//****//****//****//****//****//****//****//****//****//****//****//****//****//
//variables
int runCount = 0;
int runCountMax = 100;
List<IMyTextSurface> consoles = new List<IMyTextSurface>();
List<IMyTextSurface> lcds = new List<IMyTextSurface>();
List<IMyShipController> controlls = new List<IMyShipController>();

List<IMyMotorSuspension> wheelsS = new List<IMyMotorSuspension>();
List<IMyMotorStator> wheelsR = new List<IMyMotorStator>();
List<IMyMotorAdvancedStator> wheelsADV = new List<IMyMotorAdvancedStator>();

Vector3 mov = new Vector3(); //move
Vector2 rot = new Vector2(); //rotation
float rol = 0; //roll



bool brek = false; //break toggle

int lastBrek = 1;

float rtrRPM = 0; //rotor rpm
float rtrSRPM = 0; //rotor steer rpm
Vector3 currPos= new Vector3(); //current position of first programmable block on grid
Vector3 lastPos= new Vector3(); //last position of first programmable block on grid
IMyProgrammableBlock progBlock;

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

mov = new Vector3(); //move
rot = new Vector2(); //rotation
rol = 0; //roll
rtrRPM = 60*RPM; //rotor rpm
rtrSRPM = 60*SRPM; //rotor steer rpm
brek = false;

progBlock = Me;

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
if(lcdCDLC.IndexOf("jifconsolets") != -1){consoles.Add(lcd);};
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
if(lcdCDLC.IndexOf("jifconsolets") != -1){consoles.Add(lcd);};
//consoles init end
//****//****//****//****//****//****//****//****//****//****//****//****//
whil++;
}; //whil end
//lcds end
//****//****//****//****//****//****//****//****//****//****//****//****//****//
//wheels start
if(controllBlock==1){ //suspension
wheelsS = new List<IMyMotorSuspension>();
GridTerminalSystem.GetBlocksOfType<IMyMotorSuspension>(wheelsS);
}; //suspension
if(controllBlock==2){ //rotor
wheelsR = new List<IMyMotorStator>();
GridTerminalSystem.GetBlocksOfType<IMyMotorStator>(wheelsR);
}; //rotor
if(controllBlock==3){ //ADVrotor
wheelsADV = new List<IMyMotorAdvancedStator>();
GridTerminalSystem.GetBlocksOfType<IMyMotorAdvancedStator>(wheelsADV);
}; //ADVrotor
//wheels end
//****//****//****//****//****//****//****//****//****//****//****//****//****//
//controll start
controlls = new List<IMyShipController>();
List<IMyShipController> controllsTmp = new List<IMyShipController>();
GridTerminalSystem.GetBlocksOfType<IMyShipController>(controllsTmp);
int whilCtrlsTmp=0;
while(controllsTmp.Count > whilCtrlsTmp){
IMyShipController ctrl = controllsTmp[whilCtrlsTmp];
if(ctrl.IsUnderControl!=false && ctrl.ControlWheels!=false){
controlls.Add(ctrl);
};
whilCtrlsTmp++;
};
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
mov=mov+ctrl.MoveIndicator;
rot=rot+ctrl.RotationIndicator;
rol=rol+ctrl.RollIndicator;
if(ctrl.HandBrake==true){brek=true;};
whilCtrls++;
};
log(consoles,"movment: "+mov);
log(consoles,"rotation: "+rot);
log(consoles,"roll: "+rol);
log(consoles,"brake: "+brek);
//controlls cycle end

//wheel cycle
log(consoles,"wheels: "+wheelsS.Count);
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
int whilWheelS=0;
while(wheelsS.Count > whilWheelS){
IMyMotorSuspension wheel = wheelsS[whilWheelS];
string wCN = wheel.CustomName;
string wCNLC = wCN.ToLower();
string wCD = wheel.CustomData;
log(consoles,"wheel: "+wCN+" | "+wCD+" | "+wheel.EntityId);
float movDir = 0;

if(wCD=="TS"){
try{
if(wCNLC.IndexOf("left") != -1){
movDir=1;
};
if(wCNLC.IndexOf("right") != -1){
movDir=-1;
};
}catch{};

if(brek==true){
wheel.SetValue("Speed Limit",0.00f);
wheel.SetValue("Propulsion override",0f);
//wheel.SetValue("Force weld",true);
//wheel.SetValue("Weld speed",0f);
wheel.SetValue("Propulsion override",(RPM*-mov.Z*movDir)+(SRPM*mov.X)+brekMov*movDir); //break
}else{
wheel.SetValue("Speed Limit",speedLimit);
//wheel.SetValue("Force weld",false);
wheel.SetValue("Propulsion override",(RPM*-mov.Z*movDir)+(SRPM*mov.X)); //nobreak
};
};
whilWheelS++;
};
//wheel cycle end

//rotor cycle
log(consoles,"rotors: "+wheelsR.Count);
int whilWheelR=0;
while(wheelsR.Count > whilWheelR){
IMyMotorStator wheel = wheelsR[whilWheelR];
string wCN = wheel.CustomName;
string wCNLC = wCN.ToLower();
string wCD = wheel.CustomData;
log(consoles,"wheel: "+wCN+" | "+wCD+" | "+wheel.EntityId);
float movDir = 0;
if(wCD=="TS"){
try{
if(wCNLC.IndexOf("left") != -1){
movDir=1;
};
if(wCNLC.IndexOf("right") != -1){
movDir=-1;
};
}catch{};

wheel.TargetVelocityRPM=(rtrRPM*-mov.Z*movDir)+(rtrSRPM*mov.X);
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
float movDir = 0;
if(wCD=="TS"){
try{
if(wCNLC.IndexOf("left") != -1){
movDir=1;
};
if(wCNLC.IndexOf("right") != -1){
movDir=-1;
};
}catch{};

wheel.TargetVelocityRPM=(rtrRPM*-mov.Z*movDir)+(rtrSRPM*mov.X);
};
whilWheelADV++;
};
//ADVrotor cycle end
//****//****//****//****//****//****//****//****//****//****//****//****//****//****//****//
lastPos=currPos; //set last position to curr position
} //void main end
//****//****//****//****//****//****//****//****//****//****//****//****//****//****//****//
public static void log (List<IMyTextSurface> consoles,string strng,bool clr = false,bool nl = true) {
int whil = 0;
while(consoles.Count > whil){ //consoles > whil
IMyTextSurface cnsl = consoles[whil];

if(clr == true){
cnsl.WriteText("jifconsolets\n");
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
