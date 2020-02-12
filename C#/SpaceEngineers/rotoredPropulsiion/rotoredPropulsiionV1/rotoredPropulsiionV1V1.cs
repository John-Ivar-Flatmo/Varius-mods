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
//lcd cycle
List<IMyTextPanel> lcds = new List<IMyTextPanel>();
if(runCount==1){ //get lcds
consoles = new List<IMyTextPanel>();
//if(runCount==runCountMax){ //get lcds
GridTerminalSystem.GetBlocksOfType<IMyTextPanel>(lcds);
//GridTerminalSystem.GetBlocksOfType<IMyTextPanel>(lcds);
int whil = 0;
while(whil < lcds.Count){  //while
IMyTextPanel lcd = lcds[whil];
string lcdCD = lcd.CustomData;
string lcdCDLC = lcd.CustomData.ToLower();

//lcd cycle lcd part end
//****//****//****//****//****//****//****//****//****//****//****//****//****//
//consoles init
if(lcdCDLC.IndexOf("jifconsole") != -1){consoles.Add(lcd);};

//consoles init end
//****//****//****//****//****//****//****//****//****//****//****//****//****/
whil++;
}; //whil end
}; //get lcds end

//****//****//****//****//****//****//****//****//****//****//****//****//****//****/
log(consoles,"console start",true,false);
log(consoles,"runCount: "+runCount+"/"+runCountMax+" | "+"consoles: "+consoles.Count+" | "+"lcds: "+lcds.Count);
log(consoles,"Code start");

//wheel cycle
if(runCount==1){
log(consoles,"serching object for wheels");
wheels = new List<IMyMotorSuspension>();
GridTerminalSystem.GetBlocksOfType<IMyMotorSuspension>(wheels);
}; //runcount 1
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
