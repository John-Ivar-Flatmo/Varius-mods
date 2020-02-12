//read carefully do not use script with timer
//this script does not save to memory.
// in space engineers 1 sec is 60 ticks
//the script runs once eavry tick by defaoult, once evry 10 or 100 ticks can be done by putting ither 10 or 100 as argument
//options
int runSpeed = 10; //1/10/100 determines how mutch the script shuld run in ticks. Default 1
int lcdDelay = 10; //lcd detect delay, waits this many secounds to check for new lcds
//options end
//****//****//****//****//****//****//****//****//****//****//****//****//****//****//****//
//variables
int runCount = 0;
int runCountMax = 100;
List<IMyTextPanel> consoles = new List<IMyTextPanel>();
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
runCountMax = ((lcdDelay*60)/runSpeed);
}
//****//****//****//****//****//****//****//****//****//****//****//****//****//****//****//
//main
public void Main(string argument) { //main script loop
runCount++;

//****//****//****//****//****//****//****//****//****//****//****//****//****//****//
//lcd cycle
List<IMyTextPanel> lcds = new List<IMyTextPanel>();
if(runCount==1){ //get lcds
consoles = new List<IMyTextPanel>();
GridTerminalSystem.GetBlocksOfType<IMyTextPanel>(lcds);
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
if(runCount==runCountMax){
runCount=0;
};
//****//****//****//****//****//****//****//****//****//****//****//****//****//****/
log(consoles,"console start",true,false);
log(consoles,"runCount: "+runCount+"/"+runCountMax+" | "+"consoles: "+consoles.Count+" | "+"lcds: "+lcds.Count);
log(consoles,"Code start");
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
