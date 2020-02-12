//read carefully do not use script with timer
//wheels must be tagged with custom info in order vertical(Y) horizontal(X) depth(Z) values are numbers from -1.1
//example wheel custom data: -1|1|-1 //this wuld be a wheel that is bottom right back
//this script does not save to memory.
// in space engineers 1 sec is 60 ticks
//the script runs once eavry tick by defaoult, once evry 10 or 100 ticks can be done by putting ither 10 or 100 as argument
//options
int runSpeed = 1; //1/10/100 determines how mutch the script shuld run in ticks. Default 1
int controllType = 1; // |1: Turn|2: Tank|3: ?| 
int blockDelay = 10; //block detect delay, waits this many secounds to check for new blocks, lcds | wheels
//options end
//****//****//****//****//****//****//****//****//****//****//****//****//****//****//****//
//variables
int runCount = 0;
int runCountMax = 100;
List<IMyTextPanel> consoles = new List<IMyTextPanel>();
List<IMyMotorSuspension> wheels = new List<IMyMotorSuspension>();
List<IMyShipController> controlls = new List<IMyShipController>();

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
wheels = new List<IMyMotorSuspension>();
GridTerminalSystem.GetBlocksOfType<IMyMotorSuspension>(wheels);
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
log(consoles,"wheels: "+wheels.Count);
int whilWheel=0;
while(wheels.Count > whilWheel){
//log(consoles,"wheel: "+wheels[whilWheel].EntityId);
log(consoles,"wheel: "+wheels[whilWheel].CustomData+" | "+wheels[whilWheel].EntityId);

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
