////////////////////////////////////////////////////////////////////////////////////////
//options
int lcdUpdateDelay = 1; //delays checking for new lcds to improve performance value 1-100


//options end
////////////////////////////////////////////////////////////////////////////////////////
//variables
int mainRunCount = 100;
List<IMyTextPanel> consoles = new List<IMyTextPanel>();
const int transRate = 1;
VRage.MyFixedPoint transRateFP = (VRage.MyFixedPoint)transRate;
//variables
////////////////////////////////////////////////////////////////////////////////////////
public Program()
{
  Runtime.UpdateFrequency = UpdateFrequency.Update100;
}
////////////////////////////////////////////////////////////////////////////////////////
public void Main(string argument) {
meths meth = new meths();
//try{
mainRunCount++;
////////////////////////////////////////////////////////////////////////////////
//lcd cycle
List<IMyTextPanel> consoles = new List<IMyTextPanel>();
List<IMyTextPanel> lcds = new List<IMyTextPanel>();
 if(mainRunCount > lcdUpdateDelay){ //get lcds
mainRunCount = 0;
GridTerminalSystem.GetBlocksOfType<IMyTextPanel>(lcds);
}; //get lcds end
int whil = 0;
while(whil < lcds.Count){  //while
IMyTextPanel lcd = lcds[whil];
string lcdCD = lcd.CustomData;
string lcdCDLC = lcd.CustomData.ToLower();

////////////////////////////////////////////////////////////////////
//consoles init
if(lcdCDLC.Length >= 10){
 if(lcdCDLC.IndexOf("jifconsole") != -1){
  consoles.Add(lcd);
 };
};
int whil2 = 0;
meth.log(consoles,"Consoles",true,false);
while(consoles.Count > whil2){
IMyTextPanel console = consoles[whil2];
meth.log(consoles,"Init"+console);
whil2++;
};
meth.log(consoles,"Code start");

//consoles init end
////////////////////////////////////////////////////////////////////
try{
if(lcdCDLC.IndexOf("script") == -1){ //
int aftrscript = lcdCDLC.IndexOf(":",lcdCDLC.IndexOf("script"));
string scrpt = lcdCD.Substring(aftrscript+1,lcdCDLC.IndexOf(":",aftrscript+1)-(aftrscript+1));
if(scrpt == "IE"){
//inventory
/////////////////////////////////////
//info
if(lcdCDLC.IndexOf("info") == -1){lcd.WritePublicText(lcd.GetPublicText()+"\n"+"info not specified usage info:info in here: examples info:all: info:Ice:");};
if(lcdCDLC.IndexOf("info") != -1){
int aftrinf = lcdCDLC.IndexOf(":",lcdCDLC.IndexOf("info"));
string res = lcdCD.Substring(aftrinf+1,lcdCDLC.IndexOf(":",aftrinf+1)-(aftrinf+1));
/////////////////////////////

//all.all

if(res == "all"){





}; //inventory.all end
/////////////////////////////
//all.info
if(res != "all"){
//////////////////////
//getinfo
//try{
List<IMyEntity> entitys = new List<IMyEntity>();
GridTerminalSystem.GetBlocksOfType<IMyEntity>(entitys);

float resTot = 0;
float resEach = 0;
List<float> invCounts = new List<float>();
List<float> invCountsPlus = new List<float>();
int invCount = 0;
List<IMyInventory> invs = new List<IMyInventory>();
List<IMyInventory> invsPlus = new List<IMyInventory>();


int whilIA = 0;
while(entitys.Count > whilIA){
IMyEntity entity = entitys[whilIA];
IMyInventory inv = entity.GetInventory();
//try{
List<IMyInventoryItem> items = inv.GetItems();
invCount++;
invs.Add(inv);

string org = "";
float numb = 0;
int whilIAC = 0;
while(items.Count > whilIAC){
IMyInventoryItem item = items[whilIAC];
org = ""+item;
if(org.IndexOf(res) != -1){
numb = float.Parse(org.Substring(0,org.IndexOf("x")));
//numb = 5;
resTot = resTot+numb;
invCounts.Add(numb);
}; //if res
whilIAC++;
};

//}
//catch(Exception e){meth.log(consoles,"inventory.info caught: "+e);}

whilIA++;
}; //whilIA end
resEach = resTot/invCounts.Count;

//}
//catch(Exception e){meth.log(consoles,"inventory.info caught: "+e);}
//getinfo end
//////////////////////
//equalize items
int whilIAI = 0;
while(invs.Count > whilIAI){
IMyInventory inv = invs[whilIAI];
float invCunt = invCounts[whilIAI];

if(invCunt > resEach){
invCountsPlus.Add(invCunt);
invsPlus.Add(inv);
};

whilIAI++;
}; //whilIAI end

int whilIAI2 = 0;
int invsPlusIndx = 0;
while(invs.Count > whilIAI2){
float invCunt = invCounts[whilIAI2];
while(resEach-transRate > invCunt){

try{
if(invCountsPlus[invsPlusIndx] <= resEach){if(invsPlusIndx > invsPlus.Count){invsPlusIndx++;};};
if(invCountsPlus[invsPlusIndx] > resEach){
invs[whilIAI].TransferItemFrom(invsPlus[invsPlusIndx],0,null,false,transRateFP);
invCounts[whilIAI2] = invCounts[whilIAI2] + transRate;
invCountsPlus[whilIAI2] = invCountsPlus[whilIAI2] - transRate;

};
if(invCountsPlus[invsPlusIndx] <= resEach){meth.log(consoles,"ERROR Equalize items no inventory greater than resEach");};
}
catch(Exception e){meth.log(consoles,"caught TransferItemFrom: "+e);};

}; //resEach > invcunt

whilIAI2++;
}; //whilIAI2 end
//equalize items end
//////////////////////


lcd.WritePublicText("total "+res+": "+Math.Round(resTot,2));
lcd.WritePublicText(lcd.GetPublicText()+"\n"+"inventories: "+invCount);
lcd.WritePublicText(lcd.GetPublicText()+"\n"+res+" in each: "+Math.Round(resEach,2));

//}
//catch(Exception e){meth.log(consoles,"inventory.info caught: "+e);}
}; //inventory.info end
/////////////////////////////


}; //info end
/////////////////////////////////////

//inventory end
////////////////////////////////////////////////////////////////////

};
};
} //main try end
catch(Exception e){meth.log(consoles,"caught main try: "+e);};
//catch(Exception e){consoles[0].WritePublicText("caught main try: "+e);};

whil++;
};  //while end

//lcd cycle end
////////////////////////////////////////////////////////////////////////////////
//} //main try end
//catch(Exception e){meth.log(consoles,"caught main try: "+e);};
//catch(Exception e){consoles[0].WritePublicText("caught main try: "+e);};
} //void main end
//} //program close
////////////////////////////////////////////////////////////////////////////////////////
public class meths
{
////////////////////////////////////////////////////////////////////////////////
public void log (List<IMyTextPanel> consoles,string strng,bool clr = false,bool nl = true) {
int whil = 0;
while(consoles.Count > whil){ //consoles > whil
IMyTextPanel console = consoles[whil];
if(clr == true){
console.WritePublicText("");
};
if(nl == true){ //nl true
console.WritePublicText(console.GetPublicText()+"\n");
}; //nl true end
console.WritePublicText(console.GetPublicText()+strng);
whil++;
}; //consoles > whil end

} //log end
////////////////////////////////////////////////////////////////////////////////
}//meths end no } this must be last class
////////////////////////////////////////////////////////////////////////////////////////
