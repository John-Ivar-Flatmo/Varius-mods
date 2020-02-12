//read carefully do not use script with timer
//this script does not save to memory.
//in space engineers 1 sec is 60 ticks
//options

int runSpeed = 1; //1/10/100 determines how mutch the script shuld run in ticks. //default:1
int blockDelay = 10; //block detect delay, waits this many secounds to check for new blocks, lcds | wheels
//options end
//****//****//****//****//****//****//****//****//****//****//****//****//****//****//****//
//variables
int runCount = 0;
int runCountMax = 100;
List<IMyTextSurface> consoles = new List<IMyTextSurface>();
List<IMyTextSurface> lcds = new List<IMyTextSurface>();
List<IMyTextSurfaceProvider> lcdsProv = new List<IMyTextSurfaceProvider>();
string lcdWord;
string consoleWord;

//variables
//****//****//****//****//****//****//****//****//****//****//****//****//****//****//****//
public Program() //runs once on initialition
{
lcdWord = "jifsb"; //the one i void log must match this one
consoleWord = "jifconsole"+lcdWord;
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
if(lcdCDLC.IndexOf(consoleWord) != -1){consoles.Add(lcd);};
//consoles init end
//****//****//****//****//****//****//****//****//****//****//****//****//
whilSurf++;
}; //whilesurf end
whilProv++;
}; //whileprov end

int whil = 0;
while(lcds.Count > whil){  //while
IMyTextSurface lcd = lcds[whil];
string lcdCD = lcd.GetText();
string lcdCDLC = lcdCD.ToLower();
//lcd cycle lcd part end
//****//****//****//****//****//****//****//****//****//****//****//****//
//consoles init
if(lcdCDLC.IndexOf(consoleWord) != -1){consoles.Add(lcd);};
//consoles init end
//****//****//****//****//****//****//****//****//****//****//****//****//
whil++;
}; //whil end
//lcds end
//****//****//****//****//****//****//****//****//****//****//****//****//****//
}; //get blocks end

//****//****//****//****//****//****//****//****//****//****//****//****//****//****/
log(consoles,"[console start]",true,false);
log(consoles,"runCount: "+runCount+"/"+runCountMax+" | "+"consoles: "+consoles.Count+" | "+"lcds: "+lcds.Count);
log(consoles,"[Code start]");

} //void main end
//****//****//****//****//****//****//****//****//****//****//****//****//****//****//****//
public static void log (List<IMyTextSurface> consoles,string strng,bool clr = false,bool nl = true) {
string lcdWord = "jifsb";
string consoleWord = "jifconsole"+lcdWord;
int whil = 0;
while(consoles.Count > whil){ //consoles > whil
IMyTextSurface cnsl = consoles[whil];

if(clr == true){
cnsl.WriteText(consoleWord+"\n");
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
